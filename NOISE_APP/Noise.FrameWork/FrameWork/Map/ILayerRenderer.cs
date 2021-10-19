using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork.Map {
    public interface ILayerRenderer {
        void Simple();
        void UniqueValue();
        void Classify();
        void DotDensity();
    }
}
