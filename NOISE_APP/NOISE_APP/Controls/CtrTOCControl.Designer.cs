namespace NOISE_APP.Controls
{
    partial class CtrTOCControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrTOCControl));
            this.axTOC = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.pmActions = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnZoomToLayer = new DevExpress.XtraBars.BarButtonItem();
            this.btnAttributesTable = new DevExpress.XtraBars.BarButtonItem();
            this.btnRemoveLayer = new DevExpress.XtraBars.BarButtonItem();
            this.bmTOC = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.axTOC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmActions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bmTOC)).BeginInit();
            this.SuspendLayout();
            // 
            // axTOC
            // 
            this.axTOC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOC.Location = new System.Drawing.Point(0, 0);
            this.axTOC.Name = "axTOC";
            this.axTOC.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOC.OcxState")));
            this.axTOC.Size = new System.Drawing.Size(252, 285);
            this.axTOC.TabIndex = 0;
            // 
            // pmActions
            // 
            this.pmActions.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnZoomToLayer),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAttributesTable),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRemoveLayer, true)});
            this.pmActions.Manager = this.bmTOC;
            this.pmActions.Name = "pmActions";
            // 
            // btnZoomToLayer
            // 
            this.btnZoomToLayer.Caption = "Phóng tới lớp bản đồ";
            this.btnZoomToLayer.Glyph = global::NOISE_APP.Properties.Resources.find_16x16;
            this.btnZoomToLayer.Id = 1;
            this.btnZoomToLayer.LargeGlyph = global::NOISE_APP.Properties.Resources.find_32x32;
            this.btnZoomToLayer.Name = "btnZoomToLayer";
            // 
            // btnAttributesTable
            // 
            this.btnAttributesTable.Caption = "Bảng thuộc tính";
            this.btnAttributesTable.Glyph = global::NOISE_APP.Properties.Resources.table_16x16;
            this.btnAttributesTable.Id = 2;
            this.btnAttributesTable.LargeGlyph = global::NOISE_APP.Properties.Resources.table_32x32;
            this.btnAttributesTable.Name = "btnAttributesTable";
            // 
            // btnRemoveLayer
            // 
            this.btnRemoveLayer.Caption = "Xóa lớp bản đồ";
            this.btnRemoveLayer.Glyph = global::NOISE_APP.Properties.Resources.delete_16x16;
            this.btnRemoveLayer.Id = 4;
            this.btnRemoveLayer.LargeGlyph = global::NOISE_APP.Properties.Resources.delete_32x32;
            this.btnRemoveLayer.Name = "btnRemoveLayer";
            // 
            // bmTOC
            // 
            this.bmTOC.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.bmTOC.DockControls.Add(this.barDockControlTop);
            this.bmTOC.DockControls.Add(this.barDockControlBottom);
            this.bmTOC.DockControls.Add(this.barDockControlLeft);
            this.bmTOC.DockControls.Add(this.barDockControlRight);
            this.bmTOC.Form = this;
            this.bmTOC.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnZoomToLayer,
            this.btnAttributesTable,
            this.btnRemoveLayer});
            this.bmTOC.MaxItemId = 5;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.FloatLocation = new System.Drawing.Point(450, 424);
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(252, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 285);
            this.barDockControlBottom.Size = new System.Drawing.Size(252, 29);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 285);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(252, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 285);
            // 
            // CtrTOCControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axTOC);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "CtrTOCControl";
            this.Size = new System.Drawing.Size(252, 314);
            this.Load += new System.EventHandler(this.CtrTOCControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axTOC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmActions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bmTOC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxTOCControl axTOC;
        private DevExpress.XtraBars.PopupMenu pmActions;
        private DevExpress.XtraBars.BarManager bmTOC;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnZoomToLayer;
        private DevExpress.XtraBars.BarButtonItem btnAttributesTable;
        private DevExpress.XtraBars.BarButtonItem btnRemoveLayer;
    }
}
