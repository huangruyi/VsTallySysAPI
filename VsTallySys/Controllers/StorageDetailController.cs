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
    /// 存储详情类
    /// </summary>
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [Authorize("Permission")]
    public class StorageDetailController : ControllerBase
    {
        readonly StorageDetailService _storageDetailService;
        public StorageDetailController()
        {
            _storageDetailService = new StorageDetailService();
        }

        /// <summary>
        /// 获取存储信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult Get()
        {
            List<VsStorageDetail> pFind = _storageDetailService.Query();
            return JsonRes.Success(pFind.ToArray());
        }
        /// <summary>
        /// 新增存储详情信息
        /// </summary>
        /// <param name="dTime"></param>
        /// <param name="dRmb"></param>
        /// <param name="sCode"></param>
        /// <param name="iOperation"></param>
        /// <param name="sDesc"></param>
        /// <param name="sOwner"></param>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult Post(DateTime dTime, double dRmb, string sCode, int iOperation, string sDesc, string sOwner)
        {
            VsStorageDetail entity = new VsStorageDetail
            {
                DTime = dTime,
                FRmb = dRmb,
                SCode = sCode,
                IOperation = iOperation,
                SDesc = sDesc,
                SOwner = sOwner,
            };
            string error = "";
            int res = _storageDetailService.TryAdd(out error, entity);
            if (res == 0)
            {
                return JsonRes.Fail(entity, error);
            }
            return JsonRes.Success(entity);
        }


        /// <summary>
        /// 修改存储详情信息
        /// </summary>
        /// <param name="dTime"></param>
        /// <param name="dRmb"></param>
        /// <param name="sCode"></param>
        /// <param name="iOperation"></param>
        /// <param name="sDesc"></param>
        /// <param name="sOwner"></param>
        /// <returns></returns>
        [HttpPut]
        public OkObjectResult Put(string id, DateTime dTime, double dRmb, string sCode, int iOperation, string sDesc, string sOwner)
        {
            if (string.IsNullOrEmpty(id))
            {
                return JsonRes.Fail("id无效");
            }
            VsStorageDetail pExist = _storageDetailService.QueryByID(id);
            if (pExist == null)
            {
                return JsonRes.Fail("数据不存在");
            }

            pExist.DTime = dTime;
            pExist.FRmb = dRmb;
            pExist.SCode = sCode;
            pExist.IOperation = iOperation;
            pExist.SDesc = sDesc;
            pExist.SOwner = sOwner;
         
            string error = "";
            int res = _storageDetailService.TryUpdate(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }

        /// <summary>
        /// 删除存储信息
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
            VsStorageDetail pExist = _storageDetailService.QueryByID(id);
            if (pExist == null)
            {
                return JsonRes.Fail("数据不存在");
            }
            string error = "";
            int res = _storageDetailService.TryDelete(out error, pExist);
            if (res == 0)
            {
                return JsonRes.Fail(pExist, error);
            }
            return JsonRes.Success(pExist);
        }
    }
}
