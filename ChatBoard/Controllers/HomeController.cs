﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatBoard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Anonymous Chat Board
        /// </summary>
        /// <returns></returns>
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult TestView()
        {
            return View();
        }
    }
}