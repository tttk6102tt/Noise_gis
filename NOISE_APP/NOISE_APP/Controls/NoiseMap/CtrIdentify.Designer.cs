namespace NOISE_APP.Controls.MolarMap
{
    partial class CtrIdentify
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrIdentify));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.bgvData = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gbField = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colField = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gbValue = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colValue = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.txtLocation = new DevExpress.XtraEditors.TextEdit();
            this.tlLayers = new DevExpress.XtraTreeList.TreeList();
            this.cboLayers = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.bmIdentify = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.stbResult = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlLayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bmIdentify)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gcData);
            this.layoutControl1.Controls.Add(this.txtLocation);
            this.layoutControl1.Controls.Add(this.tlLayers);
            this.layoutControl1.Controls.Add(this.cboLayers);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(461, 298);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gcData
            // 
            this.gcData.Location = new System.Drawing.Point(170, 60);
            this.gcData.MainView = this.bgvData;
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(279, 226);
            this.gcData.TabIndex = 7;
            this.gcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bgvData});
            // 
            // bgvData
            // 
            this.bgvData.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gbField,
            this.gbValue});
            this.bgvData.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colField,
            this.colValue});
            this.bgvData.GridControl = this.gcData;
            this.bgvData.Name = "bgvData";
            this.bgvData.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.bgvData.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.bgvData.OptionsBehavior.Editable = false;
            this.bgvData.OptionsView.ShowColumnHeaders = false;
            this.bgvData.OptionsView.ShowGroupPanel = false;
            // 
            // gbField
            // 
            this.gbField.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gbField.AppearanceHeader.Options.UseFont = true;
            this.gbField.AppearanceHeader.Options.UseTextOptions = true;
            this.gbField.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbField.Caption = "Tên trường";
            this.gbField.Columns.Add(this.colField);
            this.gbField.Name = "gbField";
            this.gbField.VisibleIndex = 0;
            this.gbField.Width = 75;
            // 
            // colField
            // 
            this.colField.Caption = "Tên trường";
            this.colField.FieldName = "ID";
            this.colField.Name = "colField";
            this.colField.Visible = true;
            // 
            // gbValue
            // 
            this.gbValue.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gbValue.AppearanceHeader.Options.UseFont = true;
            this.gbValue.AppearanceHeader.Options.UseTextOptions = true;
            this.gbValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbValue.Caption = "Gía trị";
            this.gbValue.Columns.Add(this.colValue);
            this.gbValue.Name = "gbValue";
            this.gbValue.VisibleIndex = 1;
            this.gbValue.Width = 75;
            // 
            // colValue
            // 
            this.colValue.Caption = "Gía trị";
            this.colValue.FieldName = "Name";
            this.colValue.Name = "colValue";
            this.colValue.Visible = true;
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(238, 36);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(211, 20);
            this.txtLocation.StyleController = this.layoutControl1;
            this.txtLocation.TabIndex = 6;
            // 
            // tlLayers
            // 
            this.tlLayers.Location = new System.Drawing.Point(12, 36);
            this.tlLayers.Name = "tlLayers";
            this.tlLayers.Size = new System.Drawing.Size(154, 250);
            this.tlLayers.TabIndex = 5;
            // 
            // cboLayers
            // 
            this.cboLayers.Location = new System.Drawing.Point(80, 12);
            this.cboLayers.Name = "cboLayers";
            this.cboLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLayers.Size = new System.Drawing.Size(369, 20);
            this.cboLayers.StyleController = this.layoutControl1;
            this.cboLayers.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(461, 298);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.cboLayers;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(441, 24);
            this.layoutControlItem1.Text = "Lớp bản đồ";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(65, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.tlLayers;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(158, 254);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.gcData;
            this.layoutControlItem4.Location = new System.Drawing.Point(158, 48);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(283, 230);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.txtLocation;
            this.layoutControlItem3.Location = new System.Drawing.Point(158, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(283, 24);
            this.layoutControlItem3.Text = "Vị trí tọa độ";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(65, 13);
            // 
            // bmIdentify
            // 
            this.bmIdentify.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.bmIdentify.DockControls.Add(this.barDockControlTop);
            this.bmIdentify.DockControls.Add(this.barDockControlBottom);
            this.bmIdentify.DockControls.Add(this.barDockControlLeft);
            this.bmIdentify.DockControls.Add(this.barDockControlRight);
            this.bmIdentify.Form = this;
            this.bmIdentify.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.stbResult});
            this.bmIdentify.MaxItemId = 1;
            this.bmIdentify.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.stbResult)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // stbResult
            // 
            this.stbResult.Id = 0;
            this.stbResult.Name = "stbResult";
            this.stbResult.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(461, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 298);
            this.barDockControlBottom.Size = new System.Drawing.Size(461, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 298);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(461, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 298);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.InsertGalleryImage("point_16x16.png", "images/chart/point_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/chart/point_16x16.png"), 0);
            this.imageCollection1.Images.SetKeyName(0, "point_16x16.png");
            this.imageCollection1.InsertGalleryImage("line_16x16.png", "images/chart/line_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/chart/line_16x16.png"), 1);
            this.imageCollection1.Images.SetKeyName(1, "line_16x16.png");
            this.imageCollection1.InsertGalleryImage("highlowlines_16x16.png", "images/analysis/highlowlines_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/analysis/highlowlines_16x16.png"), 2);
            this.imageCollection1.Images.SetKeyName(2, "highlowlines_16x16.png");
            // 
            // CtrIdentify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "CtrIdentify";
            this.Size = new System.Drawing.Size(461, 323);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlLayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bmIdentify)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraEditors.TextEdit txtLocation;
        private DevExpress.XtraTreeList.TreeList tlLayers;
        private DevExpress.XtraEditors.LookUpEdit cboLayers;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bgvData;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gbField;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colField;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gbValue;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colValue;
        private DevExpress.XtraBars.BarManager bmIdentify;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarStaticItem stbResult;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.Utils.ImageCollection imageCollection1;
    }
}
