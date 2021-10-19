using System;
using FrameWork.Map;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace FrameWork.Domain {
    public class Component : GeoObjBase, IPointEdit {

        #region Properties
        public double X {
            get {
                IPoint p = _feature.Shape as IPoint;
                return p.X; 
            }
            set {
                IPoint p = _feature.Shape as IPoint;
                p.X = value; 
            }
        }

        public double Y {
            get {
                IPoint p = _feature.Shape as IPoint;
                return p.Y;
            }
            set {
                IPoint p = _feature.Shape as IPoint;
                p.Y = value;
            }
        }

        public int ID {
            get { return _feature.OID; }
        }

        public IPoint Geometry {
            get { return _feature.ShapeCopy as IPoint; }
            set { _feature.Shape = value; }
        }

        public IDisplayFeedback DrawFeedBack {
            get { return _displayFB; }
            set { _displayFB = value; }
        }
        
        #endregion

        #region Constructor
        public Component(IFeature feature) : base(feature) {}
        public Component() { }
        #endregion

        #region IPointEdit Members

        public void Move(double x, double y) {
            IPoint toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            _displayFB.MoveTo(toPoint);
        }

        public void Stop() {
            IMovePointFeedback movePointFB = _displayFB as IMovePointFeedback;
            _feature.Shape = movePointFB.Stop();
        }

        #endregion
    }
}
