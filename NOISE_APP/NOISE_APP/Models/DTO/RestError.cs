using NOISE_APP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOISE_APP.Models.DTO
{
    public class RestError : RestBase
    {
        public RestError() : base(EnumError.ERROR)
        {

        }
        public RestError(string status) : base(status)
        {

        }
        public RestErrorDetail[] errors { get; set; }
    }
    public class RestErrorDetail
    {
        public int code { get; set; }
        public string message { get; set; }
        public string type { get; set; }
    }
}
