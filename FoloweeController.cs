﻿using Microsoft.AspNet.Identity;
using Project.Models;
using Project.Stabil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class FoloweeController : Controller
    {
        private readonly IBitOfWork _bitOfWork;
        public FoloweeController(IBitOfWork bitofWork)
        {
            _bitOfWork = bitofWork;
        }



        // GET: Folowee
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var master = _bitOfWork.Followings.GetAppLUser(userId);
            return View(master);
        }
    }
}
