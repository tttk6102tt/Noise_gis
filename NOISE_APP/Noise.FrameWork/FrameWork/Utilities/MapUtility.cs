using System;
using System.Collections.Generic;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using FrameWork.Map;

namespace FrameWork.Utilities {
    public static class MapUtility {
        public static Layer GetLayerByAliasName(IMap map,string name){
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < map.LayerCount; i++) {
                stkLayers.Push(map.get_Layer(i));
            }
            while (stkLayers.Count > 0) {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer) {
                    IFeatureClass fc = ((IFeatureLayer)layer).FeatureClass;
                    if (fc.AliasName == name) {
                        return new Layer(layer as IFeatureLayer);
                    }
                } else if (layer is ICompositeLayer) {
                    ICompositeLayer cpLayer = layer as ICompositeLayer;
                    for (int i = 0; i < cpLayer.Count; i++) {
                        stkLayers.Push(cpLayer.get_Layer(i));
                    }
                }
            }
            return null;
        }

        public static IGeometry CreateQueryPoint(IPoint point,IActiveView actView) {
            ITopologicalOperator tp = point as ITopologicalOperator;
            return tp.Buffer(ConvertPixelsToMapUnits(4, actView));
        }

        public static IColor GetRGBColor(int red, int green, int blue) {
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = red;
            rgbColor.Green = green;
            rgbColor.Blue = blue;
            rgbColor.UseWindowsDithering = false;
            return rgbColor;
        }

        public static double ConvertPixelsToMapUnits(double pixelUnits,IActiveView activeView) {
            IPoint p1 = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperLeft;
            IPoint p2 = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperRight;
            int x1, x2, y1, y2;
            activeView.ScreenDisplay.DisplayTransformation.FromMapPoint(p1, out x1, out y1);
            activeView.ScreenDisplay.DisplayTransformation.FromMapPoint(p2, out x2, out y2);
            double pixelExtent = x2 - x1;
            double realWorldDisplayExtent = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return pixelUnits * sizeOfOnePixel;
        }

        public static IRendererConfiguration GetSimpleConfiguration(int red, int green, int blue, string text,double size,LayerType type) {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point) {
                ISimpleMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            } else if (type == LayerType.Line) {
                ISimpleLineSymbol line = new SimpleLineSymbolClass();
                line.Color = GetRGBColor(red,green,blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            } else if (type == LayerType.Polygon) {
                ISimpleFillSymbol fill = new SimpleFillSymbolClass();
                fill.Color = GetRGBColor(red, green, blue);
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetClassConfiguration(double value,int red, int green, int blue, string text, double size, LayerType type) {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point) {
                ISimpleMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            } else if (type == LayerType.Line) {
                ISimpleLineSymbol line = new SimpleLineSymbolClass();
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            } else if (type == LayerType.Polygon) {
                ISimpleFillSymbol fill = new SimpleFillSymbolClass();
                fill.Color = GetRGBColor(red, green, blue);
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            renderConfig.Value = value;
            return renderConfig;
        }
    }
}
