using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VsTallySys.Models
{
    public partial class VsSysPower
    {
        public string Id { get; set; }
        public string SUserid { get; set; }
        public string SModuleid { get; set; }
        public string SApimoduleid { get; set; }
        public bool BIsdeleted { get; set; }

        [NotMapped]
        public VsSysUser User { get; set; }
        [NotMapped] // 不做数据库映射
        public VsSysApiModule ApiModule { get; set; }
    }
}
