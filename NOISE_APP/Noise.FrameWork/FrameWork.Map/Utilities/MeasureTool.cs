using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace FrameWork.Map.Utilities {
    public class MeasureTool {
        public MeasureTool() {
        }
        public MeasureTool(IMap map) {
            pActiveView = map as IActiveView;
        }
        #region  Coding by Ducbb in WWMS Hue
        #region Variable List for Measure
        public bool m_bInUse;
        private ILineSymbol m_pLineSymbol;
        private IPolyline m_pLinePolyline;
        private IPoint m_pStartPoint;
        public IActiveView pActiveView;
        private Double dblTotal, dbCurrentTotal;
        public esriUnits m_pUnit = esriUnits.esriMeters;
        #endregion
        #region Functions for measure distance
        public double ChangeUnit(double inValue, esriUnits outUnit) {
            IUnitConverter pUnitConvert = new UnitConverterClass();
            double outValue = pUnitConvert.ConvertUnits(inValue, pActiveView.FocusMap.MapUnits, outUnit);
            //			dbMeasure = pUnitConvert.ConvertUnits(distance, pDefaultUnit, pCurrentUnitDis);
            m_pUnit = outUnit;
            return outValue;
        }

        public void MouseDown(int x, int y) {
            if (!m_bInUse) {
                m_bInUse = true;
                m_pLinePolyline = null;
                m_pLineSymbol = null;
                dblTotal = 0;
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            } else {
                if (m_pLineSymbol == null)
                    return;
                dblTotal = dbCurrentTotal;
                //Draw measure line and text
                pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC, -1);
                pActiveView.ScreenDisplay.SetSymbol((ISymbol)m_pLineSymbol);
                if (m_pLinePolyline.Length > 0)
                    pActiveView.ScreenDisplay.DrawPolyline(m_pLinePolyline);
                pActiveView.ScreenDisplay.FinishDrawing();
            }
            m_pStartPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
        }

        public void MouseDownSnap(double x, double y) {
            if (!m_bInUse) {
                m_bInUse = true;
                m_pLinePolyline = null;
                m_pLineSymbol = null;
                dblTotal = 0;
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            } else {
                if (m_pLineSymbol == null)
                    return;
                dblTotal = dbCurrentTotal;
                //Draw measure line and text
                pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC, -1);
                pActiveView.ScreenDisplay.SetSymbol((ISymbol)m_pLineSymbol);
                if (m_pLinePolyline.Length > 0)
                    pActiveView.ScreenDisplay.DrawPolyline(m_pLinePolyline);
                pActiveView.ScreenDisplay.FinishDrawing();
            }
            IPoint p = new PointClass();
            p.PutCoords(x, y);
            m_pStartPoint = p;
        }

        public bool MouseMove(int x, int y, ref Double dbMeasure, ref Double dbTotal) {
            if (!m_bInUse)
                return false;

            //for measure
            bool bfirstTime = false;
            if (m_pLineSymbol == null)
                bfirstTime = true;

            //Get current point
            ESRI.ArcGIS.Geometry.IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

            pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC, -1);

            if (bfirstTime) {
                IRgbColor pRGBColor;
                ISymbol pSymbol;
                //Line Symbol
                m_pLineSymbol = new SimpleLineSymbolClass();
                m_pLineSymbol.Width = 1.5;
                pRGBColor = new RgbColorClass();
                pRGBColor.Red = 255;
                pRGBColor.Green = 0;
                pRGBColor.Blue = 0;
                m_pLineSymbol.Color = pRGBColor;
                pSymbol = (ISymbol)m_pLineSymbol;
                pSymbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPXOrPen;

            } else {
                //Use existing symbols and draw existing text and polyline
                pActiveView.ScreenDisplay.SetSymbol((ISymbol)m_pLineSymbol);
                if (m_pLinePolyline.Length > 0)
                    pActiveView.ScreenDisplay.DrawPolyline(m_pLinePolyline);
            }

            ILine pLine = new Line();
            //Get line between from and to points
            pLine.PutCoords(m_pStartPoint, pPoint);

            //For drawing text, get text(distance), angle, and point
            Double deltaX, deltaY, distance;
            deltaX = pPoint.X - m_pStartPoint.X;
            deltaY = pPoint.Y - m_pStartPoint.Y;
            distance = Math.Round(Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)), 5);
            dbCurrentTotal = dblTotal + distance;
            dbTotal = dbCurrentTotal;
            dbMeasure = distance;

            IPolyline pPolyLine = new PolylineClass();
            //ISegmentCollection pSegColl;
            ISegmentCollection m_pSegmentCollection = (ISegmentCollection)pPolyLine;
            ISegment segm = (ISegment)pLine;
            m_pSegmentCollection.InsertSegments(0, 1, ref segm);
            m_pLinePolyline = GetSmashedLine(pActiveView.ScreenDisplay, pPolyLine);

            //Draw polyline
            pActiveView.ScreenDisplay.SetSymbol((ISymbol)m_pLineSymbol);
            if (m_pLinePolyline.Length > 0)
                pActiveView.ScreenDisplay.DrawPolyline(m_pLinePolyline);

            pActiveView.ScreenDisplay.FinishDrawing();
            //			dbMeasure = ChangeUnit(dbMeasure, m_pUnit);
            //			dbTotal = ChangeUnit(dbTotal, m_pUnit);
            //			pActiveView.FocusMap.MapUnits = m_pUnit;
            return true;
        }

        public bool MouseMoveSnap(double x, double y, ref double dbMeasure, ref double dbTotal) {
            if (!m_bInUse)
                return false;

            //for measure
            bool bfirstTime = false;
            if (m_pLineSymbol == null)
                bfirstTime = true;

            //Get current point
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(x, y);

            pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC, -1);

            if (bfirstTime) {
                IRgbColor pRGBColor;
                ISymbol pSymbol;
                //Line Symbol
                m_pLineSymbol = new SimpleLineSymbolClass();
                m_pLineSymbol.Width = 1.5;
                pRGBColor = new RgbColorClass();
                pRGBColor.Red = 255;
                pRGBColor.Green = 0;
                pRGBColor.Blue = 0;
                m_pLineSymbol.Color = pRGBColor;
                pSymbol = (ISymbol)m_pLineSymbol;
                pSymbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPXOrPen;

            } else {
                //Use existing symbols and draw existing text and polyline
                pActiveView.ScreenDisplay.SetSymbol((ISymbol)m_pLineSymbol);
                if (m_pLinePolyline.Length > 0)
                    pActiveView.ScreenDisplay.DrawPolyline(m_pLinePolyline);
            }

            ILine pLine = new Line();
            //Get line between from and to points
            pLine.PutCoords(m_pStartPoint, pPoint);

            //For drawing text, get text(distance), angle, and point
            Double deltaX, deltaY, distance;
            deltaX = pPoint.X - m_pStartPoint.X;
            deltaY = pPoint.Y - m_pStartPoint.Y;
            distance = Math.Round(Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)), 5);
            dbCurrentTotal = dblTotal + distance;
            dbTotal = dbCurrentTotal;
            dbMeasure = distance;

            IPolyline pPolyLine = new PolylineClass();
            //ISegmentCollection pSegColl;
            ISegmentCollection m_pSegmentCollection = (ISegmentCollection)pPolyLine;
            ISegment segm = (ISegment)pLine;
            m_pSegmentCollection.InsertSegments(0, 1, ref segm);
            m_pLinePolyline = GetSmashedLine(pActiveView.ScreenDisplay, pPolyLine);

            //Draw polyline
            pActiveView.ScreenDisplay.SetSymbol((ISymbol)m_pLineSymbol);
            if (m_pLinePolyline.Length > 0)
                pActiveView.ScreenDisplay.DrawPolyline(m_pLinePolyline);

            pActiveView.ScreenDisplay.FinishDrawing();
            //			dbMeasure = ChangeUnit(dbMeasure, m_pUnit);
            //			dbTotal = ChangeUnit(dbTotal, m_pUnit);
            //			pActiveView.FocusMap.MapUnits = m_pUnit;
            return true;
        }

        public void MouseUp(int x, int y) {
            //			if(!m_bInUse)
            //				return;
            //
            //			if(m_pLineSymbol == null) 
            //				return;
            //
            //			//Draw measure line and text
            //			pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC,-1);
            //			pActiveView.ScreenDisplay.SetSymbol((ISymbol)m_pLineSymbol);
            //			if(m_pLinePolyline.Length > 0) 
            //				pActiveView.ScreenDisplay.DrawPolyline(m_pLinePolyline);
            //			pActiveView.ScreenDisplay.FinishDrawing();
        }

        public void Mouse_DoubleClick() {
            if (!m_bInUse)
                return;
            m_bInUse = false;
            //Draw measure line and text
            pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC, -1);
            pActiveView.ScreenDisplay.SetSymbol((ISymbol)m_pLineSymbol);
            if (m_pLinePolyline.Length > 0)
                pActiveView.ScreenDisplay.DrawPolyline(m_pLinePolyline);
            pActiveView.ScreenDisplay.FinishDrawing();
        }

        private IPolyline GetSmashedLine(IScreenDisplay pDisplay, IPolyline pPolyLine) {
            //Returns a Polyline with a blank space for the text to go in
            //IPolyline pSmashed;
            IPolygon pBoundary;
            pBoundary = new PolygonClass();
            //pTextSymbol.QueryBoundary(pDisplay.hDC, pDisplay.DisplayTransformation, pPoint, pBoundary);

            ITopologicalOperator pTopo;
            pTopo = (ITopologicalOperator)pBoundary;
            IPolyline pIntersect;
            pIntersect = (IPolyline)pTopo.Intersect(pPolyLine, esriGeometryDimension.esriGeometry1Dimension);
            pTopo = (ITopologicalOperator)pPolyLine;
            return (IPolyline)pTopo.Difference(pIntersect);
        }

        #endregion
        #endregion

    }
}
