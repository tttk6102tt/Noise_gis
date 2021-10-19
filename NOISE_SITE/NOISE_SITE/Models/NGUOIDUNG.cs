using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NOISE_SITE.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Users")]
    public class NGUOIDUNG
    {
        [Key]
        public string ID { get; set; }

        [System.ComponentModel.DisplayName("Tên đăng nhập")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]

        [System.ComponentModel.DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }
        public int LockoutEnabled { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}