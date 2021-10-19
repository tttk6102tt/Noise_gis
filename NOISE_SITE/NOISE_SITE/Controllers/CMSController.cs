using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOISE_SITE.Controllers
{
    public class CMSController : Controller
    {
        // GET: CMS
        public ActionResult Index()
        {
            if (Session["Role"] != null && Session["Role"].ToString() == "sa")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }
    }
}