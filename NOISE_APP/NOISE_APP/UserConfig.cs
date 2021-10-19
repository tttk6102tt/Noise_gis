using DevExpress.XtraExport;
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
    public partial class UserConfig : Form
    {
        //private string _ConnectionString = "Server=DESKTOP-1V8JKKD;Database=NOISE_CONFIG;User Id=sa;Password=Abc@123456;MultipleActiveResultSets=true";
        private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public UserConfig()
        {
            InitializeComponent();
        }
        private bool _isInsert = false;
        private void UserConfig_Load(object sender, EventArgs e)
        {
            BindComboRole();

            setEnableButton(true);

            var listUser = GetAllNguoiDung();

            BindingSource objSource = new BindingSource();

            objSource.DataSource = listUser;
            nvUser.BindingSource = objSource;
            grvUser.DataSource = objSource;
            grvUser.Enabled = true;
        }

        private List<NGUOIDUNG> GetAllNguoiDung()
        {
            DataSet ds = new DataSet();
            var result = new List<NGUOIDUNG>();
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
                        cmd.CommandText = @"SELECT [UserName]
                                              ,[FullName]
                                              ,[Email]
                                              ,[Password]
                                              ,[ID]
                                              ,[LockoutEnabled]
                                              ,[PhoneNumber]
                                              ,[Role]
                                          FROM [Users]";


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {

                                result.Add(new NGUOIDUNG()
                                {
                                    ID = row["ID"].ToString(),
                                    FullName = row["FullName"].ToString(),
                                    Email = row["Email"].ToString(),
                                    PhoneNumber = row["PhoneNumber"].ToString(),
                                    UserName = row["UserName"].ToString(),
                                    Role = row["Role"].ToString(),
                                    Role_str = GetRoleFromData(row["Role"].ToString())
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
            }


            return result;
        }

        private NGUOIDUNG GetUserByID(string userName)
        {
            DataSet ds = new DataSet();
            var result = new NGUOIDUNG();
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
                        cmd.CommandText = string.Format(@"SELECT [UserName]
                                              ,[FullName]
                                              ,[Email]
                                              ,[Password]
                                              ,[ID]
                                              ,[LockoutEnabled]
                                              ,[PhoneNumber]
                                              ,[Role]
                                          FROM [Users] WHERE UserName = '{0}'", txtUserName.Text);


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {

                                result = new NGUOIDUNG()
                                {
                                    ID = row["ID"].ToString(),
                                    FullName = row["FullName"].ToString(),
                                    Email = row["Email"].ToString(),
                                    PhoneNumber = row["PhoneNumber"].ToString(),
                                    UserName = row["UserName"].ToString(),
                                    Role = row["Role"].ToString(),
                                    Role_str = GetRoleFromData(row["Role"].ToString())
                                };
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
            }


            return result;
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
        public List<ComboboxItem> lstTrangThaiGCN = new List<ComboboxItem>();
        private void BindComboRole()
        {
            lstTrangThaiGCN = (new List<ComboboxItem>(){
                new ComboboxItem()
                {
                    Text = "Nhân viên quản lý",
                    Value = "administrator"
                },
                  new ComboboxItem()
                {
                    Text = "Quản trị viên",
                    Value = "sa"
                },
                 new ComboboxItem()
                {
                    Text = "Người dùng",
                    Value = "user"
                }
            });
            cbbRole.Items.Clear();
            cbbRole.Items.AddRange(lstTrangThaiGCN.ToArray());
        }

        private string GetRoleFromData(string role)
        {
            switch (role)
            {
                case "administrator":
                    return "Nhân viên quản lý";
                case "sa":
                    return "Quản trị viên";
                default:
                    return "Người dùng";
            }
        }

        private void grvUser_SelectionChanged(object sender, EventArgs e)
        {
            if (grvUser.CurrentRow.Index >= 0)
            {
                txtUserName.Text = grvUser.CurrentRow.Cells["UserName"].Value.ToString();
                txtFullName.Text = grvUser.CurrentRow.Cells["FullName"].Value.ToString();
                txtPhoneName.Text = grvUser.CurrentRow.Cells["PhoneNumber"].Value.ToString();
                txtEmail.Text = grvUser.CurrentRow.Cells["Email"].Value.ToString();

                cbbRole.SelectedItem = string.IsNullOrEmpty(grvUser.CurrentRow.Cells["Role"].Value.ToString()) ? lstTrangThaiGCN.FirstOrDefault(s => s.Value.ToString() == "user") : lstTrangThaiGCN.FirstOrDefault(s => s.Value.ToString() == grvUser.CurrentRow.Cells["Role"].Value.ToString());
            }
        }

        private bool UpdateOrCreate()
        {
            try
            {
                if (_isInsert)
                {
                    Create();
                }
                else
                {
                    Update();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }

        private bool Create()
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
                        cmd.CommandText = string.Format(@"INSERT INTO [dbo].[Users]
                                               ([UserName]
                                               ,[FullName]
                                               ,[Email]
                                               ,[Password]
                                               ,[ID]
                                               ,[LockoutEnabled]
                                               ,[PhoneNumber]
                                               ,[Role])
                                         VALUES
                                               ('{0}'
                                               ,N'{1}'
                                               ,'{2}'
                                               ,'123456'
                                               ,'{3}'
                                               ,0
                                               ,'{4}'
                                               ,'{5}')", txtUserName.Text, txtFullName.Text, txtEmail.Text, Guid.NewGuid().ToString(), txtPhoneName.Text, (cbbRole.SelectedItem as ComboboxItem).Value.ToString());
                        cmd.ExecuteNonQuery();

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
            return true;
        }

        private bool Update()
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
                        cmd.CommandText = string.Format(@"UPDATE [Users]
                                                           SET 
                                                              [FullName] = N'{0}'
                                                              ,[Email] = '{1}'
                                                              ,[PhoneNumber] = '{2}'
                                                              ,[Role] = '{3}'
                                                         WHERE UserName = '{4}'", txtFullName.Text, txtEmail.Text, txtPhoneName.Text, (cbbRole.SelectedItem as ComboboxItem).Value.ToString(), txtUserName.Text);
                        cmd.ExecuteNonQuery();

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
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ResetText();
            grvUser.Enabled = false;

            _isInsert = true;

            setEnableButton(false);
        }

        private void ResetText()
        {
            txtEmail.Text = "";
            txtFullName.Text = "";
            txtPhoneName.Text = "";
            txtUserName.Text = "";
            cbbRole.SelectedItem = lstTrangThaiGCN.FirstOrDefault(s => s.Value.ToString() == "user");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _isInsert = false;
            txtUserName.Enabled = false;
            setEnableButton(false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        void setEnableButton(bool status)
        {
            grvUser.Enabled = status;

            btnDelete.Visible = status;
            btnAdd.Visible = status;
            btnUpdate.Visible = status;

            btnCancel.Visible = !status;
            btnSave.Visible = !status;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateOrCreate();

            UserConfig_Load(null, null);
            setEnableButton(true);
            txtUserName.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UserConfig_Load(null, null);
            setEnableButton(true);
        }
    }
}
