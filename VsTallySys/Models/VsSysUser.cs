using System;
using System.Collections.Generic;

namespace VsTallySys.Models
{
    public partial class VsSysUser
    {
        public string SUsername { get; set; }
        public string SPassword { get; set; }
        public string SName { get; set; }
        public DateTime? DLastlogin { get; set; }
        public DateTime DCreatetime { get; set; }
        public string SLogo { get; set; }
        public string SDesc { get; set; }
        public DateTime? DUpdatetime { get; set; }
    }
}
