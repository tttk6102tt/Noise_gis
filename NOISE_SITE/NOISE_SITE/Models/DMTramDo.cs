using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NOISE_SITE.Models
{
    public class DMTramDo
    {
        [Key]
        public string MaTramDo { get; set; }
        public string DiaDiem { get; set; }
    }
}