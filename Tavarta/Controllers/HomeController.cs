﻿using System.Web.Mvc;

namespace Tavarta.Controllers
{
    [Authorize]
    public class HomeController:Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pop()
        {
            return View();
        }

        public ActionResult Single()
        {
            return View();
        }
        
         
    }
}