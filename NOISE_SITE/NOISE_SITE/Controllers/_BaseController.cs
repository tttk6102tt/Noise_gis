using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NOISE_SITE.Controllers
{
    public class _BaseController : Controller
    {
        // GET: _Base
        protected readonly IDbFactory _dbFactory;

        public _BaseController(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
    }
}