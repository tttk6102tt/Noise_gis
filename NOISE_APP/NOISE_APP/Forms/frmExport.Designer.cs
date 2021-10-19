namespace NOISE_APP.Forms
{
    partial class frmExport
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExport));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtTenFile = new DevExpress.XtraEditors.TextEdit();
            this.btnSelectFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnTinhToan = new DevExpress.XtraEditors.CheckButton();
            this.txtGioTinh = new DevExpress.XtraEditors.TextEdit();
            this.txtNgayTinh = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.simpleSeparator1 = new DevExpress.XtraLayout.SimpleSeparator();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTenFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGioTinh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNgayTinh.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNgayTinh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txtTenFile);
            this.layoutControl1.Controls.Add(this.btnSelectFolder);
            this.layoutControl1.Controls.Add(this.btnCancel);
            this.layoutControl1.Controls.Add(this.btnTinhToan);
            this.layoutControl1.Controls.Add(this.txtGioTinh);
            this.layoutControl1.Controls.Add(this.txtNgayTinh);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1311, 148, 593, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(465, 146);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtTenFile
            // 
            this.txtTenFile.EditValue = "test_20211007";
            this.txtTenFile.Location = new System.Drawing.Point(93, 86);
            this.txtTenFile.Name = "txtTenFile";
            this.txtTenFile.Size = new System.Drawing.Size(360, 20);
            this.txtTenFile.StyleController = this.layoutControl1;
            this.txtTenFile.TabIndex = 10;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(93, 60);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::NOISE_APP.Properties.Resources.loadfrom_16x16, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.btnSelectFolder.Size = new System.Drawing.Size(360, 22);
            this.btnSelectFolder.StyleController = this.layoutControl1;
            this.btnSelectFolder.TabIndex = 9;
            this.btnSelectFolder.EditValueChanged += new System.EventHandler(this.btnSelectFolder_EditValueChanged);
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(271, 110);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 22);
            this.btnCancel.StyleController = this.layoutControl1;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Thoát";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTinhToan
            // 
            this.btnTinhToan.Image = ((System.Drawing.Image)(resources.GetObject("btnTinhToan.Image")));
            this.btnTinhToan.Location = new System.Drawing.Point(121, 110);
            this.btnTinhToan.Name = "btnTinhToan";
            this.btnTinhToan.Size = new System.Drawing.Size(129, 22);
            this.btnTinhToan.StyleController = this.layoutControl1;
            this.btnTinhToan.TabIndex = 7;
            this.btnTinhToan.Text = "Tính toán";
            this.btnTinhToan.CheckedChanged += new System.EventHandler(this.btnTinhToan_CheckedChanged);
            // 
            // txtGioTinh
            // 
            this.txtGioTinh.EditValue = "13";
            this.txtGioTinh.Location = new System.Drawing.Point(93, 36);
            this.txtGioTinh.Name = "txtGioTinh";
            this.txtGioTinh.Size = new System.Drawing.Size(360, 20);
            this.txtGioTinh.StyleController = this.layoutControl1;
            this.txtGioTinh.TabIndex = 6;
            // 
            // txtNgayTinh
            // 
            this.txtNgayTinh.EditValue = new System.DateTime(2021, 10, 2, 21, 2, 25, 0);
            this.txtNgayTinh.Location = new System.Drawing.Point(93, 12);
            this.txtNgayTinh.Name = "txtNgayTinh";
            this.txtNgayTinh.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtNgayTinh.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtNgayTinh.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Classic;
            this.txtNgayTinh.Properties.DisplayFormat.FormatString = "";
            this.txtNgayTinh.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtNgayTinh.Properties.EditFormat.FormatString = "";
            this.txtNgayTinh.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtNgayTinh.Properties.HighlightTodayCell = DevExpress.Utils.DefaultBoolean.True;
            this.txtNgayTinh.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtNgayTinh.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtNgayTinh.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.False;
            this.txtNgayTinh.Size = new System.Drawing.Size(360, 20);
            this.txtNgayTinh.StyleController = this.layoutControl1;
            this.txtNgayTinh.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.emptySpaceItem1,
            this.emptySpaceItem4,
            this.simpleSeparator1,
            this.emptySpaceItem3,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(465, 146);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtNgayTinh;
            this.layoutControlItem1.CustomizationFormText = "Ngày lập bản đồ";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(445, 24);
            this.layoutControlItem1.Text = "Ngày lập bản đồ";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtGioTinh;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(445, 24);
            this.layoutControlItem3.Text = "Giờ lập bản đồ";
            this.layoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnTinhToan;
            this.layoutControlItem2.Location = new System.Drawing.Point(109, 98);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(133, 28);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnCancel;
            this.layoutControlItem4.Location = new System.Drawing.Point(259, 98);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(120, 28);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(242, 98);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(17, 28);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.Location = new System.Drawing.Point(379, 98);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(66, 28);
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // simpleSeparator1
            // 
            this.simpleSeparator1.AllowHotTrack = false;
            this.simpleSeparator1.Location = new System.Drawing.Point(0, 98);
            this.simpleSeparator1.Name = "simpleSeparator1";
            this.simpleSeparator1.Size = new System.Drawing.Size(2, 28);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(2, 98);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(107, 28);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnSelectFolder;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(445, 26);
            this.layoutControlItem5.Text = "Chọn vị trí lưu ";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(78, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtTenFile;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 74);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(445, 24);
            this.layoutControlItem6.Text = "Tên file";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(78, 13);
            // 
            // frmExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 146);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frmExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cấu hình xuất file";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTenFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGioTinh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNgayTinh.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNgayTinh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.CheckButton btnTinhToan;
        private DevExpress.XtraEditors.TextEdit txtGioTinh;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraEditors.DateEdit txtNgayTinh;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.ButtonEdit btnSelectFolder;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.TextEdit txtTenFile;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}