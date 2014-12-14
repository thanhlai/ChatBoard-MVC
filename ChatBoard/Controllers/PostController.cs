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
/*
 * @author: Team Virus
 * @project: Chat Board
 * @setID: 4D
 * @courseID: COMP 4952
 * @instructors: Mirela Gutica & Medhat Elmasry 
 * **/
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
        /// <summary>
        /// Show all the available posts, built-in custom search - 
        /// when the user enter the search string on the Search Box, this action controller
        /// will return the appropriate posts associate with the search string provided.
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult Index(string searchString)
        {
            var postsByTopic = from post in db.Posts
                        select post;
            var postsByUserName = from post in db.Posts
                        select post;
            var postsByTag = from post in db.Posts
                        select post;
            if (!string.IsNullOrEmpty(searchString))
            {
                postsByTopic = postsByTopic.Where(s => s.Title.Contains(searchString));   // search by topic
                if (postsByTopic.Count() == 0) // no posts found by topic, current state, postsByTopic is null, not the same as the value that has been assigned above
                {
                    // search by owner username
                    postsByUserName = postsByUserName.Where(s => s.Owner.Contains(searchString));

                    //search by tag
                    if (postsByUserName.Count() == 0)
                    {
                        postsByTag = postsByTag.Where(s => s.Tag.Contains(searchString));
                        return View(postsByTag);
                    }
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
        /// <summary>
        /// Return Create Post view if the user is authorized.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Receive Post attributes and save it into the db (Post table)
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
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
            post.Views = 0;
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit, same as Details
        /// <summary>
        /// Edit Post View is redirected if the logged in user is the post's owner
        /// else, redirect to the post's detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null || db == null)
            {
                return HttpNotFound();
            }

            string currentUserName = User.Identity.GetUserName();

            //CHECK FOR POST'S OWNER
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
        /// <summary>
        /// Get Edit post attributes and update it in the db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                post.DateCreated = DateTime.Now;
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
        /// <summary>
        /// If the user is authorized and is post's owner, redirect to Delete view
        /// else, redirect to the post's details view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Remove the post from Post table
        /// and redirect users to Index action
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Show post details, along with owner avatar,
        /// increment views and store in the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

            // Increment number of hits (views)
            Post p =    (from post in db.Posts
                        where post.Id == id.Value
                        select post).Single();
            p.Views++;
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return View(db.Posts.Find(id));
        }

        /// <summary>
        /// Show owner avatar helper
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Show login user avatar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowUserAvatar(string id)
        {
            if (id.Equals("Anonymous"))
                return File("/Content/Images/default_avatar2.png", "image/jpg");    // anonymous

            var userAvatar = from user in db.Users
                             where user.Id == id
                             select user.Avatar;
            byte[] avatar = userAvatar.FirstOrDefault();
            if (avatar != null)
                return File(avatar, "image/jpg");
            else
                return File("/Content/Images/default_avatar1.png", "image/jpg");    // default avatar
        }

        /// <summary>
        /// Increment love post number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Love(int? id)
        {
            Post p = (from post in db.Posts
                      where post.Id == id.Value
                      select post).Single();
            p.Loves++;
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Post", new { id = id.Value });
        }
    }
}