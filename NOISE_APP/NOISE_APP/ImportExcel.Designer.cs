namespace NOISE_APP
{
    partial class ImportExcel
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtLAT = new System.Windows.Forms.TextBox();
            this.txtLONG = new System.Windows.Forms.TextBox();
            this.txtTIME = new System.Windows.Forms.TextBox();
            this.txtLOCATION = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtdB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(137, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Import";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(112, 23);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 20);
            this.txtID.TabIndex = 1;
            // 
            // txtLAT
            // 
            this.txtLAT.Location = new System.Drawing.Point(112, 49);
            this.txtLAT.Name = "txtLAT";
            this.txtLAT.Size = new System.Drawing.Size(100, 20);
            this.txtLAT.TabIndex = 2;
            // 
            // txtLONG
            // 
            this.txtLONG.Location = new System.Drawing.Point(112, 75);
            this.txtLONG.Name = "txtLONG";
            this.txtLONG.Size = new System.Drawing.Size(100, 20);
            this.txtLONG.TabIndex = 3;
            // 
            // txtTIME
            // 
            this.txtTIME.Location = new System.Drawing.Point(112, 101);
            this.txtTIME.Name = "txtTIME";
            this.txtTIME.Size = new System.Drawing.Size(100, 20);
            this.txtTIME.TabIndex = 4;
            // 
            // txtLOCATION
            // 
            this.txtLOCATION.Location = new System.Drawing.Point(112, 136);
            this.txtLOCATION.Name = "txtLOCATION";
            this.txtLOCATION.Size = new System.Drawing.Size(100, 20);
            this.txtLOCATION.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "LAT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "LONG";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "TIME";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "LOCATION";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "dB";
            // 
            // txtdB
            // 
            this.txtdB.Location = new System.Drawing.Point(112, 162);
            this.txtdB.Name = "txtdB";
            this.txtdB.Size = new System.Drawing.Size(100, 20);
            this.txtdB.TabIndex = 11;
            // 
            // ImportExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtdB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLOCATION);
            this.Controls.Add(this.txtTIME);
            this.Controls.Add(this.txtLONG);
            this.Controls.Add(this.txtLAT);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.button1);
            this.Name = "ImportExcel";
            this.Text = "ImportExcel";
            this.Load += new System.EventHandler(this.ImportExcel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtLAT;
        private System.Windows.Forms.TextBox txtLONG;
        private System.Windows.Forms.TextBox txtTIME;
        private System.Windows.Forms.TextBox txtLOCATION;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtdB;
    }
}