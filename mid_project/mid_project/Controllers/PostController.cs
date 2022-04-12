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
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AllPost()
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = db.Posts.ToList();
            return View(data);
        }



        [HttpPost]
        public ActionResult ReplyAdd(int C_Id, string reply_comment)
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
        public ActionResult CommentAdd(int P_Id, string new_comment)
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

        [HttpPost]
        public ActionResult LikeReactAdd(int P_Id,int? R_Id)
        {
            int id = Int32.Parse(Session["Id"].ToString());
            friend_finderEntities db = new friend_finderEntities();
            if(R_Id != null)
            {
                var data = (from r in db.Reacts where r.Id == R_Id select r).FirstOrDefault();
                data.User_React = "Like";
                db.SaveChanges();
            }
            else
            {
                var r = new React();
                r.Post_id = P_Id;
                r.User_id = id;
                r.User_React = "Like";
                db.Reacts.Add(r);
                db.SaveChanges();
            }


            return RedirectToAction("AllPost");
        }
        

        [HttpPost]
        public ActionResult DisLikeReactAdd(int P_Id, int? R_Id)
        {
            int id = Int32.Parse(Session["Id"].ToString());
            friend_finderEntities db = new friend_finderEntities();
            if (R_Id != null)
            {
                var data = (from r in db.Reacts where r.Id == R_Id select r).FirstOrDefault();
                data.User_React = "Dislike";
                db.SaveChanges();
            }
            else
            {
                var r = new React();
                r.Post_id = P_Id;
                r.User_id = id;
                r.User_React = "Dislike";
                db.Reacts.Add(r);
                db.SaveChanges();
            }


            return RedirectToAction("AllPost");
        }


        
        [HttpPost]
        public ActionResult ReactAlreadyAdd(int R_Id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from r in db.Reacts where r.Id == R_Id select r).FirstOrDefault();

            db.Reacts.Remove(data);
            db.SaveChanges();
            return RedirectToAction("AllPost");

            
        }

        public ActionResult ReplyDelete(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var data = (from r in db.Replies where r.Id == id select r).FirstOrDefault();
            db.Replies.Remove(data);
            db.SaveChanges();
            return RedirectToAction("AllPost");
        }

        
        public ActionResult CommentDelete(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var r_data = (from r in db.Replies where r.Comment_id == id select r).ToList();
            var c_data = (from c in db.Comments where c.Id == id select c).FirstOrDefault();
            foreach(var r in r_data)
            {
                db.Replies.Remove(r);
            }
            /*db.Replies.Remove(r_data);*/
            db.Comments.Remove(c_data);
            db.SaveChanges();
            return RedirectToAction("AllPost");
        }


        public ActionResult Delete(int id)
        {
            friend_finderEntities db = new friend_finderEntities();
            var p_data = (from p in db.Posts where p.Id == id select p).FirstOrDefault();
            var c_data = (from c in db.Comments where c.Post_id == id select c).ToList();
            var react_data = (from rt in db.Reacts where rt.Post_id == id select rt).ToList();
            foreach (var c in c_data)
            {
                var r_data = (from r in db.Replies where r.Comment_id == c.Id select r).ToList();
                foreach (var r in r_data)
                {
                    db.Replies.Remove(r);
                }
                db.Comments.Remove(c);
            }
            foreach (var rt in react_data)
            {
                db.Reacts.Remove(rt);
            }
            db.Posts.Remove(p_data);
            db.SaveChanges();
            return RedirectToAction("AllPost");
        }


        [HttpPost]
        public ActionResult Add(Post p)
        {
            //image
            if (p.ImageFile == null && p.User_Post == null)
            {
                TempData["error"] = "Please enter any text or image";
                return RedirectToAction("AllPost");
            }
                if (p.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(p.ImageFile.FileName);
                string extension = Path.GetExtension(p.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                p.Image = "~/Images/Post/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Post/"), fileName);
                p.ImageFile.SaveAs(fileName);
            }


            friend_finderEntities db = new friend_finderEntities();
            int id = Int32.Parse(Session["Id"].ToString());

            var post = new Post();
            post.User_Post = p.User_Post;
            post.Image = p.Image;
            post.Date = DateTime.Now;
            //post.Privacy = "Everyone";
            post.User_id = id;
            db.Posts.Add(post);
            db.SaveChanges();

            return RedirectToAction("AllPost");
        }


    }

}