using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using NOISE_APP.Controls;
using System.Threading;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using FrameWork.Core.MapInterfaces;
using ESRI.ArcGIS.Display;
using FrameWork.Map.Utilities;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;

namespace NOISE_APP.Forms
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private XtraForm mForm;
        private CtrTOCControl mTOCControl;
        private CancellationTokenSource mCancelTasks;
        private CancellationToken mCancelToken;
        private _ProgressCancellable.Locker mLocker;
        private BackgroundWorker mBgWorker;
        private IToolbarAction mToolBarActions;
        /*
        private bool LoadTOC()
        {
            mTOCControl = new CtrTOCControl(axMap);
            mTOCControl.Dock = DockStyle.Fill;
            dockTOC.Controls.Add(mTOCControl);
            return true;
        }
        private void AxMap_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            if (axMap.MapScale != 0)
                bbItemScale.Caption = $"Tỷ lệ 1 : {string.Format("{0:n0}", axMap.ActiveView.FocusMap.MapScale)}";
        }

        private void AxMap_Resize(object sender, EventArgs e)
        {
            if (axMap.Map.LayerCount == 0)
                return;
            bbItemScale.Caption = $"Tỷ lệ 1 : {string.Format("{0:n0}", axMap.MapScale)}";
        }

        private void AxMap_OnDoubleClick(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnDoubleClickEvent e)
        {
            MouseEventInfo me = new MouseEventInfo();
            me.mapX = e.mapX;
            me.mapY = e.mapY;
            me.x = e.x;
            me.y = e.y;
        }

        private void AxMap_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            //set coordinate for status panel
            bsItemCoords.Caption = $"X= {string.Format("{0:n6}", e.mapX)} Y= {string.Format("{0:n7}", e.mapY)} {FrameWork.Map.Utilities.MapUtility.getCaptionUnit(this.axMap.Map.MapUnits)}";
            if (e.button == 4)
            {
                ESRI.ArcGIS.Display.IScreenDisplay m_pScreenDisplay = axMap.ActiveView.ScreenDisplay;
                IPoint pMoveToPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                m_pScreenDisplay.PanMoveTo(pMoveToPoint);
                return;
            }
            int Action = (int)mToolBarActions.MapAction;
            switch (Action)
            {
                default:
                    MouseEventInfo me = new MouseEventInfo();
                    me.mapX = e.mapX;
                    me.mapY = e.mapY;
                    me.shift = e.shift;
                    me.x = e.x;
                    me.y = e.y;
                    if (mToolBarActions.MapAction == BaseMapAction.Distance)
                    {
                        string sUnit = string.Empty;
                        double total = 0;
                        double ms = 0;
                        mToolBarActions.MouseMoveAction(me, ref ms, ref total, ref sUnit);
                        lblActionMessage.Caption = string.Format("Độ dài hiện tại: {0} " + sUnit + " | Tổng chiều dài: {1} " + sUnit, ms.ToString("0.00"), total.ToString("0.00"));
                    }
                    else
                        mToolBarActions.MouseMoveAction(me);
                    break;
            }
        }

        private void AxMap_OnMouseUp(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseUpEvent e)
        {
            if (e.button == 2) return;
            if (e.button == 4)
            {
                IScreenDisplay m_pScreenDisplay = axMap.ActiveView.ScreenDisplay;
                IEnvelope pEnvelope = m_pScreenDisplay.PanStop();
                //					if(pEnvelope!=null)
                //						_activeView.Extent = pEnvelope;
                if (pEnvelope != null)
                {
                    axMap.ActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds = pEnvelope;
                    axMap.ActiveView.ScreenDisplay.Invalidate(null, true, (short)esriScreenCache.esriAllScreenCaches);
                }
                return;
            }
            int Action = (int)mToolBarActions.MapAction;
            switch (Action)
            {
                default:
                    MouseEventInfo me = new MouseEventInfo();
                    me.mapX = e.mapX;
                    me.mapY = e.mapY;
                    me.shift = e.shift;
                    me.x = e.x;
                    me.y = e.y;
                    mToolBarActions.MouseUpAction(me);
                    break;
            }
        }

        private void AxMap_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {

            #region SetShowPopup
            if (e.button == 2)
            {
                return;
            }
            else if (e.button == 4)
            {
                ESRI.ArcGIS.Display.IScreenDisplay m_pScreenDisplay = axMap.ActiveView.ScreenDisplay;
                IPoint pStartPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                IMap hitMap = axMap.ActiveView.HitTestMap(pStartPoint);
                if (hitMap == null)
                    return;
                axMap.ActiveView.ScreenDisplay.PanStart(pStartPoint);
                return;
            }
            #endregion
            int Action = (int)mToolBarActions.MapAction;
            switch (Action)
            {
                case (int)BaseMapAction.Identify:
                    #region Identify
                    //mIdentifyForm = new frmIdentify(axMap.Map);
                    //mIdentifyForm.IdentifyInfo(e.x, e.y);
                    //mIdentifyForm.ShowDialog();
                    #endregion
                    break;
                default:
                    MouseEventInfo me = new MouseEventInfo();
                    me.mapX = e.mapX;
                    me.mapY = e.mapY;
                    me.shift = e.shift;
                    me.x = e.x;
                    me.y = e.y;
                    mToolBarActions.MouseDownAction(me);
                    break;
            }

        }
        
        */
        public FormMain()
        {
            InitializeComponent();
            //if (LoadTOC())
            //{
            //    BindEvents();
            //}
        }
       
        private void FormMain_Load(object sender, EventArgs e)
        {

        }
        /*
        private void BindEvents()
        {
            axMap.OnMouseUp += AxMap_OnMouseUp;
            axMap.OnMouseMove += AxMap_OnMouseMove;
            axMap.OnDoubleClick += AxMap_OnDoubleClick;
            axMap.Resize += AxMap_Resize;
            axMap.OnExtentUpdated += AxMap_OnExtentUpdated;
            try
            {
                //
                //axMap.LoadMxFile(System.IO.Path.Combine(Application.StartupPath, "Mxd", "temp.mxd"));

                var mxdPath = @"E:\20210825\CSDLTest\VN2000.mxd";
                string filegdb = @"E:\20210825\CSDLTest\CSDL_ONhiemTiengOn.gdb";

                var rasterName = "Raster_20210930_18";

                var mGDBWsFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                var mGDBWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IFeatureWorkspace;

                var mGDBRasterWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IRasterWorkspaceEx;

                SetDataSource(mxdPath, ((IWorkspace)mGDBWs).PathName, mGDBRasterWs.OpenRasterDataset(rasterName));

                axMap.LoadMxFile(mxdPath);
                mToolBarActions = new ToolbarUtilites(axMap);
                mTOCControl.Update();
                axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

                mToolBarActions = new ToolbarUtilites(axMap);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public static void SetDataSource(string sMxdPath, string sTargetPath, IRasterDataset rasterDataset = null, string kyTinh = "")
        {
            IMapDocument pMapDocument = new MapDocumentClass();
            pMapDocument.Open(sMxdPath, "");
            IWorkspaceFactory pWorkFactory = new FileGDBWorkspaceFactory() as IWorkspaceFactory2;
            IWorkspace pWorkspace = pWorkFactory.OpenFromFile(sTargetPath, 0);

            IFeatureWorkspace pFeaClsWks = pWorkspace as IFeatureWorkspace;
            for (int i = 0; i < pMapDocument.MapCount; i++)
            {
                IMap map = pMapDocument.Map[i];
                for (int j = 0; j < map.LayerCount; j++)
                {
                    ILayer layer = map.Layer[j] as IFeatureLayer;
                    ICompositeLayer cpLayer = map.Layer[j] as ICompositeLayer;
                    IRasterLayer rLayer = map.Layer[j] as IRasterLayer;
                    if (rLayer != null)
                    {
                        IRasterLayer pFeaLyr = rLayer as IRasterLayer;
                        pFeaLyr.Name = "Kết quả quan trắc tiếng ổn " + DateTime.Now.AddHours(-1).ToString("dd/MM/yyyy") + " khung giờ " + DateTime.Now.AddHours(-1).Hour + " giờ";
                        pFeaLyr.CreateFromDataset(rasterDataset);
                    }
                    if (layer != null)
                    {
                        IFeatureLayer pFeaLyr = layer as IFeatureLayer;
                        string sDsName = ((pFeaLyr as IDataLayer).DataSourceName as IDatasetName).Name;
                        if ((pWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, sDsName))
                        {
                            switch (sDsName)
                            {
                                case "DiemDiaDanh":
                                case "DiaPhanCapXa":
                                case "DuongBinhDo":
                                case "DuongDiaGioiCapXa":
                                case "DuongDiaGioiCapHuyen":
                                case "DuongDiaGioiCapTinh":
                                case "MatDuongBo":
                                case "VungThuyHe":
                                    pFeaLyr.FeatureClass = pFeaClsWks.OpenFeatureClass(sDsName);
                                    pFeaLyr.Name = pFeaLyr.FeatureClass.AliasName;
                                    break;
                                default:
                                    pFeaLyr.FeatureClass = pFeaClsWks.OpenFeatureClass(sDsName);
                                    pFeaLyr.Name = pFeaLyr.FeatureClass.AliasName;// + " " + kyTinh;
                                    break;
                            }

                        }
                    }
                    if (cpLayer != null)
                    {
                        for (int k = 0; k < cpLayer.Count; k++)
                        {
                            layer = cpLayer.Layer[k];
                            IFeatureLayer pFeaLyr = cpLayer.Layer[k] as IFeatureLayer;
                            string sDsName = ((pFeaLyr as IDataLayer).DataSourceName as IDatasetName).Name;
                            if ((pWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, sDsName))
                            {
                                pFeaLyr.FeatureClass = pFeaClsWks.OpenFeatureClass(sDsName);
                                switch (sDsName)
                                {
                                    case "DuongDiaGioiCapXa":
                                    case "DuongDiaGioiCapHuyen":
                                    case "DuongDiaGioiCapTinh":
                                        pFeaLyr.Name = pFeaLyr.FeatureClass.AliasName;
                                        break;
                                    default:
                                        pFeaLyr.Name = pFeaLyr.FeatureClass.AliasName;// + " " + kyTinh;
                                        break;
                                }

                            }
                        }
                    }
                }
            }
            pMapDocument.Save(true, true);

            pMapDocument.Close();


            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pMapDocument);
        }
        */
    }
}