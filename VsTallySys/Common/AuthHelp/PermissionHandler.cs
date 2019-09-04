using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VsTallySys.Models;

namespace VsTallySys.Common
{
    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        ///// <summary>
        ///// services 层注入
        ///// </summary>
        //public IRoleModulePermissionServices _roleModulePermissionServices { get; set; }

        //public DbContext _db = new testPermissionContext();

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes)
        {
            Schemes = schemes;
            //_roleModulePermissionServices = roleModulePermissionServices;
        }

        public async Task<List<VsSysPower>> GetUserModule()
        {
            using (var context = new VS_TALLY_DBContext())
            {
                var powers = await Task.Run(() => context.VsSysPower.Where(p => p.BIsdeleted == false).ToList());

                if (powers.Count > 0)
                {
                    foreach (var item in powers)
                    {
                        item.User = await Task.Run(() => context.VsSysUser.Where(p => p.SUsername == item.SUserid).FirstOrDefault());
                        item.ApiModule = await Task.Run(() => context.VsSysApiModule.Where(p => p.Id == item.SApimoduleid).FirstOrDefault());
                    }
                }
                return powers;
            }

        }

        /// <summary>
        /// 正则表达式判断权限是否存在
        /// </summary>
        /// <param name="questUrl"></param>
        /// <param name="powerUrl"></param>
        /// <returns></returns>
        private bool Match(string questUrl, string powerUrl)
        {
            if (string.IsNullOrEmpty(questUrl) || string.IsNullOrEmpty(powerUrl))
            {
                return false;
            }
            // 判断权限url是否存在{xxx}类型参数字符串
            Regex regexObj = new Regex(@"\{(.*)\}"); // 正则表达式
            Match matchResult = regexObj.Match(powerUrl);
            if (matchResult.Success) // 匹配成功,将权限url参数部分字符串替换为正则表达式用于匹配请求的url
            {
                string paramStr = matchResult.Groups[0].Value; // 取到参数部分字符串 
                powerUrl = powerUrl.Replace(paramStr, @"(.*)"); // 替换参数部分字符串为正则表达式
            }
            Regex matchQuest = new Regex(powerUrl);
            bool res = matchQuest.Match(questUrl).Success;
            return res;
        }

        // 重载异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 将最新的角色和接口列表更新
            var data = await GetUserModule();
            var list = (from item in data
                        where item.BIsdeleted == false
                        orderby item.Id
                        select new PermissionItem
                        {
                            Url = item.ApiModule?.SLinkurl,
                            User = item.User?.SUsername,
                            Action = item.ApiModule?.SAction
                        }).ToList();

            requirement.Permissions = list;

            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            // 请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower();
            //questUrl = questUrl.Replace(new Regex(@"\{(.*)\}").Match(questUrl).Groups[0].Value, @"(.*)");
            // 请求的动作 get post put delete
            var action = httpContext.Request.Method.ToLower();

            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            //判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                //result?.Principal不为空即登录成功
                if (result?.Principal != null)
                {
                    httpContext.User = result.Principal;
                    // 获取当前用户的信息
                    var currentUser = (from item in httpContext.User.Claims
                                            where item.Type == requirement.ClaimType
                                            select item.Value).ToList();

                    if (questUrl == AppSettings.App("ConfigSettrings", "RefreshTokenUrl").ToLower()) // 如果请求的是刷新token，则只需要验证token是否有效，不需要验证角色
                    {
                        context.Succeed(requirement);
                        return;
                    }

                    // 当前用户  用户信息赋值到对象上，用以传给控制器
                    requirement.CurrUserAccount = httpContext.User.FindFirst(ClaimTypes.UserData).Value; 
                    requirement.CurrUserIp = httpContext.Connection.RemoteIpAddress.ToString();

                    // 判断是否是超管，超管拥有所有的权限，直接进入控制器请求资源
                    //if (requirement.CurrUserRole == AppSettings.App("ConfigSettrings", "SuperAdminRole"))
                    //{
                    //    context.Succeed(requirement);
                    //    return;
                    //}

                    // 获取当前用户相关权限
                    var permisssionUsers = requirement.Permissions.Where(w => currentUser.Contains(w.User));

                    // 判断当前用户是否有当前请求url权限
                    var permissionExist = permisssionUsers.Where(d => Match(questUrl, d.Url?.ObjToString().ToLower()) && d.Action?.ObjToString().ToLower() == action);

                    if (permissionExist.Count() == 0)
                    {
                        context.Fail();
                        return;
                    }

                    //判断过期时间（这里仅仅是最坏验证原则，你可以不要这个if else的判断，因为我们使用的官方验证，Token过期后上边的result?.Principal 就为 null 了，进不到这里了，因此这里其实可以不用验证过期时间，只是做最后严谨判断）
                    if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                    return;

                }
            }
            context.Fail();
            //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
            //if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST")
            //   || !httpContext.Request.HasFormContentType))
            //{
            //    context.Fail();
            //    return;
            //}
            //context.Fail();
        }
    }
}
