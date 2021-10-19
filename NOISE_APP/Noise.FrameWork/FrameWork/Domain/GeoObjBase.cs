using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
namespace FrameWork.Domain {
    public class GeoObjBase {
        protected IFeature _feature;
        protected IDisplayFeedback _displayFB;

        public GeoObjBase(IFeature feature) {
            _feature = feature;
        }
        public GeoObjBase() { }
        public void Save() {
            _feature.Store();
        }

        public void Delete() {
            _feature.Delete();
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
    }
}
