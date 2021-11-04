//using Molar.Business.Helpers;
using Newtonsoft.Json;
using NOISE_APP.Models;
using NOISE_APP.Models.DTO;
using RestSharp;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace NOISE_APP
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //private string FileSaveName = System.IO.Path.Combine(Application.StartupPath, ConfigurationManager.AppSettings["FileJsonDatabase"]);//"database.json";
        public FrmLogin()
        {
            InitializeComponent();
            //
            LoadData();
        }

        private void LoadData()
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //var mKhuVucLamViec = JsonHelper.GetDatabaseJson(FileSaveName);
            //if (mKhuVucLamViec.AutoLogin.Remember)
            //{
            //    txtUname.Text = mKhuVucLamViec.AutoLogin.Account.UserName;
            //    txtPasswd.Text = mKhuVucLamViec.AutoLogin.Account.Password;
            //    checkSavePasswd.Checked = mKhuVucLamViec.AutoLogin.Remember;
            //}

            this.txtUname.KeyDown += TxtUname_KeyDown;
            this.txtPasswd.KeyDown += TxtUname_KeyDown;
            this.txtUname.GotFocus += TxtUname_GotFocus;
            this.txtPasswd.GotFocus += TxtPasswd_GotFocus;

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
            if (string.IsNullOrWhiteSpace(txtPasswd.Text) == false)
                txtPasswd.SelectAll();
        }

        private void TxtUname_GotFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUname.Text) == false)
                txtUname.SelectAll();
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

        public class ResponseData
        {
            public string Message { get; set; }
            public object Data { get; set; }
            public bool Success { get; set; }
            public string Content { get; set; }
            public string ErrorCode { get; set; }
        }
        private NguoiDung _nguoidung;

        public class RestDataUser : RestData
        {
            public NguoiDung NguoiDung { get; set; }
        }

        //private bool Login(string userName, string password)
        //{
        //    try
        //    {
        //        //RestClient restClient = new RestClient("http://localhost:44373");
        //        RestClient restClient = new RestClient("http://tiengontructuyen.vn");
        //        RestRequest restRequest = new RestRequest("/Home/LoginUser");
        //        restRequest.RequestFormat = DataFormat.Json;
        //        restRequest.Method = Method.POST;
        //        restRequest.AddHeader("Authorization", "Authorization");
        //        restRequest.AddHeader("Content-Type", "multipart/form-data");
        //        restRequest.AddBody(new NguoiDung()
        //        {
        //            UserName = userName,
        //            Password = password
        //        });

        //        var response = restClient.Execute(restRequest);
        //        var result = JsonConvert.DeserializeObject<RestDataUser>(response.Content);
        //        if (result.status == Enums.EnumError.OK)
        //        {
        //            return true;
        //        }
        //        //if (response.StatusCode == HttpStatusCode.OK)
        //        //{
        //        //    return true;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return false;
        //}

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

        private bool Login(string userName, string password)
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
                        cmd.CommandText = string.Format(@"select  * from users where username = '{0}'", txtUname.Text);
                        cmd.ExecuteNonQuery();

                        var ds = new DataSet();
                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);


                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (txtPasswd.Text == "")
                            {
                                if (ds.Tables[0].Rows[0]["Password"].ToString() == "")
                                {
                                    frmChangePassword frm = new frmChangePassword();
                                    frm.UserName = txtUname.Text;
                                    if (frm.ShowDialog() == DialogResult.OK)
                                    {
                                        _NoiseMesageBox.ShowInfoMessage("Thông báo", "Đổi mật khẩu thành công");
                                        return true;
                                    }
                                }
                            }
                            var f_password = GetMD5(txtPasswd.Text);
                            if (ds.Tables[0].Rows[0]["Password"].ToString() == f_password)
                            {
                                return true;
                            }
                        }
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
            return false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtUname.Text))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập tên đăng nhập!");
                txtUname.Focus();
                return;
            }
            //if (string.IsNullOrWhiteSpace(txtPasswd.Text))
            //{
            //    _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập mật khẩu!");
            //    txtPasswd.Focus();
            //    return;
            //}
            //
            //_NoiseMesageBox.ShowSplash(this, "", "Đang đăng nhập");

            if (Login(txtUname.Text, txtPasswd.Text))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                _NoiseMesageBox.ShowErrorMessage("Đăng nhập thất bại! Vui lòng kiếm tra lại thông tin đăng nhập!");

            }
            //_NoiseMesageBox.HideSplash();
        }
    }
}