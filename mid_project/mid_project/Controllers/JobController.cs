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
    public class JobController : Controller
    {
        // GET: Job

        [HttpGet]
        public ActionResult Show()
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = db.Jobs.ToList();
            return View(data);
        }

        public ActionResult Show(String type)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Jobs where u.Type.Contains(type) select u).ToList();

            if (data.Count() == 0)
            {
                ViewBag.error = "Nothing match";
            }
            if (String.IsNullOrEmpty(type))
            {
                ViewBag.error = "Please search something!!";
            }
            return View(data);
        }

        public ActionResult Delete(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var j_data = (from j in db.Jobs where j.Id == id select j).FirstOrDefault();
            var a_data = (from a in db.Applicants where a.Job_id == id select a).ToList();
            foreach (var a in a_data)
            {
                db.Applicants.Remove(a);
            }
            db.Jobs.Remove(j_data);
            db.SaveChanges();
            return RedirectToAction("Show");
        }

        public ActionResult Details(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from j in db.Jobs where j.Id == id select j).FirstOrDefault();
            return View(data);
        }
    }
}