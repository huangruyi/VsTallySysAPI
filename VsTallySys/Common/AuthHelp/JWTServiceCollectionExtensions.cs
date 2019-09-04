using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VsTallySys.Common
{
    public static class JWTServiceCollectionExtensions
    {
        /// <summary>
        /// jwt认证配置
        /// </summary>
        /// <param name="services"></param>
        public static void AddJWT(this IServiceCollection services)
        {
            string SSecretKey = AppSettings.App("JWTSettings", "SecretKey");
            string SRefreshTime = AppSettings.App("JWTSettings", "RefreshTime");
            string SAbsoluteTime = AppSettings.App("JWTSettings", "AbsoluteTime");
            string SIssuer = AppSettings.App("JWTSettings", "Issuer");
            string SAudience = AppSettings.App("JWTSettings", "Audience");
            //读取配置文件
            //var audienceConfig = Configuration.GetSection("JWTSettings");
            var symmetricKeyAsBase64 = SSecretKey; // 密钥
            var refreshTime = int.Parse(SRefreshTime); // 刷新token时间
            var absoluteTime = int.Parse(SAbsoluteTime); // 绝对过期时间

            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            // 生成加密签名
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            //角色列表
            var permission = new List<PermissionItem>();
            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,
                ClaimTypes.UserData,//基于用户的授权
                SIssuer,//发行人
                SAudience,//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromSeconds(absoluteTime),//接口的过期时间
                refresh: TimeSpan.FromSeconds(refreshTime) // token刷新时间
                );
            //【授权】
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission",
                         policy => policy.Requirements.Add(permissionRequirement));
            });

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = SIssuer,//发行人
                ValidateAudience = true,
                ValidAudience = SAudience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero, // 缓冲过期时间
                // RequireExpirationTime = true,
            };

            //2.1【认证】、core自带官方JWT认证
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         // 如果过期，则把<是否过期>添加到，返回头信息中
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             });

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
        }
    }
}
