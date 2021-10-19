using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOISE_APP.Models
{

    public class NguoiDung
    {
        public string ID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public int LockoutEnabled { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
