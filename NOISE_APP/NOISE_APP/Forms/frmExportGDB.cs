using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;
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
    public partial class frmExportGDB : Form
    {
   
        private DateTime mNgayTinh;
        private int mGioTinh;
        private string mSaveFolder;
        private string mFileName;

        public string FileName => mFileName;
        public DateTime NgayTinh => mNgayTinh;
        public int GioTinh => mGioTinh;
        public string SaveFolder => mSaveFolder;

        public frmExportGDB()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnTinhToan_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenFile.Text))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập tên file export");
                return;
            }
            else
            {
                mFileName = txtTenFile.Text;
            }
            mNgayTinh = Convert.ToDateTime(txtNgayTinh.EditValue);
            if (!int.TryParse(txtGioTinh.Text, out mGioTinh))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập giờ tính");
                return;
            }


            DialogResult = DialogResult.OK;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            try
            {
                IGxDialog pGxDialog = new GxDialog();
                //IGxObjectFilter pGxObjFilter = new GxFilterRasterDatasets();
                IGxObjectFilter gxObjectFilter = new GxFilterDefaultDatabaseWorkspaces (); //GxFilterGeoDatasets();
                IEnumGxObject pEnumGxObj;
                pGxDialog.ObjectFilter = gxObjectFilter;
                pGxDialog.Title = "Chọn lớp ranh giới";
                pGxDialog.ButtonCaption = "OK";
                if (pGxDialog.DoModalOpen(0, out pEnumGxObj))
                {
                    IGxObject pGxObj = pEnumGxObj.Next();
                    IGxDataset pGxDataset = pGxObj as IGxDataset;
                    btnSelectFolder.EditValue = System.IO.Path.GetFileName(pGxObj.FullName);
                    mSaveFolder = pGxObj.FullName;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng chọn định dạng FeatureClass");
            }
        }

        private void btnSelectFolder_EditValueChanged(object sender, EventArgs e)
        {
           
        }
    }
}
