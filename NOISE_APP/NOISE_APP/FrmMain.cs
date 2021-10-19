using ESRI.ArcGIS.Geodatabase;
using NOISE_APP.Forms;
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
    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {



        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConfigMap_Click(object sender, EventArgs e)
        {
            var frm = new FrmConfig();
            frm.ShowDialog();
        }


        private void btnConfiguser_Click(object sender, EventArgs e)
        {
            var frm = new UserConfig();
            frm.ShowDialog();
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            var ngayTinh = new DateTime();
            var gioTinh = 0;
            frmConfigCreateMap frm = new frmConfigCreateMap();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ngayTinh = frm.NgayTinh;
                gioTinh = frm.GioTinh;

                string strGioTinh;

                if (gioTinh < 10)
                {
                    strGioTinh = string.Format("0{0}", gioTinh);
                }
                else
                {
                    strGioTinh = gioTinh.ToString();
                }
                
                    ctrNoiseMapMain.TinhToan(ngayTinh, gioTinh.ToString());
            }
            else
            {
                return;
            }



            //var frm = new FormNoise();
            //frm.Show();
        }


    }
}
