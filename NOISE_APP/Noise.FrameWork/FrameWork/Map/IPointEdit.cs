using System;
using ESRI.ArcGIS.Geometry;
namespace FrameWork.Map {
    public interface IPointEdit {
        void Move(double x, double y);
        void Stop();
    }
}
