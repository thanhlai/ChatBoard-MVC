using ChatBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Drawing;
using System.IO;

namespace ChatBoard.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Post - Show all posts
        //public ActionResult Index()
        //{
        //    return View(db.Posts.ToList());
        //}

        // GET: Post - Search for posts
        public ActionResult Index(string searchString)
        {
            var postsByTopic = from post in db.Posts
                        select post;
            var postsByUserName = from post in db.Posts
                        select post;
            if (!string.IsNullOrEmpty(searchString))
            {
                postsByTopic = postsByTopic.Where(s => s.Title.Contains(searchString));   // search by topic
                if (postsByTopic.Count() == 0) // no posts found by topic, current state, postsByTopic is null, not the same as the value that has been assigned above
                {
                    // search by owner username
                    postsByUserName = postsByUserName.Where(s => s.Owner.Contains(searchString));
                    return View(postsByUserName);
                }
            }
            else
            {
                return View(db.Posts.ToList());
            }
            return View(postsByTopic);
        }

        // GET: Create
        public ActionResult Create()
        {
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated) && (System.Web.HttpContext.Current.User != null))
            {
                return View();
            }   
            else
                return RedirectToAction("Index");
        }
        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // Remember to validate these info first
            post.UserId = User.Identity.GetUserId();
            post.Owner = User.Identity.GetUserName();
            post.DateCreated = DateTime.Now;
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit, same as Details
        public ActionResult Edit(int? id)
        {
            if (id == null || db == null)
            {
                return HttpNotFound();
            }

            string currentUserName = User.Identity.GetUserName();

            //CHECK FOR POST'S OWNER HERE
            var owner = from post in db.Posts
                        where post.Id == id.Value
                        select post.Owner;

           if (!(User.Identity.GetUserName() == owner.FirstOrDefault()))  // the logged in user is not logged in
           {
               return RedirectToAction("Details", "Post", new { id = id.Value});
           }

            return View(db.Posts.Find(id));
        }

        // POST: Edit
        [HttpPost]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                post.UserId = User.Identity.GetUserId();
                post.Owner = User.Identity.GetUserName();
                db.Entry(post).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Delete
        public ActionResult Delete(int? id)
        {
            if (id == null || db == null)
            {
                return HttpNotFound();
            }

            string currentUserName = User.Identity.GetUserName();

            //CHECK FOR POST'S OWNER HERE
            var owner = from post in db.Posts
                        where post.Id == id.Value
                        select post.Owner;

            if (!(User.Identity.GetUserName() == owner.FirstOrDefault()))  // the logged in user is not logged in
            {
                return RedirectToAction("Details", "Post", new { id = id.Value });
            }

            return View(db.Posts.Find(id));
        }

        // POST: Delete
        [HttpPost]
        public ActionResult Delete(int id, Post post)
        {
            //Will fix to allow users directly remove the post
            try
            {
                db.Entry(post).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Details
        public ActionResult Details(int? id)
        {
            if (id == null || db == null)
            {
                return HttpNotFound();
            }
            var owner = from post in db.Posts
                        where post.Id == id.Value
                        select post.Owner;
            string ownerUserName = "";
            if (owner != null)
                ownerUserName = owner.FirstOrDefault();
            var ownerAvatar = from user in db.Users
                              where user.UserName == ownerUserName
                              select user.Avatar;

            byte[] avatar = ownerAvatar.FirstOrDefault();

            // if the user logged in
            ViewBag.IsUserLoggedIn = (System.Web.HttpContext.Current.User.Identity.IsAuthenticated) && (System.Web.HttpContext.Current.User != null);
            // if the user is the owner of the post
            ViewBag.IsOwner = (User.Identity.GetUserName() == ownerUserName);
            
            return View(db.Posts.Find(id));
        }

        public ActionResult ShowOwnerAvatar(int? id)
        {
            var owner = from post in db.Posts
                        where post.Id == id.Value
                        select post.Owner;
            string ownerUserName = "";
            if (owner != null)
                ownerUserName = owner.FirstOrDefault();
            var ownerAvatar = from user in db.Users
                              where user.UserName == ownerUserName
                              select user.Avatar;

            byte[] avatar = ownerAvatar.FirstOrDefault();
            if (avatar != null)
                return File(avatar, "image/jpg");
            else
                return File("/Content/Images/default_avatar1.png", "image/jpg");
        }

        public ActionResult ShowUserAvatar(string id)
        {
            var userAvatar = from user in db.Users
                             where user.Id == id
                             select user.Avatar;
            byte[] avatar = userAvatar.FirstOrDefault();
            if (avatar != null)
                return File(avatar, "image/jpg");
            else
                return File("/Content/Images/default_avatar2.png", "image/jpg");    // anonymous
        }
    }
}