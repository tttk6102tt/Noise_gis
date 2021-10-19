using System;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace FrameWork.Core.MapInterfaces {
    public interface IGeometryManipulation : IDisposable{
        IPoint CreatePointAction(double x, double y);
        void StartCreateNewLineAction(double x, double y);
        IPolyline EndCreateNewLineAction();
        void MoveTo(double x, double y);
        void MoveTo(double x, double y, out bool isMove);
        void Add(double x, double y);
        void StartMovePointAction(IPoint inputPoint, double x, double y);
        IPoint EndMovePointAction();
        void StartMoveLineAction(IPolyline inputLine, double x, double y);
        IPolyline EndMoveLineAction();
        void StartEditLineAction(IPolyline inputLine, int pointIndex, double x, double y);
        IPolyline EndEditLineAction();
        void StartCreatePolygonAction(double x, double y);
        IPolygon EndCreatePolygonAction();
        void StartMovePolygonAction(IPolygon inputPolygon, double x, double y);
        IPolygon EndMovePolygonAction();
        void StartEditPolygonAction(IPolygon inputPolygon, int pointIndex, double x, double y);
        IPolygon EndEditPolygonAction();
        bool SplitLineByPoint(IPolyline inputLine,IPoint splitPoint, out IPolyline line1,out IPolyline line2);
        bool SplitPolygonByLine(IPolygon inputLine, IPolyline splitLine, out IPolygon polygon1, out IPolygon polygon2);
        bool ClipPolygonGetIntersect(IPolygon inputPolygon, IPolygon intersectPolygon, out IPolygon polygonResult);
        bool ClipPolygonGetDifference(IPolygon inputPolygon, IPolygon intersectPolygon, out IPolygon polygonResult);
        bool UnionGeometry(IGeometry inputGeo, IGeometry otherGeo, out IGeometry geoResult);
        void InvalidateObject(IGeometry geometry);
        void ClearAllDisplay();
    }
}
