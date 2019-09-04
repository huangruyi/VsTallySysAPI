using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VsTallySys.Services;
using VsTallySys.Models;
using VsTallySys.Common;
using Microsoft.AspNetCore.Authorization;

namespace VsTallySys.Controllers
{
    /// <summary>
    /// 模块操作类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class ModuleController : ControllerBase
    {
        public ModuleService _moduleService;
        public ModuleController()
        {
            _moduleService = new ModuleService();
        }

        /// <summary>
        /// 获取模块树信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult Get()
        {
            List<VsSysModule> pModuleList = _moduleService.Query().OrderBy(d => d.ILevel).ThenBy(d => d.IOrder).ToList();
            List<VsSysModule> modulesTree = new List<VsSysModule>();
            foreach (var module in pModuleList)
            {
                if (module.ILevel == 0) // 0级模块
                {
                    modulesTree.Add(module);
                }
                else
                { // 1级模块则需找到它的0级模块
                    var parentModule = pModuleList.Where(d => d.Id == module.SParentid).FirstOrDefault();
                    if (parentModule != null)
                    {
                        var parnetModuleExist = modulesTree.Where(d => d.Id == parentModule.Id).FirstOrDefault();
                        if (parnetModuleExist == null)// 模块树list里面有没有当前模块
                        {
                            modulesTree.Add(parentModule);
                        }
                        parentModule.ModuleChildren.Add(module);
                    }
                }
            }

            return JsonRes.Success(modulesTree.ToArray());
        }

        /// <summary>
        /// 新增模块
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sLinkurl"></param>
        /// <param name="iOrder"></param>
        /// <param name="sIcon"></param>
        /// <param name="bIsshow"></param>
        /// <param name="iLevel"></param>
        /// <param name="sParentid"></param>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult Post(string sName, string sLinkurl, int iOrder, string sIcon, bool bIsshow = true, int iLevel = 0, string sParentid = "-1")
        {
            // 判断模块是否存在 根据SLinkurl
            VsSysModule pExist = _moduleService.Query(d => d.SLinkurl == sLinkurl).FirstOrDefault();
            if (pExist != null)
            {
                return JsonRes.Fail("模块已存在");
            }
            VsSysModule entity = new VsSysModule
            {
                Id = Guid.NewGuid().ToString(),               
                SName = sName,
                SLinkurl = sLinkurl,
                IOrder = iOrder,
                SIcon = sIcon,
                BIsshow = bIsshow,
                ILevel = iLevel,
                SParentid = sParentid,
            };
            string error = "";
            int res = _moduleService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);          
        }

        /// <summary>
        /// 修改模块
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sName"></param>
        /// <param name="sLinkurl"></param>
        /// <param name="iOrder"></param>
        /// <param name="sIcon"></param>
        /// <param name="bIsshow"></param>
        /// <param name="iLevel"></param>
        /// <param name="sParentid"></param>
        /// <returns></returns>
        [HttpPut]
        public OkObjectResult Put(string id, string sName, string sLinkurl, int iOrder, string sIcon, bool bIsshow = true, int iLevel = 0, string sParentid = "-1")
        {
            if (string.IsNullOrEmpty(id))
            {
                return JsonRes.Fail("id无效");
            }
            VsSysModule pExit = _moduleService.QueryByID(id);
            if (pExit == null)
            {
                return JsonRes.Fail("模块不存在");
            }
            pExit.SName = sName;
            pExit.SLinkurl = sLinkurl;
            pExit.IOrder = iOrder;
            pExit.SIcon = sIcon;
            pExit.BIsshow = bIsshow;
            pExit.ILevel = iLevel;
            pExit.SParentid = sParentid;
            string error = "";
            int res = _moduleService.TryUpdate(out error, pExit);
            if (res == 0)
            {
                return JsonRes.Fail(pExit, error);
            }
            return JsonRes.Success(pExit);
        }

        /// <summary>
        /// 删除模块
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
            VsSysModule pExit = _moduleService.QueryByID(id);
            if (pExit == null)
            {
                return JsonRes.Fail("模块不存在");
            }
            string error = "";
            int res = _moduleService.TryDelete(out error, pExit);
            if (res == 0)
            {
                return JsonRes.Fail(pExit, error);
            }
            return JsonRes.Success(pExit);

        }
    }
}
