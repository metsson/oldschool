using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieScore.Models;
using MovieScore.Models.Repositories;
using MovieScore.Models.ViewModels;

namespace MovieScore.Controllers
{
    public class HomeController : Controller
    {
        #region Properties and fields

        private IRepository _repository;
        private MovieCheck _movieCheck;

        #endregion

        #region Constructors

        public HomeController()
            : this(new EFRepository())
        {
            // Chained        
        }

        public HomeController(IRepository repository)
        {
            _repository = repository;
            _movieCheck = new MovieCheck(repository);
        }

        #endregion

        #region Controller methods

        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SearchViewModel Search)
        {
            Response.TrySkipIisCustomErrors = true;

            // Ajax request with errors added to model state
            // Based on solution by Mats Loock http://goo.gl/nUEdwU
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Movie movie = _movieCheck.GetMovie(Search.Keyword);
                        return PartialView("_MovieScore", movie);
                    }
                    catch (ApplicationException)
                    {
                        ModelState.AddModelError("Keyword", "No movie found by that title!");
                    }
                }

                Response.StatusCode = 400;

                // Transform the modelstate errors to a dictionary where property names is 
                // associated with there errors.
                var errors = ModelState
                    .Where(kvp => kvp.Value.Errors.Any())
                    .ToDictionary(kvp => kvp.Key,
                                  kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return Json(new { Errors = errors }, JsonRequestBehavior.AllowGet);
            }

            return View();
        }

        public ActionResult InstantSearch(string keyword)
        {
            return Json(_repository.InstantSearch(keyword), JsonRequestBehavior.AllowGet);
        }
   
        #endregion

        #region SPA partial views

        // Serves as default page for Router.js
        public ActionResult SPAStart()
        {
            return PartialView("_SPAMainPage");
        }

        // Home/Top100
        public ActionResult Top100()
        {
            var movies = _repository.GetTop100();
            return PartialView("_Top100", movies);
        }

        // Home/About
        public ActionResult About()
        {
            return PartialView("_About");
        }        

        // Home/Movie/MovieID
        public ActionResult Movie(int id) 
        {
            var movie = _repository.GetMovieByID(id);

            if (movie != null)
            {
                return PartialView("_MovieDetails", movie);
            }

            return PartialView("_MovieNotFound");
        }

        public ActionResult NewSearch(string keyword)
        {
            var webservice = new Webservices.TheMdb();

            var apiData = webservice.FindMovieByTitle(keyword);

            var hits = apiData.Where(data => data.Title.Any())
                                .ToDictionary(data => data.Title, data => data.ReleaseDate.Take(4))
                                .ToArray();

            return Json(new { Hits = hits }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}
