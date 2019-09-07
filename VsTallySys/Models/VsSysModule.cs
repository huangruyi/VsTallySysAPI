using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VsTallySys.Models
{
    public partial class VsSysModule
    {
        public string Id { get; set; }
        public string SName { get; set; }
        public int ILevel { get; set; }
        public string SParentid { get; set; }
        public string SLinkurl { get; set; }
        public string SIcon { get; set; }
        public bool BIsshow { get; set; }
        public int IOrder { get; set; }

        /// <summary>
        /// 子模块节点
        /// </summary>
        [NotMapped]
        public List<VsSysModule> ModuleChildren { get; set; } = new List<VsSysModule>();
        /// <summary>
        /// 接口节点
        /// </summary>
        [NotMapped]
        public List<VsSysApiModule> ApiModuleChildren { get; set; } = new List<VsSysApiModule>();
    }
}
