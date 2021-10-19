namespace NOISE_APP.Forms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bbItemScale = new DevExpress.XtraBars.BarButtonItem();
            this.lblActionMessage = new DevExpress.XtraBars.BarButtonItem();
            this.bsItemCoords = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageCategory1 = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.dockMain = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockTOC = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.barTbMap = new DevExpress.XtraBars.Bar();
            this.bmMain = new DevExpress.XtraBars.BarManager(this.components);
            this.bsItemToolIcon = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bbItemSelect = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemZoomIn = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemZoomout = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemPan = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemFullExtent = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemPrevExtent = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemNextExtent = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemIdentify = new DevExpress.XtraBars.BarButtonItem();
            this.bbItemFind = new DevExpress.XtraBars.BarButtonItem();
            this.bsiSystem = new DevExpress.XtraBars.BarSubItem();
            this.bsiAddLayer = new DevExpress.XtraBars.BarSubItem();
            this.bbiAddShapefile = new DevExpress.XtraBars.BarButtonItem();
            this.bbiAddGdb = new DevExpress.XtraBars.BarButtonItem();
            this.bbiAddMdb = new DevExpress.XtraBars.BarButtonItem();
            this.bbiAddMxd = new DevExpress.XtraBars.BarButtonItem();
            this.bbiCreateLayer = new DevExpress.XtraBars.BarButtonItem();
            this.bbiInterpolate = new DevExpress.XtraBars.BarButtonItem();
            this.bbConfigPhanVung = new DevExpress.XtraBars.BarButtonItem();
            this.bbiLoadMap = new DevExpress.XtraBars.BarButtonItem();
            this.bsiExportMap = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
            this.bbiPrint = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.riPicToolIcon = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.hideContainerLeft = new DevExpress.XtraBars.Docking.AutoHideContainer();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockMain)).BeginInit();
            this.dockTOC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPicToolIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.hideContainerLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.bbItemScale,
            this.lblActionMessage,
            this.bsItemCoords,
            this.barButtonItem1});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 5;
            this.ribbon.Name = "ribbon";
            this.ribbon.PageCategories.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageCategory[] {
            this.ribbonPageCategory1});
            this.ribbon.Size = new System.Drawing.Size(1169, 49);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // bbItemScale
            // 
            this.bbItemScale.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.bbItemScale.Caption = "Tỉ lệ 1:2000";
            this.bbItemScale.Id = 223;
            this.bbItemScale.Name = "bbItemScale";
            // 
            // lblActionMessage
            // 
            this.lblActionMessage.Caption = "Chọn";
            this.lblActionMessage.Id = 221;
            this.lblActionMessage.Name = "lblActionMessage";
            // 
            // bsItemCoords
            // 
            this.bsItemCoords.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.bsItemCoords.Caption = "X, Y";
            this.bsItemCoords.Id = 224;
            this.bsItemCoords.Name = "bsItemCoords";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Chọn";
            this.barButtonItem1.Glyph = global::NOISE_APP.Properties.Resources.pointer_16x16;
            this.barButtonItem1.Id = 2;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // ribbonPageCategory1
            // 
            this.ribbonPageCategory1.Name = "ribbonPageCategory1";
            this.ribbonPageCategory1.Text = "ribbonPageCategory1";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.bbItemScale);
            this.ribbonStatusBar.ItemLinks.Add(this.bsItemCoords);
            this.ribbonStatusBar.ItemLinks.Add(this.barButtonItem1);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 615);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1169, 31);
            // 
            // dockMain
            // 
            this.dockMain.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerLeft});
            this.dockMain.DockingOptions.ShowCaptionImage = true;
            this.dockMain.DockingOptions.ShowMaximizeButton = false;
            this.dockMain.Form = this;
            this.dockMain.MenuManager = this.bmMain;
            this.dockMain.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl"});
            // 
            // dockTOC
            // 
            this.dockTOC.Controls.Add(this.dockPanel1_Container);
            this.dockTOC.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockTOC.ID = new System.Guid("1ee4212c-4ee6-44b1-a5e8-e51d4d605880");
            this.dockTOC.Location = new System.Drawing.Point(-200, 0);
            this.dockTOC.Name = "dockTOC";
            this.dockTOC.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockTOC.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockTOC.SavedIndex = 0;
            this.dockTOC.Size = new System.Drawing.Size(200, 537);
            this.dockTOC.Text = "Lớp dữ liêu";
            this.dockTOC.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(191, 510);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // barTbMap
            // 
            this.barTbMap.BarName = "Chức năng bản đồ";
            this.barTbMap.DockCol = 0;
            this.barTbMap.DockRow = 1;
            this.barTbMap.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barTbMap.OptionsBar.AllowQuickCustomization = false;
            this.barTbMap.OptionsBar.DisableClose = true;
            this.barTbMap.OptionsBar.RotateWhenVertical = false;
            this.barTbMap.OptionsBar.UseWholeRow = true;
            this.barTbMap.Text = "Chức năng bản đồ";
            // 
            // bmMain
            // 
            this.bmMain.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.bmMain.DockControls.Add(this.barDockControlTop);
            this.bmMain.DockControls.Add(this.barDockControlBottom);
            this.bmMain.DockControls.Add(this.barDockControlLeft);
            this.bmMain.DockControls.Add(this.barDockControlRight);
            this.bmMain.DockManager = this.dockMain;
            this.bmMain.Form = this;
            this.bmMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticItem3,
            this.bbItemSelect,
            this.bbItemZoomIn,
            this.bbItemZoomout,
            this.bbItemPan,
            this.bbItemFullExtent,
            this.bbItemPrevExtent,
            this.bbItemNextExtent,
            this.bbItemRefresh,
            this.bbItemFind,
            this.bbItemIdentify,
            this.bsItemToolIcon,
            this.barStaticItem1,
            this.barStaticItem2,
            this.bsiSystem,
            this.bsiAddLayer,
            this.bbiAddShapefile,
            this.bbiAddGdb,
            this.bbiAddMdb,
            this.bbiAddMxd,
            this.bbiLoadMap,
            this.bsiExportMap,
            this.bbiPrint,
            this.bbiExit,
            this.barButtonItem8,
            this.barButtonItem9,
            this.barButtonItem10,
            this.bbiCreateLayer,
            this.barButtonItem2,
            this.bbiInterpolate,
            this.bbConfigPhanVung,
            this.barSubItem1});
            this.bmMain.MaxItemId = 54;
            this.bmMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riPicToolIcon,
            this.repositoryItemTextEdit1});
            // 
            // bsItemToolIcon
            // 
            this.bsItemToolIcon.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.bsItemToolIcon.Id = 15;
            this.bsItemToolIcon.Name = "bsItemToolIcon";
            this.bsItemToolIcon.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.bsItemToolIcon.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "Chọn";
            this.barStaticItem1.Id = 16;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem2.Caption = "Tỉ lệ 1:2000";
            this.barStaticItem2.Id = 18;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem3.Caption = "X, Y";
            this.barStaticItem3.Id = 0;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // bar1
            // 
            this.bar1.BarName = "Chức năng bản đồ";
            this.bar1.CanDockStyle = ((DevExpress.XtraBars.BarCanDockStyle)((DevExpress.XtraBars.BarCanDockStyle.Top | DevExpress.XtraBars.BarCanDockStyle.Right)));
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Right;
            this.bar1.FloatLocation = new System.Drawing.Point(434, 188);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bbItemSelect, DevExpress.XtraBars.BarItemPaintStyle.Standard),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bbItemZoomIn, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbItemZoomout),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbItemPan, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbItemFullExtent),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbItemPrevExtent, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbItemNextExtent),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbItemRefresh, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbItemIdentify),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbItemFind)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.AutoPopupMode = DevExpress.XtraBars.BarAutoPopupMode.OnlyMenu;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.OptionsBar.DisableCustomization = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Chức năng bản đồ";
            // 
            // bbItemSelect
            // 
            this.bbItemSelect.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.bbItemSelect.Caption = "Lựa chọn";
            this.bbItemSelect.Down = true;
            this.bbItemSelect.Glyph = global::NOISE_APP.Properties.Resources.pointer_16x16;
            this.bbItemSelect.Id = 1;
            this.bbItemSelect.ImageIndex = 0;
            this.bbItemSelect.Name = "bbItemSelect";
            // 
            // bbItemZoomIn
            // 
            this.bbItemZoomIn.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.bbItemZoomIn.Caption = "Phóng to";
            this.bbItemZoomIn.Id = 2;
            this.bbItemZoomIn.ImageIndex = 2;
            this.bbItemZoomIn.Name = "bbItemZoomIn";
            // 
            // bbItemZoomout
            // 
            this.bbItemZoomout.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.bbItemZoomout.Caption = "Thu nhỏ";
            this.bbItemZoomout.Id = 3;
            this.bbItemZoomout.ImageIndex = 3;
            this.bbItemZoomout.Name = "bbItemZoomout";
            // 
            // bbItemPan
            // 
            this.bbItemPan.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.bbItemPan.Caption = "Pan";
            this.bbItemPan.Id = 4;
            this.bbItemPan.ImageIndex = 4;
            this.bbItemPan.Name = "bbItemPan";
            // 
            // bbItemFullExtent
            // 
            this.bbItemFullExtent.Caption = "Toàn cảnh";
            this.bbItemFullExtent.Id = 5;
            this.bbItemFullExtent.ImageIndex = 5;
            this.bbItemFullExtent.Name = "bbItemFullExtent";
            // 
            // bbItemPrevExtent
            // 
            this.bbItemPrevExtent.Caption = "Trạng thái trước";
            this.bbItemPrevExtent.Id = 6;
            this.bbItemPrevExtent.ImageIndex = 6;
            this.bbItemPrevExtent.Name = "bbItemPrevExtent";
            // 
            // bbItemNextExtent
            // 
            this.bbItemNextExtent.Caption = "Trạng thái sau";
            this.bbItemNextExtent.Id = 7;
            this.bbItemNextExtent.ImageIndex = 7;
            this.bbItemNextExtent.Name = "bbItemNextExtent";
            // 
            // bbItemRefresh
            // 
            this.bbItemRefresh.Caption = "Làm mới bản đồ";
            this.bbItemRefresh.Id = 8;
            this.bbItemRefresh.ImageIndex = 8;
            this.bbItemRefresh.Name = "bbItemRefresh";
            // 
            // bbItemIdentify
            // 
            this.bbItemIdentify.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.bbItemIdentify.Caption = "Identify";
            this.bbItemIdentify.Id = 10;
            this.bbItemIdentify.ImageIndex = 1;
            this.bbItemIdentify.Name = "bbItemIdentify";
            // 
            // bbItemFind
            // 
            this.bbItemFind.Caption = "Tìm kiếm";
            this.bbItemFind.Id = 9;
            this.bbItemFind.ImageIndex = 9;
            this.bbItemFind.Name = "bbItemFind";
            // 
            // bsiSystem
            // 
            this.bsiSystem.AllowDrawArrow = DevExpress.Utils.DefaultBoolean.False;
            this.bsiSystem.Caption = "Hệ thống";
            this.bsiSystem.Id = 34;
            this.bsiSystem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiAddLayer),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbConfigPhanVung, true),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Caption, this.bbiLoadMap, "Tải bản đồ (MXD)", true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiExportMap),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiPrint, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiExit, true)});
            this.bsiSystem.Name = "bsiSystem";
            // 
            // bsiAddLayer
            // 
            this.bsiAddLayer.Caption = "Thêm lớp";
            this.bsiAddLayer.Id = 37;
            this.bsiAddLayer.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiAddShapefile),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiAddGdb, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiAddMdb),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiAddMxd, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiCreateLayer, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiInterpolate)});
            this.bsiAddLayer.Name = "bsiAddLayer";
            // 
            // bbiAddShapefile
            // 
            this.bbiAddShapefile.Caption = "Shapefile (*.shp)";
            this.bbiAddShapefile.Id = 38;
            this.bbiAddShapefile.Name = "bbiAddShapefile";
            // 
            // bbiAddGdb
            // 
            this.bbiAddGdb.Caption = "Geodatabase (*.gdb)";
            this.bbiAddGdb.Id = 39;
            this.bbiAddGdb.Name = "bbiAddGdb";
            // 
            // bbiAddMdb
            // 
            this.bbiAddMdb.Caption = "Personal Geodatabase (*.mdb)";
            this.bbiAddMdb.Id = 40;
            this.bbiAddMdb.Name = "bbiAddMdb";
            // 
            // bbiAddMxd
            // 
            this.bbiAddMxd.Caption = "Map Exchange Document (*.mxd)";
            this.bbiAddMxd.Id = 41;
            this.bbiAddMxd.Name = "bbiAddMxd";
            // 
            // bbiCreateLayer
            // 
            this.bbiCreateLayer.Caption = "Tạo lớp từ bảng XY";
            this.bbiCreateLayer.Id = 49;
            this.bbiCreateLayer.Name = "bbiCreateLayer";
            // 
            // bbiInterpolate
            // 
            this.bbiInterpolate.Caption = "Nội suy";
            this.bbiInterpolate.Id = 51;
            this.bbiInterpolate.Name = "bbiInterpolate";
            // 
            // bbConfigPhanVung
            // 
            this.bbConfigPhanVung.Caption = "Cấu hình phân vùng địa hình";
            this.bbConfigPhanVung.Id = 52;
            this.bbConfigPhanVung.Name = "bbConfigPhanVung";
            // 
            // bbiLoadMap
            // 
            this.bbiLoadMap.Caption = "Tải bản đồ (MXD)";
            this.bbiLoadMap.Id = 42;
            this.bbiLoadMap.Name = "bbiLoadMap";
            // 
            // bsiExportMap
            // 
            this.bsiExportMap.Caption = "Xuất bản đồ";
            this.bsiExportMap.Id = 43;
            this.bsiExportMap.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10)});
            this.bsiExportMap.Name = "bsiExportMap";
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "Lưu MXD";
            this.barButtonItem8.Id = 46;
            this.barButtonItem8.Name = "barButtonItem8";
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "Xuất ảnh";
            this.barButtonItem9.Id = 47;
            this.barButtonItem9.Name = "barButtonItem9";
            // 
            // barButtonItem10
            // 
            this.barButtonItem10.Caption = "Xuất PDF";
            this.barButtonItem10.Id = 48;
            this.barButtonItem10.Name = "barButtonItem10";
            // 
            // bbiPrint
            // 
            this.bbiPrint.Caption = "In";
            this.bbiPrint.Id = 44;
            this.bbiPrint.Name = "bbiPrint";
            // 
            // bbiExit
            // 
            this.bbiExit.Caption = "Thoát";
            this.bbiExit.Id = 45;
            this.bbiExit.Name = "bbiExit";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1200, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 646);
            this.barDockControlBottom.Size = new System.Drawing.Size(1200, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 646);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1169, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(31, 646);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Thêm bảng dữ liệu";
            this.barButtonItem2.Id = 50;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "barSubItem1";
            this.barSubItem1.Id = 53;
            this.barSubItem1.Name = "barSubItem1";
            // 
            // riPicToolIcon
            // 
            this.riPicToolIcon.Name = "riPicToolIcon";
            this.riPicToolIcon.ZoomAccelerationFactor = 1D;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // hideContainerLeft
            // 
            this.hideContainerLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.hideContainerLeft.Controls.Add(this.dockTOC);
            this.hideContainerLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.hideContainerLeft.Location = new System.Drawing.Point(0, 49);
            this.hideContainerLeft.Name = "hideContainerLeft";
            this.hideContainerLeft.Size = new System.Drawing.Size(19, 566);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 646);
            this.Controls.Add(this.hideContainerLeft);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FormMain";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "RibbonForm1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockMain)).EndInit();
            this.dockTOC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPicToolIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.hideContainerLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.Docking.DockManager dockMain;
        private DevExpress.XtraBars.BarButtonItem bbItemScale;
        private DevExpress.XtraBars.BarButtonItem bsItemCoords;
        private DevExpress.XtraBars.BarButtonItem lblActionMessage;
        private DevExpress.XtraBars.Docking.DockPanel dockTOC;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageCategory ribbonPageCategory1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;

        private DevExpress.XtraBars.Bar barTbMap;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarManager bmMain;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem bbItemSelect;
        private DevExpress.XtraBars.BarButtonItem bbItemZoomIn;
        private DevExpress.XtraBars.BarButtonItem bbItemZoomout;
        private DevExpress.XtraBars.BarButtonItem bbItemPan;
        private DevExpress.XtraBars.BarButtonItem bbItemFullExtent;
        private DevExpress.XtraBars.BarButtonItem bbItemPrevExtent;
        private DevExpress.XtraBars.BarButtonItem bbItemNextExtent;
        private DevExpress.XtraBars.BarButtonItem bbItemRefresh;
        private DevExpress.XtraBars.BarButtonItem bbItemIdentify;
        private DevExpress.XtraBars.BarButtonItem bbItemFind;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.BarStaticItem bsItemToolIcon;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarSubItem bsiSystem;
        private DevExpress.XtraBars.BarSubItem bsiAddLayer;
        private DevExpress.XtraBars.BarButtonItem bbiAddShapefile;
        private DevExpress.XtraBars.BarButtonItem bbiAddGdb;
        private DevExpress.XtraBars.BarButtonItem bbiAddMdb;
        private DevExpress.XtraBars.BarButtonItem bbiAddMxd;
        private DevExpress.XtraBars.BarButtonItem bbiCreateLayer;
        private DevExpress.XtraBars.BarButtonItem bbiInterpolate;
        private DevExpress.XtraBars.BarButtonItem bbConfigPhanVung;
        private DevExpress.XtraBars.BarButtonItem bbiLoadMap;
        private DevExpress.XtraBars.BarSubItem bsiExportMap;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
        private DevExpress.XtraBars.BarButtonItem bbiPrint;
        private DevExpress.XtraBars.BarButtonItem bbiExit;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit riPicToolIcon;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
    }
}