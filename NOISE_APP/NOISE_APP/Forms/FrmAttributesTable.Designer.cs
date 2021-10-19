namespace NOISE_APP.Forms
{
    partial class FrmAttributesTable
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnToExcel = new DevExpress.XtraEditors.SimpleButton();
            this.btnZoom = new DevExpress.XtraEditors.SimpleButton();
            this.gcAttributes = new DevExpress.XtraGrid.GridControl();
            this.gvAttributes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcAttributes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAttributes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnToExcel);
            this.layoutControl1.Controls.Add(this.btnZoom);
            this.layoutControl1.Controls.Add(this.gcAttributes);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(773, 246, 400, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(768, 438);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnToExcel
            // 
            this.btnToExcel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnToExcel.Appearance.Options.UseFont = true;
            this.btnToExcel.Image = global::NOISE_APP.Properties.Resources.exporttoxls_16x16;
            this.btnToExcel.Location = new System.Drawing.Point(655, 405);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Padding = new System.Windows.Forms.Padding(3);
            this.btnToExcel.Size = new System.Drawing.Size(108, 28);
            this.btnToExcel.StyleController = this.layoutControl1;
            this.btnToExcel.TabIndex = 6;
            this.btnToExcel.Text = "Xuất ra excel";
            // 
            // btnZoom
            // 
            this.btnZoom.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnZoom.Appearance.Options.UseFont = true;
            this.btnZoom.Image = global::NOISE_APP.Properties.Resources.zoom_16x16;
            this.btnZoom.Location = new System.Drawing.Point(517, 405);
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.Padding = new System.Windows.Forms.Padding(3);
            this.btnZoom.Size = new System.Drawing.Size(128, 28);
            this.btnZoom.StyleController = this.layoutControl1;
            this.btnZoom.TabIndex = 5;
            this.btnZoom.Text = "Phóng tới bản đồ";
            this.btnZoom.Visible = false;
            // 
            // gcAttributes
            // 
            this.gcAttributes.Location = new System.Drawing.Point(2, 2);
            this.gcAttributes.MainView = this.gvAttributes;
            this.gcAttributes.Name = "gcAttributes";
            this.gcAttributes.Size = new System.Drawing.Size(764, 396);
            this.gcAttributes.TabIndex = 4;
            this.gcAttributes.UseEmbeddedNavigator = true;
            this.gcAttributes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvAttributes});
            // 
            // gvAttributes
            // 
            this.gvAttributes.GridControl = this.gcAttributes;
            this.gvAttributes.Name = "gvAttributes";
            this.gvAttributes.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvAttributes.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvAttributes.OptionsBehavior.Editable = false;
            this.gvAttributes.OptionsView.ColumnAutoWidth = false;
            this.gvAttributes.OptionsView.ShowGroupPanel = false;
            this.gvAttributes.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gvAttribute_SelectionChanged);
            this.gvAttributes.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvAttributes_FocusedRowChanged);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.emptySpaceItem1,
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(768, 438);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcAttributes;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(768, 400);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnToExcel;
            this.layoutControlItem3.Location = new System.Drawing.Point(650, 400);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(118, 38);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(118, 38);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem3.Size = new System.Drawing.Size(118, 38);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 400);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(104, 24);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(512, 38);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnZoom;
            this.layoutControlItem2.Location = new System.Drawing.Point(512, 400);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(138, 38);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(138, 38);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem2.Size = new System.Drawing.Size(138, 38);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // FrmAttributesTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 438);
            this.Controls.Add(this.layoutControl1);
            this.Name = "FrmAttributesTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bảng thuộc tính đối tượng";
            this.Load += new System.EventHandler(this.FrmAttributesTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcAttributes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAttributes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton btnToExcel;
        private DevExpress.XtraEditors.SimpleButton btnZoom;
        private DevExpress.XtraGrid.GridControl gcAttributes;
        private DevExpress.XtraGrid.Views.Grid.GridView gvAttributes;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}