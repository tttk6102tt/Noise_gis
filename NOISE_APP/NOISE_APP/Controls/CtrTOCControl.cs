using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using DevExpress.XtraBars;
using NOISE_APP.Forms;
using ESRI.ArcGIS.Display;

namespace NOISE_APP.Controls
{
    public partial class CtrTOCControl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void OnRanchTypeChange(string sQuery);

        #region Fields
        private AxMapControl mMapControl;
        private ITOCControl mTOCControl;
        private ILayer mLayerSelect;
        private ILayer mLayerGetFromHit;
        private IFeatureLayer mFeatureLayer = null;
        private IGeoFeatureLayer mGeoFeatureLayer = null;
        private string mLayerName = "";
        private DataTable _dtSymbolCategories;

        public event CtrTOCControl.OnRanchTypeChange ItemRanchTypeChange;

        #endregion

        #region Properties
        #endregion

        #region Constructor
        public CtrTOCControl(AxMapControl mapControl)
        {
            mMapControl = mapControl;
            InitializeComponent();
            btnAttributesTable.ItemClick += btnAttributesTable_ItemClick;
            btnZoomToLayer.ItemClick += btnZoomToLayer_ItemClick;
            btnRemoveLayer.ItemClick += btnRemoveLayer_ItemClick;
        }
        #endregion

        #region Form events
        private void CtrTOCControl_Load(object sender, EventArgs e)
        {
            //Bitmap bmpTemp = (Bitmap)imageList1.Images[0];
            //bmpTemp.MakeTransparent(bmpTemp.GetPixel(1, 1));
            //pMouseIcon = Icon.FromHandle(bmpTemp.GetHicon());
            //if (mMapControl.LayerCount > 0)
            //{
            axTOC.SetBuddyControl(mMapControl);
            mTOCControl = (ITOCControl)axTOC.GetOcx();
            //}
            axTOC.EnableLayerDragDrop = true;
            axTOC.OnDoubleClick += axTOC_OnDoubleClick;
            axTOC.OnMouseDown += axTOC_OnMouseDown;
        }
        #endregion

        #region Control events
        private void btnAttributesTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.mFeatureLayer != null)
                {
                    Cursor = Cursors.WaitCursor;
                    new FrmAttributesTable(mMapControl.Map, mFeatureLayer).Show();
                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                // GISMessageBox.ShowErrorMessage(ex.Message);
            }
        }
        private void btnZoomToLayer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (mFeatureLayer != null)
                {
                    ZoomToLayer(mFeatureLayer);
                }
            }
            catch (Exception ex)
            {
                // GISMessageBox.ShowErrorMessage(ex.Message);
            }
        }
        private void btnRemoveLayer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (mFeatureLayer != null)
                {
                    RemoveLayer(mFeatureLayer);
                }
            }
            catch (Exception ex)
            {
                // GISMessageBox.ShowErrorMessage(ex.Message);
            }
        }
        private void btnSetLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //FrmLabelManager FLM = new FrmLabelManager(this.mLayerName, mMapControl, this._gisMain);
            //FLM.ShowDialog();
            //FLM.isFirstLabel = true;
            //this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }
        private void bntShowSimpleLabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ShowSimplelabel();
            }
            catch (Exception ex)
            {
                // GISMessageBox.ShowErrorMessage(ex.Message);
            }
        }
        private void ExportData_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //FrmAddData insAddNewData = new FrmAddData(this.mMapControl, this._gisMain, this);
                //insAddNewData.ShowDialog();
            }
            catch (Exception ex) { }
        }
        private void btnSavaMXD_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                //FrmExportMXD frm = new FrmExportMXD(this.mMapControl, _gisMain);
                //frm.ShowDialog();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                // new GISException(ex, ex.Message, 1);
            }
        }
        #endregion

        #region Map Action
        private void axTOC_OnDoubleClick(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnDoubleClickEvent e)
        {
            IBasicMap map = null;
            ILayer pLayerTOCSelect = null;
            object other = null;
            object index = null;

            ISymbol pSymbol = null;
            esriTOCControlItem itemTocSelect = esriTOCControlItem.esriTOCControlItemNone;
            //this.LoadContentMenuLayer();
            //Determine what kind of item has been clicked on
            mTOCControl.HitTest(e.x, e.y, ref itemTocSelect, ref map, ref pLayerTOCSelect, ref other, ref index);
            //Only layer items can have their labels edited
            if (itemTocSelect == esriTOCControlItem.esriTOCControlItemLegendClass)
            {
                IFeatureLayer pFLayer = (IFeatureLayer)pLayerTOCSelect;
                ILegendGroup pLegendGroup = (ILegendGroup)other;
                int classIndex = (int)index;
                ILegendClass pLegendClass = pLegendGroup.get_Class(classIndex);
                pSymbol = pLegendClass.Symbol;
                // GIS.Common.Forms.FrmSymbolSelector SymbolSelector = new GIS.Common.Forms.FrmSymbolSelector(pSymbol, pFLayer.FeatureClass.ShapeType, this._gisMain);
                //if (SymbolSelector.ShowDialog().Equals(DialogResult.OK))
                //{
                //    pLegendClass.Symbol = SymbolSelector.Symbol;
                //    this.mMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                //}
            }
        }
        private void axTOC_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnMouseDownEvent e)
        {
            try
            {
                IBasicMap map = null;
                ILayer pLayerTOCSelect = null;
                object other = null;
                object index = null;
                esriTOCControlItem itemTocSelect = esriTOCControlItem.esriTOCControlItemNone;
                //loadmenuTransparency
                //Determine what kind of item has been clicked on
                mTOCControl.HitTest(e.x, e.y, ref itemTocSelect, ref map, ref pLayerTOCSelect, ref other, ref index);
                mLayerSelect = pLayerTOCSelect;
                int pbreakindex = Convert.ToInt16(index);
                //Only layer items can have their labels edited
                if (itemTocSelect == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                }
                if (e.button == 1)
                {
                    if ((itemTocSelect == esriTOCControlItem.esriTOCControlItemLayer) || (itemTocSelect == esriTOCControlItem.esriTOCControlItemLegendClass))
                    {
                        IFeatureLayer pFLayer = (IFeatureLayer)pLayerTOCSelect;
                        this.mFeatureLayer = pFLayer;
                        IGeoFeatureLayer pGeoFLayer = (IGeoFeatureLayer)pFLayer;
                        this.mGeoFeatureLayer = pGeoFLayer;
                        mLayerName = pFLayer.FeatureClass.AliasName;
                    }
                }
                if (e.button == 2)
                {
                    if ((itemTocSelect == esriTOCControlItem.esriTOCControlItemLayer) || (itemTocSelect == esriTOCControlItem.esriTOCControlItemLegendClass))
                    {
                        if (pLayerTOCSelect is IFeatureLayer)
                        {
                            IFeatureLayer pFLayer = (IFeatureLayer)pLayerTOCSelect;
                            this.mFeatureLayer = pFLayer;
                            IGeoFeatureLayer pGeoFLayer = (IGeoFeatureLayer)pFLayer;
                            this.mGeoFeatureLayer = pGeoFLayer;
                            // mLayerName = pFLayer.FeatureClass.AliasName;
                            mLayerGetFromHit = pLayerTOCSelect;
                            ILayerEffects pPlayerEffects = (ILayerEffects)pLayerTOCSelect;
                            //if (mGeoFeatureLayer.DisplayAnnotation)
                            //    btnShowSimpleLabel.Down = true;
                            //else
                            //    btnShowSimpleLabel.Down = false;
                            btnAttributesTable.Enabled = true;
                            btnZoomToLayer.Enabled = true;
                            btnRemoveLayer.Enabled = true;
                            // btnShowSimpleLabel.Enabled = true;
                        }
                        else if (pLayerTOCSelect is ICompositeLayer)
                        {
                            btnAttributesTable.Enabled = false;
                            btnZoomToLayer.Enabled = false;
                            btnRemoveLayer.Enabled = false;
                        }
                    }
                    else
                    {
                        btnAttributesTable.Enabled = false;
                        btnZoomToLayer.Enabled = false;
                        btnRemoveLayer.Enabled = false;
                    }
                    pmActions.ShowPopup(MousePosition);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        #endregion

        #region Public functions
        public void ZoomToLayer(ILayer pLayer)
        {
            try
            {
                if (pLayer == null)
                    return;
                this.mMapControl.ActiveView.Extent = pLayer.AreaOfInterest;
                this.mMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGraphics | esriViewDrawPhase.esriViewGraphicSelection, pLayer, this.mMapControl.ActiveView.Extent);
            }
            catch (Exception ex)
            {
                // new GISException(ex, ex.Message, "Thông báo", true);
            }
        }
        public void RemoveLayer(ILayer layer)
        {
            try
            {
                if (layer == null)
                    return;
                mMapControl.Map.DeleteLayer(layer);
                mMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, layer.AreaOfInterest);
            }
            catch (Exception ex)
            {
                // new GISException(ex, ex.Message, "Thông báo", true);
            }
        }
        public void Update()
        {
            axTOC.Update();
        }
        #endregion

        #region Private functions
        private void ShowSimplelabel()
        {
            try
            {
                if (this.mGeoFeatureLayer != null)
                {
                    ILabelEngineLayerProperties le = new LabelEngineLayerPropertiesClass();
                    IAnnotateLayerProperties alp = le as IAnnotateLayerProperties;
                    alp.FeatureLayer = this.mGeoFeatureLayer;
                    if (mGeoFeatureLayer.DisplayAnnotation) this.mGeoFeatureLayer.DisplayAnnotation = false;
                    else this.mGeoFeatureLayer.DisplayAnnotation = true;
                    this.mMapControl.ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, (object)mGeoFeatureLayer, this.mMapControl.ActiveView.Extent);

                }
            }
            catch (Exception ex)
            {
                // new GISException(ex, ex.Message, "Thông báo", true);
            }

        }
        #endregion

        private void btnExportShp_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (this.mFeatureLayer != null)
                {
                    //FrmExportShp f_shp = new FrmExportShp(this.mFeatureLayer, this._gisMain);
                    //f_shp.Show();
                }
            }
            catch (Exception ex) { }
        }
    }
}
