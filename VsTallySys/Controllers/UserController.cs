using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VsTallySys.Common;
using VsTallySys.Models;
using VsTallySys.Services;

namespace VsTallySys.Controllers
{
    /// <summary>
    /// 用户操作类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class UserController : ControllerBase
    {
        readonly UserService _userService;
        readonly ApiModuleService _apiModuleService;
        readonly PowerService _powerService;
        public UserController()
        {
            _userService = new UserService();
            _apiModuleService = new ApiModuleService();
            _powerService = new PowerService();
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult Get()
        {
            var list = _userService.Query();
            return JsonRes.Success(list.ToArray());
        }

        /// <summary>
        /// 新增用户  新用户注册时使用
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="logo"></param>
        /// <param name="desc"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public OkObjectResult Post(string username, string password, string name, string logo, string desc, string email, string phone)
        {
            VsSysUser pExist = _userService.QueryByID(username);
            if (pExist != null)
            {
                return JsonRes.Fail("用户名已存在");
            }
            // 循环将所有接口写入权限表
            List<VsSysApiModule> pFindModule = _apiModuleService.Query();
            foreach (var module in pFindModule)
            {
                VsSysPower powerEntity = new VsSysPower
                {
                    Id = System.Guid.NewGuid().ToString(),
                    SUserid = username,
                    SModuleid = module.SModuleid,
                    SApimoduleid = module.Id,
                    BIsdeleted = false,
                };
                string powerError = "";
                int powerRes = _powerService.TryAdd(out powerError, powerEntity);
                if (powerRes == 0)
                {
                    return JsonRes.Fail(powerEntity, powerError);
                }
            }
            VsSysUser entity = new VsSysUser
            {
                SUsername = username,
                SPassword = MD5Generate.Encrypt(password),
                SName = name,
                DCreatetime = DateTime.Now.ToLocalTime(),
                SLogo = logo,
                SDesc = desc,
                SEmail = email,
                SPhone = phone
            };
            string error = "";
            int res = _userService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }          
            return JsonRes.Success(entity);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <param name="logo"></param>
        /// <param name="desc"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPut]
        public OkObjectResult Put(string username, string name, string logo, string desc, string email, string phone)
        {
            if (string.IsNullOrEmpty(username))
            {
                return JsonRes.Fail("用户名无效");
            }

            VsSysUser pExist = _userService.QueryByID(username);
            if (pExist == null)
            {
                return JsonRes.Fail("用户名不存在");
            }

           
            pExist.SUsername = username;
            pExist.SName = name;
            pExist.DUpdatetime = DateTime.Now.ToLocalTime();
            pExist.SLogo = logo;
            pExist.SDesc = desc;
            pExist.SEmail = email;
            pExist.SPhone = phone;
            string error = "";
            int res = _userService.TryUpdate(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPut("ModifyPassword")]
        public OkObjectResult ModifyPassword(string username, string oldPassword, string newPassword)
        {
            VsSysUser pExist = _userService.QueryByID(username);
            if (pExist == null)
            {
                return JsonRes.Fail("用户名不存在");
            }
            if (pExist.SPassword != oldPassword)
            {
                return JsonRes.Fail("原密码不正确");
            }
            pExist.SPassword = newPassword;
            string error = "";
            int res = _userService.TryUpdate(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);

        }

        /// <summary>
        /// 删除用户  用户注销账号时使用
        /// </summary> 
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete]
        public OkObjectResult Delete(string username)
        {
            VsSysUser pExist = _userService.QueryByID(username);
            if (pExist == null) {
                return JsonRes.Fail("用户名不存在");
            }
            // 循环删除权限表的数据
            List<VsSysPower> pFindPower = _powerService.Query(d => d.SUserid == username);
            foreach (var power in pFindPower)
            {
                string powerError = "";
                int powerRes = _powerService.TryDelete(out powerError, power);
                if (powerRes == 0)
                {
                    return JsonRes.Fail(power, powerError);
                }
            }
            string error = "";
            int res = _userService.TryDelete(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            // TODO：注销用户时同时删除掉此用户的对应数据
            return JsonRes.Success(pExist);
        }

    }
}
