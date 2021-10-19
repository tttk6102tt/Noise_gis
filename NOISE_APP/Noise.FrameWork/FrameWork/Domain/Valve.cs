using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace FrameWork.Domain {
    [TableNameAttr("Valves")]
    public class Valve : Component {
        public Valve(IFeature feature) : base(feature) { }
        public Valve() { }
    }
}
