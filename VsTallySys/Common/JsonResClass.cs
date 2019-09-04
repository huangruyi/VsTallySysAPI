using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VsTallySys.Common
{
    public class JsonResClass
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 错误详细
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonResClass(int status)
        {
            Status = status;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonResClass(int status, string msg)
        {
            Status = status;
            Msg = msg;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonResClass(int status, string msg, string detail)
        {
            Status = status;
            Msg = msg;
            Detail = detail;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonResClass(int status, object data)
        {
            Status = status;
            Data = data;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonResClass(int status, string msg, object data)
        {
            Status = status;
            Msg = msg;
            Data = data;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonResClass(int status, string msg, string detail, object data)
        {
            Status = status;
            Msg = msg;
            Detail = detail;
            Data = data;
        }
    }
}
