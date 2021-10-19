using FrameWork.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork.Core.Domain
{
    [ObjectAttribute("ONhiemDat", ObjectTypeEnums.Feature)]
    public class ONhiemDat
    {
        public int OBJECTID { get; set; }
        public string Ma_Diem { get; set; }
        public string Ten_Mau { get; set; }
        public string KQ_As { get; set; }
        public string KQ_Cd { get; set; }
        public string KQ_Cu { get; set; }
        public string KQ_Pb { get; set; }
        public string KQ_Zn { get; set; }
        public double Toado_B { get; set; }
        public double Toado_L { get; set; }
        public string Ghi_chu { get; set; }
        public long ThoiGian { get; set; }
    }
}
