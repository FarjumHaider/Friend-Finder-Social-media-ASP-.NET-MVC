using mid_project.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using mid_project.Models.Entities;
using mid_project.Auth;

namespace mid_project.Controllers
{
    [Authorize]
    [AdminAccess]
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Show()
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = db.Employees.ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Show(String name)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = db.Employees.ToList();
            data = (from u in db.Employees where u.Name.Contains(name) select u).ToList();

            if (data.Count() == 0)
            {
                ViewBag.error = "Nothing match";
            }
            if (String.IsNullOrEmpty(name))
            {
                ViewBag.error = "Please search something!!";
            }
            return View(data);
        }

        public ActionResult Verify()
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = db.Employees.ToList();
            return View(data);
        }

        
        public ActionResult ConfirmRequest(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Employees where u.Id == id select u).FirstOrDefault();
            data.Status = "Active";
            db.SaveChanges();
            return RedirectToAction("Show");
        }

        public ActionResult Active(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Employees where u.Id == id select u).FirstOrDefault();
            data.Status = "Active";
            db.SaveChanges();
            return RedirectToAction("Show");
        }

        public ActionResult Deactive(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Employees where u.Id == id select u).FirstOrDefault();
            data.Status = "Deactive";
            db.SaveChanges();
            return RedirectToAction("Show");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Employees where u.Id == id select u).FirstOrDefault();
            //TempData["imgPath"] = data.Image;
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Employee edit_employee)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Employees where u.Id == edit_employee.Id select u).FirstOrDefault();
            if (ModelState.IsValid)
            {
                //friend_finderEntities db = new friend_finderEntities();
                //var data = (from u in db.Employees where u.Id == edit_employee.Id select u).FirstOrDefault();

                if (edit_employee.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(edit_employee.ImageFile.FileName);
                    string extension = Path.GetExtension(edit_employee.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    edit_employee.Image = "~/Images/Admin/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Admin/"), fileName);
                    edit_employee.ImageFile.SaveAs(fileName);
                    data.Image = edit_employee.Image;
                }


                data.Name = edit_employee.Name;
                data.Email = edit_employee.Email;
                data.Name = edit_employee.Name;
                data.joiningDate = edit_employee.joiningDate;
                data.Status = edit_employee.Status;

                db.SaveChanges();

                return RedirectToAction("Show");
            }

            return View(data);

        }


        public ActionResult Delete(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var emp_data = (from u in db.Employees where u.Id == id select u).FirstOrDefault();
            var user_data = (from u in db.Users where u.Id == emp_data.User_id select u).FirstOrDefault();

            //Applicants
            var Applicants_data = (from ap in db.Applicants where ap.User_id == emp_data.User_id select ap).ToList();
            foreach (var apli in Applicants_data)
            {
                db.Applicants.Remove(apli);
            }


            //Job
            var Job_data = (from j in db.Jobs where j.User_id == emp_data.User_id select j).ToList();
            foreach (var job in Job_data)
            {
                //Applicants under single Job
                var App_data = (from ap in db.Applicants where ap.Job_id == job.Id select ap).ToList();
                foreach (var a in App_data)
                {
                    db.Applicants.Remove(a);
                }
                db.Jobs.Remove(job);
            }

            //React
            var react_data = (from rat in db.Reacts where rat.User_id == emp_data.User_id select rat).ToList();
            foreach (var react in react_data)
            {
                db.Reacts.Remove(react);
            }

            //Reply
            var reply_data = (from rep in db.Replies where rep.User_id == emp_data.User_id select rep).ToList();
            foreach (var reply in reply_data)
            {
                db.Replies.Remove(reply);
            }

            //comment
            var comment_data = (from c in db.Comments where c.User_id == emp_data.User_id select c).ToList();
            foreach (var comment in comment_data)
            {
                var r_data = (from r in db.Replies where r.Comment_id == comment.Id select r).ToList();
                foreach (var r in r_data)
                {
                    db.Replies.Remove(r);
                }
                db.Comments.Remove(comment);
            }

            //Post
            var Post_data = (from p in db.Posts where p.User_id == emp_data.User_id select p).ToList();

            foreach (var post in Post_data)
            {
                var comnt_data = (from c in db.Comments where c.Post_id == post.Id select c).ToList();
                foreach (var cent in comnt_data)
                {
                    var r_data = (from r in db.Replies where r.Comment_id == cent.Id select r).ToList();
                    foreach (var r in r_data)
                    {
                        db.Replies.Remove(r);
                    }
                    db.Comments.Remove(cent);
                }
                var ract_data = (from rat in db.Reacts where rat.Post_id == post.Id select rat).ToList();
                foreach (var r in ract_data)
                {
                    db.Reacts.Remove(r);
                }
                db.Posts.Remove(post);
            }


            db.Employees.Remove(emp_data);
            db.Users.Remove(user_data);
            db.SaveChanges();
            return RedirectToAction("Show");
        }

        public ActionResult VerifyDelete(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var emp_data = (from u in db.Employees where u.Id == id select u).FirstOrDefault();
            var user_data = (from u in db.Users where u.Id == emp_data.User_id select u).FirstOrDefault();

            //Applicants
            var Applicants_data = (from ap in db.Applicants where ap.User_id == emp_data.User_id select ap).ToList();
            foreach (var apli in Applicants_data)
            {
                db.Applicants.Remove(apli);
            }


            //Job
            var Job_data = (from j in db.Jobs where j.User_id == emp_data.User_id select j).ToList();
            foreach (var job in Job_data)
            {
                //Applicants under single Job
                var App_data = (from ap in db.Applicants where ap.Job_id == job.Id select ap).ToList();
                foreach (var a in App_data)
                {
                    db.Applicants.Remove(a);
                }
                db.Jobs.Remove(job);
            }

            //React
            var react_data = (from rat in db.Reacts where rat.User_id == emp_data.User_id select rat).ToList();
            foreach (var react in react_data)
            {
                db.Reacts.Remove(react);
            }

            //Reply
            var reply_data = (from rep in db.Replies where rep.User_id == emp_data.User_id select rep).ToList();
            foreach (var reply in reply_data)
            {
                db.Replies.Remove(reply);
            }

            //comment
            var comment_data = (from c in db.Comments where c.User_id == emp_data.User_id select c).ToList();
            foreach (var comment in comment_data)
            {
                var r_data = (from r in db.Replies where r.Comment_id == comment.Id select r).ToList();
                foreach (var r in r_data)
                {
                    db.Replies.Remove(r);
                }
                db.Comments.Remove(comment);
            }

            //Post
            var Post_data = (from p in db.Posts where p.User_id == emp_data.User_id select p).ToList();

            foreach (var post in Post_data)
            {
                var comnt_data = (from c in db.Comments where c.Post_id == post.Id select c).ToList();
                foreach (var cent in comnt_data)
                {
                    var r_data = (from r in db.Replies where r.Comment_id == cent.Id select r).ToList();
                    foreach (var r in r_data)
                    {
                        db.Replies.Remove(r);
                    }
                    db.Comments.Remove(cent);
                }
                var ract_data = (from rat in db.Reacts where rat.Post_id == post.Id select rat).ToList();
                foreach (var r in ract_data)
                {
                    db.Reacts.Remove(r);
                }
                db.Posts.Remove(post);
            }


            db.Employees.Remove(emp_data);
            db.Users.Remove(user_data);
            db.SaveChanges();
            return RedirectToAction("Verify","Employee");
        }

        public ActionResult Details(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from e in db.Employees where e.Id == id select e).FirstOrDefault();
            //TempData["imgPath"] = data.Image;
            return View(data);
        }

        [HttpGet]
        public ActionResult Report()
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = db.Employees.ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Report(DateTime? fromDate, DateTime? toDate)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Employees where (u.joiningDate >= fromDate && u.joiningDate <= toDate) select u).ToList();

            if (data.Count() == 0)
            {
                ViewBag.error = "Nothing match";
            }
            if (string.IsNullOrEmpty(fromDate.ToString()) || string.IsNullOrEmpty(toDate.ToString()))
            {
                var edata = db.Employees.ToList();
                ViewBag.error = "Please search something!!";
                return View(edata);
            }
            return View(data);
        }


    }
}