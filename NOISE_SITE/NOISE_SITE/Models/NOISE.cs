using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NOISE_SITE.Models
{
    [Table("NOISE")]
    public class NOISE
    {
        public string ID { get; set; }
        public string TIME { get; set; }
        public string LAT { get; set; }
        public string LONG { get; set; }
        public string dB { get; set; }
        [NotMapped]
        public string DiaDiem { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sTT { get; set; }
    }
}