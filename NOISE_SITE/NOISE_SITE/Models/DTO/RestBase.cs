using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOISE_SITE.Models.DTO
{
    public class RestBase
    {
        public RestBase(string status,string redirect = "")
        {
            this.status = status;
            this.redirect = redirect;
        }
        public virtual string status { get; set; }
        public virtual string redirect { get; set; }
    }
}
