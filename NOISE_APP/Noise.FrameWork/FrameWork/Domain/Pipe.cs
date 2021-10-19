using System;
using ESRI.ArcGIS.Geodatabase;

namespace FrameWork.Domain {
    [TableNameAttr("Pipes")]
    public class Pipe : PipeLine{
        public Pipe(IFeature feature) : base(feature) { }
        public Pipe() { }
    }
}
