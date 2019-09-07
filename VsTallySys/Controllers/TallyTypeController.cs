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
    /// 记账类型
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class TallyTypeController : ControllerBase
    {
        readonly TallyTypeService _tallyTypeService;
        public TallyTypeController()
        {
            _tallyTypeService = new TallyTypeService();
        }

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult Get()
        {
            List<VsTallyType> pFind = _tallyTypeService.Query();
            return JsonRes.Success(pFind.ToArray());
        }

        /// <summary>
        /// 新增类型信息
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sCode"></param>
        /// <param name="sType"></param>
        /// <param name="sDesc"></param>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult Post(string sName, string sCode, string sType, string sDesc)
        {
            VsTallyType entity = new VsTallyType
            {
                Id = System.Guid.NewGuid().ToString(),
                SName = sName,
                SCode = sCode,
                SDesc = sDesc,
                SType = sType,
            };
            string error = "";
            int res = _tallyTypeService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);
        }

        /// <summary>
        /// 修改类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sName"></param>
        /// <param name="sCode"></param>
        /// <param name="sType"></param>
        /// <param name="sDesc"></param>
        /// <returns></returns>
        [HttpPut]
        public OkObjectResult Put(string id, string sName, string sCode, string sType, string sDesc)
        {
            if (string.IsNullOrEmpty(id))
            {
                return JsonRes.Fail("id无效");
            }
            VsTallyType pExit = _tallyTypeService.QueryByID(id);
            if (pExit == null)
            {
                return JsonRes.Fail("类型不存在");
            }
            pExit.SName = sName;
            pExit.SCode = sCode;
            pExit.SDesc = sDesc;
            pExit.SType = sType;
            string error = "";
            int res = _tallyTypeService.TryUpdate(out error, pExit);
            if (res == 0)
            {
                return JsonRes.Fail(pExit, error);
            }
            return JsonRes.Success(pExit);
        }

        /// <summary>
        /// 删除类型
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
            VsTallyType pExit = _tallyTypeService.QueryByID(id);
            if (pExit == null)
            {
                return JsonRes.Fail("类型不存在");
            }
            string error = "";
            int res = _tallyTypeService.TryDelete(out error, pExit);
            if (res == 0)
            {
                return JsonRes.Fail(pExit, error);
            }
            return JsonRes.Success(pExit);
        }
    }
}
