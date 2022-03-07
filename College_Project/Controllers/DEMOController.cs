using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College_Project.Controllers
{
    public class DEMOController : Controller
    {
        // GET: DEMO
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create( int a)
        {
            return View();
        }
    }
}