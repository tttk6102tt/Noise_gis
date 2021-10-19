using NOISE_SITE.Models;
using NOISE_SITE.Models.DTO;
using NOISE_SITE.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOISE_SITE.Controllers
{
    public class TramDoController : Controller
    {
        private string _ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        private IDMTramDoRepository _dmTramDoRepository;
        public TramDoController(DMTramDoRepository dMTramDoRepository)
        {
            _dmTramDoRepository = dMTramDoRepository;
        }
        // GET: TramDo
        public ActionResult Index()
        {
            if (Session["Role"] != null && Session["Role"].ToString() == "sa")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult List()
        {
            var result = new List<DMTramDo>();
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
                        cmd.CommandText = "SELECT * FROM DMTramDo";


                        var dap = new SqlDataAdapter(cmd);
                        dap.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                result.Add(new DMTramDo()
                                {
                                    MaTramDo = row["MaTramDo"].ToString(),
                                    DiaDiem = row["DiaDiem"].ToString(),
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
                return Json(new RestData
                {
                    data = result
                }, JsonRequestBehavior.AllowGet); ;
            }




        }

        [HttpPost]

        public ActionResult Create(DMTramDo dMTramDo)
        {
            try
            {
                var check = _dmTramDoRepository.FindAll().Where(s => s.MaTramDo == dMTramDo.MaTramDo).FirstOrDefault();
                if (check == null)
                {
                    _dmTramDoRepository.Insert(dMTramDo);

                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.OK), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.error = "Đã tồn tại trạm đo này";
                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.ERROR), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]

        public ActionResult Update(DMTramDo dMTramDo)
        {
            try
            {
                var check = _dmTramDoRepository.FindAll().Where(s => s.MaTramDo == dMTramDo.MaTramDo).FirstOrDefault();
                if (check != null)
                {
                    _dmTramDoRepository.Update(dMTramDo);

                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.OK), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.error = "Đã tồn tại trạm đo này";
                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.ERROR), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]

        public ActionResult Delete(DMTramDo user)
        {
            try
            {
                var check = _dmTramDoRepository.FindAll().Where(s => s.MaTramDo == user.MaTramDo).FirstOrDefault();
                if (check != null)
                {
                    _dmTramDoRepository.Delete(user);

                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.OK), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.error = "Đã tồn tại trạm đo này";
                    return Json(new RestBase(NOISE_SITE.Enums.EnumError.ERROR), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}