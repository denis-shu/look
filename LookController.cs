using Microsoft.AspNet.Identity;
using MyFirstProject.Models;
using MyFirstProject.ViewModels;
using System.Linq;
using System.Web.Mvc;
using MyFirstProject.Stabil;

namespace MyFirstProject.Controllers
{
    public class LookController : Controller
    {
        private readonly IBitOfWork _bitOfWork;

        public LookController(IBitOfWork bitOfWork)
        {
            _bitOfWork = bitOfWork;
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var mine = _bitOfWork.Projects.GetUpProjectsByMaster(userId);

            return View(mine);

        }
        public ActionResult Details(int id)
        {
            var proj = _bitOfWork.Projects.GetProject(id);

            if (proj == null)
                return HttpNotFound();

            var viewModel = new ProjectDetailsViewModel { Project = proj };
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsAttending = _bitOfWork.Visits.GetVisit(proj.Id, userId) != null;

                viewModel.IsFollowing = _bitOfWork.Followings.GetFollowing(proj.MasterId, userId) != null;
            }
            return View("Details", viewModel);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userid = User.Identity.GetUserId();

            var viewModel = new ProjectsViewModel()
            {
                UpComingProject = _bitOfWork.Projects.GetProjectsUserAttending(userid),
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Projects I Will",
                Visits = _bitOfWork.Visits.GetFutureVisits(userid).ToLookup(v => v.ProjectId)
            };
            return View("Projects", viewModel);


        }
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new LookFormViewModel()
            {
                Genres = _bitOfWork.Genres.GetGenres(),
                Heading = "Add a Look"

            };
            return View("ProjectForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userid = User.Identity.GetUserId();

            var pr = _bitOfWork.Projects.GetProject(id);
            if (pr == null)
                return HttpNotFound();
            if (pr.MasterId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();


            var viewModel = new LookFormViewModel()
            {
                Id = pr.Id,
                Heading = "Edit a look",
                Genres = _bitOfWork.Genres.GetGenres(),
                Date = pr.DateTime.ToString("d MMM yyyy"),
                Time = pr.DateTime.ToString("HH:mm"),
                Genre = pr.GenreId,
                Venue = pr.Venue
            };
            return View("ProjectForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LookFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _bitOfWork.Genres.GetGenres();
                return View("ProjectForm", viewModel);
            }

            var pr = new Project
            {
                MasterId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };


            _bitOfWork.Projects.Add(pr);
            _bitOfWork.Complete();
            //  _context.Projects.Add(pr);
            // _context.SaveChanges();

            return RedirectToAction("Mine", "Look");

        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(LookFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _bitOfWork.Genres.GetGenres();
                return View("ProjectForm", viewModel);
            }

            var pr = _bitOfWork.Projects.GetProjectWithVisitors(viewModel.Id);
            if (pr == null)
                return HttpNotFound();
            if (pr.MasterId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            pr.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);
            //pr.Venue = viewModel.Venue;
            //pr.DateTime = viewModel.GetDateTime();
            //pr.GenreId = viewModel.Genre;
            _bitOfWork.Complete();
            return RedirectToAction("Mine", "Look");
        }
    }
}