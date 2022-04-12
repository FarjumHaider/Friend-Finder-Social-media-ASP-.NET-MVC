using mid_project.Auth;
using mid_project.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mid_project.Controllers
{
    [Authorize]
    [AdminAccess]
    public class ApplicantsController : Controller
    {
        // GET: Applicants
        public ActionResult List(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
   
            var data = (from a in db.Applicants where a.Job_id == id select a).ToList();

            return View(data);
        }

        public ActionResult Delete(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Applicants where u.User_id == id select u).FirstOrDefault();;
            db.Applicants.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Show", "Job");
        }

        public ActionResult Details(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Employees where u.Id == id select u).FirstOrDefault(); ;
            return View(data);
        }


    }
}