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
    /// 接口操作类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class ApiModuleController : ControllerBase
    {
        readonly ApiModuleService _apiModuleService;
        readonly ModuleService _moduleService;
        public ApiModuleController()
        {
            _apiModuleService = new ApiModuleService();
            _moduleService = new ModuleService();
        }

        /// <summary>
        /// 获取所有接口列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult Get()
        {
            List<VsSysApiModule> pFind = _apiModuleService.Query();
            return JsonRes.Success(pFind.ToArray());
        }

        /// <summary>
        /// 通过模块id获取接口列表
        /// </summary>
        /// <param name="sMoudleid"></param>
        /// <returns></returns>
        [HttpGet("GetByModule")]
        public OkObjectResult GetByModule(string sMoudleid)
        {
            if (string.IsNullOrEmpty(sMoudleid))
            {
                return JsonRes.Fail("id无效");
            }
            List<VsSysApiModule> pFind = _apiModuleService.Query(d => d.SModuleid == sMoudleid);
            return JsonRes.Success(pFind.ToArray());
        }

        /// <summary>
        /// 新增接口信息
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sModuleid"></param>
        /// <param name="sLinkurl"></param>
        /// <param name="sController"></param>
        /// <param name="sAction"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public OkObjectResult Post(string sName, string sModuleid, string sLinkurl, string sController, string sAction)
        {
            VsSysModule pExistModule = _moduleService.QueryByID(sModuleid);
            if (pExistModule == null)
            {
                return JsonRes.Fail("模块不存在");
            }
            VsSysApiModule entity = new VsSysApiModule
            {
                Id = System.Guid.NewGuid().ToString(),
                SName = sName,
                SModuleid = sModuleid,
                SLinkurl = sLinkurl,
                SController = sController,
                SAction = sAction,
            };
            string error = "";
            int res = _apiModuleService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);
        }

        /// <summary>
        /// 修改接口信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sName"></param>
        /// <param name="sMoudleid"></param>
        /// <param name="sLinkurl"></param>
        /// <param name="sController"></param>
        /// <param name="sAction"></param>
        /// <returns></returns>
        [HttpPut]
        public OkObjectResult Put(string id, string sName, string sMoudleid, string sLinkurl, string sController, string sAction)
        {
            if (string.IsNullOrEmpty(id))
            {
                return JsonRes.Fail("id无效");
            }
            VsSysApiModule pExist = _apiModuleService.QueryByID(id);
            if (pExist == null)
            {
                return JsonRes.Fail("接口不存在");
            }
            pExist.SName = sName;
            pExist.SModuleid = sMoudleid;
            pExist.SLinkurl = sLinkurl;
            pExist.SController = sController;
            pExist.SAction = sAction;
            string error = "";
            int res = _apiModuleService.TryUpdate(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }

        /// <summary>
        /// 删除接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public OkObjectResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return JsonRes.Fail("id无效");
            }
            VsSysApiModule pExist = _apiModuleService.QueryByID(id);
            if (pExist == null)
            {
                return JsonRes.Fail("接口不存在");
            }
            // TODO:先去接口权限表里删除此接口对应的权限信息，再删掉此接口
            string error = "";
            int res = _apiModuleService.TryDelete(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }
    }
}
