using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Carto;
using DevExpress.XtraBars;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using FrameWork.Core.MapInterfaces;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Threading;
using NOISE_APP.Forms;
using NOISE_APP.Controls.MolarMap;
using FrameWork.Map.Utilities;
using System.Configuration;
using FrameWork.Data.DB;
using Noise.Common.GIS.Classes;
using ESRI.ArcGIS.Geoprocessing;
using System.IO;
using System.Runtime.InteropServices;
using NOISE_APP.Classes;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ESRI.ArcGIS.esriSystem;
using System;
using ESRI.ArcGIS.DataSourcesRaster;

namespace NOISE_APP.Controls
{
    public partial class CtrNoiseMapMain : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields
        private XtraForm mForm;

        private BarButtonItem mCurrentTool;

        private IWorkspaceFactory2 mInMemWsFactory;
        private IWorkspaceFactory2 mGDBWsFactory;
        private IRasterWorkspaceEx mGDBRasterWs;
        private IRasterWorkspaceEx mInMemRasterWs;
        private IWorkspaceName2 mInMemWsName;
        private IWorkspaceName2 mInMemRasterWsName;
        private IWorkspaceName2 mGDBWsName;
        private IFeatureWorkspace mInMemWs;
        private IFeatureWorkspace mGDBWs;
        private IWorkspaceEdit2 mGDBWsEdit;
        private IWorkspaceEdit2 mGDBWsSeverEdit;
        private IFeatureWorkspace mGDBWsSever;

        private IWorkspaceFactory2 mInMemRasterWsFactory;

        private GroupLayer mMapLayerGroup;
        private GroupLayer mBaseLayerGroup;


        private IToolbarAction mToolBarActions;

        private CancellationTokenSource mCancelTasks;
        private CancellationToken mCancelToken;

        private _ProgressCancellable.Locker mLocker;

        private BackgroundWorker mBgWorker;

        private IRasterRadius mIDWRasterRadius;

        private CtrTOCControl mTOCControl;
        private CtrIdentify mIdentifyControl;
        private frmIdentify mIdentifyForm;

        private IQueryFilter mQueryFilter;

        public List<string> COMExceptionString = new List<string>();
        #endregion
        private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        #region Constructor
        public CtrNoiseMapMain()
        {
            InitializeComponent();
            //

        }
        #endregion

        #region Controls Events
        private void AxMap_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            bbItemScale.Caption = $"Tỷ lệ 1 : {string.Format("{0:n0}", axMap.MapScale)}";
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
                    //mIdentifyControl.IdentifyInfo(e.x, e.y);
                    mIdentifyForm.IdentifyInfo(e.x, e.y);
                    mIdentifyForm.ShowDialog();
                    //dockIdentify.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    //    if (dockIdentify.FloatForm != null)
                    //        dockIdentify.FloatForm.Size = new Size(400, 400);
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

        private void bbToolbar_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (e.Item.Tag?.ToString())
            {
                case nameof(BaseMapAction_Layout.FullExtent):
                    mToolBarActions.FullExtent();
                    axMap.ActiveView.Refresh();
                    break;
                case nameof(BaseMapAction_Layout.BackExtent):
                    mToolBarActions.BackAction();
                    break;
                case nameof(BaseMapAction_Layout.NextExtent):
                    mToolBarActions.NextAction();
                    break;
                case nameof(BaseMapAction_Layout.Refresh):
                    axMap.Map.ClearSelection();
                    axMap.ActiveView.Refresh();
                    break;
                default:
                    SetTool((BarButtonItem)e.Item);
                    break;
            }
        }


        private void btnShowData_Click(object sender, EventArgs e)
        {
            //var node = tlMap.FocusedNode;
            //if (node != null)
            //{
            //    EntityMolarMap map = node.Tag as EntityMolarMap;
            //    if (map == null)
            //        return;
            //    AddMapLayer(map.Id);
            //    //
            //    dockMain.ActivePanel = dockTOC;
            //}
        }
        #endregion
        private string RasterName = string.Empty;
        private bool _Created = false;
        #region Private
        private bool LoadMap()
        {

            //mForm = (XtraForm)this.ParentForm;
            ////
            //mGDBWsFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;
            //mGDBWs = mGDBWsFactory.OpenFromFile(_MolarMain.Instance.GDBPath, 0) as IFeatureWorkspace;
            //mGDBWsName = (IWorkspaceName2)((IDataset)mGDBWs).FullName;
            //mGDBWsEdit = (IWorkspaceEdit2)mGDBWs;
            ////
            //mInMemWsFactory = new ESRI.ArcGIS.DataSourcesGDB.InMemoryWorkspaceFactory() as IWorkspaceFactory2;
            //mInMemWsName = mInMemWsFactory.Create("", $"{MolarGISCommon.RandomWsName(3)}_Ws", null, 0) as IWorkspaceName2;
            //mInMemWs = (mInMemWsName as IName).Open() as IFeatureWorkspace;
            ////
            //// mInMemRasterWsFactory = new esriArcGISUri.data() as IWorkspaceFactory2;
            //mInMemRasterWsName = mInMemWsFactory.Create("", $"{MolarGISCommon.RandomWsName(3)}_Ws", null, 0) as IWorkspaceName2;
            //mInMemRasterWs = (mInMemRasterWsName as IName).Open() as IRasterWorkspaceEx;
            ////
            //mGDBRasterWs = mGDBWsFactory.OpenFromFile(_MolarMain.Instance.GDBPath, 0) as IRasterWorkspaceEx;
            ////
            //IFeatureLayer duongDiaGioiCapHuyen = new FeatureLayer()
            //{
            //    FeatureClass = mGDBWs.OpenFeatureClass("DuongDiaGioiCapHuyen"),
            //    Name = "Đường địa giới cấp Huyện"
            //};
            //IFeatureLayer duongDiaGioiCapXa = new FeatureLayer()
            //{
            //    FeatureClass = mGDBWs.OpenFeatureClass("DuongDiaGioiCapXa"),
            //    Name = "Đường địa giới cấp Xã"
            //};
            //IFeatureLayer diaPhanHuyen = new FeatureLayer()
            //{
            //    FeatureClass = mGDBWs.OpenFeatureClass("DiaPhanCapHuyen"),
            //    Name = "Địa phận Huyện"
            //};
            //IFeatureLayer diaPhanXa = new FeatureLayer()
            //{
            //    FeatureClass = mGDBWs.OpenFeatureClass("PhanVungDiaHinh"),
            //    Name = "Địa phận Xã"
            //};
            //IFeatureLayer vungThuyHe = new FeatureLayer()
            //{
            //    FeatureClass = mGDBWs.OpenFeatureClass("VungThuyHe"),
            //    Name = "Vùng thủy hệ"
            //};
            ////
            //mBaseLayerGroup = new GroupLayer();
            //mBaseLayerGroup.Add(vungThuyHe);
            //mBaseLayerGroup.Add(diaPhanXa);
            //mBaseLayerGroup.Add(diaPhanHuyen);
            //mBaseLayerGroup.Add(duongDiaGioiCapXa);
            //mBaseLayerGroup.Add(duongDiaGioiCapHuyen);
            //mBaseLayerGroup.Name = "Bản đồ nền";
            //axMap.AddLayer(mBaseLayerGroup, 0);
            ////
            //(diaPhanHuyen as IGeoFeatureLayer).Renderer = (IFeatureRenderer)new SimpleRenderer()
            //{
            //    Symbol = (ISymbol)new SimpleFillSymbol()
            //    {
            //        Outline = new SimpleLineSymbol()
            //        {
            //            Style = esriSimpleLineStyle.esriSLSSolid,
            //            Width = 4,
            //            Color = CommonUtils.GetIColor(255, 190, 232)
            //        },
            //        Style = esriSimpleFillStyle.esriSFSNull
            //    }
            //};
            //(diaPhanXa as IGeoFeatureLayer).Renderer = (IFeatureRenderer)new SimpleRenderer()
            //{
            //    Symbol = (ISymbol)new SimpleFillSymbol()
            //    {
            //        Style = esriSimpleFillStyle.esriSFSNull
            //    }
            //};
            //
            mToolBarActions = new ToolbarUtilites(axMap);
            //
            mIdentifyControl = new CtrIdentify(axMap.Map) { Dock = DockStyle.Fill };
            mIdentifyForm = new frmIdentify(axMap.Map);
            ((IActiveViewEvents_Event)axMap.Map).ItemAdded += (e) =>
            {
                mIdentifyForm.UpdateLayer();
            };
            ((IActiveViewEvents_Event)axMap.Map).ItemDeleted += (e) =>
            {
                mIdentifyForm.UpdateLayer();
            };
            ((IActiveViewEvents_Event)axMap.Map).ItemReordered += (sender, e) =>
            {
                mIdentifyForm.UpdateLayer();
            };
            dockIdentify_Container.Controls.Add(mIdentifyControl);
            //

            return true;
        }

        private bool LoadTOC()
        {
            mTOCControl = new CtrTOCControl(axMap);
            mTOCControl.Dock = DockStyle.Fill;
            dockTOC.Controls.Add(mTOCControl);
            return true;
        }

        private void SetGDBServer()
        {
            string server = ConfigurationSettings.AppSettings["SERVER"];
            string instance = ConfigurationSettings.AppSettings["INSTANCE"];
            string database = ConfigurationSettings.AppSettings["DATABASE"];
            string user = ConfigurationSettings.AppSettings["USERNAME"];
            string password = ConfigurationSettings.AppSettings["PASSWORD"];
            string version = ConfigurationSettings.AppSettings["VERSION"];

            mGDBWsSever = ESRIDBHelper.Instance.OpenSDEDBWorkspace(server, user, password, database) as IFeatureWorkspace;
        }

        private bool LoadData()
        {
            mQueryFilter = new QueryFilter();
            //
            mLocker = new _ProgressCancellable.Locker();
            mCancelTasks = new CancellationTokenSource();
            mCancelToken = mCancelTasks.Token;
            //
            // tlMap.FocusedNode.ExpandAll();
            tlMap.OptionsView.ShowColumns = false;
            tlMap.OptionsView.ShowIndicator = false;
            tlMap.OptionsView.ShowVertLines = false;
            tlMap.OptionsView.ShowHorzLines = false;
            tlMap.OptionsBehavior.Editable = true;
            tlMap.OptionsSelection.EnableAppearanceFocusedCell = false;
            tlMap.StateImageList = imgCollection;
            //
            bbItemSelect.Tag = BaseMapAction.Select;
            bbItemZoomIn.Tag = BaseMapAction.ZoomIn;
            bbItemZoomout.Tag = BaseMapAction.ZoomOut;
            bbItemPan.Tag = BaseMapAction.Pan;
            bbItemFullExtent.Tag = BaseMapAction_Layout.FullExtent;
            bbItemPrevExtent.Tag = BaseMapAction_Layout.BackExtent;
            bbItemNextExtent.Tag = BaseMapAction_Layout.NextExtent;
            bbItemRefresh.Tag = BaseMapAction_Layout.Refresh;
            bbItemIdentify.Tag = BaseMapAction.Identify;
            //
            mCurrentTool = bbItemSelect;
            //
            mIDWRasterRadius = new RasterRadius();
            mIDWRasterRadius.SetVariable(12);
            //
            //dockMapConfig.Dock = dockIdentify.Dock = DevExpress.XtraBars.Docking.DockingStyle.Float;
            //dockMapConfig.Hide();
            dockIdentify.Hide();
            //
            return true;
        }
        private bool LoadTreeMap()
        {
            // Create three columns.
            tlMap.BeginUpdate();
            //
            tlMap.EndUpdate();
            //
            tlMap.BeginUnboundLoad();
            //using (var scope = _MolarMain.DI.Container.BeginLifetimeScope())
            //{
            //    var dbFactory = scope.Resolve<IDbFactory>();

            //    using (var session = dbFactory.Create<IAppSession>())
            //    {
            //        TreeListNode baseNode = null;
            //        var mapGroups = session.Find<EntityMolarMapGroup>(statement => statement
            //            .Include<EntityMolarMap>(join => join.LeftOuterJoin())
            //            .Include<EntityMolarMapDomain>(join => join.LeftOuterJoin())
            //            .Where($"{nameof(EntityMolarMap.isShow)} = 1"));
            //        foreach (EntityMolarMapGroup mapGroup in mapGroups)
            //        {
            //            mMolarMaps.AddRange(mapGroup.Maps);
            //            //
            //            TreeListNode mapGroupNode = tlMap.AppendNode(new object[] { mapGroup.MapGroupName }, baseNode);
            //            mapGroupNode.StateImageIndex = 0;
            //            foreach (EntityMolarMap map in mapGroup.Maps)
            //            {
            //                tlMap.AppendNode(new object[] { map.MapName }, mapGroupNode, map).StateImageIndex = 1;
            //            }
            //        }
            //    }
            //}

            tlMap.EndUnboundLoad();
            tlMap.ExpandAll();
            //
            return true;
        }
        private void BindEvents()
        {
            axMap.OnMouseDown += AxMap_OnMouseDown;
            axMap.OnMouseUp += AxMap_OnMouseUp;
            axMap.OnMouseMove += AxMap_OnMouseMove;
            axMap.OnDoubleClick += AxMap_OnDoubleClick;
            axMap.Resize += AxMap_Resize;
            axMap.OnExtentUpdated += AxMap_OnExtentUpdated;
            ///
            bbItemSelect.ItemClick += bbToolbar_Click;
            bbItemZoomIn.ItemClick += bbToolbar_Click;
            bbItemZoomout.ItemClick += bbToolbar_Click;
            bbItemPan.ItemClick += bbToolbar_Click;
            bbItemFullExtent.ItemClick += bbToolbar_Click;
            bbItemPrevExtent.ItemClick += bbToolbar_Click;
            bbItemNextExtent.ItemClick += bbToolbar_Click;
            bbItemIdentify.ItemClick += bbToolbar_Click;
            //
            //btnShowData.Click += btnShowData_Click;
            //
            bbiLoadMap.ItemClick += (sender, e) =>
            {
                OpenFileDialog o = new OpenFileDialog();
                o.DefaultExt = "*.mxd";
                o.Filter = "Map exchange document (*.mxd)|*.mxd|All files (*.*)|*.*";
                if (o.ShowDialog() == DialogResult.OK)
                    axMap.LoadMxFile(o.FileName);
            };
            //
            bbExportShapeFile.ItemClick += (sender, e) =>
            {
                RasterName = "Raster_20211002_16";
                NoiseGISCommon.RasterToPolygon((IWorkspace)mGDBWs, RasterName, @"E:\20210825\CSDLTest.Test.shp");
                //if (_Created && !string.IsNullOrEmpty(RasterName))
                //{

                //}
            };
            tlMap.FocusedNodeChanged += (sender, e) =>
            {
                if (e.Node.Tag == null)
                    return;
                //if (e.Node.Tag.GetType() == typeof(EntityMolarMap))
                //{
                //    EntityMolarMap m = e.Node.Tag as EntityMolarMap;
                //    LoadMapConfig(m.Id);
                //    //
                //    dockMapConfig.Show();
                //}
            };
            //
            bbiCreateLayer.ItemClick += (sender, e) =>
            {
                //using (var f = new FrmCreateLayerFromTbl(axMap.SpatialReference))
                //{
                //    if (f.ShowDialog() == DialogResult.OK)
                //    {
                //        IFeatureLayer ll = new FeatureLayer()
                //        {
                //            FeatureClass = f.FC
                //        };
                //        axMap.AddLayer(ll);
                //    }
                //}
            };
            bbiInterpolate.ItemClick += (sender, e) =>
            {
                //using (var f = new FrmInterpolate())
                //{
                //    if (f.ShowDialog() == DialogResult.OK)
                //    {
                //        IRasterLayer rasterLayer = new RasterLayer();
                //        rasterLayer.CreateFromRaster(f.Raster);
                //        axMap.AddLayer(rasterLayer);
                //    }
                //}
            };
            bbConfigPhanVung.ItemClick += (sender, e) =>
            {
                //using (var f = new FrmConfigTopographic())
                //{
                //    if (f.ShowDialog() == DialogResult.OK)
                //    {

                //    }
                //};
            };
            bbiAddGdb.ItemClick += (sender, e) =>
            {

            };
            bbiAddShapefile.ItemClick += (sender, e) =>
            {
                //using (var f = new FrmCreateShapeFile(axMap.SpatialReference))
                //{
                //    if (f.ShowDialog() == DialogResult.OK)
                //    {
                //        axMap.AddShapeFile(f.pathFolder, f.shapeName);
                //    }
                //}
            };
        }

        private void SetTool(BarButtonItem btn)
        {
            mToolBarActions.MapAction = BaseMapAction.None;
            //
            if (btn == null)
                return;
            if (btn.ButtonStyle.Equals(BarButtonStyle.Check))
                btn.Down = true;
            if (mCurrentTool == null)
                mCurrentTool = btn;
            else if (!mCurrentTool.Equals(btn))
            {
                mCurrentTool.Down = false;
                mCurrentTool = btn;
            }
            bsItemToolIcon.Glyph = icArcToolbar.Images[btn.ImageIndex];
            int tag = (int)btn.Tag;
            switch (tag)
            {
                case (int)BaseMapAction.None:
                    mToolBarActions.MapAction = BaseMapAction.None;
                    this.lblActionMessage.Caption = "";
                    break;
                case (int)BaseMapAction.Select:
                    mToolBarActions.MapAction = BaseMapAction.Select;
                    this.lblActionMessage.Caption = "Lựa chọn đối tượng bằng cách nhấn hoặc giữ và kéo con trỏ chuột... ";
                    break;
                case (int)BaseMapAction.SelectByLocation:
                    mToolBarActions.MapAction = BaseMapAction.SelectByLocation;
                    this.lblActionMessage.Caption = "Lựa chọn đối tượng bằng cách nhấn hoặc giữ và kéo con trỏ chuột... ";
                    break;
                case (int)BaseMapAction.ZoomIn:
                    mToolBarActions.MapAction = BaseMapAction.ZoomIn;
                    this.lblActionMessage.Caption = "Phóng to bản đồ bằng cách nhấn hoặc giữ và kéo con trỏ chuột...";
                    break;
                case (int)BaseMapAction.ZoomOut:
                    mToolBarActions.MapAction = BaseMapAction.ZoomOut;
                    this.lblActionMessage.Caption = "Thu nhỏ bản đồ bằng cách nhấn hoặc giữ và kéo con trỏ chuột...";
                    break;
                case (int)BaseMapAction.Pan:
                    mToolBarActions.MapAction = BaseMapAction.Pan;
                    this.lblActionMessage.Caption = "Di chuyển bản đồ...";
                    break;
                case (int)BaseMapAction.Identify:
                    mToolBarActions.MapAction = BaseMapAction.Identify;
                    this.lblActionMessage.Caption = "Di chuyển con trỏ chuột lên đối tượng cần xem thuộc tính...";
                    break;
                case (int)BaseMapAction.Distance:
                    mToolBarActions.MapAction = BaseMapAction.Distance;
                    break;
                default:
                    break;
            }
        }
        private void StartEditOp()
        {
            if (mGDBWsEdit.IsInEditOperation)
                mGDBWsEdit.AbortEditOperation();
            if (mGDBWsEdit.IsBeingEdited())
                mGDBWsEdit.StopEditing(false);
            mGDBWsEdit.StartEditing(false);
            mGDBWsEdit.StartEditOperation();
        }
        private void StopEditOp(bool saveEdits)
        {
            //if (mGDBWsEdit.IsInEditOperation)
            //    mGDBWsEdit.AbortEditOperation();
            //if (mGDBWsEdit.IsBeingEdited())
            //    mGDBWsEdit.StopEditing(saveEdits);
            mGDBWsEdit.StopEditOperation();
            mGDBWsEdit.StopEditing(saveEdits);
        }
        private IEnumerable<IFeature> loopFeatures(IFeatureClass featureClass, IQueryFilter q = null)
        {
            IFeature f = null;
            IFeatureCursor cursor = featureClass.Search(q, false);
            while ((f = cursor.NextFeature()) != null)
            {
                yield return f;
            }
        }
        #endregion

        private List<NOISE> GetDataFromSever(DateTime ngayTinh, string gioTinh)
        {
            //<add name="ConnectionString" connectionString="Server=DESKTOP-1V8JKKD;Database=NOISE_CONFIG;User Id=sa;Password=Abc@123456;MultipleActiveResultSets=true" />
            DataSet ds = new DataSet();
            var result = new List<NOISE>();
            using (var conn = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (var cmd = new SqlCommand())
                    {
                        //string startHour = "000000";
                        //string endHour = "003000";
                        //var dateCal = new DateTime(2021, 09, 01);
                        var today = DateTime.Now.AddHours(-8);

                        //20210610
                        //switch (today.Hour)
                        //{
                        //    case 0:
                        //        startHour = "000000";
                        //        endHour = "003000";
                        //        break;
                        //    case 1:
                        //        startHour = "010000";
                        //        endHour = "013000";
                        //        break;
                        //    case 2:
                        //        startHour = "020000";
                        //        endHour = "023000";
                        //        break;
                        //    case 3:
                        //        startHour = "030000";
                        //        endHour = "033000";
                        //        break;
                        //    case 4:
                        //        startHour = "040000";
                        //        endHour = "043000";
                        //        break;
                        //    case 5:
                        //        startHour = "090000";
                        //        endHour = "110000";
                        //        break;
                        //    case 6:
                        //        startHour = "110000";
                        //        endHour = "130000";
                        //        break;
                        //    case 7:
                        //        startHour = "130000";
                        //        endHour = "150000";
                        //        break;
                        //    case 8:
                        //        startHour = "150000";
                        //        endHour = "170000";
                        //        break;
                        //    case 9:
                        //        startHour = "070000";
                        //        endHour = "090000";
                        //        break;
                        //    case 10:
                        //        startHour = "090000";
                        //        endHour = "110000";
                        //        break;
                        //    case 11:
                        //        startHour = "110000";
                        //        endHour = "130000";
                        //        break;
                        //    case 12:
                        //        startHour = "130000";
                        //        endHour = "150000";
                        //        break;
                        //    case 13:
                        //        startHour = "150000";
                        //        endHour = "170000";
                        //        break;
                        //    case 14:
                        //        startHour = "070000";
                        //        endHour = "090000";
                        //        break;
                        //    case 15:
                        //        startHour = "090000";
                        //        endHour = "110000";
                        //        break;
                        //    case 16:
                        //        startHour = "110000";
                        //        endHour = "130000";
                        //        break;
                        //    case 17:
                        //        startHour = "130000";
                        //        endHour = "150000";
                        //        break;
                        //    case 18:
                        //        startHour = "150000";
                        //        endHour = "170000";
                        //        break;
                        //    case 19:
                        //        startHour = "070000";
                        //        endHour = "090000";
                        //        break;
                        //    case 20:
                        //        startHour = "090000";
                        //        endHour = "110000";
                        //        break;
                        //    case 21:
                        //        startHour = "110000";
                        //        endHour = "130000";
                        //        break;
                        //    case 22:
                        //        startHour = "130000";
                        //        endHour = "150000";
                        //        break;
                        //    case 23:
                        //        startHour = "150000";
                        //        endHour = "170000";
                        //        break;
                        //    default:
                        //        break;
                        //}
                        /*
                        if (today.Minute < 30)
                        {
                            var hour = (today.Hour + 1) > 10 ? (today.Hour - 1).ToString() : "0" + (today.Hour - 1).ToString();
                            startHour = hour + "3000";
                            endHour = hour + "5900";
                        }
                        else
                        {
                            var hour = today.Hour > 10 ? today.Hour.ToString() : "0" + today.Hour.ToString();
                            startHour = hour + "3000";
                            endHour = hour + "5900";
                        }
                        */

                        string startTime = ngayTinh.Date.AddHours(-7).AddHours(Convert.ToDouble(gioTinh)).ToString("yyyyMMddHH") + "0000"; // today.ToString("yyyyMMddHH") + "0000";// startHour;
                        string endTime = ngayTinh.Date.AddHours(-7).AddHours(Convert.ToDouble(gioTinh)).ToString("yyyyMMddHH") + "5959";// today.ToString("yyyyMMddHH") + "5959";// endHour;

                        cmd.Connection = conn;

                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format(@"
                                SELECT n.ID, 
                                        d.DiaDiem,   
                                        sum(CAST(n.db as numeric(9,6)))/count(n.ID) as dB,
                                        sum(CAST(n.LONG as numeric(9,6)))/count(n.ID) as Long,
                                        sum(CAST(n.LAT as numeric(9,6)))/count(n.ID) as LAT	
                                        FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  
                                        WHERE n.ID <> ''  AND n.dB <> '' AND (ISNUMERIC(TIME) = 1 AND (CAST(n.TIME as DECIMAL(16,2)) < {0} AND CAST(n.TIME as DECIMAL(16,2)) > {1})) 
                                        group by n.ID, d.DiaDiem
                                ", endTime, startTime);


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                result.Add(new NOISE()
                                {
                                    dB = row["dB"].ToString(),
                                    ID = row["ID"].ToString(),
                                    DiaDiem = row["DiaDiem"].ToString(),
                                    LAT = row["LAT"].ToString(),
                                    LONG = row["LONG"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (COMException comEx)
                {
                    Save("Lấy dữ liệu", comEx.Message, "");
                }
                catch (Exception ex)
                {
                    Save("Lấy dữ liệu", ex.Message, ex.InnerException.Message);
                }
                finally
                {
                    conn.Close();
                }

                return result;
            }
        }
        public void Save(string content, string exceptionMessage, string stackTrace)
        {
            string path = Application.StartupPath + @"\\checkLog.txt";
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("*** Create by DoanhLV ***");
                        sw.WriteLine("--- Cuộn xuống dưới để thấy được dữ liệu mới nhất -------------------------------------------------");
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("Content: " + content);
                    sw.WriteLine("ExceptionMessage: " + exceptionMessage);
                    sw.WriteLine("ExceptionStackTrace: " + stackTrace);
                    sw.WriteLine("CreateTime: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"));
                    sw.WriteLine("-------------------------------------------------------------------------------------------------------");
                    sw.WriteLine("");
                }
            }
            catch
            {
            }
        }
        List<TBLConfig> GetDataForForm()
        {
            DataSet ds = new DataSet();
            var result = new List<TBLConfig>();
            using (var conn = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"SELECT [ID]
                                              ,[TenThamSo]
                                              ,[GiaTri]
                                              ,[Param_Config]
                                          FROM [TBLCONFIG]";


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {

                                result.Add(new TBLConfig()
                                {
                                    TenThamSo = row["TenThamSo"].ToString(),
                                    GiaTri = row["GiaTri"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (COMException comEx)
                {
                    Save("Lấy config", comEx.Message, "");
                }
                catch (Exception ex)
                {
                    Save("Lấy config", ex.Message, ex.InnerException.Message);
                }
                finally
                {
                    conn.Close();
                }
                return result;
            }
        }

        public void TinhToan(DateTime ngayTinhTime, string gioTinh)
        {
            mLocker = new _ProgressCancellable.Locker();
            mBgWorker = new BackgroundWorker();
            mBgWorker.WorkerSupportsCancellation = true;
            mBgWorker.DoWork += (sender, e) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        _ProgressCancellable.locker = ((_ProgressCancellable.Locker)e.Argument);
                        _NoiseMesageBox.ShowSplash((XtraForm)this.ParentForm, "Kết nối dữ liệu sever...", "", true);
                        _NoiseMesageBox.StartCounting();
                        _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.ShowProgress);
                        _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0);

                        _ProgressCancellable.ILocked locker = ((_ProgressCancellable.ILocked)e.Argument);

                        try
                        {



                            SetGDBServer();
                            if (mGDBWsSever == null)
                            {
                                return;
                            }
                            if (locker.IsCanceled)
                            {
                                _Created = false;
                                return;
                            }

                            var ngayTinh = ngayTinhTime.ToString("yyyyMMdd");
                            //var mxdResult = System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FOLDERMXDPATH"].ToString(), string.Format("{0}.mxd", "bando_" + ngayTinh + "_" + gioTinh));

                            var mxdResult = System.IO.Path.Combine(Application.StartupPath, "MXD_KQ", string.Format("{0}.mxd", "bando_" + ngayTinh + "_" + gioTinh));

                            if (System.IO.File.Exists(mxdResult))
                            {
                                _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.2);
                                _NoiseMesageBox.SetSplashCaption("Cấu hình dữ liệu hiển thị");
                                //NoiseGISCommon.SetDataSource(mxdResult, (IWorkspace)mGDBWsSever, null, null, ngayTinhTime.ToString("MM/dd/yyyy"), gioTinh);// ((IWorkspace)mGDBWsSever).PathName);
                                _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.5);
                                _NoiseMesageBox.SetSplashCaption("Load bản đồ");
                                axMap.LoadMxFile(mxdResult);
                                _Created = true;
                            }
                            else
                            {
                                //
                                var paramConfig = GetDataForForm();

                                var radius = Convert.ToDouble(paramConfig.FirstOrDefault(s => s.TenThamSo == "RADIUS_IDW").GiaTri);// 12;// string.IsNullOrEmpty(txtRad.Text) ? 12 : Convert.ToInt16(txtRad.Text);
                                var cellSize = Convert.ToDouble(paramConfig.FirstOrDefault(s => s.TenThamSo == "CELL_SIZE_IDW").GiaTri);
                                var cellHeight = Convert.ToDouble(paramConfig.FirstOrDefault(s => s.TenThamSo == "SIZE_HEIGHT").GiaTri);// 12;// string.IsNullOrEmpty(txtRad.Text) ? 12 : Convert.ToInt16(txtRad.Text);
                                var cellWidth = Convert.ToDouble(paramConfig.FirstOrDefault(s => s.TenThamSo == "SIZE_WIDTH").GiaTri);
                                try
                                {


                                    var thoiGianTinhToanHHMM = ngayTinh + "_" + gioTinh; //DateTime.Now.ToString("yyyyMMddHHmmss");// "";



                                    string thoiGianTinhToan = thoiGianTinhToanHHMM;// DateTime.Now.ToString("yyyyMMddHHmm");
                                    //string filegdb = paramConfig.FirstOrDefault(s => s.TenThamSo == "FILE_GDB").GiaTri;//) btnSelectGDB.EditValue.ToString();
                                    string filegdb = ConfigurationSettings.AppSettings["GDBPATH"]; //@"E:\20210825\CSDLTest\CSDL_ONhiemTiengOn.gdb";

                                    string tempFC = paramConfig.FirstOrDefault(s => s.TenThamSo == "FC_TEMP").GiaTri;

                                    var data = GetDataFromSever(ngayTinhTime, gioTinh);

                                    if (data.Count <= 1)
                                    {
                                        //txtProcess.Text += "\n\nKhông đủ dữ liệu " + data.Count;
                                        _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.2);
                                        _NoiseMesageBox.SetSplashCaption("Không đủ dữ liệu");
                                        Save("Lỗi dữ liệu", "Không có dữ liệu", "");
                                        return;
                                    }
                                    else
                                    {
                                        _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.2);
                                        _NoiseMesageBox.SetSplashCaption("Đã có dữ liệu");
                                    }

                                    if (locker.IsCanceled)
                                    {
                                        _Created = false;
                                        return;
                                    }

                                    mGDBWsFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                                    mGDBWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IFeatureWorkspace;
                                    mGDBWsName = (IWorkspaceName2)((IDataset)mGDBWs).FullName;
                                    mGDBWsEdit = (IWorkspaceEdit2)mGDBWs;

                                    mInMemWsFactory = new ESRI.ArcGIS.DataSourcesGDB.InMemoryWorkspaceFactory() as IWorkspaceFactory2;
                                    mInMemWsName = mInMemWsFactory.Create("", $"{Guid.NewGuid().ToString()}_Ws", null, 0) as IWorkspaceName2;
                                    mInMemWs = (mInMemWsName as IName).Open() as IFeatureWorkspace;


                                    mInMemRasterWsName = mInMemWsFactory.Create("", $"{Guid.NewGuid().ToString()}_Ws", null, 0) as IWorkspaceName2;
                                    mInMemRasterWs = (mInMemRasterWsName as IName).Open() as IRasterWorkspaceEx;

                                    mGDBRasterWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IRasterWorkspaceEx;
                                    //chuyển bảng dữ liệu => layer

                                    mGDBWsSeverEdit = (IWorkspaceEdit2)mGDBWsSever;

                                    IFields fields = new Fields();
                                    IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                                    NoiseGISCommon.AddFieldToTable(fieldsEdit, "ID", "ID", esriFieldType.esriFieldTypeString);
                                    NoiseGISCommon.AddFieldToTable(fieldsEdit, "tenDiemQuanTrac", "tenDiemQuanTrac", esriFieldType.esriFieldTypeString);
                                    NoiseGISCommon.AddFieldToTable(fieldsEdit, "dB", "dB", esriFieldType.esriFieldTypeDouble);
                                    NoiseGISCommon.AddFieldToTable(fieldsEdit, "LONG", "LONG", esriFieldType.esriFieldTypeDouble);
                                    NoiseGISCommon.AddFieldToTable(fieldsEdit, "LA", "LA", esriFieldType.esriFieldTypeDouble);

                                    string strBangDuLieuDongBo = "BangDuLieuDongBo_" + thoiGianTinhToan;

                                    if (NoiseGISCommon.DoesTableExist((IWorkspace)mGDBWs, strBangDuLieuDongBo, false))
                                    {
                                        IFeatureWorkspace ws2 = (IFeatureWorkspace)mGDBWs;
                                        ITable tbl = ws2.OpenTable(strBangDuLieuDongBo);
                                        ((IDataset)tbl).Delete();
                                    }

                                    ITable tblBangDuLieu = NoiseGISCommon.CreateTable(mGDBWs, strBangDuLieuDongBo, fields);

                                    foreach (var item in data)
                                    {
                                        var newRow = tblBangDuLieu.CreateRow();
                                        newRow.Value[0] = item.ID;
                                        newRow.Value[1] = item.DiaDiem;
                                        newRow.Value[2] = item.dB;
                                        newRow.Value[3] = item.LONG;
                                        newRow.Value[4] = item.LAT;
                                        newRow.Store();
                                    }

                                    IFeatureLayer flBangDuLieu = new FeatureLayer()
                                    {
                                        FeatureClass = NoiseGISCommon.XYTableToFeatureClass(tblBangDuLieu, "LONG", "LA", "", NoiseGISCommon.CreateSpatialRefGCS(esriSRGeoCSType.esriSRGeoCS_WGS1984))
                                    };
                                    if (locker.IsCanceled)
                                    {
                                        _Created = false;
                                        return;
                                    }
                                    _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.3);
                                    _NoiseMesageBox.SetSplashCaption("1. Hoàn thành đồng bộ dữ liệu");
                                    //txtProcess.Text += "\n 1. Hoàn thành đồng bộ dữ liệu";

                                    string tranferName = "DuLieuDongBo_tranf_" + thoiGianTinhToan;

                                    var flTramDo = mGDBWsSever.OpenFeatureClass("TramQuanTracCoDinh");

                                    int maTramOrgIdx = flBangDuLieu.FeatureClass.FindField("ID");
                                    int TenTramOrgIdx = flBangDuLieu.FeatureClass.FindField("tenDiemQuanTrac");
                                    int dBOrgIdx = flBangDuLieu.FeatureClass.FindField("dB");


                                    int maTramDesIdx = flTramDo.FindField("ID");
                                    int TenTramDesIdx = flTramDo.FindField("tenDiemQuanTrac");
                                    int thoiGianDesIdx = flTramDo.FindField("thoiGianQuanTrac");
                                    int ngayDesIdx = flTramDo.FindField("ngayQuanTrac");
                                    int dBDesIdx = flTramDo.FindField("dB");

                                    IFeature feature = null;
                                    IFeatureCursor featureCursor = flBangDuLieu.FeatureClass.Search(null, false);
                                    /*
                                    try
                                    {
                                        if (mGDBWsSeverEdit.IsInEditOperation)
                                            mGDBWsSeverEdit.AbortEditOperation();
                                        if (mGDBWsSeverEdit.IsBeingEdited())
                                            mGDBWsSeverEdit.StopEditing(false);
                                        mGDBWsSeverEdit.StartEditing(false);
                                        mGDBWsSeverEdit.StartEditOperation();
                                        while ((feature = featureCursor.NextFeature()) != null)
                                        {
                                            IFeature newFeature = flTramDo.CreateFeature();
                                            newFeature.Value[maTramDesIdx] = feature.Value[maTramOrgIdx].ToString();
                                            newFeature.Value[TenTramDesIdx] = feature.Value[TenTramOrgIdx].ToString();
                                            newFeature.Value[dBDesIdx] = feature.Value[dBOrgIdx].ToString();
                                            newFeature.Value[ngayDesIdx] = DateTime.Now.AddHours(-1).ToString("MM/dd/yyyy");
                                            newFeature.Value[thoiGianDesIdx] = DateTime.Now.AddHours(-2).Hour;
                                            newFeature.Shape = feature.Shape;
                                            newFeature.Store();
                                        }
                                        mGDBWsSeverEdit.StopEditOperation();
                                        mGDBWsSeverEdit.StopEditing(true);
                                    }
                                    catch (Exception)
                                    {

                                        //throw;
                                    }
                                    */

                                    NoiseGISCommon.GeographicTranfer((IWorkspace)mGDBWs, flBangDuLieu.FeatureClass, tranferName);

                                    IFeatureLayer flDiemDuLieuSave = new FeatureLayer()
                                    {
                                        FeatureClass = mGDBWs.OpenFeatureClass(tranferName)
                                    };




                                    IFeatureLayer flRTemp = new FeatureLayer()
                                    {
                                        FeatureClass = mGDBWs.OpenFeatureClass(tempFC)
                                    };

                                    string FishnetName = "fisnet_" + thoiGianTinhToan;

                                    IFeatureClass fcFishnet = NoiseGISCommon.CreateFishnet((IWorkspace)mGDBWs, FishnetName, flRTemp, cellHeight, cellWidth);

                                    IFeatureLayer fcFishnetLabel = new FeatureLayer()
                                    {
                                        FeatureClass = mGDBWs.OpenFeatureClass(FishnetName + "_label")
                                    };
                                    if (locker.IsCanceled)
                                    {
                                        _Created = false;
                                        return;
                                    }
                                    _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.5);
                                    _NoiseMesageBox.SetSplashCaption("2. Hoàn thành tạo lưới tính toán tiếng ồn tổng hợp");
                                    //txtProcess.Text += "\n 2. Hoàn thành tạo lưới tính toán tiếng ồn tổng hợp";

                                    var strBangDuLieuKhoangCachSave = "BangDuLieuKhoangCachSave_" + thoiGianTinhToan;

                                    ITable tblDistance = NoiseGISCommon.PointDistance((IWorkspace)mGDBWs, FishnetName + "_label", tranferName, strBangDuLieuKhoangCachSave);

                                    _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.6);
                                    _NoiseMesageBox.SetSplashCaption("3. Hoàn thành tính toán khoảng cách từ các trạm tới các điểm lưới");
                                    //txtProcess.Text += "\n 3. Hoàn thành tính toán khoảng cách từ các trạm tới các điểm lưới";

                                    if (tblDistance == null)
                                    {
                                        MessageBox.Show("Lỗi rồi");
                                        return;
                                    }

                                    NoiseGISCommon.StopEditOp(true, mGDBWsEdit);

                                    int idxCheck = tblDistance.FindField("TongHop");
                                    if (idxCheck < 0)
                                    {
                                        ISchemaLock schemaLock = (ISchemaLock)tblDistance;
                                        try
                                        {
                                            schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                                            // Add your field.
                                            IFieldEdit2 field = new FieldClass() as IFieldEdit2;
                                            field.Name_2 = "TongHop";
                                            field.Type_2 = esriFieldType.esriFieldTypeDouble;
                                            field.DefaultValue_2 = "TongHop";
                                            tblDistance.AddField(field);
                                        }
                                        catch (System.Runtime.InteropServices.COMException comExc)
                                        {
                                            // Handle the exception appropriately for the application.
                                        }
                                        finally
                                        {
                                            // Demote the exclusive lock to a shared lock.
                                            schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                                        }
                                    }


                                    ITable tblBangDulieu = mGDBWs.OpenTable(tranferName);

                                    ITable flJoinDistance = NoiseGISCommon.TableToTableJoin(tblDistance, $"NEAR_FID", tblBangDulieu, "OBJECTID", esriRelCardinality.esriRelCardinalityOneToMany, esriJoinType.esriLeftInnerJoin, null);

                                    int IDTramIdx = fcFishnetLabel.FeatureClass.FindField("OID");
                                    int oInputFidIdx = flJoinDistance.FindField(string.Format("{0}.INPUT_FID", strBangDuLieuKhoangCachSave));
                                    int onearFidIdx = flJoinDistance.FindField(string.Format("{0}.NEAR_FID", strBangDuLieuKhoangCachSave));
                                    var distanceIdx = flJoinDistance.FindField(string.Format("{0}.DISTANCE", strBangDuLieuKhoangCachSave));
                                    var tongIDx = flJoinDistance.FindField(string.Format("{0}.TongHop", strBangDuLieuKhoangCachSave));
                                    var dbIdx = flJoinDistance.FindField(string.Format("{0}.dB", tranferName));

                                    NoiseGISCommon.calculate(flJoinDistance, string.Format("{0}.TongHop", strBangDuLieuKhoangCachSave), string.Format("math.pow(10,(!{0}.dB! - 20*math.log10(!{1}.DISTANCE!))/10)", tranferName, strBangDuLieuKhoangCachSave));
                                    if (locker.IsCanceled)
                                    {
                                        _Created = false;
                                        return;
                                    }
                                    _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.8);
                                    _NoiseMesageBox.SetSplashCaption("4. Hoàn thành tính toán dữ liệu tiếng ồn tổng hợp trên các điểm lưới");
                                    //txtProcess.Text += "\n 4. Hoàn thành tính toán dữ liệu tiếng ồn tổng hợp trên các điểm lưới";

                                    IGpValueTableObject valTbl = new GpValueTableObjectClass();
                                    valTbl.SetColumns(2);

                                    object row1 = "TongHop Sum";
                                    object row2 = "INPUT_FID Count";
                                    valTbl.AddRow(ref row1);
                                    valTbl.AddRow(ref row2);

                                    string tblSummary = "TBLSummary_" + thoiGianTinhToan;

                                    NoiseGISCommon.SummaryStatic((IWorkspace)mGDBWs, strBangDuLieuKhoangCachSave, valTbl, "INPUT_FID", tblSummary);

                                    ITable tblDuLieu = mGDBWs.OpenTable(tblSummary);

                                    NoiseGISCommon.AddFieldToTable(tblDuLieu, "total");

                                    NoiseGISCommon.calculate(tblDuLieu, "total", "10*math.log10( !SUM_TongHop! )");

                                    ////txtProcess.Text += "\n Hoàn thành tính toán lớp lưới";

                                    IGeoFeatureLayer joinSummaryWithTestLabel = NoiseGISCommon.TableToLayerJoin(tblDuLieu, "INPUT_FID", fcFishnetLabel, "OID", esriRelCardinality.esriRelCardinalityOneToOne, esriJoinType.esriLeftInnerJoin, "");

                                    IGeoDataset geoDataset = NoiseGISCommon.CreateGeoDataset(joinSummaryWithTestLabel.DisplayFeatureClass, string.Format("{0}.total", tblSummary));

                                    IRaster idwRaster = NoiseGISCommon.IDW((IWorkspace)mGDBWs, geoDataset, 2, cellSize);
                                    if (locker.IsCanceled)
                                    {
                                        _Created = false;
                                        return;
                                    }
                                    _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 0.9);
                                    _NoiseMesageBox.SetSplashCaption("5. Hoàn thành nội suy Raster tiếng ồn tổng hợp");
                                    //txtProcess.Text += "\n 5. Hoàn thành nội suy Raster tiếng ồn tổng hợp";

                                    var rasterName = "Raster_";

                                    rasterName = "Raster_TinhToan_" + thoiGianTinhToan;

                                    RasterName = rasterName;



                                    NoiseGISCommon.Clip((IWorkspace)mGDBWs, idwRaster, flRTemp.FeatureClass, rasterName,mGDBRasterWs);

                                    var mxdPath = System.IO.Path.Combine(Application.StartupPath, "mxd", "VN2000.mxd");// System.Configuration.ConfigurationManager.AppSettings["MXDPATH"].ToString();

                                    System.IO.File.Copy(mxdPath, mxdResult);

                                    var rasterName2 = "Kết quả quan trắc tiếng ổn " + ngayTinhTime.ToString("dd/MM/yyyy") + " khung giờ " + gioTinh + " giờ";
                                    if (locker.IsCanceled)
                                    {
                                        _Created = false;
                                        return;
                                    }
                                    _NoiseMesageBox.SendCommand(_ProgressCancellable.Commands.SetProgress, 1);
                                    _NoiseMesageBox.SetSplashCaption("6. Trình bày bản đồ");
                                    NoiseGISCommon.SetDataSource(mxdResult, ((IWorkspace)mGDBWsSever), mGDBRasterWs.OpenRasterDataset(rasterName), rasterName2, ngayTinhTime.ToString("MM/dd/yyyy"), gioTinh);
                                    //NoiseGISCommon.SetDataSource(mxdResult, ((IWorkspace)mGDBWsSever), mGDBRasterWs.OpenRasterDataset(rasterName), rasterName2, ngayTinhTime.ToString("MM/dd/yyyy"), gioTinh);
                                    //NoiseGISCommon.SetDataSource(mxdResult, ((IWorkspace)mGDBWsSever));
                                    if (locker.IsCanceled)
                                    {
                                        _Created = false;
                                        return;
                                    }
                                    axMap.LoadMxFile(mxdResult);
                                    _Created = true;
                                }
                                catch (COMException comEx)
                                {
                                    Save("Tính toán", comEx.Message, "");
                                    _Created = false;
                                }
                                catch (Exception ex)
                                {
                                    _Created = false;
                                    Save("Tính toán", ex.Message, ex.InnerException.Message);
                                }
                            }
                            if (LoadMap() && LoadTOC() && LoadData() && LoadTreeMap())
                            {
                                BindEvents();
                            }
                            else
                                _NoiseMesageBox.ShowErrorMessage("Đã xảy ra lỗi khi tải bản đồ!");

                        }
                        catch (COMException comEx)
                        {

                            Save("Tính toán", comEx.Message, "");

                        }
                        catch (Exception ex)
                        {
                            Save("Tính toán", ex.Message, ex.InnerException.Message);
                        }
                    }));
                }
            };
            mBgWorker.RunWorkerCompleted += (sender2, e2) =>
            {
                _ProgressCancellable.locker.IsCanceled = false;
                _NoiseMesageBox.HideSplash();
            };
            mBgWorker.RunWorkerAsync(mLocker);
            _NoiseMesageBox.ShowSplash((XtraForm)this.ParentForm, "", "", true);

        }

        private void bbExportShapeFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmExport frm = new frmExport();

            if (frm.ShowDialog() == DialogResult.OK)
            {

                //var mxdResult = System.IO.Path.Combine(Application.StartupPath, "MXD_KQ", string.Format("{0}.mxd", "bando_20211002_16"));

                mGDBWsFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                string filegdb = ConfigurationSettings.AppSettings["GDBPATH"];
                mGDBRasterWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IRasterWorkspaceEx;
                mGDBWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IFeatureWorkspace;

                mGDBWsEdit = (IWorkspaceEdit2)mGDBWs;

                RasterName = "Raster_TinhToan_" + frm.NgayTinh.ToString("yyyyMMdd") + "_" + frm.GioTinh;// 20211002_16";

                if (!NoiseGISCommon.DoesRasterExist((IWorkspace)mGDBWs, RasterName, false))
                {
                    _NoiseMesageBox.ShowErrorMessage("Vui lòng tạo bản đồ tiếng ôn " + frm.GioTinh + " ngày " + frm.NgayTinh.ToString("dd/MM/yyyy") + " trước");
                    return;
                }

                IRasterDataset rRaster = mGDBRasterWs.OpenRasterDataset(RasterName);


               
                IRasterLayer pRasterLayer = new RasterLayer();

                pRasterLayer.CreateFromDataset(rRaster);

                var pOutRasterDataset = NoiseGISCommon.ReclassifyRasterLayerAndSave(pRasterLayer, 4, "", ((IWorkspace)mGDBWs));


                var rasterSave = "Raster_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                ISaveAs pSaveAs = pOutRasterDataset as ISaveAs;
                pSaveAs.SaveAs(rasterSave, ((IWorkspace)mGDBWs), "GDB");

                var rasterPolygon = "RasterPolygon_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                NoiseGISCommon.RasterToPolygon((IWorkspace)mGDBWs, rasterSave, rasterPolygon);


                // phân khoảng

                ITable tblResult = mGDBWs.OpenTable(rasterPolygon);



                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                int idxCheck = tblResult.FindField("PhanKhoang");
                if (idxCheck < 0)
                {
                    ISchemaLock schemaLock = (ISchemaLock)tblResult;
                    try
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                        // Add your field.
                        IFieldEdit2 field = new FieldClass() as IFieldEdit2;
                        field.Name_2 = "PhanKhoang";
                        field.Type_2 = esriFieldType.esriFieldTypeString;
                        field.DefaultValue_2 = "PhanKhoang";
                        tblResult.AddField(field);
                    }
                    catch (System.Runtime.InteropServices.COMException comExc)
                    {
                        // Handle the exception appropriately for the application.
                    }
                    finally
                    {
                        // Demote the exclusive lock to a shared lock.
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    }
                }

                //NoiseGISCommon.AddFieldToTable(fieldsEdit, "Phân khoảng", "PhanKhoang", esriFieldType.esriFieldTypeString);



                IWorkspaceFactoryLockControl iwsLock = (IWorkspaceFactoryLockControl)mGDBWsFactory;

                if (iwsLock.SchemaLockingEnabled)
                {
                    iwsLock.DisableSchemaLocking();
                }


                NoiseGISCommon.calculate(tblResult, "PhanKhoang", "a", "dim a \n IF [gridcode] = 1 Then \n a = \"< 45 đB\" \n elseif [gridcode] = 2 Then \n a = \"45 - 55 dB\"  \n elseif [gridcode] = 3 Then \n a =\"55 - 70 dB\" \n  elseif [gridcode] = 4 Then \n  a = \">70 dB\" \n else \n a=\"NoData\" \n END IF", true);

                IFeatureLayer flEx = new FeatureLayer()
                {
                    FeatureClass = mGDBWs.OpenFeatureClass(rasterPolygon)
                };

                NoiseGISCommon.ExportLayerToShapefile(frm.SaveFolder, frm.FileName, flEx);


                _NoiseMesageBox.ShowInfoMessage("Tải xuống thành công");
            }



            //NoiseGISCommon.ReclassifyRasterLayerAndSave(rLayer, 4, "", "", "");



            //IMapDocument pMapDocument = new MapDocumentClass();
            //pMapDocument.Open(mxdResult, "");
            ////IWorkspaceFactory pWorkFactory = new FileGDBWorkspaceFactory() as IWorkspaceFactory2;
            ////IWorkspace pWorkspace = pWorkFactory.OpenFromFile(sTargetPath, 0);

            ////IFeatureWorkspace pFeaClsWks = pWorkspace as IFeatureWorkspace;
            //for (int i = 0; i < pMapDocument.MapCount; i++)
            //{
            //    IMap map = pMapDocument.Map[i];
            //    for (int j = 0; j < map.LayerCount; j++)
            //    {
            //        ILayer layer = map.Layer[j] as IFeatureLayer;
            //        ICompositeLayer cpLayer = map.Layer[j] as ICompositeLayer;
            //        IRasterLayer rLayer = map.Layer[j] as IRasterLayer;
            //        if (rLayer != null)
            //        {
            //            NoiseGISCommon.ReclassifyRasterLayerAndSave(rLayer,4,"","","");
            //            //NoiseGISCommon.RasterToPolygon((IWorkspace)mGDBWs, rLayer, @"E:\20210825\CSDLTest\Test.shp");
            //        }
            //    }
            //}


            //        //NoiseGISCommon.RasterToPolygon((IWorkspace)mGDBWs, RasterName, @"E:\20210825\CSDLTest\Test.shp");
        }

        private void bbExportGDB_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmExportGDB frm = new frmExportGDB();

            if (frm.ShowDialog() == DialogResult.OK)
            {

                //var mxdResult = System.IO.Path.Combine(Application.StartupPath, "MXD_KQ", string.Format("{0}.mxd", "bando_20211002_16"));

                mGDBWsFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                string filegdb = ConfigurationSettings.AppSettings["GDBPATH"];
                mGDBRasterWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IRasterWorkspaceEx;
                mGDBWs = mGDBWsFactory.OpenFromFile(filegdb, 0) as IFeatureWorkspace;

                mGDBWsEdit = (IWorkspaceEdit2)mGDBWs;

                RasterName = "Raster_TinhToan_" + frm.NgayTinh.ToString("yyyyMMdd") + "_" + frm.GioTinh;// 20211002_16";

                if (!NoiseGISCommon.DoesRasterExist((IWorkspace)mGDBWs,RasterName,false))
                {
                    _NoiseMesageBox.ShowErrorMessage("Vui lòng tạo bản đồ tiếng ôn " + frm.GioTinh + " ngày " + frm.NgayTinh.ToString("dd/MM/yyyy") + " trước");
                    return;
                }

                IRasterDataset rRaster = mGDBRasterWs.OpenRasterDataset(RasterName);

              

                IRasterLayer pRasterLayer = new RasterLayer();

                pRasterLayer.CreateFromDataset(rRaster);

                var pOutRasterDataset = NoiseGISCommon.ReclassifyRasterLayerAndSave(pRasterLayer, 4, "", ((IWorkspace)mGDBWs));


                var rasterSave = "Raster_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                ISaveAs pSaveAs = pOutRasterDataset as ISaveAs;
                pSaveAs.SaveAs(rasterSave, ((IWorkspace)mGDBWs), "GDB");

                var rasterPolygon = "RasterPolygon_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                NoiseGISCommon.RasterToPolygon((IWorkspace)mGDBWs, rasterSave, rasterPolygon);


                // phân khoảng

                ITable tblResult = mGDBWs.OpenTable(rasterPolygon);



                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                int idxCheck = tblResult.FindField("PhanKhoang");
                if (idxCheck < 0)
                {
                    ISchemaLock schemaLock = (ISchemaLock)tblResult;
                    try
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                        // Add your field.
                        IFieldEdit2 field = new FieldClass() as IFieldEdit2;
                        field.Name_2 = "PhanKhoang";
                        field.Type_2 = esriFieldType.esriFieldTypeString;
                        field.DefaultValue_2 = "PhanKhoang";
                        tblResult.AddField(field);
                    }
                    catch (System.Runtime.InteropServices.COMException comExc)
                    {
                        // Handle the exception appropriately for the application.
                    }
                    finally
                    {
                        // Demote the exclusive lock to a shared lock.
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    }
                }

                //NoiseGISCommon.AddFieldToTable(fieldsEdit, "Phân khoảng", "PhanKhoang", esriFieldType.esriFieldTypeString);



                IWorkspaceFactoryLockControl iwsLock = (IWorkspaceFactoryLockControl)mGDBWsFactory;

                if (iwsLock.SchemaLockingEnabled)
                {
                    iwsLock.DisableSchemaLocking();
                }


                NoiseGISCommon.calculate(tblResult, "PhanKhoang", "a", "dim a \n IF [gridcode] = 1 Then \n a = \"< 45 đB\" \n elseif [gridcode] = 2 Then \n a = \"45 - 55 dB\"  \n elseif [gridcode] = 3 Then \n a =\"55 - 70 dB\" \n  elseif [gridcode] = 4 Then \n  a = \">70 dB\" \n else \n a=\"NoData\" \n END IF", true);

                IFeatureLayer flEx = new FeatureLayer()
                {
                    FeatureClass = mGDBWs.OpenFeatureClass(rasterPolygon)
                };

                //NoiseGISCommon.ExportLayerToShapefile(frm.SaveFolder, "test", flEx);

                var mGDBWsFactoryExport = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                var mGDBWsExport = mGDBWsFactory.OpenFromFile(frm.SaveFolder, 0) as IFeatureWorkspace;

                //NoiseGISCommon.FeatureToFeatureByGP((IWorkspace)mGDBWs, frm.SaveFolder, rasterPolygon, frm.FileName, "", "1=1");
                NoiseGISCommon.FeatureToFeatureByGP((IWorkspace)mGDBWsExport, flEx.FeatureClass, frm.FileName,"","1=1");

                _NoiseMesageBox.ShowInfoMessage("Tải xuống thành công");
            }

        }

     
    }
}