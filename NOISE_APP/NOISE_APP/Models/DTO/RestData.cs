
using NOISE_APP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOISE_APP.Models.DTO
{
    public class RestData : RestBase
    {
        public RestData() : base(EnumError.OK)
        {

        }
        public RestData(string status) : base(status)
        {

        }
        public object data { get; set; }
    }
}
