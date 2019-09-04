using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VsTallySys.Common;
using VsTallySys.Models;
using VsTallySys.Services;

namespace VsTallySys.Controllers
{
    /// <summary>
    /// token操作类
    /// </summary>
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        readonly UserService _userService;
        PermissionRequirement _requirement;
        // GET: api/Login
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="requirement"></param>
        public LoginController(PermissionRequirement requirement)
        {
            _requirement = requirement;
            _userService = new UserService();
        }

        /// <summary>
        /// 获取JWT的方法
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("JWTToken")]
        public object GetJWTToken3(string username, string password)
        {
            string jwtStr = string.Empty;
            bool suc = false;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return JsonRes.Fail("用户名或密码不能为空");
            }

            var enPassword = MD5Generate.Encrypt(password);
            var user = _userService.QuerySingle(d => d.SUsername == username);                
            if (user != null)
            {
                //更新用户最近登录时间
                user.DLastlogin = DateTime.Now.ToLocalTime();
                string error = "";
                int res = _userService.TryUpdate(out error, user);
                if (res == 0)
                {
                    return JsonRes.Fail(user, error);
                }
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.UserData, username), // 使用用户id认证授权
                    new Claim(JwtRegisteredClaimNames.Jti, username),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                return JsonRes.Success(token);
            }
            else
            {
                return JsonRes.Fail("认证失败");
            }
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RefreshToken")]
        [Authorize("Permission")]
        public object RefreshToken(string token = "")
        {
            string jwtStr = string.Empty;
            bool suc = false;

            if (string.IsNullOrEmpty(token))
            {
                return JsonRes.Fail("token无效，请重新登录！");                  
            }

            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);

            if (!string.IsNullOrEmpty(jwtToken.Id))
            {              
                var user = _userService.QueryByID(jwtToken.Id);
                if (user != null)
                {
                    //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.UserData, user.SUsername),// 使用用户id认证授权
                    new Claim(JwtRegisteredClaimNames.Jti, jwtToken.Id.ObjToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };


                    //用户标识
                    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    identity.AddClaims(claims);

                    var refreshToken = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                    return JsonRes.Success(refreshToken);
                }
            }

            return JsonRes.Fail("认证失败");
        }
    }
}
