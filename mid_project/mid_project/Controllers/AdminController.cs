using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mid_project.Auth;
using mid_project.Models.Database;
using mid_project.Models.Entities;
using System.IO;

namespace mid_project.Controllers
{
    [Authorize]
    [AdminAccess]
    public class AdminController : Controller
    {
        // GET: Admin
        
        public ActionResult AllPost()
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = db.Posts.ToList();
            return View(data);
        }

        public ActionResult Dashboard()
        {
            friend_finderEntities db = new friend_finderEntities();
            var User_data = (from u in db.Users where u.Type == "User" select u).ToList().Count;
            var Business_data = (from u in db.Users where u.Type == "Business" select u).ToList().Count;
            var Recruiter_data = (from u in db.Users where u.Type == "Recruiter" select u).ToList().Count;
            var Job_data = db.Jobs.ToList().Count; ;
            /*var Post_data = db.Posts.ToList()*/;

            ViewBag.User = User_data;
            ViewBag.Business = Business_data;
            ViewBag.Recruiter = Recruiter_data;
            ViewBag.Job = Job_data;
            ViewBag.Order = 5;
            ViewBag.Product = 4;

            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {

            return View(new UserAdminModel());
        }

        [HttpPost]
        public ActionResult Add(UserAdminModel ua)
        {
            //image
            string fileName = Path.GetFileNameWithoutExtension(ua.ImageFile.FileName);
            string extension = Path.GetExtension(ua.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            ua.Image = "~/Images/Admin/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/Admin/"), fileName);
            ua.ImageFile.SaveAs(fileName);

/*            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            ImageFile = "~/Images/Admin/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/Admin/"), fileName);
            ImageFile.SaveAs(fileName);*/

            friend_finderEntities db = new friend_finderEntities();
            var u = new User();
            u.Password = ua.Password;
            u.Username = ua.Username;
            u.Type = ua.Type;

            db.Users.Add(u);
            db.SaveChanges();

            var a = new Admin();

            a.Name = ua.Name;
            a.Email = ua.Email;
            a.Image = ua.Image;
            a.User_id = u.Id;

            db.Admins.Add(a);
            db.SaveChanges();

            return View();
        }

        public ActionResult AllAdmin()
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = db.Admins.ToList();
            return View(data);
        }

        public ActionResult ViewProfile()
        {
            int id = Int32.Parse(Session["Id"].ToString());
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Admins where u.User_id == id select u).FirstOrDefault();
            //TempData["imgPath"] = data.Image;
            return View(data);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            int id = Int32.Parse(Session["Id"].ToString());
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Admins where u.User_id == id select u).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult EditProfile(Admin a)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from u in db.Admins where u.Id == a.Id select u).FirstOrDefault();

            if (ModelState.IsValid)
            {

                if (a.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(a.ImageFile.FileName);
                    string extension = Path.GetExtension(a.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    a.Image = "~/Images/Admin/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Admin/"), fileName);
                    a.ImageFile.SaveAs(fileName);
                    data.Image = a.Image;
                }


                data.Name = a.Name;
                data.Email = a.Email;

                db.SaveChanges();

                return RedirectToAction("ViewProfile");
            }

            return View(data);

        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel cp)
        {
            int id = Int32.Parse(Session["Id"].ToString());
            if (ModelState.IsValid)
            {
                if (Session["Password"].ToString() != cp.Password)
                {
                    ViewBag.oldPass = "Current Password Doesn't match!";
                    return View(cp);
                }
                else if (cp.New_Password != cp.Con_Password)
                {
                    ViewBag.newPass = "New password and Confirm password Doesn't match!";
                    return View(cp);
                }
                else
                {
                    friend_finderEntities db = new friend_finderEntities();
                    var data = (from pa in db.Users
                                    where pa.Id == id
                                    select pa).FirstOrDefault();
                    data.Password = cp.New_Password;
                    db.SaveChanges();
                    Session["Password"] = cp.New_Password;
                    return RedirectToAction("Logout", "Home");
                }
            }
            return View(cp);
        }


        [HttpPost]
        public ActionResult Reply(int C_Id,string reply_comment)
        {
            int id = Int32.Parse(Session["Id"].ToString());
            friend_finderEntities db = new friend_finderEntities();
            var r = new Reply();
            r.Reply_Cmnt = reply_comment;
            r.Comment_id = C_Id;
            r.User_id = id;
            r.Date = DateTime.Now;
            db.Replies.Add(r);
            db.SaveChanges();

            return RedirectToAction("AllPost");
        }

        [HttpPost]
        public ActionResult Comment(int P_Id, string new_comment)
        {
            int id = Int32.Parse(Session["Id"].ToString());
            friend_finderEntities db = new friend_finderEntities();
            var c = new Comment();
            c.Cmnt = new_comment;
            c.Post_id = P_Id;
            c.User_id = id;
            c.Date = DateTime.Now;
            db.Comments.Add(c);
            db.SaveChanges();

            return RedirectToAction("AllPost");
        }
    }
}