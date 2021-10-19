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
    public partial class frmConfigCreateMap : Form
    {
   
        private DateTime mNgayTinh;
        private int mGioTinh;

        public DateTime NgayTinh => mNgayTinh;
        public int GioTinh => mGioTinh;
        public frmConfigCreateMap()
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
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập kỳ tính");
                return;
            }


            DialogResult = DialogResult.OK;
        }
    }
}
