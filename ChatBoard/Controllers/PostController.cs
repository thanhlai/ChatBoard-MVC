using ChatBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ChatBoard.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Post - Show all posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }
        // GET: Create
        public ActionResult Create()
        {
            return View();
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
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit, same as Details
        public ActionResult Edit(int id)
        {
            if (db == null)
            {
                return HttpNotFound();
            }
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
        public ActionResult Delete(int id)
        {
            if (db == null)
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
        public ActionResult Details(int id)
        {
            if (db == null)
            {
                return HttpNotFound();
            }
            return View(db.Posts.Find(id));
        }
    }
}