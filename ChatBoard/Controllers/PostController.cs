using ChatBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
            //CHECK FOR POST OWNER HERE
            return View(db.Posts.Find(id));
        }

        // POST: Edit
        [HttpPost]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
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
            return View(db.Posts.Find(id));
        }
    }
}