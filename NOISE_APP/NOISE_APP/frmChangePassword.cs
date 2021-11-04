//using Molar.Business.Helpers;
using Newtonsoft.Json;
using NOISE_APP.Models;
using NOISE_APP.Models.DTO;
using RestSharp;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace NOISE_APP
{
    public partial class frmChangePassword : DevExpress.XtraEditors.XtraForm
    {
        private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public frmChangePassword()
        {
            InitializeComponent();
            //
            LoadData();
        }

        public string UserName {get;set;}

        private string mUserName;

        private void LoadData()
        {
            this.txtPwd.KeyDown += TxtUname_KeyDown;
            this.txtRePasswd.KeyDown += TxtUname_KeyDown;
            this.txtPwd.GotFocus += TxtUname_GotFocus;
            this.txtRePasswd.GotFocus += TxtPasswd_GotFocus;

            btnOk.Click += (sender, e) =>
            {
                btnOk_Click(sender, e);
            };
            btnCancel.Click += (sender, e) =>
            {
                btnCancel_Click(sender, e);
            };
        }

        private void TxtPasswd_GotFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRePasswd.Text) == false)
                txtRePasswd.SelectAll();
        }

        private void TxtUname_GotFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPwd.Text) == false)
                txtPwd.SelectAll();
        }

        private void TxtUname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOk_Click(sender, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private bool ChangePassword(string password)
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
                                                              Password = '{0}'
                                                         WHERE UserName = '{1}'", GetMD5(password),UserName);
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

        public string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập mật khẩu!");
                txtPwd.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtRePasswd.Text))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập mật khẩu nhắc lại!");
                txtRePasswd.Focus();
                return;
            }
            //
            if (ChangePassword(txtPwd.Text))
            {
                
                DialogResult = DialogResult.OK;
            }
            else
            {
                _NoiseMesageBox.ShowErrorMessage("Đăng nhập thất bại! Vui lòng kiếm tra lại!");

            }
            _NoiseMesageBox.HideSplash();
        }
    }
}