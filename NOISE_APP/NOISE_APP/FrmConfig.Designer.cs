namespace NOISE_APP
{
    partial class FrmConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfig));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGDB = new DevExpress.XtraEditors.ButtonEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSizeWidth = new System.Windows.Forms.TextBox();
            this.txtCellSizeIDW = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSizeHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRadius = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnLayerTemp = new DevExpress.XtraEditors.ButtonEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnGDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLayerTemp.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File GDB";
            // 
            // btnGDB
            // 
            this.btnGDB.Enabled = false;
            this.btnGDB.Location = new System.Drawing.Point(103, 17);
            this.btnGDB.Name = "btnGDB";
            this.btnGDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("btnGDB.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.btnGDB.Size = new System.Drawing.Size(600, 22);
            this.btnGDB.TabIndex = 1;
            this.btnGDB.Click += new System.EventHandler(this.btnGDB_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cell size with";
            // 
            // txtSizeWidth
            // 
            this.txtSizeWidth.Enabled = false;
            this.txtSizeWidth.Location = new System.Drawing.Point(103, 83);
            this.txtSizeWidth.Name = "txtSizeWidth";
            this.txtSizeWidth.Size = new System.Drawing.Size(267, 20);
            this.txtSizeWidth.TabIndex = 3;
            // 
            // txtCellSizeIDW
            // 
            this.txtCellSizeIDW.Enabled = false;
            this.txtCellSizeIDW.Location = new System.Drawing.Point(478, 83);
            this.txtCellSizeIDW.Name = "txtCellSizeIDW";
            this.txtCellSizeIDW.Size = new System.Drawing.Size(225, 20);
            this.txtCellSizeIDW.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(383, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "CellSize nội suy";
            // 
            // txtSizeHeight
            // 
            this.txtSizeHeight.Enabled = false;
            this.txtSizeHeight.Location = new System.Drawing.Point(103, 124);
            this.txtSizeHeight.Name = "txtSizeHeight";
            this.txtSizeHeight.Size = new System.Drawing.Size(267, 20);
            this.txtSizeHeight.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Cell size height";
            // 
            // txtRadius
            // 
            this.txtRadius.Enabled = false;
            this.txtRadius.Location = new System.Drawing.Point(478, 124);
            this.txtRadius.Name = "txtRadius";
            this.txtRadius.Size = new System.Drawing.Size(225, 20);
            this.txtRadius.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(383, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Radius";
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::NOISE_APP.Properties.Resources.cancel_16x16;
            this.btnCancel.Location = new System.Drawing.Point(628, 164);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Hủy bỏ";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::NOISE_APP.Properties.Resources.save_16x16;
            this.btnSave.Location = new System.Drawing.Point(547, 164);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Lưu";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLayerTemp
            // 
            this.btnLayerTemp.Enabled = false;
            this.btnLayerTemp.Location = new System.Drawing.Point(103, 55);
            this.btnLayerTemp.Name = "btnLayerTemp";
            this.btnLayerTemp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("btnLayerTemp.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.btnLayerTemp.Size = new System.Drawing.Size(600, 22);
            this.btnLayerTemp.TabIndex = 13;
            this.btnLayerTemp.Click += new System.EventHandler(this.btnLayerTemp_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Lớp phạm vi";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::NOISE_APP.Properties.Resources.edit_16x16;
            this.btnUpdate.Location = new System.Drawing.Point(547, 164);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 14;
            this.btnUpdate.Text = "Sửa";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 199);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnLayerTemp);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtRadius);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSizeHeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCellSizeIDW);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSizeWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGDB);
            this.Controls.Add(this.label1);
            this.Name = "FrmConfig";
            this.Text = "Cấu hình dữ liệu lập bản đồ";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnGDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLayerTemp.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ButtonEdit btnGDB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSizeWidth;
        private System.Windows.Forms.TextBox txtCellSizeIDW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSizeHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRadius;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.ButtonEdit btnLayerTemp;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.SimpleButton btnUpdate;
    }
}