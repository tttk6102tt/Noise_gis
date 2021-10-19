using System;
using ESRI.ArcGIS.Geometry;

namespace FrameWork.Core.MapInterfaces {
    public struct MouseEventInfo
    {
        public int x;
        public int y;
        public double mapX;
        public double mapY;
        public int shift;
    }
    public enum IdentifyStatus
    {
        None,
        All,
        TopMostLayer,
        VisibleLayer,
        SelectableLayer,
        Layer
    }
    public enum BaseMapAction : int
    {
        None = -1,
        Select = 0,
        SelectByLocation = 1,
        ZoomIn = 2,
        ZoomOut = 3,
        Pan = 4,
        Identify = 5,
        Distance = 6,
        CreatePoint = 7,
        EditPoint = 8,
        DeletePoint = 9,
        CreateLine = 10,
        EditLine = 11,
        SplitLine = 12,
        MergeLine = 13,
        DeleteLine = 14,
        CreatePolygon = 15,
        EditPolygon = 16,
        DeletePolygon = 17,
        DelInPolygon = 18,
        DelOutPolygon = 19,
        UnionPolygon = 20,
        SplitPolygon = 21,
        EditObject = 22
    }


    public enum BaseMapAction_Layout : int
    {
        None = -1,
        FullExtent = 1,
        FixedZoomIn = 2,
        FixedZoomOut = 3,
        BackExtent = 4,
        NextExtent = 5,
        Refresh = 6
    }


    public interface IToolbarAction : IDisposable{
        void ZoomIn(double x,double y);
        void ZoomOut(double x, double y);
        IGeometry Selection(double x, double y);
        void FixZoomIn();
        void FixZoomOut();
        void Pan();
        void Select();
        void FullExtent();
        void NextAction();
        void BackAction();
        void MouseDownAction(MouseEventInfo me);
        void MouseMoveAction(MouseEventInfo me);
        void MouseMoveAction(MouseEventInfo me, ref double ms, ref double total, ref string unit);
        void MouseUpAction(MouseEventInfo me);
        void DoubleClickAction(MouseEventInfo me);
        BaseMapAction MapAction { get; set; }
        string SetCale(string oldval, string newval);
    }
}
