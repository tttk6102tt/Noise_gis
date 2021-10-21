using Microsoft.Ajax.Utilities;
using NOISE_SITE.Helper;
using NOISE_SITE.Models;
using NOISE_SITE.Models.DTO;
using NOISE_SITE.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.SessionState;

namespace NOISE_SITE.Controllers
{
    public class HomeController : Controller
    {
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly INoiseRepository _noiseRepository;
        private string _ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public HomeController(NguoiDungRepository nguoiDungRepository,
            NoiseRepository noiseRepository)
        {
            _nguoiDungRepository = nguoiDungRepository;
            _noiseRepository = noiseRepository;
        }
        public ActionResult Index()
        {
            return View();

        }

        public class NoiseChart
        {
            public NoiseChart()
            {
                noise = new List<NOISE>();
            }
            public string TenTram { get; set; }
            public string ID { get; set; }
            public List<NOISE> noise { get; set; }
        }

        public class ChartOption
        {
            public ChartOption()
            {
                this.series = new List<ChartSeries>();
            }
            public List<ChartSeries> series { get; set; }
        }

        [HttpGet]
        public ActionResult GetData()
        {
            ChartOption chartOption = new ChartOption();
            DataSet ds = new DataSet();
            using (var conn = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var noises = new List<NOISE>();
                    using (var cmd = new SqlCommand())
                    {
                        var today = DateTime.Today;
                        var todayNow = DateTime.Now.AddHours(-7);//new DateTime(2021, 10, 2).AddHours(7); //

                        var hour = todayNow.Hour > 10 ? todayNow.Hour.ToString() : "0" + todayNow.Hour;

                        //var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        //var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));


                        var yesterday = Convert.ToDouble(todayNow.AddHours(-7).AddMinutes(-5).ToString("yyyyMMddhhmmss"));
                        var tomorow = Convert.ToDouble(todayNow.AddHours(-7).ToString("yyyyMMddhhmmss"));


                        var yesterHour = (todayNow.AddMinutes(-5).Hour > 10 ? todayNow.AddMinutes(-5).Hour.ToString() : "0" + todayNow.AddMinutes(-5).Hour);
                        var yesterMi = (todayNow.AddMinutes(-5).Minute > 10 ? todayNow.AddMinutes(-5).Minute.ToString() : "0" + todayNow.AddMinutes(-5).Minute);
                        var yesterss = (todayNow.AddMinutes(-5).Second > 10 ? todayNow.AddMinutes(-5).Second.ToString() : "0" + todayNow.AddMinutes(-5).Second);

                        yesterday = Convert.ToDouble(todayNow.AddMinutes(-5).ToString("yyyyMMdd") + yesterHour + yesterMi + yesterss);

                        var nowHour = (todayNow.Hour > 10 ? todayNow.Hour.ToString() : "0" + todayNow.Hour);
                        var nowMi = (todayNow.Minute > 10 ? todayNow.Minute.ToString() : "0" + todayNow.Minute);
                        var nowss = (todayNow.Second > 10 ? todayNow.Second.ToString() : "0" + todayNow.Second);

                        tomorow = Convert.ToDouble(todayNow.ToString("yyyyMMdd") + nowHour + nowMi + nowss);

                        cmd.Connection = conn;
                        cmd.CommandTimeout = 5 * 60 * 60;
                        //cmd.CommandText = string.Format("SELECT * FROM NOISE WHERE CONVERT(float,  TIME) > {0} and CONVERT(float,  TIME) < {1} and ID <> '' AND TIME <> '' AND dB <> ''  order by stt", yesterday,tomorow);
                        //cmd.CommandText = "SELECT n.ID,n.sTT, d.DiaDiem,n.dB,n.TIME FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  WHERE n.ID <> ''  AND n.dB <> '' AND n.TIME <> '' order by stt desc";

                        //cmd.CommandText = string.Format("SELECT n.ID,n.sTT, d.DiaDiem,n.dB, n.TIME FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  WHERE n.ID <> ''  AND n.dB <> '' AND n.TIME <> '' and ISNUMERIC(n.TIME) = 1 AND n.TIME LIKE '{0}%' order by stt desc", todayNow.ToString("yyyyMMdd") + hour);

                        //cmd.CommandText = string.Format("SELECT n.ID,n.sTT, d.DiaDiem,n.dB, n.TIME FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  WHERE n.TIME > '{0}' and n.TIME < '{1}'  ", yesterday,tomorow);//n.TIME LIKE '{0}%' ---- todayNow.ToString("yyyyMMdd") + hour,
                        cmd.CommandText = string.Format("SELECT n.ID,n.sTT, n.LOCATION,n.dB, n.TIME FROM NOISE n WHERE n.TIME LIKE '{0}%'  ", todayNow.ToString("yyyyMMddHH"));// ---- todayNow.ToString("yyyyMMdd") + hour,
                        //cmd.CommandText = string.Format("SELECT n.ID,n.sTT, d.DiaDiem,n.dB, n.TIME FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  WHERE n.ID <> ''  AND n.dB <> '' AND n.TIME <> '' and n.TIME LIKE '20210610%' order by stt desc", todayNow.ToString("yyyyMMdd") + hour);

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                //double time = 0.0;
                                //var doTime = double.TryParse(row["TIME"].ToString(), out time);

                                //if (doTime && time < tomorow && time > yesterday)
                                //{

                                //}

                                noises.Add(new NOISE()
                                {
                                    DiaDiem = row["LOCATION"].ToString() == "" ? row["ID"].ToString() : row["LOCATION"].ToString(),
                                    ID = row["ID"].ToString(),
                                    dB = row["dB"].ToString(),
                                    TIME = row["TIME"].ToString(),
                                    sTT = Convert.ToInt32(row["sTT"].ToString())
                                });
                            }

                            noises = noises.OrderBy(s => s.sTT).ToList();

                            var series = BuildChartSeries(noises);

                            var seriesLine = new List<ChartSeries>();
                            foreach (ChartSeries item in series)
                            {
                                seriesLine.Add(item);
                            }

                            var seriesOrder = new List<ChartSeries>();

                            seriesOrder.AddRange(seriesLine);

                            chartOption.series = seriesOrder;
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
                return Json(new { data = chartOption }, JsonRequestBehavior.AllowGet);
            }
            /*
            ChartOption chartOption = new ChartOption();
            var result = new List<NoiseChart>();
            var noises = _noiseRepository.FindAll().Where(s => !string.IsNullOrEmpty(s.TIME) && !string.IsNullOrEmpty(s.ID) && !string.IsNullOrEmpty(s.dB)).ToList();// _noiseEntity.NOISEs.Where(s => !string.IsNullOrEmpty(s.ID)).ToList();


            */


        }


        [HttpGet]
        public ActionResult GetDataStatic(string ngayTinh, string gioTinh)
        {
            ChartOption chartOption = new ChartOption();
            DataSet ds = new DataSet();
            using (var conn = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    var noises = new List<NOISE>();
                    using (var cmd = new SqlCommand())
                    {
                        var dateStrSpl = ngayTinh.Split('/');
                        var today = new DateTime(Convert.ToInt32(dateStrSpl[2]), Convert.ToInt32(dateStrSpl[1]), Convert.ToInt32(dateStrSpl[0]));// DateTime.Today;
                        var todayNow = today.AddHours(Convert.ToInt32(gioTinh)).AddHours(-7);

                        var hour = todayNow.Hour > 10 ? todayNow.Hour.ToString() : "0" + todayNow.Hour;

                        var todayStr = todayNow.ToString("yyyyMMddHH");// + gioTinh;

                        cmd.Connection = conn;

                        //cmd.CommandText = string.Format(@"SELECT  n.ID,n.sTT, d.DiaDiem,n.dB, n.TIME 
                        //                                FROM NOISE n 
                        //                                INNER JOIN DMTramDo d on d.MaTramDo = n.ID 
                        //                                WHERE 
                        //                                  TIME > '{0}' and TIME < '{1}' ",todayStr + "0000",todayStr + "5959");
                        cmd.CommandText = string.Format(@"SELECT  n.ID,n.sTT, n.LOCATION,n.dB, n.TIME 
                                                        FROM NOISE n 
                                                        
                                                        WHERE 
                                                          TIME > '{0}' and TIME < '{1}' ", todayStr + "0000", todayStr + "5959");
                        cmd.CommandTimeout = 5 * 60 * 60;

                        //cmd.CommandText = string.Format("SELECT n.ID,n.sTT, d.DiaDiem,n.dB, n.TIME FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  WHERE n.ID <> ''  AND n.dB <> '' AND n.TIME <> '' and n.TIME LIKE '20210610%' order by stt desc", todayNow.ToString("yyyyMMdd") + hour);

                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);


                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //ds.Tables[0].DefaultView.Sort = "stt desc";
                            //var data = ds.Tables[0].DefaultView.ToTable();
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                //double time = 0.0;
                                //var doTime = double.TryParse(row["TIME"].ToString(), out time);

                                //if (doTime && time < tomorow && time > yesterday)
                                //{
                                noises.Add(new NOISE()
                                {
                                    DiaDiem = row["LOCATION"].ToString() == "" ? row["ID"].ToString() : row["LOCATION"].ToString(),
                                    ID = row["ID"].ToString(),
                                    dB = row["dB"].ToString(),
                                    TIME = row["TIME"].ToString(),
                                    sTT = Convert.ToInt32(row["sTT"].ToString())
                                });
                                //}
                            }

                            noises = noises.OrderBy(s => s.sTT).ToList();

                            var series = BuildChartSeries(noises);

                            var seriesLine = new List<ChartSeries>();
                            foreach (ChartSeries item in series)
                            {
                                seriesLine.Add(item);
                            }

                            var seriesOrder = new List<ChartSeries>();

                            seriesOrder.AddRange(seriesLine);

                            chartOption.series = seriesOrder;
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
                return Json(new { data = chartOption }, JsonRequestBehavior.AllowGet);
            }
            /*
            ChartOption chartOption = new ChartOption();
            var result = new List<NoiseChart>();
            var noises = _noiseRepository.FindAll().Where(s => !string.IsNullOrEmpty(s.TIME) && !string.IsNullOrEmpty(s.ID) && !string.IsNullOrEmpty(s.dB)).ToList();// _noiseEntity.NOISEs.Where(s => !string.IsNullOrEmpty(s.ID)).ToList();


            */


        }

        [HttpGet]
        public ActionResult GetAllDataNoise()
        {

            //var noises = _noiseRepository.FindAll().Where(s => !string.IsNullOrEmpty(s.TIME) && !string.IsNullOrEmpty(s.ID) && !string.IsNullOrEmpty(s.dB)).ToList();
            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
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
                        var today = DateTime.Today;

                        var todayNow = DateTime.Now.AddHours(-7);

                        //var yesterday = Convert.ToDouble(today.AddDays(-1).ToString("yyyyMMddhhmmss"));
                        //var tomorow = Convert.ToDouble(today.AddDays(1).ToString("yyyyMMddhhmmss"));

                        var yesterday = Convert.ToDouble(todayNow.AddMinutes(-5).ToString("yyyyMMddhhmmss"));

                        var tomorow = Convert.ToDouble(todayNow.ToString("yyyyMMddhhmmss"));

                        var hour = todayNow.Hour > 10 ? todayNow.Hour.ToString() : "0" + todayNow.Hour;

                        var yesterHour = (todayNow.AddMinutes(-5).Hour > 10 ? todayNow.AddMinutes(-5).Hour.ToString() : "0" + todayNow.AddMinutes(-5).Hour);
                        var yesterMi = (todayNow.AddMinutes(-5).Minute > 10 ? todayNow.AddMinutes(-5).Minute.ToString() : "0" + todayNow.AddMinutes(-5).Minute);
                        var yesterss = (todayNow.AddMinutes(-5).Second > 10 ? todayNow.AddMinutes(-5).Second.ToString() : "0" + todayNow.AddMinutes(-5).Second);

                        yesterday = Convert.ToDouble(todayNow.AddMinutes(-5).ToString("yyyyMMdd") + yesterHour + yesterMi + yesterss);

                        var nowHour = (todayNow.Hour > 10 ? todayNow.Hour.ToString() : "0" + todayNow.Hour);
                        var nowMi = (todayNow.Minute > 10 ? todayNow.Minute.ToString() : "0" + todayNow.Minute);
                        var nowss = (todayNow.Second > 10 ? todayNow.Second.ToString() : "0" + todayNow.Second);

                        tomorow = Convert.ToDouble(todayNow.ToString("yyyyMMdd") + nowHour + nowMi + nowss);

                        cmd.Connection = conn;

                        cmd.CommandTimeout = 5 * 60 * 60;
                        //cmd.CommandText = string.Format("SELECT n.ID,n.sTT, d.DiaDiem,n.dB, n.TIME FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  WHERE n.ID <> ''  AND n.dB <> '' AND n.TIME <> '' and ISNUMERIC(n.TIME) = 1 AND n.TIME LIKE '{0}%' order by stt desc", todayNow.ToString("yyyyMMdd") + hour);

                        //cmd.CommandText = string.Format("SELECT n.ID,n.sTT, d.DiaDiem,n.dB, n.TIME FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  WHERE n.TIME LIKE '{0}%'", todayNow.ToString("yyyyMMdd") + hour);

                        //cmd.CommandText = string.Format("SELECT n.ID,n.sTT, d.DiaDiem,n.dB, n.TIME FROM NOISE n INNER JOIN DMTramDo d on d.MaTramDo = n.ID  WHERE n.TIME < '{0}' and n.TIME > '{1}'", tomorow,yesterday);

                        //cmd.CommandText = string.Format("SELECT n.ID,n.sTT, n.LOCATION,n.dB, n.TIME FROM NOISE n WHERE n.TIME < '{0}' and n.TIME > '{1}'", tomorow, yesterday);
                        cmd.CommandText = string.Format("SELECT n.ID,n.sTT, n.LOCATION,n.dB, n.TIME FROM NOISE n WHERE  n.TIME LIKE '{0}%' ", todayNow.ToString("yyyyMMddHHmm"));
                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                //double time = 0.0;
                                //var doTime = double.TryParse(row["TIME"].ToString(), out time);
                                //if (doTime && time < tomorow && time > yesterday)
                                //{

                                //}
                                noises.Add(new NOISE()
                                {
                                    DiaDiem = row["LOCATION"].ToString() == "" ? row["ID"].ToString() : row["LOCATION"].ToString(),
                                    ID = row["ID"].ToString(),
                                    dB = row["dB"].ToString(),
                                    TIME = row["TIME"].ToString(),
                                    sTT = Convert.ToInt32(row["sTT"].ToString())
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

                noises = noises.OrderBy(s => s.sTT).ToList();

                return Json(new { data = noises }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult GetLiveChart(string idTram, int oldCount)
        {
            //var result = new List<NoiseChart>();

            //var noises = _noiseRepository.FindAll().Where(s => !string.IsNullOrEmpty(s.TIME) && s.ID == idTram && !string.IsNullOrEmpty(s.dB)).OrderBy(s => s.sTT).FirstOrDefault();

            var noises = new List<NOISE>();
            DataSet ds = new DataSet();
            DataSet dsCurrent = new DataSet();
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
                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = "SELECT * FROM NOISE WHERE ID='" + idTram + "' ID <> '' AND TIME <> '' AND dB <> ''";


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count > oldCount)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                noises.Add(new NOISE()
                                {
                                    ID = row["ID"].ToString(),
                                    dB = row["dB"].ToString(),
                                    TIME = row["TIME"].ToString(),
                                    sTT = Convert.ToInt32(row["sTT"].ToString())
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


                //if (_noiseRepository.FindAll().Count() > oldCount)
                //{

                //    result.Add(new NoiseChart()
                //    {
                //        ID = idTram,
                //        TenTram = idTram,
                //        noise = new List<NOISE>()
                //    {
                //        noises
                //    }
                //    });

                //}

                noises = noises.OrderBy(s => s.sTT).ToList();

                return Json(new { data = noises }, JsonRequestBehavior.AllowGet);
            }





            //return Json(new
            //{
            //    data = result
            //}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ConnectDB()
        {
            // ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Configsys()
        {
            //var cf = _config.Configs.FirstOrDefault();
            // cf
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "login success";

            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(NGUOIDUNG nguoiDung)
        {
            var f_password = GetMD5(nguoiDung.Password);
            //var data = _nguoiDungRepository.FindAll().Where(s => s.UserName.Equals(username) && s.Password.Equals(f_password));// _config.Users.Where(s => s.UserName.Equals(username) && s.Password.Equals(f_password)).ToList();



            DataSet ds = new DataSet();
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
                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format("SELECT * FROM USERS WHERE UserName = '{0}' AND Password='{1}'", nguoiDung.UserName, f_password);


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            return Json(new RestData()
                            {
                                data = new NGUOIDUNG()
                                {
                                    Role = ds.Tables[0].Rows[0]["Role"].ToString(),
                                    Email = ds.Tables[0].Rows[0]["Email"].ToString(),//data.FirstOrDefault().Email;
                                    UserName = ds.Tables[0].Rows[0]["UserName"].ToString(),
                                    //data.FirstOrDefault().UserName;
                                },
                                status = NOISE_SITE.Enums.EnumError.OK
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng";
                            return Json(new RestBase(NOISE_SITE.Enums.EnumError.ERROR), JsonRequestBehavior.AllowGet);
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




        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var f_password = GetMD5(password);
            //var data = _nguoiDungRepository.FindAll().Where(s => s.UserName.Equals(username) && s.Password.Equals(f_password));// _config.Users.Where(s => s.UserName.Equals(username) && s.Password.Equals(f_password)).ToList();

            DataSet ds = new DataSet();
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
                        cmd.CommandTimeout = 5 * 60 * 60;
                        cmd.CommandText = string.Format("SELECT * FROM USERS WHERE UserName = '{0}'", username);


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (password == "")
                            {
                                if (ds.Tables[0].Rows[0]["Password"].ToString() == "")
                                {
                                    Session["UserName"] = username;
                                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.OK,username), JsonRequestBehavior.AllowGet);
                                }
                            }

                            if (ds.Tables[0].Rows[0]["Password"].ToString() == f_password)
                            {
                                Session["Role"] = ds.Tables[0].Rows[0]["Role"].ToString();
                                Session["FullName"] = ds.Tables[0].Rows[0]["FullName"].ToString();// data.FirstOrDefault().FullName;
                                Session["Email"] = ds.Tables[0].Rows[0]["Email"].ToString();//data.FirstOrDefault().Email;
                                Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();//data.FirstOrDefault().UserName;
                                return Json(new RestBase(NOISE_SITE.Enums.EnumError.OK), JsonRequestBehavior.AllowGet);
                            }
                        }

                        ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng";
                        return Json(new RestBase(NOISE_SITE.Enums.EnumError.ERROR), JsonRequestBehavior.AllowGet);
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




        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }

        //create a string MD5
        public static string GetMD5(string str)
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(NGUOIDUNG _user)
        {
            try
            {
                var check = _nguoiDungRepository.FindAll().Where(s => s.UserName == _user.UserName).FirstOrDefault();
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _user.ID = Guid.NewGuid().ToString();
                    _user.Role = "user";
                    _nguoiDungRepository.Insert(_user);

                    Session["Role"] = _user.Role;
                    Session["FullName"] = _user.FullName;
                    Session["Email"] = _user.Email;
                    Session["UserName"] = _user.UserName;

                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.OK), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.error = "Đã tồn tại địa chỉ tên đăng nhập này";
                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.ERROR), JsonRequestBehavior.AllowGet);
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePassword(NGUOIDUNG _user)
        {
            try
            {
                var check = _nguoiDungRepository.FindAll().Where(s => s.UserName == _user.UserName).FirstOrDefault();
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _user.ID = Guid.NewGuid().ToString();
                    _user.Role = "user";
                    _nguoiDungRepository.Update(_user);

                    Session["Role"] = _user.Role;
                    Session["FullName"] = _user.FullName;
                    Session["Email"] = _user.Email;
                    Session["UserName"] = _user.UserName;

                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.OK), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.error = "Đã tồn tại địa chỉ tên đăng nhập này";
                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.ERROR), JsonRequestBehavior.AllowGet);
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Captcha(string __ssacidRegister2 = "", bool __rn = true)
        {
            if (string.IsNullOrWhiteSpace(__ssacidRegister2))
            {
                __ssacidRegister2 = StringHelper.RandomNumber(6);
                Session["CaptChaRegister"] = __ssacidRegister2;
            }

            if (__rn)
                Session["CaptChaRegister"] = StringHelper.RandomNumber(6);// = ;
            ViewBag.__ssacid2 = Session["CaptChaRegister"].ToString();
            return File(UtilitiesHelper.CaptchaImage(Session["CaptChaRegister"].ToString(), 38, 120), "image/png");
        }
        [HttpGet]
        public bool CheckCaptchaRegister(string __submit_captcha_register)
        {
            if (__submit_captcha_register == Session["CaptChaRegister"].ToString())
            {
                return true;
            }
            return false;
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConnectDB(string loai, DateTime fromDate, DateTime toDate)
        {
            // SqlDataReader datareader;
            // string Output = "";
            // ScriptManager.RegisterStartupScript(, GetType(), "showalert", "alert('Only alert Message');", true);
            string connectionString = ConfigurationManager.ConnectionStrings["localDB"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCmm = new SqlCommand();
                sqlCmm.Connection = connection;
                sqlCmm.CommandType = CommandType.StoredProcedure;
                // string select = "select top 5 maTram,giaTriDo from TramQuanTracP  ";
                if (loai == "1") // theo ngay
                {
                    sqlCmm.CommandText = "dbo.import_tramdo_ngay";
                    // SqlCommand sqlCmm = new SqlCommand("dbo.import_tramdo_ngay", connection);
                    // sqlCmm.CommandType = CommandType.StoredProcedure;

                    sqlCmm.Parameters.Add("@thoigiando", SqlDbType.DateTime).Value = fromDate;//DateTime.Now;

                }
                else if (loai == "2")
                {

                }
                else
                {
                    sqlCmm.CommandText = "dbo.import_tramdo";
                    // SqlCommand sqlCmm = new SqlCommand("dbo.import_tramdo_ngay", connection);


                    sqlCmm.Parameters.Add("@thoigiando", SqlDbType.DateTime).Value = null;
                    sqlCmm.Parameters.Add("@thoigian_min", SqlDbType.DateTime).Value = null;
                    sqlCmm.Parameters.Add("@thoigian_max", SqlDbType.DateTime).Value = null;
                    sqlCmm.Parameters.Add("@loai", SqlDbType.NVarChar).Value = "";

                }
                connection.Open();
                int kq = sqlCmm.ExecuteNonQuery();
                if (kq > 0) ViewBag.Message = "Successfull";
                else
                    ViewBag.Message = "Khong thanh cong. Hay kiem tra lai!";

                connection.Close();

            }
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Configsys(Config _cf)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var check = _config.Configs.FirstOrDefault(s => s.tan_suat_lay_mau > 0);
                    if (check == null)
                    {

                        _config.Configuration.ValidateOnSaveEnabled = false;
                        _config.Configs.Add(_cf);
                        _config.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _config.Configuration.ValidateOnSaveEnabled = false;
                        _config.Configs.Remove(check);
                        _config.Configs.Add(_cf);
                        _config.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                return View();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return View();
        }
         */
        public class ChartSeries
        {
            public ChartSeries()
            {
                data = new List<DataChart>();
            }
            public string name { get; set; }
            public List<DataChart> data { get; set; }
            public int yAxis { get; set; }
            public int xAxis { get; set; }
        }
        public class DataChart
        {
            public DateTime x { get; set; }
            public string time { get; set; }
            public double y { get; set; }
        }
        public List<ChartSeries> BuildChartSeries(List<NOISE> noises)
        {
            var IDs = noises.OrderBy(s => s.ID).Select(s => s.DiaDiem).Distinct().ToList();

            List<ChartSeries> series = new List<ChartSeries>();

            foreach (var row in IDs)
            {
                var currentData = noises.Where(s => s.DiaDiem == row).OrderByDescending(s => s.sTT).Take(20);
                ChartSeries objSeries = new ChartSeries();
                objSeries.name = row;
                //objSeries.color = color;

                List<DataChart> data = new List<DataChart>();
                foreach (var station in currentData)
                {
                    data.Add(new DataChart()
                    {
                        time = station.TIME,
                        y = Convert.ToDouble(station.dB)
                    });
                }

                objSeries.data = data;

                series.Add(objSeries);

                try
                {
                    var max = series.Max(p => ((List<decimal?>)(((dynamic)p).data)).Max(c => c));
                    if (max > 10000000000)
                    {
                        var tp = max / 10;
                        if (tp < 1000000000) tp = 1000000000;
                        foreach (ChartSeries sbieudo in series)
                        {
                            var m = Convert.ToDecimal(sbieudo.data.Max(s => s.y)); ;//((List<decimal?>)sbieudo.data).Max(c => c);
                            if (m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                    else if (max > 1000000000)
                    {
                        var tp = max / 10;
                        if (tp < 100000000) tp = 100000000;
                        foreach (dynamic sbieudo in series)
                        {
                            var m = ((List<decimal?>)sbieudo.data).Max(c => c);
                            if (!string.IsNullOrEmpty(sbieudo.type) && m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                    else if (max > 100000000)
                    {
                        var tp = max / 10;
                        if (tp < 10000000) tp = 10000000;
                        foreach (dynamic sbieudo in series)
                        {
                            var m = ((List<decimal?>)sbieudo.data).Max(c => c);
                            if (!string.IsNullOrEmpty(sbieudo.type) && m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                    else if (max > 10000000)
                    {
                        var tp = max / 10;
                        if (tp < 1000000) tp = 1000000;
                        foreach (dynamic sbieudo in series)
                        {
                            var m = ((List<decimal?>)sbieudo.data).Max(c => c);
                            if (!string.IsNullOrEmpty(sbieudo.type) && m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                    else if (max > 1000000)
                    {
                        var tp = max / 10;
                        if (tp < 100000) tp = 100000;
                        foreach (dynamic sbieudo in series)
                        {
                            var m = ((List<decimal?>)sbieudo.data).Max(c => c);
                            if (!string.IsNullOrEmpty(sbieudo.type) && m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                    else if (max > 100000)
                    {
                        var tp = max / 10;
                        if (tp < 10000) tp = 10000;
                        foreach (dynamic sbieudo in series)
                        {
                            var m = ((List<decimal?>)sbieudo.data).Max(c => c);
                            if (!string.IsNullOrEmpty(sbieudo.type) && m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                    else if (max > 10000)
                    {
                        var tp = max / 10;
                        if (tp < 1000) tp = 1000;
                        foreach (dynamic sbieudo in series)
                        {
                            var m = ((List<decimal?>)sbieudo.data).Max(c => c);
                            if (!string.IsNullOrEmpty(sbieudo.type) && m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                    else if (max > 1000)
                    {
                        var tp = max / 10;
                        if (tp < 100) tp = 100;
                        foreach (dynamic sbieudo in series)
                        {
                            var m = ((List<decimal?>)sbieudo.data).Max(c => c);
                            if (!string.IsNullOrEmpty(sbieudo.type) && m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                    else if (max > 100)
                    {
                        var tp = max / 10;
                        if (tp < 10) tp = 10;
                        foreach (dynamic sbieudo in series)
                        {
                            var m = ((List<decimal?>)sbieudo.data).Max(c => c);
                            if (!string.IsNullOrEmpty(sbieudo.type) && m < tp)
                            {
                                sbieudo.yAxis = 1;
                            }
                        }
                    }
                }
                catch { }
            }

            return series;
        }

    }
}