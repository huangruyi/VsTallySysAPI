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
    /// 权限操作类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class PowerController : ControllerBase
    {
        readonly PowerService _powerService;
        readonly PermissionRequirement _requirement; 
        readonly string _currUserAccount;
        readonly string _currUserRole;
        readonly string _currUserIp;
        readonly UserService _userService;
        readonly ApiModuleService _apiModuleService;
        readonly ModuleService _moduleService;
        public PowerController()
        {
            _powerService = new PowerService();
            _requirement = new PermissionRequirement();
            _currUserAccount = _requirement.CurrUserAccount;
            //_currUserRole = _requirement.CurrUserRole;
            _currUserIp = _requirement.CurrUserIp;
            _userService = new UserService();
            _apiModuleService = new ApiModuleService();
            _moduleService = new ModuleService();
        }

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult Get()
        {
            List<VsSysPower> pFind = _powerService.Query();
            return JsonRes.Success(pFind.ToArray());
        }

        /// <summary>
        /// 新增权限信息
        /// </summary>
        /// <param name="sUserid"></param>
        /// <param name="sModuleid"></param>
        /// <param name="sApiModuleid"></param>
        /// <param name="bIsdeleted"></param>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult Post(string sUserid, string sModuleid, string sApiModuleid, bool bIsdeleted = false)
        {
            VsSysUser pFindUser = _userService.QueryByID(sUserid);
            if (pFindUser == null)
            {
                return JsonRes.Fail("用户不存在");
            }
            VsSysModule pFindModule = _moduleService.QueryByID(sModuleid);
            if (pFindModule == null)
            {
                return JsonRes.Fail("模块不存在");
            }
            VsSysApiModule pFindApi = _apiModuleService.QueryByID(sApiModuleid);
            if (pFindApi == null)
            {
                return JsonRes.Fail("接口不存在");
            }
            VsSysPower pExit = _powerService.QuerySingle(d => d.SUserid == sUserid && d.SApimoduleid == sApiModuleid);
            if (pExit != null)
            {
                return JsonRes.Fail("接口已存在");
            }
            VsSysPower entity = new VsSysPower
            {
                Id = System.Guid.NewGuid().ToString(),
                SUserid = sUserid,
                SModuleid = sModuleid,
                SApimoduleid = sApiModuleid,
                BIsdeleted = bIsdeleted,
            };
            string error = "";
            int res = _powerService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);
        }
    }
}
