namespace NOISE_APP
{
    partial class FormNoisePrivateDangTuyen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNoisePrivateDangTuyen));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSelectGDB = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProcess = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.txtDate = new DevExpress.XtraEditors.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTemp = new System.Windows.Forms.TextBox();
            this.btnGDB = new DevExpress.XtraEditors.ButtonEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.btnMXD = new DevExpress.XtraEditors.ButtonEdit();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectGDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMXD.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(407, 299);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Tính toán";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSelectGDB
            // 
            this.btnSelectGDB.EditValue = "E:\\Thuy\\CSDL_ONhiemTiengOn.gdb";
            this.btnSelectGDB.Location = new System.Drawing.Point(64, 350);
            this.btnSelectGDB.Name = "btnSelectGDB";
            this.btnSelectGDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.Default, ((System.Drawing.Image)(resources.GetObject("btnSelectGDB.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.btnSelectGDB.Size = new System.Drawing.Size(338, 22);
            this.btnSelectGDB.TabIndex = 1;
            this.btnSelectGDB.Visible = false;
            this.btnSelectGDB.EditValueChanged += new System.EventHandler(this.btnSelectGDB_EditValueChanged);
            this.btnSelectGDB.Click += new System.EventHandler(this.btnSelectGDB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Chọn gdb";
            this.label1.Visible = false;
            // 
            // txtProcess
            // 
            this.txtProcess.Location = new System.Drawing.Point(12, 12);
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.Size = new System.Drawing.Size(473, 138);
            this.txtProcess.TabIndex = 3;
            this.txtProcess.Text = "";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(429, 349);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtDate
            // 
            this.txtDate.EditValue = new System.DateTime(2021, 11, 5, 21, 1, 59, 0);
            this.txtDate.Location = new System.Drawing.Point(111, 163);
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Classic;
            this.txtDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.False;
            this.txtDate.Size = new System.Drawing.Size(223, 20);
            this.txtDate.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Ngày";
            // 
            // btnFolder
            // 
            this.btnFolder.EditValue = "D:\\MXD_result_DangTuyen";
            this.btnFolder.Location = new System.Drawing.Point(111, 189);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.Default, ((System.Drawing.Image)(resources.GetObject("btnFolder.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.btnFolder.Size = new System.Drawing.Size(371, 22);
            this.btnFolder.TabIndex = 7;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Chọn folder lưu trữ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Tên tiền tố bản đồ";
            // 
            // btnTemp
            // 
            this.btnTemp.Location = new System.Drawing.Point(111, 215);
            this.btnTemp.Name = "btnTemp";
            this.btnTemp.Size = new System.Drawing.Size(371, 20);
            this.btnTemp.TabIndex = 10;
            // 
            // btnGDB
            // 
            this.btnGDB.EditValue = "E:\\20210825\\CSDLTest\\CSDL_ONhiemTiengOn.gdb";
            this.btnGDB.Location = new System.Drawing.Point(111, 271);
            this.btnGDB.Name = "btnGDB";
            this.btnGDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.Default, ((System.Drawing.Image)(resources.GetObject("btnGDB.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.btnGDB.Size = new System.Drawing.Size(371, 22);
            this.btnGDB.TabIndex = 20;
            this.btnGDB.Click += new System.EventHandler(this.btnGDB_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 275);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Chọn file gdb";
            // 
            // btnMXD
            // 
            this.btnMXD.EditValue = "E:\\20210825\\CSDLTest\\VN2000.mxd";
            this.btnMXD.Location = new System.Drawing.Point(111, 243);
            this.btnMXD.Name = "btnMXD";
            this.btnMXD.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.Default, ((System.Drawing.Image)(resources.GetObject("btnMXD.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true)});
            this.btnMXD.Size = new System.Drawing.Size(371, 22);
            this.btnMXD.TabIndex = 18;
            this.btnMXD.Click += new System.EventHandler(this.btnMXD_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 247);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Chọn file mxd temp";
            // 
            // FormNoisePrivateDangTuyen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 384);
            this.Controls.Add(this.btnGDB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnMXD);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnTemp);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.txtProcess);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectGDB);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Name = "FormNoisePrivateDangTuyen";
            this.Text = "Phần mềm thành lập dữ liệu tiếng ồn tự động (ANoise)";
            this.Load += new System.EventHandler(this.FormNoise_Load);
            this.Shown += new System.EventHandler(this.FormNoise_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectGDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMXD.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private DevExpress.XtraEditors.ButtonEdit btnSelectGDB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtProcess;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private DevExpress.XtraEditors.DateEdit txtDate;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit btnFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox btnTemp;
        private DevExpress.XtraEditors.ButtonEdit btnGDB;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.ButtonEdit btnMXD;
        private System.Windows.Forms.Label label7;
    }
}