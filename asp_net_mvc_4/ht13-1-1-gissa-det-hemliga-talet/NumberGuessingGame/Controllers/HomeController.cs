using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NumberGuessingGame.ViewModels;

namespace NumberGuessingGame.Controllers
{
    public class HomeController : Controller
    {

        public HomeIndexViewModel ViewModel { get { return new HomeIndexViewModel(); } }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            if (!ViewModel.SecretNumber.CanMakeGuess)
            {
                ViewModel.Restart();
            }
            
            return View("Index", ViewModel);
        }

        //
        // POST: /Home/
        [HttpPost]
        public ActionResult Index(HomeIndexViewModel viewModel)
        {
            if (Session.IsNewSession)
            {
                return View("GamOver");
            }

            if (ModelState.IsValid)
            {                              
                try
                {
                    viewModel.SecretNumber.MakeGuess(viewModel.Guess);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            return View("Index", viewModel);
        }
    }
}
