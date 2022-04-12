using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mid_project.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}