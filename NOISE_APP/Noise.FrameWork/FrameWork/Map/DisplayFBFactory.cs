using System;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using FrameWork.Domain;
using FrameWork.Utilities;
namespace FrameWork.Map {
    public class DisplayFBFactory : IDisplayFBFactory {
        private IActiveView _activeView;
        public DisplayFBFactory(IActiveView activeView){
            _activeView=activeView;
        }

        #region IDisplayFBFactory Members

        public IDisplayFeedback GetMoveObjectFB(IGeometry geometry, IPoint point) {
            IDisplayFeedback displayFB = null;
            if (geometry.GeometryType == esriGeometryType.esriGeometryPoint) {
                displayFB = new MovePointFeedbackClass();
                displayFB.Display = _activeView.ScreenDisplay;
                IMovePointFeedback movePointFB = displayFB as IMovePointFeedback;
                movePointFB.Start(geometry as IPoint, point);
            } else if ((geometry.GeometryType == esriGeometryType.esriGeometryPolyline) || (geometry.GeometryType == esriGeometryType.esriGeometryLine)) {
                displayFB = new MoveLineFeedback();
                displayFB.Display = _activeView.ScreenDisplay;
                IMoveLineFeedback moveLineFB = displayFB as IMoveLineFeedback;
                moveLineFB.Start(geometry as IPolyline, point);
            }
            return displayFB;
        }

        public IDisplayFeedback GetLineMovePointFB(IGeometry geometry, IPoint point) {
            IDisplayFeedback displayFB = null;
            int vertexIndex = -1;
            int partIndex = -1;
            bool isRight = false;
            double tolerance = MapUtility.ConvertPixelsToMapUnits(4, _activeView);
            IHitTest hitTest = geometry as IHitTest;
            IPoint hitPoint = new PointClass();
            double distHit = -1;
            if (hitTest.HitTest(point, tolerance, esriGeometryHitPartType.esriGeometryPartVertex, hitPoint,
                ref distHit, ref partIndex, ref vertexIndex, ref isRight)) {
                displayFB = new LineMovePointFeedbackClass();
                displayFB.Display = _activeView.ScreenDisplay;
                ILineMovePointFeedback linempointFB = displayFB as ILineMovePointFeedback;
                linempointFB.Start(geometry as IPolyline, vertexIndex, point);
            }
            return displayFB;
        }

        #endregion
    }
}
