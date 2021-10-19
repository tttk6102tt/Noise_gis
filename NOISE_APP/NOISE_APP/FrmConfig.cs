using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;
using NOISE_APP.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOISE_APP
{
    public partial class FrmConfig : Form
    {
        private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public FrmConfig()
        {
            InitializeComponent();
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            var data = GetDataForForm();

            txtCellSizeIDW.Text = data.FirstOrDefault(s => s.TenThamSo == "CELL_SIZE_IDW").GiaTri;
            txtRadius.Text = data.FirstOrDefault(s => s.TenThamSo == "RADIUS_IDW").GiaTri;
            txtSizeHeight.Text = data.FirstOrDefault(s => s.TenThamSo == "SIZE_HEIGHT").GiaTri;
            txtSizeWidth.Text = data.FirstOrDefault(s => s.TenThamSo == "SIZE_WIDTH").GiaTri;
            btnGDB.EditValue = data.FirstOrDefault(s => s.TenThamSo == "FILE_GDB").GiaTri;
            btnLayerTemp.EditValue = data.FirstOrDefault(s => s.TenThamSo == "FC_TEMP").GiaTri;
        }

        List<TBLConfig> GetDataForForm()
        {
            DataSet ds = new DataSet();
            var result = new List<TBLConfig>();
            using (var conn = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"SELECT [ID]
                                              ,[TenThamSo]
                                              ,[GiaTri]
                                              ,[Param_Config]
                                          FROM [TBLCONFIG]";


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {

                                result.Add(new TBLConfig()
                                {
                                    TenThamSo = row["TenThamSo"].ToString(),
                                    GiaTri = row["GiaTri"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return result;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Update();
            setEnableButton(false);
        }

        bool Update()
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        string query = string.Empty;
                        query = string.Format(@"Update TBLCONFIG set GiaTri = '{0}' where TenThamSo = 'CELL_SIZE_IDW';", txtCellSizeIDW.Text) +
                            string.Format(@"Update TBLCONFIG set GiaTri = '{0}' where TenThamSo = 'RADIUS_IDW';", txtRadius.Text) +
                        string.Format(@"Update TBLCONFIG set GiaTri = '{0}' where TenThamSo = 'SIZE_HEIGHT';", txtSizeHeight.Text) +
                        string.Format(@"Update TBLCONFIG set GiaTri = '{0}' where TenThamSo = 'SIZE_WIDTH';", txtSizeWidth.Text) +
                        string.Format(@"Update TBLCONFIG set GiaTri = '{0}' where TenThamSo = 'FILE_GDB';", btnGDB.EditValue.ToString()) +
                        string.Format(@"Update TBLCONFIG set GiaTri = '{0}' where TenThamSo = 'FC_TEMP';", btnLayerTemp.EditValue.ToString());
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnGDB_Click(object sender, EventArgs e)
        {
            try
            {
                IGxDialog pGxDialog = new GxDialog();
                //IGxObjectFilter pGxObjFilter = new GxFilterRasterDatasets();
                IGxObjectFilter gxObjectFilter = new GxFilterFileGeodatabases();
                IEnumGxObject pEnumGxObj;
                pGxDialog.ObjectFilter = gxObjectFilter;
                pGxDialog.Title = "Chọn gdb";
                pGxDialog.ButtonCaption = "OK";
                if (pGxDialog.DoModalOpen(0, out pEnumGxObj))
                {
                    IGxObject pGxObj = pEnumGxObj.Next();
                    IGxDataset pGxDataset = pGxObj as IGxDataset;
                    btnGDB.EditValue = pGxObj.FullName;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng chọn định dạng gdb");
            }
        }

        private void btnLayerTemp_Click(object sender, EventArgs e)
        {
            try
            {
                IGxDialog pGxDialog = new GxDialog();
                //IGxObjectFilter pGxObjFilter = new GxFilterRasterDatasets();
                IGxObjectFilter gxObjectFilter = new GxFilterFeatureClasses();
                IEnumGxObject pEnumGxObj;
                pGxDialog.ObjectFilter = gxObjectFilter;
                pGxDialog.Title = "Chọn lớp ranh giới";
                pGxDialog.ButtonCaption = "OK";
                if (pGxDialog.DoModalOpen(0, out pEnumGxObj))
                {
                    IGxObject pGxObj = pEnumGxObj.Next();
                    IGxDataset pGxDataset = pGxObj as IGxDataset;
                    btnLayerTemp.EditValue = System.IO.Path.GetFileName( pGxObj.FullName);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng chọn định dạng FeatureClass");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            setEnableButton(true);
        }

        void setEnableButton(bool status)
        {
            btnSave.Visible = status;
            btnUpdate.Visible = !status;
            txtCellSizeIDW.Enabled = status;
            txtRadius.Enabled = status;
            txtSizeHeight.Enabled = status;
            txtSizeWidth.Enabled = status;
            btnGDB.Enabled = status;
            btnLayerTemp.Enabled = status;
        }


    }
}
