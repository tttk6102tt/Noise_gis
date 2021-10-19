namespace NOISE_APP.Forms
{
    partial class frmIdentify
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
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.bgvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colField = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtLocation = new DevExpress.XtraEditors.TextEdit();
            this.tlLayers = new DevExpress.XtraTreeList.TreeList();
            this.cboLayers = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
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
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
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
            this.layoutControl1.Size = new System.Drawing.Size(515, 404);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gcData
            // 
            this.gcData.Location = new System.Drawing.Point(186, 60);
            this.gcData.MainView = this.bgvData;
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(317, 332);
            this.gcData.TabIndex = 7;
            this.gcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bgvData});
            // 
            // bgvData
            // 
            this.bgvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colField,
            this.colValue});
            this.bgvData.GridControl = this.gcData;
            this.bgvData.Name = "bgvData";
            this.bgvData.OptionsBehavior.Editable = false;
            this.bgvData.OptionsView.ShowGroupPanel = false;
            // 
            // colField
            // 
            this.colField.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.colField.AppearanceCell.Options.UseFont = true;
            this.colField.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.colField.AppearanceHeader.Options.UseFont = true;
            this.colField.Caption = "Tên trường";
            this.colField.FieldName = "ID";
            this.colField.Name = "colField";
            this.colField.Visible = true;
            this.colField.VisibleIndex = 0;
            // 
            // colValue
            // 
            this.colValue.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.colValue.AppearanceCell.Options.UseFont = true;
            this.colValue.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.colValue.AppearanceHeader.Options.UseFont = true;
            this.colValue.Caption = "Giá trị";
            this.colValue.FieldName = "Name";
            this.colValue.Name = "colValue";
            this.colValue.Visible = true;
            this.colValue.VisibleIndex = 1;
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(254, 36);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(249, 20);
            this.txtLocation.StyleController = this.layoutControl1;
            this.txtLocation.TabIndex = 6;
            // 
            // tlLayers
            // 
            this.tlLayers.Location = new System.Drawing.Point(12, 36);
            this.tlLayers.Name = "tlLayers";
            this.tlLayers.Size = new System.Drawing.Size(170, 356);
            this.tlLayers.TabIndex = 5;
            // 
            // cboLayers
            // 
            this.cboLayers.Location = new System.Drawing.Point(80, 12);
            this.cboLayers.Name = "cboLayers";
            this.cboLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLayers.Properties.NullText = "Chọn lớp dữ liệu";
            this.cboLayers.Size = new System.Drawing.Size(423, 20);
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
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(515, 404);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.cboLayers;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(495, 24);
            this.layoutControlItem1.Text = "Lớp bản đồ";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(65, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.tlLayers;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(174, 360);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.txtLocation;
            this.layoutControlItem3.Location = new System.Drawing.Point(174, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(321, 24);
            this.layoutControlItem3.Text = "Vị trí tọa độ";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(65, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.gcData;
            this.layoutControlItem4.Location = new System.Drawing.Point(174, 48);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(321, 336);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // frmIdentify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 404);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frmIdentify";
            this.Text = "Identify";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmIdentify_FormClosing);
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
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.LookUpEdit cboLayers;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraTreeList.TreeList tlLayers;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.TextEdit txtLocation;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView bgvData;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.Columns.GridColumn colField;
        private DevExpress.XtraGrid.Columns.GridColumn colValue;
    }
}