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
    /// 密保问题类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class SecureController : ControllerBase
    {
        readonly SecureService _vsSysSecureService;
        public SecureController()
        {
            _vsSysSecureService = new SecureService();
        }

        /// <summary>
        /// 获取所有的密保问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public OkObjectResult Get()
        {
            var list = _vsSysSecureService.Query();
            return JsonRes.Success(list.ToArray());
        }

        /// <summary>
        /// 新增密保问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult Post(string question)
        {
            VsSysSecure pExist = _vsSysSecureService.QuerySingle(d => d.SQuestion == question);
            if (pExist != null)
            {
                return JsonRes.Fail("问题已存在");
            }
            VsSysSecure entity = new VsSysSecure
            {
                Id = System.Guid.NewGuid().ToString(),
                SQuestion = question
            };
            string error = "";
            int res = _vsSysSecureService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);
        }
    }
}
