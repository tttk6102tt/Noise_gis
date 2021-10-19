using NOISE_SITE.Enums;
using NOISE_SITE.Models;
using NOISE_SITE.Models.DTO;
using NOISE_SITE.Repository.Sessions;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;

namespace NOISE_SITE.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private readonly NOISE_SITE.Repository.INguoiDungRepository _nguoiDungRepository;
        public UserController(
            NOISE_SITE.Repository.NguoiDungRepository nguoiDungRepository)
        {
            _nguoiDungRepository = nguoiDungRepository;
        }
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
            return Json(new RestData
            {
                data = _nguoiDungRepository.FindAll().ToList()
            }, JsonRequestBehavior.AllowGet); ;
        }
        [HttpPost]
        public ActionResult Delete(NGUOIDUNG user)
        {
            try
            {
                if (user == null)
                {
                    return Json(new RestError(), JsonRequestBehavior.AllowGet);
                }

                var userCheck = _nguoiDungRepository.FindAll().Where(s => s.UserName == user.UserName).FirstOrDefault();

                if (userCheck != null)
                {
                    _nguoiDungRepository.Delete(userCheck);
                    return Json(new RestBase(EnumError.OK), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new RestError(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new RestError()
                {
                    errors = new RestErrorDetail[]{
                       new RestErrorDetail()
                    {
                        message = ex.Message
                    }
                    }
                });
            }

        }
    
        [HttpPost]
        public ActionResult SetLock(NGUOIDUNG user, bool locked)
        {
            if (user == null)
            {
                return Json(new RestError());
            }
            var userCheck = _nguoiDungRepository.Find(s => s.ID == user.ID);

            userCheck.LockoutEnabled = locked == true ? 0 : 1;

            _nguoiDungRepository.Update(userCheck);

            return Json(new RestBase(EnumError.OK), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(NGUOIDUNG user)
        {
            var selectedUser = _nguoiDungRepository.Find(s => s.ID == user.ID);
            user.Password = selectedUser.Password;
            user.LockoutEnabled = selectedUser.LockoutEnabled;
            _nguoiDungRepository.Update(user);
            return Json(new RestBase(EnumError.OK), JsonRequestBehavior.AllowGet);
        }
    }
}