using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatBoard.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            Response.StatusCode = 404;
            return View("Error");
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //may want to set this to 200
            return View("NotFound");
        }
    }
}