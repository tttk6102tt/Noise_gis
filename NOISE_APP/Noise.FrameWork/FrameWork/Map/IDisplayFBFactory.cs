using System;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using FrameWork.Domain;

namespace FrameWork.Map {
    public interface  IDisplayFBFactory {
        IDisplayFeedback GetMoveObjectFB(IGeometry geometry, IPoint point);
        IDisplayFeedback GetLineMovePointFB(IGeometry geometry, IPoint point);
    }
}
