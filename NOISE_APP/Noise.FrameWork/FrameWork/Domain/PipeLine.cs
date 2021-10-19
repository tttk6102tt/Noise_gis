using System;
using FrameWork.Map;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace FrameWork.Domain {

    public class PipeLine : GeoObjBase,ILineEdit {
        #region Fields
        private Component _node1,_node2;
        #endregion


        #region Properties
        public Component Node1 {
            get { return _node1; }
            set { _node1 = value; }
        }
        public Component Node2 {
            get { return _node2; }
            set { _node2 = value; }
        }
        public int ID {
            get { return _feature.OID; }
        }
        public IDisplayFeedback DrawFeedBack {
            get { return _displayFB; }
            set { _displayFB = value; }
        }
        public IPolyline Geometry {
            get { return _feature.ShapeCopy as IPolyline; }
            set { _feature.Shape = value; }
        }
        #endregion

        #region Constructor
        public PipeLine(IFeature feature) : base(feature) {}
        public PipeLine() { }
        #endregion

        #region ILineEdit Members

        public void Move(double x, double y) {
            IPoint toPoint = new PointClass();
            toPoint.PutCoords(x,y);
            _displayFB.MoveTo(toPoint);
        }

        public void Stop() {
            if (_displayFB is IMoveLineFeedback) {
                IMoveLineFeedback moveLineFB = _displayFB as IMoveLineFeedback;
                _feature.Shape = moveLineFB.Stop();
            } else if (_displayFB is ILineMovePointFeedback) {
                ILineMovePointFeedback moveLinePointFB = _displayFB as ILineMovePointFeedback;
                _feature.Shape = moveLinePointFB.Stop();
            } else if (_displayFB is INewLineFeedback) {
                INewLineFeedback newLineFB = _displayFB as INewLineFeedback;
                _feature.Shape = newLineFB.Stop();
            }
        }

        public void Add(double x, double y) {
            if (_displayFB is INewLineFeedback) {
                IPoint addPoint = new PointClass();
                addPoint.PutCoords(x, y);
                INewLineFeedback newLineFB = _displayFB as INewLineFeedback;
                newLineFB.AddPoint(addPoint);
            }
        }

        #endregion
    }
}
