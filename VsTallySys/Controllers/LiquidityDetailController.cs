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
    /// 流动资金类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class LiquidityDetailController : ControllerBase
    {
        readonly LiquidityDetailService _liquidityDetailService;
        public LiquidityDetailController()
        {
            _liquidityDetailService = new LiquidityDetailService();
        }
        /// <summary>
        /// 获取流动资金信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult Get()
        {
            List<VsLiquidityDetail> pFind = _liquidityDetailService.Query();
            return JsonRes.Success(pFind.ToArray());
        }
        /// <summary>
        /// 新增流动资金详情信息
        /// </summary>
        /// <param name="dTime"></param>
        /// <param name="dRmb"></param>
        /// <param name="sCode"></param>
        /// <param name="sDesc"></param>
        /// <param name="sOwner"></param>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult Post(DateTime dTime, double dRmb, string sCode, string sDesc, string sOwner)
        {
            VsLiquidityDetail entity = new VsLiquidityDetail
            {
                DTime = dTime,
                FRmb = dRmb,
                SCode = sCode,
                SDesc = sDesc,
                SOwner = sOwner,
            };
            string error = "";
            int res = _liquidityDetailService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);
        }


        /// <summary>
        /// 修改流动资金详情信息
        /// </summary>
        /// <param name="dTime"></param>
        /// <param name="dRmb"></param>
        /// <param name="sCode"></param>
        /// <param name="sDesc"></param>
        /// <param name="sOwner"></param>
        /// <returns></returns>
        [HttpPut]
        public OkObjectResult Put(string id, DateTime dTime, double dRmb, string sCode, string sDesc, string sOwner)
        {
            if (string.IsNullOrEmpty(id))
            {
                return JsonRes.Fail("id无效");
            }
            VsLiquidityDetail pExist = _liquidityDetailService.QueryByID(id);
            if (pExist == null)
            {
                return JsonRes.Fail("数据不存在");
            }

            pExist.DTime = dTime;
            pExist.FRmb = dRmb;
            pExist.SCode = sCode;
            pExist.SDesc = sDesc;
            pExist.SOwner = sOwner;

            string error = "";
            int res = _liquidityDetailService.TryUpdate(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }

        /// <summary>
        /// 删除流动资金信息
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
            VsLiquidityDetail pExist = _liquidityDetailService.QueryByID(id);
            if (pExist == null)
            {
                return JsonRes.Fail("数据不存在");
            }
            string error = "";
            int res = _liquidityDetailService.TryDelete(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }
    }
}
