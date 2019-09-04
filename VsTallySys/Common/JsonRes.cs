using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VsTallySys.Common
{
    public static class JsonRes
    {
        static int SUCCESS_CODE = 1;
        static int FAIL_CODE = 0;
        static string DEFAULT_MSG_SUCCESS = "成功";
        static string DEFAULT_MSG_FAIL = "失败";

        public static OkObjectResult Success(object data)
        {
            return new OkObjectResult(new JsonResClass(SUCCESS_CODE, DEFAULT_MSG_SUCCESS, data));
        }
        public static OkObjectResult Success(object data, string msg)
        {
            return new OkObjectResult(new JsonResClass(SUCCESS_CODE, msg, data));
        }
        public static OkObjectResult Fail(string detail)
        {
            return new OkObjectResult(new JsonResClass(FAIL_CODE, DEFAULT_MSG_FAIL, detail));
        }
        public static OkObjectResult Fail(object data, string detail)
        {
            return new OkObjectResult(new JsonResClass(FAIL_CODE, DEFAULT_MSG_FAIL, detail, data));
        }
    }
}
