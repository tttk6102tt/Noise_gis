using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork.Core.MapInterfaces {
    public interface ILayerRenderer {
        void Simple();
        void UniqueValue(bool ShowDefaltSymbol);
        void Classify();
        void DotDensity();
    }
}
