using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VsTallySys.Common
{
    public static class UtilConvert
    {
        public static string TimeToString(this DateTime thisValue)
        {
            DateTime reval = DateTime.Now;
            if (thisValue == null) return null;
            if (thisValue != null && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                return reval.ToString();
            }
            return reval.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static string ObjToString(this object thisValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return "";
        }
    }
}
