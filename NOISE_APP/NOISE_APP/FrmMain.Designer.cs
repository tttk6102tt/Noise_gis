namespace NOISE_APP
{
    partial class FrmMain
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConfiguser = new System.Windows.Forms.ToolStripButton();
            this.btnConfigMap = new System.Windows.Forms.ToolStripButton();
            this.btnMap = new System.Windows.Forms.ToolStripButton();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.ctrNoiseMapMain = new NOISE_APP.Controls.CtrNoiseMapMain();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConfiguser,
            this.btnConfigMap,
            this.btnMap,
            this.btnExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(744, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnConfiguser
            // 
            this.btnConfiguser.Image = global::NOISE_APP.Properties.Resources.user_16x16;
            this.btnConfiguser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConfiguser.Name = "btnConfiguser";
            this.btnConfiguser.Size = new System.Drawing.Size(140, 22);
            this.btnConfiguser.Text = "Cấu hính người dùng";
            this.btnConfiguser.Click += new System.EventHandler(this.btnConfiguser_Click);
            // 
            // btnConfigMap
            // 
            this.btnConfigMap.Image = global::NOISE_APP.Properties.Resources.piebubble_16x16;
            this.btnConfigMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConfigMap.Name = "btnConfigMap";
            this.btnConfigMap.Size = new System.Drawing.Size(155, 22);
            this.btnConfigMap.Text = "Cấu hình Dữ liệu bản đồ";
            this.btnConfigMap.Click += new System.EventHandler(this.btnConfigMap_Click);
            // 
            // btnMap
            // 
            this.btnMap.Image = global::NOISE_APP.Properties.Resources.piemap_32x32;
            this.btnMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(86, 22);
            this.btnMap.Text = "Lập bản đồ";
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::NOISE_APP.Properties.Resources.delete_16x16;
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(57, 22);
            this.btnExit.Text = "Thoát";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ctrNoiseMapMain
            // 
            this.ctrNoiseMapMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrNoiseMapMain.Location = new System.Drawing.Point(0, 25);
            this.ctrNoiseMapMain.Name = "ctrNoiseMapMain";
            this.ctrNoiseMapMain.Size = new System.Drawing.Size(744, 514);
            this.ctrNoiseMapMain.TabIndex = 1;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 539);
            this.Controls.Add(this.ctrNoiseMapMain);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmMain";
            this.Text = "Phần mềm quan trắc tiếng ồn (bản thử nghiệm)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnConfiguser;
        private System.Windows.Forms.ToolStripButton btnConfigMap;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton btnMap;
        private Controls.CtrNoiseMapMain ctrNoiseMapMain1;
        private Controls.CtrNoiseMapMain ctrNoiseMapMain;
    }
}