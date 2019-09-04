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
    /// 密保用户类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    public class SecureUserController : ControllerBase
    {
        readonly SecureUserService _secureUserService;
        public SecureUserController()
        {
            _secureUserService = new SecureUserService();
        }

        /// <summary>
        /// 验证密保问题
        /// </summary>
        /// <param name="sUserid"></param>
        /// <param name="sQuestionid"></param>
        /// <param name="sAnswer"></param>
        /// <returns></returns>
        [HttpGet("GetAnswer")]
        public OkObjectResult GetAnswer(string sUserid, string sQuestionid, string sAnswer)
        {
            VsSysSecureUser pExitUser = _secureUserService.QuerySingle(d => d.SUserid == sUserid);
            if (pExitUser == null)
            {
                return JsonRes.Fail("用户不存在");
            }
            VsSysSecureUser pExit = _secureUserService.QuerySingle(d => d.SUserid == sUserid && d.SQuestionid == sQuestionid);
            if (pExit == null)
            {
                return JsonRes.Fail("密保问题不存在");
            }
            if (pExit.SAnswer != sAnswer)
            {
                return JsonRes.Fail("密保答案不正确");
            }
            return JsonRes.Success("验证成功");
        }

        /// <summary>
        /// 新增密保用户信息
        /// </summary>
        /// <param name="sUserid"></param>
        /// <param name="sQuestionid"></param>
        /// <param name="sAnswer"></param>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult Post(string sUserid, string sQuestionid, string sAnswer)
        {
            VsSysSecureUser pExitUser = _secureUserService.QuerySingle(d => d.SUserid == sUserid);
            if (pExitUser != null)
            {
                return JsonRes.Fail("用户密保问题已存在");
            }
            VsSysSecureUser entity = new VsSysSecureUser
            {
                Id = System.Guid.NewGuid().ToString(),
                SUserid = sUserid,
                SQuestionid = sQuestionid,
                SAnswer = sAnswer,
            };
            string error = "";
            int res = _secureUserService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);
        }

        /// <summary>
        /// 修改密保问题信息
        /// </summary>
        /// <param name="sUserid"></param>
        /// <param name="sQuestionid"></param>
        /// <param name="sAnswer"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize("Permission")]
        public OkObjectResult Put(string sUserid, string sQuestionid, string sAnswer)
        {
            VsSysSecureUser pExitUser = _secureUserService.QuerySingle(d => d.SUserid == sUserid);
            if (pExitUser == null)
            {
                return JsonRes.Fail("用户不存在");
            }
            pExitUser.SQuestionid = sQuestionid;
            pExitUser.SAnswer = sAnswer;
            string error = "";
            int res = _secureUserService.TryUpdate(out error, pExitUser);
            if (res == 0)
            {
                return JsonRes.Fail(pExitUser, error);
            }
            return JsonRes.Success(pExitUser);

        }
    }
}
