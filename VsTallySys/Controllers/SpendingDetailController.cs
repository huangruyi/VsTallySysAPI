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
    /// 支出详情类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class SpendingDetailController : ControllerBase
    {
        readonly SpendingDetailService _spendingDetailService;
        public SpendingDetailController()
        {
            _spendingDetailService = new SpendingDetailService();
        }
        /// <summary>
        /// 获取支出信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult Get()
        {
            List<VsSpendingDetail> pFind = _spendingDetailService.Query();
            return JsonRes.Success(pFind.ToArray());
        }
        /// <summary>
        /// 新增支出详情信息
        /// </summary>
        /// <param name="dTime"></param>
        /// <param name="dRmb"></param>
        /// <param name="sName"></param>
        /// <param name="sTCode"></param>
        /// <param name="sCode"></param>
        /// <param name="sDesc"></param>
        /// <param name="sOwner"></param>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult Post(DateTime dTime, double dRmb, string sName, string sTCode, string sCode, string sDesc, string sOwner)
        {
            VsSpendingDetail entity = new VsSpendingDetail
            {
                DTime = dTime,
                FRmb = dRmb,
                SName = sName,
                SCode = sCode,
                SDesc = sDesc,
                SOwner = sOwner,
                STCode = sTCode
            };
            string error = "";
            int res = _spendingDetailService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);
        }


        /// <summary>
        /// 修改支出详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dTime"></param>
        /// <param name="dRmb"></param>
        /// <param name="sName"></param>
        /// <param name="sTCode"></param>
        /// <param name="sCode"></param>
        /// <param name="sDesc"></param>
        /// <param name="sOwner"></param>
        /// <returns></returns>
        [HttpPut]
        public OkObjectResult Put(string id, DateTime dTime, double dRmb, string sName, string sTCode, string sCode, string sDesc, string sOwner)
        {
            if (string.IsNullOrEmpty(id))
            {
                return JsonRes.Fail("id无效");
            }
            VsSpendingDetail pExist = _spendingDetailService.QueryByID(id);
            if (pExist == null)
            {
                return JsonRes.Fail("数据不存在");
            }

            pExist.DTime = dTime;
            pExist.FRmb = dRmb;
            pExist.SName = sName;
            pExist.SCode = sCode;
            pExist.SDesc = sDesc;
            pExist.SOwner = sOwner;
            pExist.STCode = sTCode;

            string error = "";
            int res = _spendingDetailService.TryUpdate(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }

        /// <summary>
        /// 删除支出信息
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
            VsSpendingDetail pExist = _spendingDetailService.QueryByID(id);
            if (pExist == null)
            {
                return JsonRes.Fail("数据不存在");
            }
            string error = "";
            int res = _spendingDetailService.TryDelete(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }
    }
}
