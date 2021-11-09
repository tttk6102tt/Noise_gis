using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOISE_APP
{
    public partial class frmChonForm : Form
    {
        public frmChonForm()
        {
            InitializeComponent();
        }

        private void btnTinhToan_Click(object sender, EventArgs e)
        {
            var frm = new FormNoisePrivate();
            frm.ShowDialog();
        }

        private void btnTinhToanDangTuyen_Click(object sender, EventArgs e)
        {

            var frm = new FormNoisePrivateDangTuyen();
            frm.ShowDialog();
        }
    }
}
