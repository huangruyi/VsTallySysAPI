using System;
using System.Collections.Generic;

namespace VsTallySys.Models
{
    public partial class VsIncomeDetail
    {
        public string Id { get; set; }
        public DateTime DTime { get; set; }
        public double FRmb { get; set; }
        public string SCode { get; set; }
        public string SDesc { get; set; }
        public string SOwner { get; set; }
    }
}
