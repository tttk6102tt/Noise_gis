using System;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace FrameWork.Map.Utilities
{
    public class Refresher
    {
        public static void RefreshCurrentView(IActiveView actiview, RefreshType refreshType)
        {
            IEnvelope env = actiview.ScreenDisplay.DisplayTransformation.FittedBounds;
            if (refreshType == RefreshType.ElementChanged)
            {
                actiview.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, env);
            }
            else if (refreshType == RefreshType.GeometryChanged)
            {
                actiview.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, env);
            }
            else if (refreshType == RefreshType.None)
            {
                actiview.PartialRefresh(esriViewDrawPhase.esriViewNone, null, env);
            }
            else if (refreshType == RefreshType.SelectionChanged)
            {
                actiview.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, env);
            }
        }

        public static void ClearMap(IMap map)
        {
            IActiveView actiview = map as IActiveView;
            IEnvelope env = actiview.ScreenDisplay.DisplayTransformation.FittedBounds;
            IGraphicsContainer graphCont = actiview as IGraphicsContainer;
            graphCont.DeleteAllElements();
            actiview.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, env);
            map.ClearSelection();
            actiview.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, env);
        }

        public static void ClearElement(IMap map)
        {
            IActiveView actiview = map as IActiveView;
            IEnvelope env = actiview.ScreenDisplay.DisplayTransformation.FittedBounds;
            IGraphicsContainer graphCont = actiview as IGraphicsContainer;
            graphCont.DeleteAllElements();
            actiview.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, env);
        }
    }
}
