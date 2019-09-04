using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VsTallySys.Common
{
    /// <summary>
    /// 用户或角色或其他凭据实体,就像是订单详情一样
    /// 之前的名字是 Permission
    /// </summary>
    public class PermissionItem
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string User { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// 请求的动作
        /// </summary>
        public virtual string Action { get; set; }
    }
}
