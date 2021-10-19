using System;
using System.Collections.Generic;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using FrameWork.Map.Objects;
using FrameWork.Core.MapInterfaces;
using System.Xml;
using ESRI.ArcGIS.Controls;

namespace FrameWork.Map.Utilities
{
    public enum EnumHitVertex
    {
        None = 0,
        Vertex = 1,
        NotVertex = 2,
        Center = 3
    }
    public class MapUtility : IDisposable
    {
        #region Fields
        private IMap _map;
        private IActiveView _activeView;
        #endregion

        #region Properties
        public IMap Map
        {
            get { return _map; }
            set
            {
                _map = value;
                _activeView = _map as IActiveView;
            }
        }
        #endregion

        #region Constructor
        public MapUtility(IMap map)
        {
            _map = map;
            _activeView = _map as IActiveView;
        }
        #endregion

        #region Methods

        public Layer GetLayerByAliasName(string name)
        {
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < _map.LayerCount; i++)
                stkLayers.Push(_map.get_Layer(i));
            while (stkLayers.Count > 0)
            {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer)
                {
                    IFeatureClass fc = ((IFeatureLayer)layer).FeatureClass;
                    if (fc.AliasName == name)
                        return new Layer(layer as IFeatureLayer);
                }
                else if (layer is ICompositeLayer)
                {
                    ICompositeLayer cpLayer = layer as ICompositeLayer;
                    for (int i = 0; i < cpLayer.Count; i++)
                        stkLayers.Push(cpLayer.get_Layer(i));
                }
            }
            return null;
        }
        public Layer GetLayerByStantardName(string name)
        {
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < _map.LayerCount; i++)
                stkLayers.Push(_map.get_Layer(i));
            while (stkLayers.Count > 0)
            {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer)
                {
                    IFeatureClass fc = ((IFeatureLayer)layer).FeatureClass;
                    if (GetStantardLayerName(fc.AliasName).Equals(name))
                        return new Layer(layer as IFeatureLayer);
                }
                else if (layer is ICompositeLayer)
                {
                    ICompositeLayer cpLayer = layer as ICompositeLayer;
                    for (int i = 0; i < cpLayer.Count; i++)
                        stkLayers.Push(cpLayer.get_Layer(i));
                }
            }
            return null;
        }
        public ILayer GetILayerByAliasName(string name)
        {
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < _map.LayerCount; i++)
                stkLayers.Push(_map.get_Layer(i));
            while (stkLayers.Count > 0)
            {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer)
                {
                    IFeatureClass fc = ((IFeatureLayer)layer).FeatureClass;
                    if (fc.AliasName == name)
                        return layer;
                }
                else if (layer is ICompositeLayer)
                {
                    ICompositeLayer cpLayer = layer as ICompositeLayer;
                    for (int i = 0; i < cpLayer.Count; i++)
                        stkLayers.Push(cpLayer.get_Layer(i));
                }
            }
            return null;
        }
        public ILayer GetILayerByStantardName(string name)
        {
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < _map.LayerCount; i++)
                stkLayers.Push(_map.get_Layer(i));
            while (stkLayers.Count > 0)
            {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer)
                {
                    IFeatureClass fc = ((IFeatureLayer)layer).FeatureClass;
                    if (GetStantardLayerName(fc.AliasName).Equals(name))
                        return layer;
                }
                else if (layer is ICompositeLayer)
                {
                    ICompositeLayer cpLayer = layer as ICompositeLayer;
                    for (int i = 0; i < cpLayer.Count; i++)
                        stkLayers.Push(cpLayer.get_Layer(i));
                }
            }
            return null;
        }

        public void SetSelectableByAliasName(string name)
        {
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < _map.LayerCount; i++)
            {
                stkLayers.Push(_map.get_Layer(i));
            }
            while (stkLayers.Count > 0)
            {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer)
                {
                    IFeatureClass fc = ((IFeatureLayer)layer).FeatureClass;
                    if (fc.AliasName == name)
                    {
                        ((IFeatureLayer)layer).Selectable = true;
                    }
                    else
                    {
                        ((IFeatureLayer)layer).Selectable = false;
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
        public void SetSelectableLayer(ILayer inlayer)
        {
            Stack<ILayer> stkLayers = new Stack<ILayer>();
            for (int i = 0; i < _map.LayerCount; i++)
            {
                stkLayers.Push(_map.get_Layer(i));
            }
            while (stkLayers.Count > 0)
            {
                ILayer layer = stkLayers.Pop();
                if (layer is IFeatureLayer)
                {
                    ((IFeatureLayer)layer).Selectable = false;

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
            if (inlayer is IFeatureLayer)
            {
                ((IFeatureLayer)inlayer).Selectable = true;

            }
        }
        public IGeometry CreateQueryPoint(IPoint point)
        {
            ITopologicalOperator tp = point as ITopologicalOperator;
            return tp.Buffer(ConvertPixelsToMapUnits(4));
        }

        public double ConvertPixelsToMapUnits(double pixelUnits)
        {
            IPoint p1 = _activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperLeft;
            IPoint p2 = _activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.UpperRight;
            int x1, x2, y1, y2;
            _activeView.ScreenDisplay.DisplayTransformation.FromMapPoint(p1, out x1, out y1);
            _activeView.ScreenDisplay.DisplayTransformation.FromMapPoint(p2, out x2, out y2);
            double pixelExtent = x2 - x1;
            double realWorldDisplayExtent = _activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return pixelUnits * sizeOfOnePixel;
        }
        public int GetVertextIndexFromPoint(IGeometry geometry, IPoint point)
        {
            int vertexIndex = -1;
            int partIndex = -1;
            bool isRight = false;
            double tolerance = ConvertPixelsToMapUnits(4);
            IHitTest hitTest = geometry as IHitTest;
            IPoint hitPoint = new PointClass();
            double distHit = -1;
            hitTest.HitTest(point, tolerance, esriGeometryHitPartType.esriGeometryPartVertex, hitPoint,
              ref distHit, ref partIndex, ref vertexIndex, ref isRight);
            return vertexIndex;
        }

        public EnumHitVertex HitVetex(IGeometry pGeom, IPoint pPoint, out int vertexIndex)
        {
            EnumHitVertex _hitVertex = EnumHitVertex.None;
            vertexIndex = -1;
            if (pGeom == null)
                return _hitVertex = EnumHitVertex.None; ;
            IPoint pHitPoint = null;
            double hitDist = 0;
            int partIndex = 0;
            int vertexOffset = 0;
            bool vertex = false;
            bool centerHit = false;

            // Tolerance in pixels for line hits
            double tol = ConvertPixelsToMapUnits(4);
            switch (pGeom.GeometryType)
            {

                case esriGeometryType.esriGeometryPolygon:
                    if (TestGeometryHit(tol, pPoint, pGeom, ref pHitPoint, ref hitDist, ref partIndex, ref vertexIndex, ref vertexOffset, ref vertex, ref centerHit))
                    {
                        if (!vertex)
                        {
                            if (centerHit == true)
                                _hitVertex = EnumHitVertex.Center;
                            else
                                _hitVertex = EnumHitVertex.Vertex;
                        }
                        else
                        {
                            _hitVertex = EnumHitVertex.NotVertex;
                        }
                    }
                    else
                    {
                        _hitVertex = EnumHitVertex.None;
                    }
                    break;
                case esriGeometryType.esriGeometryPoint:
                    if (TestGeometryHit(tol, pPoint, pGeom, ref pHitPoint, ref hitDist, ref partIndex, ref vertexIndex, ref vertexOffset, ref vertex, ref centerHit))
                    {
                        if (!vertex)
                        {
                            if (centerHit == true)
                                _hitVertex = EnumHitVertex.Center;
                            else
                                _hitVertex = EnumHitVertex.Vertex;
                        }
                        else
                        {
                            _hitVertex = EnumHitVertex.NotVertex;
                        }
                    }
                    else
                    {
                        _hitVertex = EnumHitVertex.None;
                    }
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    if (TestGeometryHitForPolyline(tol, pPoint, pGeom, ref pHitPoint, ref hitDist, ref partIndex, ref vertexIndex, ref vertexOffset, ref vertex, ref centerHit))
                    {
                        if (!vertex)
                        {
                            if (centerHit == true)
                                _hitVertex = EnumHitVertex.Center;
                            else
                                _hitVertex = EnumHitVertex.Vertex;
                        }
                        else
                        {
                            _hitVertex = EnumHitVertex.NotVertex;
                        }
                    }
                    else
                    {
                        _hitVertex = EnumHitVertex.None;
                    }
                    break;
            }
            return _hitVertex;
        }
        public IGeometry InsertVertexOnPipe(IGeometry geoMetry, IPoint inPoint)
        {
            IPolyline polyLine = geoMetry as IPolyline;
            if (polyLine == null)
            {
                return null;
            }
            double longCurve = 0;
            double distanceFromCurve = 0;
            bool isRight = false;
            bool isSplitHappend = false;
            int newPartIndex = -1;
            int newSegmentIndex = -1;
            IPoint outPoint = new PointClass();
            polyLine.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, inPoint, false,
                outPoint, ref longCurve, ref distanceFromCurve, ref isRight);
            if (longCurve != 0)
            {
                polyLine.SplitAtDistance(longCurve, false, false, out isSplitHappend, out newPartIndex, out newSegmentIndex);
            }
            return polyLine;

        }
        public IGeometry InsertVertexOnPolyGon(IGeometry geoMetry, IPoint inPoint)
        {
            IPolygon polyGon = geoMetry as IPolygon;
            if (polyGon == null)
            {
                return null;
            }
            double longCurve = 0;
            double distanceFromCurve = 0;
            bool isRight = false;
            bool isSplitHappend = false;
            int newPartIndex = -1;
            int newSegmentIndex = -1;
            IPoint outPoint = new PointClass();
            polyGon.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, inPoint, false,
                outPoint, ref longCurve, ref distanceFromCurve, ref isRight);
            if (longCurve != 0)
            {
                polyGon.SplitAtDistance(longCurve, false, false, out isSplitHappend, out newPartIndex, out newSegmentIndex);
            }
            return polyGon;
        }
        public IGeometry DeleteVertexOnPipe(IGeometry pGeometry, int verTexIndex)
        {
            if (pGeometry == null) return null;
            IPolyline polyLine = pGeometry as IPolyline;
            if (polyLine == null)
                return null;
            IPointCollection pointCollect = polyLine as IPointCollection;
            if (pointCollect == null)
                return null;
            try
            {
                pointCollect.RemovePoints(verTexIndex, 1);
                return polyLine;
            }
            catch
            {
                return null;
            }
        }
        public IGeometry DeleteVertexOnPolygon(IGeometry pGeometry, int verTexIndex)
        {
            if (pGeometry == null) return null;
            IPolygon polyGon = pGeometry as IPolygon;
            if (polyGon == null)
                return null;
            IPointCollection pointCollect = polyGon as IPointCollection;
            if (pointCollect == null)
                return null;
            if (pointCollect.PointCount <= 3)
                return polyGon;
            try
            {
                pointCollect.RemovePoints(verTexIndex, 1);
                return polyGon;
            }
            catch
            {
                return null;
            }
        }
        public IQueryFilter CreateQuery(string condition, string fields, IGeometry geometry)
        {
            IQueryFilter qry = null;
            if (geometry != null)
            {
                qry = new SpatialFilterClass();
                ISpatialFilter sf = qry as SpatialFilterClass;
                sf.GeometryField = "Shape";
                if (geometry is IPoint)
                {
                    sf.Geometry = CreateQueryPoint(geometry as IPoint);
                    sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                }
                else if (geometry is IPolyline)
                {
                    sf.Geometry = geometry;
                    sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                }
                else if (geometry is IPolygon)
                {
                    sf.Geometry = geometry;
                    sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                }
                if (fields.Length > 0)
                {
                    sf.SubFields = fields;
                }
            }
            else
            {
                qry = new QueryFilterClass();
                qry.WhereClause = condition;
                if (fields.Length > 0)
                {
                    qry.SubFields = fields;
                }
            }
            return qry;
        }
        public IList<JoinTable> GetJoinTableList(IFeatureLayer sourceLayer)
        {
            List<JoinTable> joinTableList = new List<JoinTable>();
            IDataLayer2 dataLayer = sourceLayer as IDataLayer2;
            IDisplayTable displayTable = dataLayer as IDisplayTable;
            IDisplayRelationshipClass displayRelationshipClass = sourceLayer as IDisplayRelationshipClass;
            if (displayTable.DisplayTable is IRelQueryTable)
            {
                ITable joinTables = displayTable.DisplayTable;
                while (joinTables is IRelQueryTable)
                {
                    IRelQueryTable relQueryTable = joinTables as IRelQueryTable;
                    ITable destinationTable = relQueryTable.DestinationTable;
                    IDataset dataset = destinationTable as IDataset;
                    IDatasetName datasetName = dataset.FullName as IDatasetName;
                    IWorkspaceName wsName = datasetName.WorkspaceName;
                    JoinTable joinTable = new JoinTable();
                    joinTable.Name = dataset.Name;
                    joinTable.OriginPK = relQueryTable.RelationshipClass.OriginPrimaryKey;
                    joinTable.OriginFK = relQueryTable.RelationshipClass.OriginForeignKey;
                    joinTableList.Add(joinTable);
                    joinTables = relQueryTable.SourceTable;
                }
            }
            return joinTableList;
        }
        public IPoint GetConstructionPointLength(double length, IPoint fromPoint, IPoint inputPoint)
        {
            inputPoint.ConstrainDistance(length, fromPoint);
            return inputPoint;
        }
        public IPoint GetConstructionPointAngle(double angle, IPoint fromPoint, IPoint inputPoint)
        {
            inputPoint.ConstrainAngle((2.0 * angle) * 0.0087266462599716477, fromPoint, true);
            return inputPoint;
        }
        public void MoveExtent(int mapWidth, int mapHeight, int x, int y)
        {
            int autoPanTolerancePixel = 20;
            int scrollWidth = 0;
            if (_activeView.ShowScrollBars)
                scrollWidth = 20;

            if ((x < autoPanTolerancePixel)
                || (y < autoPanTolerancePixel)
                || (x > (mapWidth - autoPanTolerancePixel - scrollWidth))
                || (y > (mapHeight - autoPanTolerancePixel - scrollWidth))
                )
            {
                IScreenDisplay screenDisplay = _activeView.ScreenDisplay;

                IPoint pnt1 = _activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                IPoint pnt2 = new PointClass();

                IProximityOperator proximityOp = (IProximityOperator)_activeView.Extent;
                proximityOp.QueryNearestPoint(pnt1, esriSegmentExtension.esriNoExtension, pnt2);

                screenDisplay.PanStart(pnt2);
                screenDisplay.PanMoveTo(pnt1);
                screenDisplay.PanStop();
                _activeView.Refresh();
            }
        }
        public IAOIBookmark AddBookMark(string name)
        {
            IAOIBookmark aoiBookmark = new AOIBookmarkClass();
            aoiBookmark.Location = _activeView.Extent;
            aoiBookmark.Name = name;
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            mapBookmarks.AddBookmark(aoiBookmark);
            return aoiBookmark;
        }
        public void DeleteAllBookMarks()
        {
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            mapBookmarks.RemoveAllBookmarks();
        }
        public void DeleteBookMark(string name)
        {
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            mapBookmarks.Bookmarks.Reset();
            IEnumSpatialBookmark enumSpatialBM = mapBookmarks.Bookmarks;
            IAOIBookmark aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            while (aoiBookmark != null)
            {
                if (aoiBookmark.Name == name)
                {
                    mapBookmarks.RemoveBookmark(aoiBookmark);
                    return;
                }
                aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            }
        }
        public void SaveBookMarks(string path)
        {
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            mapBookmarks.Bookmarks.Reset();
            IEnumSpatialBookmark enumSpatialBM = mapBookmarks.Bookmarks;
            IAOIBookmark aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Bookmarks");
                while (aoiBookmark != null)
                {
                    writer.WriteStartElement("Bookmark");

                    writer.WriteAttributeString("Name", aoiBookmark.Name);
                    writer.WriteAttributeString("MinX", aoiBookmark.Location.XMin.ToString());
                    writer.WriteAttributeString("MinY", aoiBookmark.Location.YMin.ToString());
                    writer.WriteAttributeString("MaxX", aoiBookmark.Location.XMax.ToString());
                    writer.WriteAttributeString("MaxY", aoiBookmark.Location.YMax.ToString());

                    writer.WriteEndElement();
                    aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        public void LoadBookMarks(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return;
            }
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.HasAttributes)
                        {
                            IAOIBookmark aoiBookmark = new AOIBookmarkClass();
                            IEnvelope env = new EnvelopeClass();
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                if (reader.Name == "Name")
                                {
                                    aoiBookmark.Name = reader.Value;
                                }
                                else if (reader.Name == "MinX")
                                {
                                    env.XMin = double.Parse(reader.Value);
                                }
                                else if (reader.Name == "MinY")
                                {
                                    env.YMin = double.Parse(reader.Value);
                                }
                                else if (reader.Name == "MaxX")
                                {
                                    env.XMax = double.Parse(reader.Value);
                                }
                                else if (reader.Name == "MaxY")
                                {
                                    env.YMax = double.Parse(reader.Value);
                                }

                            }
                            aoiBookmark.Location = env;
                            mapBookmarks.AddBookmark(aoiBookmark);
                        }
                    }
                }
            }
        }

        public IAOIBookmark GetBookMark(string name)
        {
            IAOIBookmark item = null;
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            mapBookmarks.Bookmarks.Reset();
            IEnumSpatialBookmark enumSpatialBM = mapBookmarks.Bookmarks;
            IAOIBookmark aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            while (aoiBookmark != null)
            {
                if (aoiBookmark.Name == name)
                {
                    item = aoiBookmark;
                }
                aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            }
            return item;
        }

        public List<IAOIBookmark> GetBookMarks()
        {
            List<IAOIBookmark> bookMarks = new List<IAOIBookmark>();
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            mapBookmarks.Bookmarks.Reset();
            IEnumSpatialBookmark enumSpatialBM = mapBookmarks.Bookmarks;
            IAOIBookmark aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            while (aoiBookmark != null)
            {
                bookMarks.Add(aoiBookmark);
                aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            }
            return bookMarks;
        }
        public void ZoomToBookMark(string name)
        {
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            mapBookmarks.Bookmarks.Reset();
            IEnumSpatialBookmark enumSpatialBM = mapBookmarks.Bookmarks;
            IAOIBookmark aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            while (aoiBookmark != null)
            {
                if (aoiBookmark.Name == name)
                {
                    ISpatialBookmark spatialBM = aoiBookmark as ISpatialBookmark;
                    spatialBM.ZoomTo(_map);
                    return;
                }
                aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            }
        }
        public void PanToBookMark(string name)
        {
            IMapBookmarks mapBookmarks = _activeView as IMapBookmarks;
            mapBookmarks.Bookmarks.Reset();
            IEnumSpatialBookmark enumSpatialBM = mapBookmarks.Bookmarks;
            IAOIBookmark aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            while (aoiBookmark != null)
            {
                if (aoiBookmark.Name == name)
                {
                    IArea area = aoiBookmark.Location as IArea;
                    IEnvelope env = _activeView.Extent;
                    env.CenterAt(area.Centroid);
                    _activeView.Extent = env;
                    _activeView.Refresh();
                    return;
                }
                aoiBookmark = enumSpatialBM.Next() as IAOIBookmark;
            }
        }
        #endregion

        #region Private functions
        private string GetStantardLayerName(string name)
        {
            int index = name.LastIndexOf(".") + 1;
            if (index >= 0)
            {
                name = name.Substring(index, name.Length - index);
            }
            return name;
        }
        private bool TestGeometryHit(double tolerance, IPoint pPoint, IGeometry pGeom, ref IPoint pHitPoint, ref double hitDist, ref int partIndex, ref int vertexIndex, ref int vertexOffset, ref bool vertexHit, ref bool centerHit)
        {
            // Function returns true if a feature's shape is hit and further defines
            // if a vertex lies within the tolerance
            bool bRetVal = false;

            IHitTest pHitTest = (IHitTest)pGeom;
            pHitPoint = new ESRI.ArcGIS.Geometry.Point();
            bool bTrue = true;
            int vertexIndex1 = -1;
            int vertexIndex2 = -1;
            // First check if a vertex was hit
            if (pHitTest.HitTest(pPoint, tolerance, esriGeometryHitPartType.esriGeometryPartVertex, pHitPoint, ref hitDist, ref partIndex, ref vertexIndex, ref bTrue))
            {
                bRetVal = true;
                vertexHit = true;
                centerHit = false;
            }
            // Secondly check if a boundary was hit
            else if (pHitTest.HitTest(pPoint, tolerance, esriGeometryHitPartType.esriGeometryPartBoundary, pHitPoint, ref hitDist, ref partIndex, ref vertexIndex1, ref bTrue))
            {
                bRetVal = true;
                vertexHit = false;
                centerHit = false;
            }
            else if (pHitTest.HitTest(pPoint, tolerance, esriGeometryHitPartType.esriGeometryPartCentroid, pHitPoint, ref hitDist, ref partIndex, ref vertexIndex2, ref bTrue))
            {
                bRetVal = true;
                vertexHit = false;
                centerHit = true;
                //System.Windows.Forms.MessageBox.Show("Center");
            }

            // Calculate offset to vertexIndex for multi patch geometries
            if (partIndex > 0)
            {
                IGeometryCollection pGeomColn = (IGeometryCollection)pGeom;
                vertexOffset = 0;
                for (int i = 0; i < partIndex; i++)
                {
                    IPointCollection pPointColn = (IPointCollection)pGeomColn.get_Geometry(i);
                    vertexOffset = vertexOffset + pPointColn.PointCount;
                }
            }

            return bRetVal;
        }
        private bool TestGeometryHitForPolyline(double tolerance, IPoint pPoint, IGeometry pGeom, ref IPoint pHitPoint, ref double hitDist, ref int partIndex, ref int vertexIndex, ref int vertexOffset, ref bool vertexHit, ref bool centerHit)
        {
            // Function returns true if a feature's shape is hit and further defines
            // if a vertex lies within the tolerance
            bool bRetVal = false;
            IHitTest pHitTest = (IHitTest)pGeom;
            pHitPoint = new ESRI.ArcGIS.Geometry.Point();
            bool bTrue = true;
            int vertexIndex1 = -1;
            int vertexIndex2 = -1;
            // First check if a vertex was hit
            if (pHitTest.HitTest(pPoint, tolerance, esriGeometryHitPartType.esriGeometryPartVertex, pHitPoint, ref hitDist, ref partIndex, ref vertexIndex, ref bTrue))
            {
                bRetVal = true;
                vertexHit = true;
                centerHit = false;
            }
            // Secondly check if a boundary was hit
            else if (pHitTest.HitTest(pPoint, tolerance, esriGeometryHitPartType.esriGeometryPartBoundary, pHitPoint, ref hitDist, ref partIndex, ref vertexIndex1, ref bTrue))
            {
                bRetVal = true;
                vertexHit = false;
                centerHit = true;
            }
            else if (pHitTest.HitTest(pPoint, tolerance, esriGeometryHitPartType.esriGeometryPartCentroid, pHitPoint, ref hitDist, ref partIndex, ref vertexIndex2, ref bTrue))
            {
                bRetVal = true;
                vertexHit = false;
                centerHit = true;
                //System.Windows.Forms.MessageBox.Show("Center");
            }

            // Calculate offset to vertexIndex for multi patch geometries
            if (partIndex > 0)
            {
                IGeometryCollection pGeomColn = (IGeometryCollection)pGeom;
                vertexOffset = 0;
                for (int i = 0; i < partIndex; i++)
                {
                    IPointCollection pPointColn = (IPointCollection)pGeomColn.get_Geometry(i);
                    vertexOffset = vertexOffset + pPointColn.PointCount;
                }
            }

            return bRetVal;
        }
        #endregion

        #region Static functions
        public static double ConvertPixelsToMapUnits(IActiveView activeView, double pixelUnits)
        {
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

        public static void PantoSelected(IMap pMap)
        {
            try
            {
                IEnumFeature _iFeaturesSelected = (IEnumFeature)pMap.FeatureSelection;
                IFeature _ifeature = _iFeaturesSelected.Next();
                IEnvelope piEnvelop = null;
                IGeometry pIgeometry = null;

                while (_ifeature != null)
                {
                    pIgeometry = _ifeature.ShapeCopy;
                    if (piEnvelop == null)
                        piEnvelop = pIgeometry.Envelope;
                    else
                        piEnvelop.Union(pIgeometry.Envelope);

                    _ifeature = _iFeaturesSelected.Next();
                }
                if (piEnvelop != null)
                {
                    IActiveView pActiveView = pMap as IActiveView;
                    IEnvelope pEnv = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds;
                    IPoint ppoint = new PointClass();
                    ppoint.SpatialReference = piEnvelop.SpatialReference;
                    ppoint.X = (piEnvelop.LowerLeft.X + piEnvelop.UpperLeft.X) / 2;
                    ppoint.Y = (piEnvelop.LowerLeft.Y + piEnvelop.UpperLeft.Y) / 2;
                    pEnv.CenterAt(ppoint);
                    pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds = pEnv;
                    pActiveView.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
        public static void PantoSelected(IFeature pFeature, IMap pMap)
        {
            try
            {
                if (pFeature == null)
                    return;
                IEnvelope piEnvelop = null;
                IGeometry pIgeometry = null;
                pIgeometry = pFeature.ShapeCopy;
                if (piEnvelop == null)
                    piEnvelop = pIgeometry.Envelope;
                else
                    piEnvelop.Union(pIgeometry.Envelope);
                if (piEnvelop != null)
                {
                    IActiveView pActiveView = pMap as IActiveView;
                    IEnvelope pEnv = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds;
                    IPoint ppoint = new PointClass();
                    ppoint.SpatialReference = piEnvelop.SpatialReference;
                    ppoint.X = (piEnvelop.LowerLeft.X + piEnvelop.UpperLeft.X) / 2;
                    ppoint.Y = (piEnvelop.LowerLeft.Y + piEnvelop.UpperLeft.Y) / 2;
                    pEnv.CenterAt(ppoint);
                    pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds = pEnv;
                    pActiveView.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
        public static void PantoSelected(IGeometry pGeometry, IMap pMap)
        {
            try
            {
                if (pGeometry == null)
                    return;
                IEnvelope piEnvelop = null;
                if (piEnvelop == null)
                    piEnvelop = pGeometry.Envelope;
                else
                    piEnvelop.Union(pGeometry.Envelope);
                if (piEnvelop != null)
                {
                    IActiveView pActiveView = pMap as IActiveView;
                    IEnvelope pEnv = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds;
                    IPoint ppoint = new PointClass();
                    ppoint.SpatialReference = piEnvelop.SpatialReference;
                    ppoint.X = (piEnvelop.LowerLeft.X + piEnvelop.UpperLeft.X) / 2;
                    ppoint.Y = (piEnvelop.LowerLeft.Y + piEnvelop.UpperLeft.Y) / 2;
                    pEnv.CenterAt(ppoint);
                    pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds = pEnv;
                    pActiveView.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
        public static void ZoomToSelected(IGeometry pGeometry, IMap pMap)
        {
            try
            {
                if (pGeometry == null)
                    return;
                IActiveView pActiveView = (IActiveView)pMap;
                IEnvelope pEnvelop = null;
                if (pEnvelop == null)
                    pEnvelop = pGeometry.Envelope;
                else
                    pEnvelop.Union(pGeometry.Envelope);
                if (pGeometry is IPoint)
                {
                    IPoint ppoint = new PointClass();
                    ppoint.X = (pEnvelop.XMax + pEnvelop.XMin) / 2;
                    ppoint.Y = (pEnvelop.YMax + pEnvelop.YMin) / 2;
                    pEnvelop = pActiveView.Extent;
                    if (pMap.MapScale > 100)
                        pEnvelop.Expand(0.5, 0.5, true);
                    pEnvelop.CenterAt(ppoint);
                    pActiveView.Extent = pEnvelop;
                }
                else
                {
                    pEnvelop.Expand(1.1, 1.1, true);
                    pActiveView.Extent = pEnvelop;
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
        public static void ZoomToSelected(IMap pMap)
        {
            try
            {
                bool ZoomPoint = false;
                int numpoint = 0;
                IActiveView pActiveView = (IActiveView)pMap;
                IEnumFeature _iFeaturesSelected = (IEnumFeature)pMap.FeatureSelection;
                IFeature _ifeature = _iFeaturesSelected.Next();
                IEnvelope pEnvelop = null;
                IGeometry pGeometry = null;
                if (_ifeature != null && _ifeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                { ZoomPoint = true; }
                while (_ifeature != null)
                {
                    pGeometry = _ifeature.ShapeCopy;
                    if (pEnvelop == null)
                        pEnvelop = pGeometry.Envelope;
                    else
                        pEnvelop.Union(pGeometry.Envelope);
                    numpoint++;
                    _ifeature = _iFeaturesSelected.Next();
                }
                if (pEnvelop == null) return;
                if (ZoomPoint && numpoint <= 1)
                {
                    IPoint ppoint = new PointClass();
                    ppoint.X = (pEnvelop.XMax + pEnvelop.XMin) / 2;
                    ppoint.Y = (pEnvelop.YMax + pEnvelop.YMin) / 2;
                    pEnvelop = pActiveView.Extent;
                    if (pMap.MapScale > 5000)
                        pEnvelop.Expand(0.5, 0.5, true);
                    pEnvelop.CenterAt(ppoint);
                    pActiveView.Extent = pEnvelop;
                }
                else
                {
                    pEnvelop.Expand(1.1, 1.1, true);
                    pActiveView.Extent = pEnvelop;
                }
                pActiveView.Refresh();
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
        public static void flashObject(IGeometry pGeometry, IMap pMap)
        {
            IScreenDisplay pDisplay = ((IActiveView)pMap).ScreenDisplay;
            pDisplay.StartDrawing(0, (short)esriScreenCache.esriNoScreenCache);
            ISymbol pSymbol = null;
            IRgbColor pRGBColor = null;
            if (pGeometry is IPolygon)
            {

                ISimpleFillSymbol pFillSymbol;
                pFillSymbol = new SimpleFillSymbolClass();

                pRGBColor = new RgbColorClass();
                pRGBColor.Red = 0;
                pRGBColor.Green = 255;
                pRGBColor.Blue = 0;
                pFillSymbol.Color = pRGBColor;

                pSymbol = pFillSymbol as ISymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
            }
            else if (pGeometry is IPolyline)
            {
                ISimpleLineSymbol pLineSymbol;
                pLineSymbol = new SimpleLineSymbolClass();
                pLineSymbol.Width = 4;

                pRGBColor = new RgbColorClass();
                pRGBColor.Red = 0;
                pRGBColor.Green = 255;
                pRGBColor.Blue = 0;
                pLineSymbol.Color = pRGBColor;

                pSymbol = pLineSymbol as ISymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
            }
            else if (pGeometry is IPoint)
            {
                ISimpleMarkerSymbol pPointSymbol;
                pPointSymbol = new SimpleMarkerSymbolClass();
                pPointSymbol.Size = 10;

                pRGBColor = new RgbColorClass();
                pRGBColor.Red = 255;
                pRGBColor.Green = 0;
                pRGBColor.Blue = 0;
                pPointSymbol.Color = pRGBColor;

                pSymbol = pPointSymbol as ISymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
            }

            pDisplay.SetSymbol(pSymbol);
            if (pGeometry is IPolygon) pDisplay.DrawPolygon(pGeometry);
            else if (pGeometry is IPolyline) pDisplay.DrawPolyline(pGeometry);
            else if (pGeometry is IPoint) pDisplay.DrawPoint(pGeometry);
            System.Threading.Thread.Sleep(150);
            if (pGeometry is IPolygon) pDisplay.DrawPolygon(pGeometry);
            else if (pGeometry is IPolyline) pDisplay.DrawPolyline(pGeometry);
            else if (pGeometry is IPoint) pDisplay.DrawPoint(pGeometry);
            pDisplay.FinishDrawing();
        }
        public static string getCaptionUnit(esriUnits esirunit)
        {
            string stresriunit = esirunit.ToString();
            string valuereturn = "";
            switch (stresriunit)
            {
                case "esriUnknownUnits":
                    valuereturn = "Unknown Units";
                    break;
                case "esriInches":
                    valuereturn = "Inches";
                    break;
                case "esriPoints":
                    valuereturn = "Points";
                    break;
                case "esriFeet":
                    valuereturn = "Feet";
                    break;
                case "esriYards":
                    valuereturn = "Yards";
                    break;
                case "esriMiles":
                    valuereturn = "Miles";
                    break;
                case "esriNauticalMiles":
                    valuereturn = "Nautical miles";
                    break;
                case "esriMillimeters":
                    valuereturn = "Millimeters";
                    break;
                case "esriCentimeters":
                    valuereturn = "Centimeters";
                    break;
                case "esriMeters":
                    valuereturn = "Meters";
                    break;
                case "esriKilometers":
                    valuereturn = "Kilometers";
                    break;
                case "esriDecimalDegrees":
                    valuereturn = "Decimal degrees";
                    break;
                case "esriDecimeters":
                    valuereturn = "Decimeters";
                    break;
                default:
                    break;
            }
            return valuereturn;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _map = null;
            _activeView = null;
        }

        #endregion
    }

    public class JoinTable
    {
        public string Name { get; set; }
        public string OriginPK { get; set; }
        public string OriginFK { get; set; }
    }
}