//using Molar.Business.Helpers;
using Newtonsoft.Json;
using NOISE_APP.Models;
using NOISE_APP.Models.DTO;
using RestSharp;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace NOISE_APP
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
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

        private bool Login(string userName, string password)
        {
            try
            {
                //RestClient restClient = new RestClient("http://localhost:44373");
                RestClient restClient = new RestClient("http://tiengontructuyen.vn");
                RestRequest restRequest = new RestRequest("/Home/LoginUser");
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.Method = Method.POST;
                restRequest.AddHeader("Authorization", "Authorization");
                restRequest.AddHeader("Content-Type", "multipart/form-data");
                restRequest.AddBody(new NguoiDung()
                {
                    UserName = userName,
                    Password = password
                });

                var response = restClient.Execute(restRequest);
                var result = JsonConvert.DeserializeObject<RestDataUser>(response.Content);
                if (result.status == Enums.EnumError.OK)
                {
                    return true;
                }
                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    return true;
                //}
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        //private ResponseData Login(string userName, string password)
        //{
        //    try
        //    {
        //        RestClient restClient = new RestClient("http://localhost:44373");
        //        RestRequest restRequest = new RestRequest("/Home/Login");
        //        restRequest.RequestFormat = DataFormat.Json;
        //        restRequest.Method = Method.GET;
        //        //restRequest.AddHeader("Authorization", "Authorization");
        //        //restRequest.AddHeader("Content-Type", "multipart/form-data");
        //        restRequest.AddParameter("username", userName);
        //        restRequest.AddParameter("password", password);

        //        var response = restClient.Execute(restRequest);

        //        var result = JsonConvert.DeserializeObject<ResponseData>(response.Content);

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData()
        //        {

        //        };
        //    }
        //}

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUname.Text))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập tên đăng nhập!");
                txtUname.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPasswd.Text))
            {
                _NoiseMesageBox.ShowErrorMessage("Vui lòng nhập mật khẩu!");
                txtPasswd.Focus();
                return;
            }
            //
            _NoiseMesageBox.ShowSplash(this, "", "Đang đăng nhập");

            if (Login(txtUname.Text, txtPasswd.Text))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                _NoiseMesageBox.ShowErrorMessage("Đăng nhập thất bại! Vui lòng kiếm tra lại thông tin đăng nhập!");

            }
            _NoiseMesageBox.HideSplash();
            //Login("admin","123321");

            //if (await _MolarMain.Identity.SignIn(txtUname.Text, txtPasswd.Text))//)
            //{
            //    var jsonData = System.IO.File.ReadAllText(FileSaveName);
            //    JavaScriptSerializer jss = new JavaScriptSerializer();
            //    var mKhuVucLamViec = JsonHelper.GetDatabaseJson(FileSaveName);
            //    mKhuVucLamViec.AutoLogin.Remember = checkSavePasswd.Checked;
            //    mKhuVucLamViec.AutoLogin.Account.Password = txtPasswd.Text;
            //    mKhuVucLamViec.AutoLogin.Account.UserName = txtUname.Text;
            //    var stringObj = jss.Serialize(mKhuVucLamViec);
            //    File.WriteAllText(FileSaveName, stringObj);
            //    this.DialogResult = DialogResult.OK;

            //}
            //else

        }
    }
}