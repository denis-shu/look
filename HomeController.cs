using Project.Models;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using System;
using Microsoft.AspNet.Identity;
using Project.ViewModels;
using Project.Repos;
using Project.Stabil;

namespace MyFirstProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly BitOfWork _bitOfWork;

        public HomeController(BitOfWork bitOfWork)
        {
            _bitOfWork = bitOfWork;
        }

        public ActionResult Index()
        {
            var upcoming = _bitOfWork.Projects.UpcomingR();

            var userId = User.Identity.GetUserId();


            var visit = _bitOfWork.Visits.GetFutureVisits(userId).ToLookup(v => v.ProjectId);
                //_visitRepo.GetFutureVisits(userId).ToLookup(v => v.ProjectId);

            var viewModel = new ProjectsViewModel
            {
                UpComingProject = upcoming,
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Future Projects",
                Visits = visit
            };           


            return View("Projects", viewModel);
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
