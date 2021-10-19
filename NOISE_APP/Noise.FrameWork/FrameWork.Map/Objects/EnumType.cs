using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork.Map {
    public enum LayerType {
        Point   = 0,
        Line    = 1,
        Polygon = 2
    }

    public enum RefreshType {
        None                = -1,
        SelectionChanged    = 0,
        ElementChanged      = 1,
        GeometryChanged     = 2
    }
}
