using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NOISE_SITE.Helper
{
    public class Commons
    {
        public string connstr = ConfigurationManager.ConnectionStrings["TKTC.Data.ConnectionString"].ConnectionString;

    }
}