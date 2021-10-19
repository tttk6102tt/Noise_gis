using System;
using System.Collections.Generic;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using FrameWork.Core.MapInterfaces;

namespace FrameWork.Map.Objects
{
    public class Layer : ILayerRenderer
    {
        #region Fields
        private IFeatureLayer _flayer;
        private IGeoFeatureLayer _geoflayer;
        private List<IRendererConfiguration> _configurations;
        private LayerType _type;
        private string _rendererField;
        private ISymbol _defaultSymbol;
        private string _defaultLabel;
        private ISymbol _defaultBackgound;
        private double _minValueClassBreak;
        #endregion

        #region Properties
        public IFeatureLayer FeatureLayer
        {
            get { return _flayer; }
            set { _flayer = value; }
        }
        public IFeatureClass FeatureClass
        {
            get { return _flayer.FeatureClass; }
        }
        public string RendererField
        {
            get { return _rendererField; }
            set { _rendererField = value; }
        }
        public ISymbol DefaultSymbol {
            get { return _defaultSymbol; }
            set { _defaultSymbol = value; }
        }
        public ISymbol DefaultBackgound
        {
            get { return _defaultBackgound; }
            set { _defaultBackgound = value; }
        }
        public string DefaultLabel {
            get { return _defaultLabel; }
            set { _defaultLabel = value; }
        }
        public double MinValueClassBreak {
            get { return _minValueClassBreak; }
            set { _minValueClassBreak = value; }
        }
        public List<IRendererConfiguration> Configurations
        {
            get { return _configurations; }
            set { _configurations = value; }
        }
        public LayerType Type
        {
            get { return _type; }
        }
        #endregion

        #region Constructor
        public Layer() { }
        public Layer(IFeatureLayer fl)
        {
            _flayer = fl;
            _configurations = new List<IRendererConfiguration>();
            _geoflayer = fl as IGeoFeatureLayer;
            switch (fl.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryMultipoint:
                    _type = LayerType.Point;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryLine:
                    _type = LayerType.Line;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                case esriGeometryType.esriGeometryCircularArc:
                case esriGeometryType.esriGeometryEllipticArc:
                    _type = LayerType.Polygon;
                    break;
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Private functions
        #endregion

        #region ILayerRenderer Members

        public void Simple()
        {
            if (_configurations.Count > 0)
            {
                ISimpleRenderer simple = new SimpleRendererClass();
                simple.Symbol = _configurations[0].Symbol;
                simple.Label = _configurations[0].Text;
                _geoflayer.Renderer = simple as IFeatureRenderer;
            }
        }

        public void UniqueValue(bool ShowDefaultSymbol)
        {
            if (_configurations.Count > 0)
            {
                IUniqueValueRenderer unique = new UniqueValueRendererClass();
                unique.FieldCount = 1;
                unique.set_Field(0, _rendererField);
                unique.DefaultLabel = _defaultLabel;
                if (_defaultSymbol != null)
                {
                    unique.DefaultSymbol = _defaultSymbol;
                    if (ShowDefaultSymbol)
                        unique.UseDefaultSymbol = true;
                }
                else
                    unique.UseDefaultSymbol = false;
                for (int i = 0; i < _configurations.Count; i++)
                {
                    unique.AddValue(_configurations[i].TextValue, null, _configurations[i].Symbol);
                    unique.set_Label(_configurations[i].TextValue, _configurations[i].Text);
                    //unique.set_Symbol(_configurations[0].Text, _configurations[0].Symbol);
                }
                _geoflayer.Renderer = unique as IFeatureRenderer;
                
            }
        }

        public void Classify()
        {
            if (_configurations.Count > 0)
            {
                IClassBreaksRenderer classify = new ClassBreaksRendererClass();
                classify.Field = _rendererField;
                classify.BreakCount = _configurations.Count;
                classify.MinimumBreak = _minValueClassBreak;
                if (_defaultBackgound != null)
                    classify.BackgroundSymbol = _defaultBackgound as IFillSymbol;
                for (int i = 0; i < _configurations.Count; i++)
                {
                    classify.set_Break(i, _configurations[i].Value);
                    classify.set_Label(i, _configurations[i].Text);
                    classify.set_Symbol(i, _configurations[i].Symbol);
                }
                _geoflayer.Renderer = classify as IFeatureRenderer;
            }
        }
        public void PieChart(double sizeBar, bool disPlay3D)
        {
            if (_configurations.Count > 0)
            {
                IChartRenderer pChartRenderer = new ChartRendererClass();
                IPieChartSymbol pPieChartSymbol = new PieChartSymbolClass();
                ISymbolArray pSymbolArray = (ISymbolArray)pPieChartSymbol;
                IRendererFields pRendererFields = (IRendererFields)pChartRenderer;
                for (int i = 0; i < _configurations.Count; i++)
                {
                    pSymbolArray.AddSymbol(_configurations[i].Symbol);
                    pRendererFields.AddField(_configurations[i].TextValue, _configurations[i].Text);
                }
                IMarkerSymbol pMarkerSymbol = (IMarkerSymbol)pPieChartSymbol;
                pMarkerSymbol.Size = sizeBar;
                I3DChartSymbol p3DChart = (I3DChartSymbol)pPieChartSymbol;
                p3DChart.Display3D = disPlay3D;
                if (_defaultBackgound != null)
                    pChartRenderer.BaseSymbol = _defaultBackgound;
                pChartRenderer.UseOverposter = false;
                pChartRenderer.ChartSymbol = (IChartSymbol)pPieChartSymbol;
                pChartRenderer.CreateLegend();
                _geoflayer.Renderer = pChartRenderer as IFeatureRenderer;
            }
        }

        public void BarChart(double sizeBar, bool disPlay3D, double maxValue)
        {
            if (_configurations.Count > 0)
            {
                IChartRenderer pChartRenderer = new ChartRendererClass();
                IBarChartSymbol pBarChartSymbol = new BarChartSymbolClass();
                ISymbolArray pSymbolArray = (ISymbolArray)pBarChartSymbol;
                IRendererFields pRendererFields = (IRendererFields)pChartRenderer;
                for (int i = 0; i < _configurations.Count; i++)
                {
                    pSymbolArray.AddSymbol(_configurations[i].Symbol);
                    pRendererFields.AddField(_configurations[i].TextValue, _configurations[i].Text);
                }
                IMarkerSymbol pMarkerSymbol = (IMarkerSymbol)pBarChartSymbol;
                pMarkerSymbol.Size = sizeBar;
                I3DChartSymbol p3DChart = (I3DChartSymbol)pBarChartSymbol;
                p3DChart.Display3D = disPlay3D;
                if (_defaultBackgound != null)
                    pChartRenderer.BaseSymbol = _defaultBackgound;
                pChartRenderer.UseOverposter = false;
                pChartRenderer.ChartSymbol = (IChartSymbol)pBarChartSymbol;
                pChartRenderer.ChartSymbol.MaxValue = maxValue;
                pChartRenderer.CreateLegend();
                //pChartRenderer.Label = "AAAAAAA";
                _geoflayer.Renderer = pChartRenderer as IFeatureRenderer;
            }
        }

        public void DotDensity()
        {
            //if (_configurations.Count > 0) {
            //    IDotDensityFillSymbol dotdensity = new DotDensityRendererClass();
            //    IDotDensityFillSymbol dotdensityfill = new DotDensityFillSymbolClass();

            //}
        }

        #endregion
    }
}
