using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOISE_APP.Enums
{
    [Flags]
    public enum EnumTypeData
    {
        TABLE = 1,
        VECTOR = 2,
        RASTER = 3
    }
    public enum EnumFormat
    {
        NONE = 0,

        BOLD = 1,
        ITALIC = 2,
        UNDERLINE = 4,

        BORDER_LEFT = 8,
        BORDER_RIGHT = 16,
        BORDER_BOTTOM = 32,
        BORDER_TOP = 64,
        BORDER = BORDER_LEFT | BORDER_RIGHT | BORDER_BOTTOM | BORDER_TOP,

        LEFT = 128,
        RIGHT = 256,
        CENTER = 512,

        MIDDLE = 1024,
        BOTTOM = 2048,
        TOP = 4096
    }
}
