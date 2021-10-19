namespace NOISE_APP
{
    partial class FormNoise
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNoise));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSelectGDB = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProcess = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectGDB.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Tính toán";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSelectGDB
            // 
            this.btnSelectGDB.EditValue = "E:\\Thuy\\CSDL_ONhiemTiengOn.gdb";
            this.btnSelectGDB.Location = new System.Drawing.Point(182, 177);
            this.btnSelectGDB.Name = "btnSelectGDB";
            this.btnSelectGDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.Default, ((System.Drawing.Image)(resources.GetObject("btnSelectGDB.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.btnSelectGDB.Size = new System.Drawing.Size(338, 22);
            this.btnSelectGDB.TabIndex = 1;
            this.btnSelectGDB.Visible = false;
            this.btnSelectGDB.EditValueChanged += new System.EventHandler(this.btnSelectGDB_EditValueChanged);
            this.btnSelectGDB.Click += new System.EventHandler(this.btnSelectGDB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Chọn gdb";
            this.label1.Visible = false;
            // 
            // txtProcess
            // 
            this.txtProcess.Location = new System.Drawing.Point(0, 0);
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.Size = new System.Drawing.Size(541, 245);
            this.txtProcess.TabIndex = 3;
            this.txtProcess.Text = "";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 210);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormNoise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 245);
            this.Controls.Add(this.txtProcess);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectGDB);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Name = "FormNoise";
            this.Text = "Tính toán, xử lý dữ liệu quan trắc tiếng ồn tự động";
            this.Load += new System.EventHandler(this.FormNoise_Load);
            this.Shown += new System.EventHandler(this.FormNoise_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectGDB.Properties)).EndInit();
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
    }
}