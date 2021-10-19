using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;

namespace FrameWork.Domain {
    public abstract class GeoObjectBase {

        #region Fields
        protected int _id;
        protected IFeature _feature;
        protected IDisplayFeedback _dispFeedback;
        protected IGeometry _geometry;
        #endregion

        #region Properties
        public int ID {
            get { return _id; }
        }
        public IGeometry Geometry {
            get { return _geometry; }
            set { _geometry = value; }
        }
        public IDisplayFeedback FeedBack {
            get { return _dispFeedback; }
            set { _dispFeedback = value; }
        }
        #endregion

        #region Constructor
        public GeoObjectBase() {
        }
        public GeoObjectBase(IFeature feature) {
            _feature = feature;
            _id = _feature.OID;
            _geometry = _feature.Shape;
        }
        public bool SetValue(string field, object val) {
            int index = _feature.Fields.FindField(field);
            if (index >= 0) {
                _feature.set_Value(_feature.Fields.FindField(field), val);
                return true;
            }
            return false;
        }
        public object GetValue(string field) {
            int index = _feature.Fields.FindField(field);
            if (index >= 0) {
                _feature.get_Value(_feature.Fields.FindField(field));
            }
            return null;
        }
        public void Save() {
            try {
                _feature.Store();
            } catch (Exception ex) {
                string str = ex.Message;
            }
        }
        public void Delete() {
            _feature.Delete();
        }
        public void Move(IPoint point) {
            if (_dispFeedback != null) {
                _dispFeedback.MoveTo(point);
            }
        }
        public void Stop() {
            if (_dispFeedback is IMovePointFeedback) {
                IMovePointFeedback pointFB = _dispFeedback as IMovePointFeedback;
                _feature.Shape = pointFB.Stop();
            } else if (_dispFeedback is IMoveLineFeedback) {
                IMoveLineFeedback lineFB = _dispFeedback as IMoveLineFeedback;
                _feature.Shape = lineFB.Stop();
            } else if (_dispFeedback is ILineMovePointFeedback) {
                ILineMovePointFeedback linepointFB = _dispFeedback as ILineMovePointFeedback;
                _feature.Shape = linepointFB.Stop();
            }
        }
        #endregion
    }
}
