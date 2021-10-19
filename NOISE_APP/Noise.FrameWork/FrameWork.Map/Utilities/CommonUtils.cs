using System;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using FrameWork.Core.MapInterfaces;
using System.Drawing;

namespace FrameWork.Map.Utilities
{
    public static class CommonUtils
    {
        public static void GetRGBColor(ISymbol pSymbol, esriGeometryType geometryType, out int Red, out int Green, out int Blue)
        {
            Red = 0; Green = 0; Blue = 0;
            IColor pColor = new RgbColorClass();
            IRgbColor pRGBColor = null;
            switch (geometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryMultipoint:
                    IMarkerSymbol pMarkerSymbol = (IMarkerSymbol)pSymbol;
                    pColor.RGB = pMarkerSymbol.Color.RGB;
                    pRGBColor = (IRgbColor)pColor;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryLine:
                    ILineSymbol pLineSymbol = (ILineSymbol)pSymbol;
                    pColor.RGB = pLineSymbol.Color.RGB;
                    pRGBColor = (IRgbColor)pColor;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                case esriGeometryType.esriGeometryCircularArc:
                case esriGeometryType.esriGeometryEllipticArc:
                    IFillSymbol pFillSymbol = (IFillSymbol)pSymbol;
                    pColor.RGB = pFillSymbol.Color.RGB;
                    pRGBColor = (IRgbColor)pColor;
                    break;
            }
            if (pRGBColor != null)
            {
                Red = pRGBColor.Red;
                Green = pRGBColor.Green;
                Blue = pRGBColor.Blue;
            }
        }

        public static void GetRGBColor(ISymbol pSymbol, LayerType type, out int Red, out int Green, out int Blue)
        {
            Red = 0; Green = 0; Blue = 0;
            IColor pColor = new RgbColorClass();
            IRgbColor pRGBColor = null;
            switch (type)
            {
                case LayerType.Point:
                    IMarkerSymbol pMarkerSymbol = (IMarkerSymbol)pSymbol;
                    pColor.RGB = pMarkerSymbol.Color.RGB;
                    pRGBColor = (IRgbColor)pColor;
                    break;
                case LayerType.Line:
                    ILineSymbol pLineSymbol = (ILineSymbol)pSymbol;
                    pColor.RGB = pLineSymbol.Color.RGB;
                    pRGBColor = (IRgbColor)pColor;
                    break;
                case LayerType.Polygon:
                    IFillSymbol pFillSymbol = (IFillSymbol)pSymbol;
                    pColor.RGB = pFillSymbol.Color.RGB;
                    pRGBColor = (IRgbColor)pColor;
                    break;
            }
            if (pRGBColor != null)
            {
                Red = pRGBColor.Red;
                Green = pRGBColor.Green;
                Blue = pRGBColor.Blue;
            }
        }

        public static IRgbColor GetRGBColor(int yourRed, int yourGreen, int yourBlue)
        {
            ESRI.ArcGIS.Display.IRgbColor pRGB = new ESRI.ArcGIS.Display.RgbColorClass();
            pRGB.Red = yourRed;
            pRGB.Green = yourGreen;
            pRGB.Blue = yourBlue;
            pRGB.UseWindowsDithering = true;
            return pRGB;
        }

        public static IColor GetIColor(int red, int green, int blue)
        {
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = red;
            rgbColor.Green = green;
            rgbColor.Blue = blue;
            rgbColor.UseWindowsDithering = false;
            return rgbColor;
        }

        public static IColor GetIColor(string hexColor)
        {
            IRgbColor rgbColor = new RgbColorClass();

            try
            {
                Color color = ColorTranslator.FromHtml(hexColor);
                //
                rgbColor.Red = color.R;
                rgbColor.Green = color.G;
                rgbColor.Blue = color.B;
                rgbColor.UseWindowsDithering = false;
            }
            catch
            {

            }
            return rgbColor;
        }

        public static IColor GetIColor(Color color)
        {
            IRgbColor rgbColor = new RgbColorClass();

            try
            {
                rgbColor.Red = color.R;
                rgbColor.Green = color.G;
                rgbColor.Blue = color.B;
                rgbColor.UseWindowsDithering = false;
            }
            catch
            {

            }
            return rgbColor;
        }

        public static IColor GetRandomColor()
        {
            IRgbColor rgbColor = new RgbColorClass();
            Random r = new Random();
            rgbColor.Red = r.Next(0, 255);
            rgbColor.Green = r.Next(0, 255);
            rgbColor.Blue = r.Next(0, 255);
            rgbColor.UseWindowsDithering = false;
            return rgbColor;
        }

        public static string GetStyleClass(esriGeometryType geometryType)
        {
            string styleClass = "";
            switch (geometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryMultipoint:
                    styleClass = "Marker Symbols";
                    break;
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryLine:
                    styleClass = "Line Symbols";
                    break;
                case esriGeometryType.esriGeometryPolygon:
                case esriGeometryType.esriGeometryCircularArc:
                case esriGeometryType.esriGeometryEllipticArc:
                    styleClass = "Fill Symbols";
                    break;
            }
            return styleClass;
        }

        public static string GetStyleClass(LayerType type)
        {
            string styleClass = "";
            switch (type)
            {
                case LayerType.Point:
                    styleClass = "Marker Symbols";
                    break;
                case LayerType.Line:
                    styleClass = "Line Symbols";
                    break;
                case LayerType.Polygon:
                    styleClass = "Fill Symbols";
                    break;
            }
            return styleClass;
        }

        public static ISymbol UpdateMarkerSymbol(ISymbol symbol, double size)
        {
            try
            {
                IMarkerSymbol pMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                pMarkerSymbol = (IMarkerSymbol)symbol;
                pMarkerSymbol.Size = size;
                return (ISymbol)pMarkerSymbol;
            }
            catch
            {
                return null;
            }
        }

        public static ISymbol UpdateMarkerSymbol(ISymbol symbol, IColor color)
        {
            try
            {
                IMarkerSymbol pMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                pMarkerSymbol = (IMarkerSymbol)symbol;
                pMarkerSymbol.Color = color;
                return (ISymbol)pMarkerSymbol;
            }
            catch
            {
                return null;
            }
        }

        public static ISymbol UpdateMarkerSymbol(ISymbol symbol, IColor color, double size)
        {
            try
            {
                IMarkerSymbol pMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                pMarkerSymbol = (IMarkerSymbol)symbol;
                pMarkerSymbol.Color = color;
                pMarkerSymbol.Size = size;
                return (ISymbol)pMarkerSymbol;
            }
            catch
            {
                return null;
            }
        }

        public static IRendererConfiguration GetSimpleConfiguration(string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker.Color = GetRandomColor();
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line.Color = GetRandomColor();
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill.Color = GetRandomColor();
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetSimpleConfiguration(string text, string textvalue, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker.Color = GetRandomColor();
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line.Color = GetRandomColor();
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill.Color = GetRandomColor();
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.TextValue = textvalue;
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetSimpleConfiguration(int red, int green, int blue, string text, string textvalue, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill.Color = GetRGBColor(red, green, blue);
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.TextValue = textvalue;
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetSimpleConfiguration(ISymbol symbol, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker = (IMarkerSymbol)symbol;
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line = (ILineSymbol)symbol;
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                renderConfig.Symbol = symbol;
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetSimpleConfiguration(ISymbol symbol, int red, int green, int blue, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker = (IMarkerSymbol)symbol;
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line = (ILineSymbol)symbol;
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill = (IFillSymbol)symbol;
                fill.Color = GetRGBColor(red, green, blue);
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetSimpleConfiguration(ISymbol symbol, int red, int green, int blue, int outlineRed, int outlineGreen,
            int outlineBlue, double outlineWith, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker = (IMarkerSymbol)symbol;
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line = (ILineSymbol)symbol;
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill = (IFillSymbol)symbol;
                fill.Color = GetRGBColor(red, green, blue);
                ICartographicLineSymbol outLine = new CartographicLineSymbolClass();
                outLine.Color = GetRGBColor(outlineRed, outlineGreen, outlineBlue);
                outLine.Width = outlineWith;
                fill.Outline = outLine;
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetClassConfiguration(double value, int red, int green, int blue, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill.Color = GetRGBColor(red, green, blue);
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            renderConfig.Value = value;
            return renderConfig;
        }

        public static IRendererConfiguration GetClassConfiguration(int red, int green, int blue, string textvalue, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill.Color = GetRGBColor(red, green, blue);
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.TextValue = textvalue;
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetClassConfiguration(ISymbol symbol, double value, int red, int green, int blue,
            string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker = (IMarkerSymbol)symbol;
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line = (ILineSymbol)symbol;
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill = (IFillSymbol)symbol;
                fill.Color = GetRGBColor(red, green, blue);
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            renderConfig.Value = value;
            return renderConfig;
        }

        public static IRendererConfiguration GetClassConfiguration(ISymbol symbol, int red, int green, int blue,
            string textvalue, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker = (IMarkerSymbol)symbol;
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line = (ILineSymbol)symbol;
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill = (IFillSymbol)symbol;
                fill.Color = GetRGBColor(red, green, blue);
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.TextValue = textvalue;
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }

        public static IRendererConfiguration GetClassConfiguration(ISymbol symbol, double value, int red, int green, int blue, int outlineRed,
            int outlineGreen, int outlineBlue, double outlineWith, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker = (IMarkerSymbol)symbol;
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line = (ILineSymbol)symbol;
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill = (IFillSymbol)symbol;
                fill.Color = GetRGBColor(red, green, blue);
                ICartographicLineSymbol outLine = new CartographicLineSymbolClass();
                outLine.Color = GetRGBColor(outlineRed, outlineGreen, outlineBlue);
                outLine.Width = outlineWith;
                fill.Outline = outLine;
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.Text = text;
            renderConfig.Size = size;
            renderConfig.Value = value;
            return renderConfig;
        }
        public static IRendererConfiguration GetClassConfiguration(ISymbol symbol, string textvalue, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker = (IMarkerSymbol)symbol;
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line = (ILineSymbol)symbol;
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                renderConfig.Symbol = symbol;
            }
            renderConfig.TextValue = textvalue;
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }
        public static IRendererConfiguration GetClassConfiguration(ISymbol symbol, int red, int green, int blue, int outlineRed,
            int outlineGreen, int outlineBlue, double outlineWith, string textvalue, string text, double size, LayerType type)
        {
            IRendererConfiguration renderConfig = new RendererConfiguration();
            if (type == LayerType.Point)
            {
                IMarkerSymbol marker = new SimpleMarkerSymbolClass();
                marker = (IMarkerSymbol)symbol;
                marker.Color = GetRGBColor(red, green, blue);
                marker.Size = size;
                renderConfig.Symbol = marker as ISymbol;
            }
            else if (type == LayerType.Line)
            {
                ILineSymbol line = new SimpleLineSymbolClass();
                line = (ILineSymbol)symbol;
                line.Color = GetRGBColor(red, green, blue);
                line.Width = size;
                renderConfig.Symbol = line as ISymbol;
            }
            else if (type == LayerType.Polygon)
            {
                IFillSymbol fill = new SimpleFillSymbolClass();
                fill = (IFillSymbol)symbol;
                fill.Color = GetRGBColor(red, green, blue);
                ICartographicLineSymbol outLine = new CartographicLineSymbolClass();
                outLine.Color = GetRGBColor(outlineRed, outlineGreen, outlineBlue);
                outLine.Width = outlineWith;
                fill.Outline = outLine;
                renderConfig.Symbol = fill as ISymbol;
            }
            renderConfig.TextValue = textvalue;
            renderConfig.Text = text;
            renderConfig.Size = size;
            return renderConfig;
        }
    }
}
