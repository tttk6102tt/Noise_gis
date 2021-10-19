using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOISE_APP.Models.DTO
{
    public class RestBase
    {
        public RestBase(string status)
        {
            this.status = status;
        }
        public virtual string status { get; set; }
    }
}
