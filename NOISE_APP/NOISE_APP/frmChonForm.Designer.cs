namespace NOISE_APP
{
    partial class frmChonForm
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
            this.btnTinhToan = new System.Windows.Forms.Button();
            this.btnTinhToanDangTuyen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTinhToan
            // 
            this.btnTinhToan.Location = new System.Drawing.Point(22, 22);
            this.btnTinhToan.Name = "btnTinhToan";
            this.btnTinhToan.Size = new System.Drawing.Size(122, 23);
            this.btnTinhToan.TabIndex = 0;
            this.btnTinhToan.Text = "Tính toán";
            this.btnTinhToan.UseVisualStyleBackColor = true;
            this.btnTinhToan.Click += new System.EventHandler(this.btnTinhToan_Click);
            // 
            // btnTinhToanDangTuyen
            // 
            this.btnTinhToanDangTuyen.Location = new System.Drawing.Point(22, 62);
            this.btnTinhToanDangTuyen.Name = "btnTinhToanDangTuyen";
            this.btnTinhToanDangTuyen.Size = new System.Drawing.Size(122, 23);
            this.btnTinhToanDangTuyen.TabIndex = 1;
            this.btnTinhToanDangTuyen.Text = "Tinh toán dạng tuyến";
            this.btnTinhToanDangTuyen.UseVisualStyleBackColor = true;
            this.btnTinhToanDangTuyen.Click += new System.EventHandler(this.btnTinhToanDangTuyen_Click);
            // 
            // frmChonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(177, 97);
            this.Controls.Add(this.btnTinhToanDangTuyen);
            this.Controls.Add(this.btnTinhToan);
            this.Name = "frmChonForm";
            this.Text = "frmChonForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTinhToan;
        private System.Windows.Forms.Button btnTinhToanDangTuyen;
    }
}