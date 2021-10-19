using System;
using System.Collections.Generic;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;

namespace FrameWork.Map {
    public class Layer : ILayerRenderer {
        #region Fields
        private IFeatureLayer _flayer;
        private IGeoFeatureLayer _geoflayer;
        private List<IRendererConfiguration> _configurations;
        private LayerType _type;
        private string _rendererField;
        #endregion

        #region Properties
        public IFeatureLayer FeatureLayer {
            get { return _flayer; }
            set { _flayer = value; }
        }
        public IFeatureClass FeatureClass {
            get { return _flayer.FeatureClass; }
        }
        public string RendererField {
            get { return _rendererField; }
            set { _rendererField = value; }
        }
        public List<IRendererConfiguration> Configurations {
            get { return _configurations; }
            set { _configurations = value; }
        }
        public LayerType Type {
            get { return _type; }
        }
        #endregion

        #region Constructor
        public Layer() { }
        public Layer(IFeatureLayer fl) {
            _flayer = fl;
            _configurations = new List<IRendererConfiguration>();
            _geoflayer = fl as IGeoFeatureLayer;
            if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint) {
                _type = LayerType.Point;
            } else if ((fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryLine) || (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)) {
                _type = LayerType.Line;
            } else if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon) {
                _type = LayerType.Polygon;
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Private functions
        #endregion


        #region ILayerRenderer Members

        public void Simple() {
            if (_configurations.Count > 0) {
                ISimpleRenderer simple = new SimpleRendererClass();
                simple.Symbol = _configurations[0].Symbol;
                simple.Label = _configurations[0].Text;
                _geoflayer.Renderer = simple as IFeatureRenderer;
            }
        }

        public void UniqueValue() {
            if (_configurations.Count > 0) {
                IUniqueValueRenderer unique = new UniqueValueRendererClass();
                unique.FieldCount = 1;
                unique.set_Field(0, _rendererField);
                unique.DefaultLabel = _configurations[0].Text;
                unique.DefaultSymbol = _configurations[0].Symbol;
                unique.UseDefaultSymbol = true;
                for (int i = 1; i < _configurations.Count; i++) {
                    unique.AddValue(_configurations[i].Text, _configurations[i].Text, _configurations[0].Symbol);
                    unique.set_Label(_configurations[i].Text, _configurations[i].Text);
                    unique.set_Symbol(_configurations[0].Text, _configurations[0].Symbol);
                }
                _geoflayer.Renderer = unique as IFeatureRenderer;
            }
        }

        public void Classify() {
            if (_configurations.Count > 0) {
                IClassBreaksRenderer classify = new ClassBreaksRendererClass();
                classify.Field = _rendererField;
                classify.BreakCount = _configurations.Count;
                for (int i = 0; i < _configurations.Count; i++) {
                    classify.set_Break(i, _configurations[i].Value);
                    classify.set_Label(i, _configurations[i].Text);
                    classify.set_Symbol(i, _configurations[i].Symbol);
                }
                _geoflayer.Renderer = classify as IFeatureRenderer;
            }
        }

        public void DotDensity() {
            //if (_configurations.Count > 0) {
            //    IDotDensityFillSymbol dotdensity = new DotDensityRendererClass();
            //    IDotDensityFillSymbol dotdensityfill = new DotDensityFillSymbolClass();

            //}
        }

        #endregion
    }
}
