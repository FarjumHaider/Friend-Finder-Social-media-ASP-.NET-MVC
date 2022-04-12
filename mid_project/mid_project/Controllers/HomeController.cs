using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using mid_project.Models.Database;

namespace mid_project.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Session["type"].ToString().Equals("Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                
                else if (Session["type"].ToString().Equals("User"))
                {
                    return RedirectToAction("Dashboard", "User"); 
                }

                else if (Session["type"].ToString().Equals("Business"))
                {
                    return RedirectToAction("Dashboard", "Business");
                }

                else if (Session["type"].ToString().Equals("Recruiter"))
                {
                    return RedirectToAction("Dashboard", "Recruiter");
                }

            }
            return View(new User());
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if(ModelState.IsValid)
            {
                friend_finderEntities db = new friend_finderEntities();
                var data = (from e in db.Users
                            where e.Password.Equals(user.Password) &&
                            e.Username.Equals(user.Username)
                            select e).FirstOrDefault();
                if (data != null)
                {
                    FormsAuthentication.SetAuthCookie(data.Username, true);
                    Session["type"] = data.Type;
                    Session["Id"] = data.Id;
                    Session["UserName"] = data.Username;
                    Session["Password"] = data.Password;
                    /*                Session["UserName"] = data.Username;
                                    Session["UserType"] = data.Type;*/
                    if (data.Type.Equals("Admin"))
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }

                    else if (data.Type.Equals("User"))
                    {
                        return RedirectToAction("Dashboard", "User");
                    }

                    else if (data.Type.Equals("Business"))
                    {
                        return RedirectToAction("Dashboard", "Business");
                    }

                    else if (data.Type.Equals("Recruiter"))
                    {
                        return RedirectToAction("Dashboard", "Recruiter");
                    }

                }
                TempData["msg"] = "Invalid Username and Password";
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}