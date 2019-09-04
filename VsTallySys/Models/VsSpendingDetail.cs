using System;
using System.Collections.Generic;

namespace VsTallySys.Models
{
    public partial class VsSpendingDetail
    {
        public string Id { get; set; }
        public DateTime DTime { get; set; }
        public double FRmb { get; set; }
        public string SName { get; set; }
        public string SCode { get; set; }
        public string SDesc { get; set; }
        public string SOwner { get; set; }
    }
}
