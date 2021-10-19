using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using FrameWork.Core.MapInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace FrameWork.Map.Utilities
{
    public class SnapAgent : ISnapAgen
    {
        private bool m_bCacheHasCreated;
        private bool m_bIsUsed = false;
        private IActiveView m_pActiveView;
        private IAnchorPoint m_pAnchorPoint;
        private IFeature m_pFeature;
        private ILayer pLayer;
        private IFeatureCache2 m_pFeatureCache;
        private IMap m_pMap;
        private IPoint m_pSnapPoint;
        private Dictionary<string, SnapInfo> _listOfLayers;
        private IDisplay _display;
        public SnapAgent(IMap pMap)
        {
            this.SetMap(pMap);
            this.m_pAnchorPoint = new AnchorPointClass();
            ISimpleMarkerSymbol symbol = new SimpleMarkerSymbolClass();
            symbol.Color = FrameWork.Map.Utilities.CommonUtils.GetIColor(0, 0xff, 0xff);
            symbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            symbol.Outline = true;
            symbol.OutlineColor = FrameWork.Map.Utilities.CommonUtils.GetIColor(0, 0, 0);
            this.m_pAnchorPoint.Symbol = symbol as ISymbol;
            this.m_bCacheHasCreated = false;
            _listOfLayers = new Dictionary<string, SnapInfo>();
            _display = m_pActiveView.ScreenDisplay;
        }

        public void AddLayerInList(ILayer layer)
        {
            if (layer is IFeatureLayer)
            {
                IFeatureLayer fl = layer as IFeatureLayer;
                SnapInfo si = new SnapInfo();
                si.LayerName = fl.FeatureClass.AliasName;
                si.Layer = layer;
                _listOfLayers.Add(si.LayerName, si);
            }
        }

        public void AddLayerInList(string name, ILayer layer)
        {
            if (layer is IFeatureLayer)
            {
                if (!_listOfLayers.ContainsKey(name))
                {
                    SnapInfo si = new SnapInfo();
                    si.LayerName = name;
                    si.Layer = layer;
                    _listOfLayers.Add(si.LayerName, si);
                }
            }
        }
        private string GetStantardLayerName(string name)
        {
            int index = name.LastIndexOf(".") + 1;
            if (index >= 0)
            {
                name = name.Substring(index, name.Length - index);
            }
            return name;
        }
        public void AddLayerInList(IMap map)
        {
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < map.LayerCount; i++)
            {
                stkLayers.Push(map.get_Layer(i));
            }
            while (stkLayers.Count > 0)
            {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer)
                {
                    string name = GetStantardLayerName(((IFeatureLayer)layer).FeatureClass.AliasName);
                    if (!_listOfLayers.ContainsKey(name))
                    {
                        SnapInfo si = new SnapInfo();
                        si.LayerName = name;
                        si.Layer = layer;
                        _listOfLayers.Add(si.LayerName, si);
                    }
                }
                else if (layer is ICompositeLayer)
                {
                    ICompositeLayer cpLayer = layer as ICompositeLayer;
                    for (int i = 0; i < cpLayer.Count; i++)
                    {
                        stkLayers.Push(cpLayer.get_Layer(i));
                    }
                }
            }
        }
        private void CreateFeatureCache()
        {
            this.m_pFeatureCache = new FeatureCacheClass();
            this.m_bCacheHasCreated = true;
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

        private void FillCache(IPoint point, double size, IFeatureClass pFeatureClass)
        {
            if (!this.m_bCacheHasCreated)
            {
                this.CreateFeatureCache();
            }
            if (!this.m_pFeatureCache.Contains(point))
            {
                this.m_pFeatureCache.Initialize(point, size);
                this.m_pFeatureCache.AddFeatures(pFeatureClass, m_pActiveView.Extent);
            }
        }

        private void FillCache(IFeatureClass pFeatureClass)
        {
            if (!this.m_bCacheHasCreated)
            {
                this.CreateFeatureCache();
            }
            IPoint p = new PointClass();
            p.PutCoords((m_pActiveView.Extent.LowerLeft.X + m_pActiveView.Extent.UpperRight.X) / 2,
                        (m_pActiveView.Extent.LowerLeft.Y + m_pActiveView.Extent.UpperRight.Y) / 2);
            if (!this.m_pFeatureCache.Contains(p))
            {
                this.m_pFeatureCache.Initialize(p, 300);
                this.m_pFeatureCache.AddFeatures(pFeatureClass, m_pActiveView.Extent);
            }
        }

        private IFeature getFeature(IPoint Point, double dSearch, ILayer Layer)
        {
            IFeatureLayer layer = Layer as IFeatureLayer;
            if (layer == null)
            {
                return null;
            }
            IFeatureClass featureClass = layer.FeatureClass;
            if (featureClass == null)
            {
                return null;
            }
            double distance = FrameWork.Map.Utilities.MapUtility.ConvertPixelsToMapUnits(this.m_pActiveView, dSearch);
            IGeometry envelope = Point;
            ITopologicalOperator @operator = (ITopologicalOperator)envelope;
            envelope = @operator.Buffer(distance).Envelope;
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = envelope;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            filter.GeometryField = featureClass.ShapeFieldName;
            IQueryFilter queryFilter = filter;
            IFeatureCursor o = layer.Search(queryFilter, false);
            IFeature feature = o.NextFeature();
            Marshal.ReleaseComObject(o);
            return feature;
        }

        private IFeatureCursor getFeatureCursor(IPoint Point, double dSearch, ILayer Layer)
        {
            IFeatureLayer layer = Layer as IFeatureLayer;
            if (layer == null)
            {
                return null;
            }
            IFeatureClass featureClass = layer.FeatureClass;
            if (featureClass == null)
            {
                return null;
            }
            double distance = FrameWork.Map.Utilities.MapUtility.ConvertPixelsToMapUnits(this.m_pActiveView, dSearch);
            IGeometry envelope = Point;
            ITopologicalOperator @operator = (ITopologicalOperator)envelope;
            envelope = @operator.Buffer(distance).Envelope;
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = envelope;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            filter.GeometryField = featureClass.ShapeFieldName;
            IQueryFilter queryFilter = filter;
            return layer.Search(queryFilter, false);
        }

        //public static SnapAgent GetInstance(IMap pMap, ArrayList pListLayer)
        //{
        //    if (m_instance == null)
        //    {
        //        m_instance = new SnapAgent(pMap, pListLayer);
        //    }
        //    return m_instance;
        //}

        private IPoint getSnapPoint(IPoint queryPoint, double dSearch, IGeometry geometry, esriGeometryHitPartType hitType)
        {
            double hitDistance = 0.0;
            int hitPartIndex = 0;
            int hitSegmentIndex = 0;
            bool bRightSide = false;
            double searchRadius = 0.0;
            IPoint hitPoint = new PointClass();
            IHitTest test = geometry as IHitTest;
            searchRadius = FrameWork.Map.Utilities.MapUtility.ConvertPixelsToMapUnits(this.m_pActiveView, 10.0);
            if (test.HitTest(queryPoint, searchRadius, hitType, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide))
            {
                this.m_pAnchorPoint.MoveTo(hitPoint, this.m_pActiveView.ScreenDisplay);
                return hitPoint;
            }
            return null;
        }

        private IPoint GetSnapPoint(IPoint queryPoint, double dSearch, IGeometry pGeometry, SnapInfo pSnapInfo)
        {
            double searchRadius = 0.0;
            IPoint hitPoint = new PointClass();
            double hitDistance = 0.0;
            int hitPartIndex = -1;
            int hitSegmentIndex = -1;
            bool bRightSide = false;
            IHitTest test = pGeometry as IHitTest;
            searchRadius = FrameWork.Map.Utilities.MapUtility.ConvertPixelsToMapUnits(this.m_pActiveView, dSearch);
            if (pSnapInfo.SnapEnd)
            {
                if (test.HitTest(queryPoint, searchRadius, esriGeometryHitPartType.esriGeometryPartEndpoint, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide))
                {
                    return hitPoint;
                }
            }
            if (pSnapInfo.SnapVertex)
            {
                if (test.HitTest(queryPoint, searchRadius, esriGeometryHitPartType.esriGeometryPartVertex, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide))
                {
                    return hitPoint;
                }
            }
            if (pSnapInfo.SnapEdge)
            {
                if (test.HitTest(queryPoint, searchRadius, esriGeometryHitPartType.esriGeometryPartBoundary, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide))
                {
                    return hitPoint;
                }
            }
            return null;
        }

        public void SetMap(IMap pMap)
        {
            this.m_pMap = pMap;
            this.m_pActiveView = this.m_pMap as IActiveView;
            //((IMapCache)this.m_pMap).ScaleLimit = true;
            //((IMapCache)this.m_pMap).MaxScale = 10000.0;
            //((IMapCache)this.m_pMap).AutoCacheActive = true;
        }
        //public void SetMap(IMap pMap, ArrayList pListLayer)
        //{
        //    this.m_pMap = pMap;
        //    this.m_pActiveView = this.m_pMap as IActiveView;
        //    for (int i = 0; i < pListLayer.Count; i++)
        //    {
        //        SnapInfo info = new SnapInfo();
        //        info.LayerIndex = i;
        //        this.m_pSnapInfoCollection.Add(info);
        //    }
        //    ((IMapCache)this.m_pMap).ScaleLimit = true;
        //    ((IMapCache)this.m_pMap).MaxScale = 10000.0;
        //    ((IMapCache)this.m_pMap).AutoCacheActive = true;
        //}
        public void Setting(string layerName, bool bVertex, bool bEdge, bool bEnd)
        {
            if (_listOfLayers.ContainsKey(layerName))
            {
                SnapInfo info = _listOfLayers[layerName];
                info.SnapEdge = bEdge;
                info.SnapEnd = bEnd;
                info.SnapVertex = bVertex;
            }
        }

        public void Setting(string layerName, ILayer layer, bool bVertex, bool bEdge, bool bEnd)
        {
            AddLayerInList(layerName, layer);
        }

        public void Reset()
        {
            foreach (KeyValuePair<string, SnapInfo> kvp in _listOfLayers)
            {
                SnapInfo pSnapInfo = kvp.Value;
                pSnapInfo.SnapEdge = false;
                pSnapInfo.SnapEnd = false;
                pSnapInfo.SnapVertex = false;
            }
        }

        public void Snap(int x, int y)
        {
            IPoint point = this.m_pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            IPoint point2 = null;
            this.m_pSnapPoint = point;
            if (this.m_bIsUsed)
            {
                foreach (KeyValuePair<string, SnapInfo> kvp in _listOfLayers)
                {
                    SnapInfo pSnapInfo = kvp.Value;
                    ILayer layer = pSnapInfo.Layer;
                    if (((pSnapInfo.SnapEdge || pSnapInfo.SnapEnd) || pSnapInfo.SnapVertex) && (layer != null))
                    {
                        IFeatureCursor cursor = this.getFeatureCursor(point, 17.0, layer);
                        if (cursor == null)
                        {
                            return;
                        }
                        for (IFeature feature = cursor.NextFeature(); feature != null; feature = cursor.NextFeature())
                        {
                            point2 = this.GetSnapPoint(point, 17.0, feature.Shape, pSnapInfo);
                            if (point2 != null)
                            {
                                this.m_pSnapPoint = point2;
                                break;
                            }
                        }
                    }
                }
                if (this.m_pSnapPoint != null)
                {
                    this.m_pAnchorPoint.MoveTo(this.m_pSnapPoint, this.m_pActiveView.ScreenDisplay);
                }
            }
        }

        public void Snap1(int x, int y, double tolerance)
        {
            IPoint point = this.m_pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            IPoint point2 = null;
            this.m_pFeature = null;
            this.m_pSnapPoint = point;
            if (this.m_bIsUsed)
            {
                foreach (KeyValuePair<string, SnapInfo> kvp in _listOfLayers)
                {
                    SnapInfo pSnapInfo = kvp.Value;
                    ILayer layer = pSnapInfo.Layer;
                    if ((pSnapInfo.SnapEdge || pSnapInfo.SnapEnd) || pSnapInfo.SnapVertex)
                    {
                        if (layer != null)
                        {
                            IFeatureLayer layer2 = layer as IFeatureLayer;
                            //this.FillCache(point, 2, layer2.FeatureClass);
                            //FillCache(layer2.FeatureClass);
                            pSnapInfo.InitCache(m_pActiveView, 300);
                            IFeature feature = null;
                            IGeometry shape = null;
                            //if (this.m_pFeatureCache.Count <= 30) {
                            for (int j = 0; j < pSnapInfo.Cache.Count; j++)
                            {
                                feature = pSnapInfo.Cache.get_Feature(j);
                                if (feature != null)
                                {
                                    shape = feature.Shape;
                                    point2 = this.GetSnapPoint(point, tolerance, feature.Shape, pSnapInfo);
                                    if (point2 != null)
                                    {
                                        this.m_pSnapPoint = point2;
                                        this.pLayer = layer;
                                        this.m_pFeature = feature;
                                        break;
                                    }
                                }
                            }
                            if (point2 != null)
                            {
                                break;
                            }
                            //}
                        }
                    }
                }
                if (this.m_pSnapPoint != null)
                {
                    this.m_pAnchorPoint.MoveTo(this.m_pSnapPoint, this.m_pActiveView.ScreenDisplay);
                }
            }
        }

        public void Init()
        {
            //if (this.m_bIsUsed) {
            foreach (KeyValuePair<string, SnapInfo> kvp in _listOfLayers)
            {
                SnapInfo pSnapInfo = kvp.Value;
                ILayer layer = pSnapInfo.Layer;
                if ((pSnapInfo.SnapEdge || pSnapInfo.SnapEnd) || pSnapInfo.SnapVertex)
                {
                    if (layer != null)
                    {
                        IFeatureLayer layer2 = layer as IFeatureLayer;
                        pSnapInfo.IsUpdated = false;
                        pSnapInfo.InitCache(m_pActiveView, 300);
                    }
                }
            }
            // }
        }

        public void UpdateCache()
        {
            if (this.m_bIsUsed)
            {
                foreach (KeyValuePair<string, SnapInfo> kvp in _listOfLayers)
                {
                    SnapInfo pSnapInfo = kvp.Value;
                    ILayer layer = pSnapInfo.Layer;
                    if ((pSnapInfo.SnapEdge || pSnapInfo.SnapEnd) || pSnapInfo.SnapVertex)
                    {
                        if (layer != null)
                        {
                            IFeatureLayer layer2 = layer as IFeatureLayer;
                            pSnapInfo.UpdateCache(m_pActiveView, 300);
                        }
                    }
                }
            }
        }

        //public void Snap1(int x, int y, double tolerance) {
        //    IPoint point = this.m_pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
        //    IPoint point2 = null;
        //    this.m_pFeature = null;
        //    this.m_pSnapPoint = point;
        //    if (this.m_bIsUsed) {
        //        foreach (KeyValuePair<string, SnapInfo> kvp in _listOfLayers) {
        //            SnapInfo pSnapInfo = kvp.Value;
        //            ILayer layer = pSnapInfo.Layer;
        //            if ((pSnapInfo.SnapEdge || pSnapInfo.SnapEnd) || pSnapInfo.SnapVertex) {
        //                if (layer != null) {
        //                    pSnapInfo.InitCache(point, tolerance);
        //                    IFeature feature = null;
        //                    IGeometry shape = null;
        //                    if (pSnapInfo.Cache.Count <= 30) {
        //                        for (int j = 0; j < pSnapInfo.Cache.Count; j++) {
        //                            feature = pSnapInfo.Cache.get_Feature(j);
        //                            if (feature != null) {
        //                                shape = feature.Shape;
        //                                point2 = this.GetSnapPoint(point, 17.0, feature.Shape, pSnapInfo);
        //                                if (point2 != null) {
        //                                    this.m_pSnapPoint = point2;
        //                                    this.pLayer = layer;
        //                                    this.m_pFeature = feature;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        if (point2 != null) {
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (this.m_pSnapPoint != null) {
        //            this.m_pAnchorPoint.MoveTo(this.m_pSnapPoint, this.m_pActiveView.ScreenDisplay);
        //        }
        //    }
        //}

        public IFeature SnapFeature
        {
            get
            {
                return this.m_pFeature;
            }
        }
        public ILayer pLayerCurrentSnap
        {
            get
            {
                return this.pLayer;
            }
        }
        public IPoint SnapPoint
        {
            get
            {
                return this.m_pSnapPoint;
            }
            set
            {
                this.m_pSnapPoint = value;
            }
        }

        public bool UsedSnap
        {
            get
            {
                return this.m_bIsUsed;
            }
            set
            {
                this.m_bIsUsed = value;
                if (this.m_bIsUsed)
                {
                    m_bCacheHasCreated = false;
                }
            }
        }

        public class SnapInfo
        {
            private bool m_bSnapCenter = false;
            private bool m_bSnapEdge = false;
            private bool m_bSnapEnd = false;
            private bool m_bSnapVertex = false;
            private int m_iLayerIndex = -1;
            private IFeatureCache2 _featureCache = null;
            private bool _isCacheCreated = false;
            private string _name;
            private ILayer _layer;
            private bool _isUpdated = false;

            public bool IsUpdated
            {
                set { _isUpdated = value; }
            }

            public ILayer Layer
            {
                get { return _layer; }
                set { _layer = value; }
            }

            public string LayerName
            {
                get { return _name; }
                set { _name = value; }
            }

            public IFeatureCache2 Cache
            {
                get
                {
                    return _featureCache;
                }
            }

            public int LayerIndex
            {
                get
                {
                    return this.m_iLayerIndex;
                }
                set
                {
                    this.m_iLayerIndex = value;
                }
            }

            public bool SnapCenter
            {
                get
                {
                    return this.m_bSnapCenter;
                }
                set
                {
                    this.m_bSnapCenter = value;
                }
            }

            public bool SnapEdge
            {
                get
                {
                    return this.m_bSnapEdge;
                }
                set
                {
                    this.m_bSnapEdge = value;
                }
            }

            public bool SnapEnd
            {
                get
                {
                    return this.m_bSnapEnd;
                }
                set
                {
                    this.m_bSnapEnd = value;
                }
            }

            public bool SnapVertex
            {
                get
                {
                    return this.m_bSnapVertex;
                }
                set
                {
                    this.m_bSnapVertex = value;
                }
            }

            public void InitCache(IActiveView activeView, double size)
            {
                if (_isUpdated)
                {
                    _isUpdated = false;
                    return;
                }
                if (!_isCacheCreated)
                {
                    _featureCache = new FeatureCacheClass();
                    _isCacheCreated = true;
                }
                IPoint point = new PointClass();
                point.PutCoords((activeView.Extent.LowerLeft.X + activeView.Extent.UpperRight.X) / 2,
                        (activeView.Extent.LowerLeft.Y + activeView.Extent.UpperRight.Y) / 2);
                if (!_featureCache.Contains(point))
                {
                    _featureCache.Initialize(point, size);
                    _featureCache.AddFeatures(((IFeatureLayer)_layer).FeatureClass, activeView.Extent);
                }
            }

            public void UpdateCache(IActiveView activeView, double size)
            {
                if (_featureCache == null)
                {
                    _featureCache = new FeatureCacheClass();
                }
                IPoint point = new PointClass();
                point.PutCoords((activeView.Extent.LowerLeft.X + activeView.Extent.UpperRight.X) / 2,
                        (activeView.Extent.LowerLeft.Y + activeView.Extent.UpperRight.Y) / 2);
                _featureCache.Initialize(point, size);
                _featureCache.AddFeatures(((IFeatureLayer)_layer).FeatureClass, activeView.Extent);
                _isUpdated = true;
            }
        }

        private class SnapInfoCollection : CollectionBase
        {
            public void Add(SnapAgent.SnapInfo value)
            {
                base.List.Add(value);
            }

            public bool Contains(SnapAgent.SnapInfo value)
            {
                return base.List.Contains(value);
            }

            public SnapAgent.SnapInfo Get(int index)
            {
                if ((index >= 0) && (index < base.List.Count))
                {
                    return (SnapAgent.SnapInfo)base.List[index];
                }
                return null;
            }

            public int IndexOf(SnapAgent.SnapInfo value)
            {
                return base.List.IndexOf(value);
            }

            public void Insert(int index, SnapAgent.SnapInfo value)
            {
                base.List.Insert(index, value);
            }

            public void Remove(SnapAgent.SnapInfo value)
            {
                base.List.Remove(value);
            }
        }
    }
}

