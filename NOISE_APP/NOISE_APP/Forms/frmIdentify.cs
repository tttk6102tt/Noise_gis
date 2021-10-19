using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Carto;
using FrameWork.Core.MapInterfaces;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.esriSystem;
using System.Collections;
using ESRI.ArcGIS.Geodatabase;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList.Columns;
using System.Globalization;
using NOISE_APP.Models;

namespace NOISE_APP.Forms
{
    public partial class frmIdentify : DevExpress.XtraEditors.XtraForm
    {
        public IMap pMap = null;
        private IList<ILayer> _listLayer;
        private IActiveView activeView = null;
        private IdentifyStatus identifyStatus;

        private int iFindObject = 0;
        private string strCaption = "Caption";

        private BindingList<ObjectInfo> mProperties = new BindingList<ObjectInfo>();
        public frmIdentify(IMap axMap)
        {
            pMap = axMap;
            activeView = (IActiveView)pMap;
            InitializeComponent();
            //
            LoadLayer(pMap);
            BindEvents();
            //stbResult.Caption = "0 đối tượng được tìm thấy";
        }

        private void frmIdentify_FormClosing(object sender, FormClosingEventArgs e)
        {
            FrmOnClosing?.Invoke();
        }
        List<string> lstAliasName = new List<string>();

        private object isContain(ArrayList arrLayer, string strAliasName)
        {
            int i = 0;
            ILayer pLayer = null, pLayer_Contain = null;
            IFeatureLayer pFLayer = null;
            for (i = 0; i < arrLayer.Count; i++)
            {
                pLayer = arrLayer[i] as ILayer;
                pFLayer = pLayer as IFeatureLayer;
                if (pFLayer.FeatureClass.AliasName.ToUpper() == strAliasName.ToUpper())
                {
                    pLayer_Contain = pLayer;
                    break;
                }
            }
            return pLayer_Contain;
        }

        private void BindEvents()
        {
            gcData.DataSource = mProperties;
            //
            cboLayers.EditValueChanged += (sender, e) =>
            {
                string value = cboLayers.EditValue?.ToString();
                if (string.IsNullOrWhiteSpace(value))
                    identifyStatus = IdentifyStatus.All;
                else if (value == IdentifyStatus.TopMostLayer.ToString())
                    identifyStatus = IdentifyStatus.TopMostLayer;
                else if (value == IdentifyStatus.VisibleLayer.ToString())
                    identifyStatus = IdentifyStatus.VisibleLayer;
                else if (value == IdentifyStatus.All.ToString())
                    identifyStatus = IdentifyStatus.All;
                else
                    identifyStatus = IdentifyStatus.Layer;
            };
            //
            tlLayers.BeginUpdate();
            TreeListColumn col = tlLayers.Columns.Add();
            col.Visible = true;
            col.VisibleIndex = 0;
            tlLayers.EndUpdate();
            //
            tlLayers.OptionsView.ShowColumns = false;
            tlLayers.OptionsView.ShowIndicator = false;
            tlLayers.OptionsView.ShowVertLines = false;
            tlLayers.OptionsView.ShowHorzLines = false;
            tlLayers.OptionsBehavior.Editable = false;
            tlLayers.OptionsSelection.EnableAppearanceFocusedCell = false;
            tlLayers.FocusedNodeChanged += (sender, e) =>
            {
                if (e.Node == null || e.Node.Tag == null)
                    return;
                object tag = e.Node.Tag;
                IFeature pFeature;
                if (tag is IFeature)
                {
                    pFeature = (IFeature)tag;
                    FlashObject(pFeature.Shape);
                    mProperties.Clear();
                    for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                    {
                        IField field = pFeature.Fields.get_Field(i);
                        if (field.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
                            || field.Type.Equals(esriFieldType.esriFieldTypeBlob))
                            continue;
                        if (field.Type == esriFieldType.esriFieldTypeDate)
                        {
                            if (pFeature.Value[i] != DBNull.Value)
                                mProperties.Add(new ObjectInfo(field.AliasName, Convert.ToDateTime(pFeature.Value[i].ToString()).ToString("dd/MM/yyyy")));
                        }
                        else
                        {
                            mProperties.Add(new ObjectInfo(field.AliasName, pFeature.Value[i]?.ToString()));
                        }
                    }
                }
                else
                {
                    IEnvelope pEnvelop = new EnvelopeClass();
                    ArrayList arrGeometry = new ArrayList();
                    for (int i = 0; i < e.Node.Nodes.Count; i++)
                    {
                        pFeature = (IFeature)e.Node.Nodes[i].Tag;
                        pEnvelop.Union(pFeature.Shape.Envelope);
                        arrGeometry.Insert(arrGeometry.Count, pFeature.Shape);
                    }
                    FlashObject(arrGeometry);
                }
            };
        }

        private void LoadLayer(IMap pMap)
        {
            try
            {
                _listLayer = new List<ILayer>();
                if (pMap.LayerCount <= 0)
                    return;
                List<ObjectInfo> objInfos = new List<ObjectInfo>();
                objInfos.Add(new ObjectInfo(IdentifyStatus.TopMostLayer.ToString(), "< Lớp bản đồ trên cùng >"));
                objInfos.Add(new ObjectInfo(IdentifyStatus.VisibleLayer.ToString(), "< Các lớp bản đồ được hiển thị >"));
                objInfos.Add(new ObjectInfo(IdentifyStatus.All.ToString(), "< Tất cả >"));
                objInfos.Add(new ObjectInfo(null, "-------------------------------------------------"));

                Stack<ILayer> stkLayers = new Stack<ILayer>();
                for (int i = pMap.LayerCount - 1; i >= 0; i--)
                {
                    stkLayers.Push(pMap.get_Layer(i));
                }
                int index = 0;
                while (stkLayers.Count > 0)
                {
                    ILayer layer = stkLayers.Pop();
                    if (layer is IFeatureLayer)
                    {
                        _listLayer.Add(layer);
                        objInfos.Add(new ObjectInfo(index.ToString(), layer.Name));
                        index++;
                    }
                    else if (layer is ICompositeLayer)
                    {
                        ICompositeLayer cpLayer = layer as ICompositeLayer;
                        for (int i = cpLayer.Count - 1; i >= 0; i--)
                        {
                            stkLayers.Push(cpLayer.get_Layer(i));
                        }
                    }
                }
                cboLayers.Properties.DataSource = objInfos;
                cboLayers.Properties.ValueMember = "ID";
                cboLayers.Properties.DisplayMember = "Name";

                LookUpColumnInfo colValue = new LookUpColumnInfo("ID");
                LookUpColumnInfo colDisplay = new LookUpColumnInfo("Name");
                colValue.Visible = false;
                LookUpColumnInfoCollection colls = cboLayers.Properties.Columns;
                colls.Clear();
                colls.Add(colValue);
                colls.Add(colDisplay);
                cboLayers.Properties.ShowHeader = false;
                int iRow = objInfos.Count > 7 ? 7 : objInfos.Count;
                cboLayers.Properties.DropDownRows = iRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateLayer()
        {
            LoadLayer(pMap);
        }
        public delegate void IdentifyOnClosing();

        public event IdentifyOnClosing FrmOnClosing;
        private double convertPixelsToMapUnits(double pixelUnits)
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

        private void FlashObject(IGeometry pGeometry)
        {
            IScreenDisplay pDisplay = activeView.ScreenDisplay;
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
                pRGBColor.Red = 0;
                pRGBColor.Green = 255;
                pRGBColor.Blue = 0;
                pPointSymbol.Color = pRGBColor;

                pSymbol = pPointSymbol as ISymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
            }

            pDisplay.SetSymbol(pSymbol);
            if (pGeometry is IPolygon) pDisplay.DrawPolygon(pGeometry);
            else if (pGeometry is IPolyline) pDisplay.DrawPolyline(pGeometry);
            else if (pGeometry is IPoint) pDisplay.DrawPoint(pGeometry);
            System.Threading.Thread.Sleep(500);
            if (pGeometry is IPolygon) pDisplay.DrawPolygon(pGeometry);
            else if (pGeometry is IPolyline) pDisplay.DrawPolyline(pGeometry);
            else if (pGeometry is IPoint) pDisplay.DrawPoint(pGeometry);
            pDisplay.FinishDrawing();
        }

        private void FlashObject(ArrayList arrGeometry)
        {
            IScreenDisplay pDisplay = activeView.ScreenDisplay;
            ISymbol pSymbol = null;
            IRgbColor pRGBColor = null;
            int i = 0, j = 0;
            IGeometry pGeometry = null;
            for (j = 0; j < 2; j++)
            {
                pDisplay.StartDrawing(0, (short)esriScreenCache.esriNoScreenCache);
                for (i = 0; i < arrGeometry.Count; i++)
                {
                    pGeometry = (IGeometry)arrGeometry[i];
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
                        pRGBColor.Red = 0;
                        pRGBColor.Green = 255;
                        pRGBColor.Blue = 0;
                        pPointSymbol.Color = pRGBColor;

                        pSymbol = pPointSymbol as ISymbol;
                        pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                    }

                    pDisplay.SetSymbol(pSymbol);
                    if (pGeometry is IPolygon) pDisplay.DrawPolygon(pGeometry);
                    else if (pGeometry is IPolyline) pDisplay.DrawPolyline(pGeometry);
                    else if (pGeometry is IPoint) pDisplay.DrawPoint(pGeometry);
                    /*System.Threading.Thread.Sleep(500);
                    if (pGeometry is IPolygon) pDisplay.DrawPolygon(pGeometry);
                    else if (pGeometry is IPolyline) pDisplay.DrawPolyline(pGeometry);
                    else if (pGeometry is IPoint) pDisplay.DrawPoint(pGeometry);*/
                }
                pDisplay.FinishDrawing();
                System.Threading.Thread.Sleep(500);
            }
        }

        private void LoadIndentifyInfo(ILayer pLayer, IGeometry geometry)
        {
            IIdentify pIdentify = pLayer as IIdentify;
            if (pIdentify == null)
                return;
            IArray pArray = pIdentify.Identify(geometry);
            if (pArray == null)
                return;
            string strTableName = "";
            if (pArray.Count > 0)
            {
                tlLayers.BeginUnboundLoad();
                IFeatureLayer pFeatLayer = pLayer as IFeatureLayer;
                strTableName = pFeatLayer.FeatureClass.AliasName;
                TreeListNode node = tlLayers.AppendNode(new object[] { pLayer.Name }, null, pFeatLayer.FeatureClass.AliasName);
                for (int i = 0; i < pArray.Count; i++)
                {
                    IIdentifyObj pIdObj = pArray.get_Element(i) as IIdentifyObj;
                    IRowIdentifyObject rIdObj = pIdObj as IRowIdentifyObject;
                    IFeature pFeature = rIdObj.Row as IFeature;
                    // FlashObject(pFeature.Shape);
                    TreeListNode fNode = tlLayers.AppendNode(new object[] { pFeature.OID.ToString() }, node, pFeature);
                    if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                        fNode.StateImageIndex = 0;
                    else if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                        fNode.StateImageIndex = 1;
                    else if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                        fNode.StateImageIndex = 2;
                }
                tlLayers.EndUnboundLoad();
                tlLayers.ExpandAll();
            }
            iFindObject += pArray.Count;
        }

        public void IdentifyInfo(int x, int y)
        {
            Cursor = Cursors.WaitCursor;
            iFindObject = 0;
            mProperties.Clear();
            tlLayers.Nodes.Clear();
            IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            txtLocation.Text = "X:= " + string.Format("{0:n7}", point.X)
                            + " ; " + "Y:= " + string.Format("{0:n5}", point.Y) + " "
                            + FrameWork.Map.Utilities.MapUtility.getCaptionUnit(pMap.MapUnits);
            IRubberBand rubber = new RubberRectangularPolygonClass();
            IGeometry geometry = rubber.TrackNew(activeView.ScreenDisplay, null).Envelope;
            if (geometry.IsEmpty)
            {
                ITopologicalOperator pTopo = point as ITopologicalOperator;
                double length = convertPixelsToMapUnits(2);
                geometry = pTopo.Buffer(length);
            }

            ILayer pLayer = null;
            double dMinScale = 0;

            ArrayList arrLayer = new ArrayList();
            switch (identifyStatus)
            {
                case IdentifyStatus.TopMostLayer:
                    pLayer = _listLayer[0] as ILayer;
                    dMinScale = pLayer.MinimumScale;
                    if (dMinScale > 0)
                    {
                        if (pMap.MapScale <= dMinScale)
                            LoadIndentifyInfo(pLayer, geometry);
                    }
                    else
                        LoadIndentifyInfo(pLayer, geometry);
                    break;
                case IdentifyStatus.VisibleLayer:
                    for (int i = 0; i < _listLayer.Count; i++)
                    {
                        pLayer = _listLayer[i] as ILayer;
                        if (!pLayer.Visible)
                            continue;
                        LoadIndentifyInfo(pLayer, geometry);
                    }
                    break;
                case IdentifyStatus.All:
                    for (int i = 0; i < _listLayer.Count; i++)
                    {
                        LoadIndentifyInfo(pLayer = _listLayer[i], geometry);
                    }
                    break;
                case IdentifyStatus.Layer:
                    if (cboLayers.EditValue != null && string.IsNullOrWhiteSpace(cboLayers.EditValue.ToString()) == false)
                    {
                        int index = Convert.ToInt32(cboLayers.EditValue);
                        if (index >= 0 && index < _listLayer.Count)
                        {
                            pLayer = _listLayer[index] as ILayer;
                            if (pLayer != null)
                                LoadIndentifyInfo(pLayer, geometry);
                        }
                    }
                    break;
                case IdentifyStatus.None:
                    for (int i = 0; i < _listLayer.Count; i++)
                    {
                        LoadIndentifyInfo(pLayer = _listLayer[i], geometry);
                    }
                    break;
                default:
                    break;
            }

            if (tlLayers.Nodes.Count == 1)
            {
                if (tlLayers.Nodes[0].Nodes.Count == 1)
                    tlLayers.SelectNode(tlLayers.Nodes[0].Nodes[0]);
            }
            string sItems = "đối tượng được tìm thấy";
            //stbResult.Caption = iFindObject.ToString() + " " + sItems;
            //
            Cursor = Cursors.Default;
        }
    }
}