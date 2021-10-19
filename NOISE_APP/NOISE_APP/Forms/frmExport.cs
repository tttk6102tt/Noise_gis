using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOISE_APP.Forms
{
    public partial class frmExport : Form
    {
   
        private DateTime mNgayTinh;
        private int mGioTinh;
        private string mSaveFolder;
        private string mFileName;

        public DateTime NgayTinh => mNgayTinh;
        public int GioTinh => mGioTinh;
        public string SaveFolder => mSaveFolder;
        public string FileName => mFileName;
        public frmExport()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnTinhToan_CheckedChanged(object sender, EventArgs e)
        {
            mNgayTinh = Convert.ToDateTime(txtNgayTinh.EditValue);
            if (!int.TryParse(txtGioTinh.Text, out mGioTinh))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập giờ tính");
                return;
            }

            if (string.IsNullOrEmpty(txtTenFile.Text))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập tên file export");
                return;
            }
            else
            {
                mFileName = txtTenFile.Text;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                btnSelectFolder.Text = dialog.SelectedPath;
                mSaveFolder = dialog.SelectedPath;
            }
        }

        private void btnSelectFolder_EditValueChanged(object sender, EventArgs e)
        {
           
        }
    }
}
