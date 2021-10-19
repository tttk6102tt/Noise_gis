using System;
using System.Drawing;
using FrameWork.Core.MapInterfaces;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace FrameWork.Map.Objects
{
    public class GeometryManipulation : IGeometryManipulation, IDisposable
    {

        #region Fields
        private IMap _map;
        private IActiveView _activeView;
        private IDisplayFeedback _displayFB;
        private IGraphicsContainer _graphicCont;
        #endregion

        #region Constructor
        public GeometryManipulation() { }
        public GeometryManipulation(IMap map)
        {
            _map = map;
            _activeView = _map as IActiveView;
            _graphicCont = map as IGraphicsContainer;
        }
        #endregion

        #region IGeometryManipulation Members

        public IPoint CreatePointAction(double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            return p;
        }

        public void StartCreateNewLineAction(double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            _displayFB = new NewLineFeedbackClass();
            _displayFB.Display = _activeView.ScreenDisplay;
            INewLineFeedback newLineFB = _displayFB as INewLineFeedback;
            newLineFB.Start(p);
        }

        public IPolyline EndCreateNewLineAction()
        {
            IPolyline newPolyLine = null;
            if (_displayFB is INewLineFeedback)
            {
                INewLineFeedback newLineFB = _displayFB as INewLineFeedback;
                newPolyLine = newLineFB.Stop();
                _displayFB = null;
            }
            return newPolyLine;
        }

        public void MoveTo(double x, double y)
        {
            if (_displayFB != null)
            {
                IPoint p = new PointClass();
                p.PutCoords(x, y);
                _displayFB.MoveTo(p);
            }
        }

        public void MoveTo(double x, double y, out bool isMove)
        {
            isMove = false;
            if (_displayFB != null)
            {
                IPoint p = new PointClass();
                p.PutCoords(x, y);
                _displayFB.MoveTo(p);
                isMove = true;
            }
        }

        public void Add(double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            if (_displayFB is INewLineFeedback)
            {
                INewLineFeedback newLineFB = _displayFB as INewLineFeedback;
                newLineFB.AddPoint(p);
            }
            else if (_displayFB is INewPolygonFeedback)
            {
                INewPolygonFeedback newPolygonFB = _displayFB as INewPolygonFeedback;
                newPolygonFB.AddPoint(p);
            }
        }

        public void StartMovePointAction(IPoint inputPoint, double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            _displayFB = new MovePointFeedbackClass();
            _displayFB.Display = _activeView.ScreenDisplay;
            IMovePointFeedback movePointFB = _displayFB as IMovePointFeedback;
            movePointFB.Start(inputPoint, p);
        }

        public IPoint EndMovePointAction()
        {
            IPoint newPoint = null;
            if (_displayFB is IMovePointFeedback)
            {
                IMovePointFeedback movePointFB = _displayFB as IMovePointFeedback;
                newPoint = movePointFB.Stop();
                _displayFB = null;
            }
            return newPoint;
        }

        public void StartMoveLineAction(IPolyline inputLine, double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            _displayFB = new MoveLineFeedback();
            _displayFB.Display = _activeView.ScreenDisplay;
            IMoveLineFeedback moveLineFB = _displayFB as IMoveLineFeedback;
            moveLineFB.Start(inputLine, p);
        }

        public IPolyline EndMoveLineAction()
        {
            IPolyline newLine = null;
            if (_displayFB is IMoveLineFeedback)
            {
                IMoveLineFeedback moveLineFB = _displayFB as IMoveLineFeedback;
                newLine = moveLineFB.Stop();
                _displayFB = null;
            }
            return newLine;
        }

        public void StartEditLineAction(IPolyline inputLine, int pointIndex, double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            _displayFB = new LineMovePointFeedbackClass();
            _displayFB.Display = _activeView.ScreenDisplay;
            ILineMovePointFeedback lineMovePoint = _displayFB as ILineMovePointFeedback;
            lineMovePoint.Start(inputLine, pointIndex, p);
        }

        public IPolyline EndEditLineAction()
        {
            IPolyline newLine = null;
            if (_displayFB is ILineMovePointFeedback)
            {
                ILineMovePointFeedback lineMovePoint = _displayFB as ILineMovePointFeedback;
                newLine = lineMovePoint.Stop();
                _displayFB = null;
            }
            return newLine;
        }

        public void StartCreatePolygonAction(double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            _displayFB = new NewPolygonFeedbackClass();
            _displayFB.Display = _activeView.ScreenDisplay;
            INewPolygonFeedback newPolygonFB = _displayFB as INewPolygonFeedback;
            newPolygonFB.Start(p);
        }

        public IPolygon EndCreatePolygonAction()
        {
            IPolygon newPolygon = null;
            if (_displayFB is INewPolygonFeedback)
            {
                INewPolygonFeedback newPolygonFB = _displayFB as INewPolygonFeedback;
                newPolygon = newPolygonFB.Stop();
                _displayFB = null;
            }
            return newPolygon;
        }

        public void StartMovePolygonAction(IPolygon inputPolygon, double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            _displayFB = new MovePolygonFeedback();
            _displayFB.Display = _activeView.ScreenDisplay;
            IMovePolygonFeedback movePolygonFB = _displayFB as IMovePolygonFeedback;
            movePolygonFB.Start(inputPolygon, p);
        }

        public IPolygon EndMovePolygonAction()
        {
            IPolygon newPolygon = null;
            if (_displayFB is IMovePolygonFeedback)
            {
                IMovePolygonFeedback movePolygonFB = _displayFB as IMovePolygonFeedback;
                newPolygon = movePolygonFB.Stop();
                _displayFB = null;
            }
            return newPolygon;
        }

        public void StartEditPolygonAction(IPolygon inputPolygon, int pointIndex, double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            _displayFB = new PolygonMovePointFeedback();
            _displayFB.Display = _activeView.ScreenDisplay;
            IPolygonMovePointFeedback polygonMovePoint = _displayFB as IPolygonMovePointFeedback;
            polygonMovePoint.Start(inputPolygon, pointIndex, p);
        }

        public IPolygon EndEditPolygonAction()
        {
            IPolygon newPolygon = null;
            if (_displayFB is IPolygonMovePointFeedback)
            {
                IPolygonMovePointFeedback polygonMovePoint = _displayFB as IPolygonMovePointFeedback;
                newPolygon = polygonMovePoint.Stop();
                _displayFB = null;
            }
            return newPolygon;
        }

        public bool SplitLineByPoint(IPolyline inputLine, IPoint splitPoint, out IPolyline line1, out IPolyline line2)
        {
            line1 = null;
            line2 = null;
            double length = inputLine.Length;
            double longCurve = 0;
            double distanceFromCurve = 0;
            bool isRight = false;
            IPoint outPoint = new PointClass();
            ICurve fromToPoint = null;
            ICurve pointToEnd = null;
            inputLine.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, splitPoint, true,
                                            outPoint, ref longCurve, ref distanceFromCurve, ref isRight);
            if ((longCurve == 0) || (longCurve == 1)) return false;
            inputLine.GetSubcurve(0, longCurve, true, out fromToPoint);
            inputLine.GetSubcurve(longCurve, 1, true, out pointToEnd);
            line1 = fromToPoint as IPolyline;
            line2 = pointToEnd as IPolyline;
            return true;
        }

        public bool SplitPolygonByLine(IPolygon inputPolygon, IPolyline splitLine, out IPolygon polygon1, out IPolygon polygon2)
        {
            polygon1 = null;
            polygon2 = null;
            IGeometry leftPart = null;
            IGeometry rightPart = null;
            try
            {
                ITopologicalOperator2 topoOperator = inputPolygon as ITopologicalOperator2;
                topoOperator.IsKnownSimple_2 = false;
                topoOperator.Simplify();
                topoOperator.Cut(splitLine, out leftPart, out rightPart);
            }
            catch
            {
                return false;
            }
            if ((leftPart == null) || (rightPart == null)) return false;
            polygon1 = leftPart as IPolygon;
            polygon2 = rightPart as IPolygon;
            return true;
        }

        public bool ClipPolygonGetIntersect(IPolygon inputPolygon, IPolygon intersectPolygon, out IPolygon polygonResult)
        {
            polygonResult = null;
            IGeometry geoResult;
            try
            {
                ITopologicalOperator2 topoOperator = inputPolygon as ITopologicalOperator2;
                topoOperator.IsKnownSimple_2 = false;
                topoOperator.Simplify();
                geoResult = topoOperator.Intersect(intersectPolygon, esriGeometryDimension.esriGeometry2Dimension);
            }
            catch
            {
                return false;
            }
            if (geoResult == null) return false;
            polygonResult = geoResult as IPolygon;
            return true;
        }
        public bool ClipPolygonGetDifference(IPolygon inputPolygon, IPolygon intersectPolygon, out IPolygon polygonResult)
        {
            polygonResult = null;
            IGeometry geoResult;
            try
            {
                ITopologicalOperator2 topoOperator = inputPolygon as ITopologicalOperator2;
                topoOperator.IsKnownSimple_2 = false;
                topoOperator.Simplify();
                geoResult = topoOperator.Difference(intersectPolygon);
            }
            catch
            {
                return false;
            }
            if (geoResult == null) return false;
            polygonResult = geoResult as IPolygon;
            return true;
        }
        public bool UnionGeometry(IGeometry inputGeo, IGeometry otherGeo, out IGeometry geoResult)
        {
            geoResult = null;
            try
            {
                ITopologicalOperator2 topoOperator = inputGeo as ITopologicalOperator2;
                topoOperator.IsKnownSimple_2 = false;
                topoOperator.Simplify();
                geoResult = topoOperator.Union(otherGeo);
            }
            catch
            {
                return false;
            }
            if (geoResult == null) return false;
            return true;
        }

        public void ClearAllDisplay()
        {
            //IGraphicsContainerSelect graphContSel = _map as IGraphicsContainerSelect;
            //graphContSel.SelectAllElements();
            //_activeView.Refresh();
            //IGraphicsContainer graphCont = graphContSel as IGraphicsContainer;
            IGraphicsContainer graphCont = _activeView as IGraphicsContainer;
            graphCont.DeleteAllElements();
        }

        public void InvalidateObject(IGeometry geometry)
        {
            IInvalidArea invalidArea = new InvalidAreaClass();
            invalidArea.Display = _activeView.ScreenDisplay;
            invalidArea.Add(geometry);
            invalidArea.Invalidate((short)esriScreenCache.esriNoScreenCache);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _map = null;
            _activeView = null;
        }

        #endregion
    }
}
