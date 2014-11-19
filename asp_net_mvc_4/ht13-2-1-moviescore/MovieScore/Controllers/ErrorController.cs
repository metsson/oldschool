using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// @author http://goo.gl/lqKBSx 
/// Note: Actually not in use as upon ajax requests (using sammy.js)
/// the responseText will be flushed and set to "Bad Request".
/// That means that the SPA uses internal javascript based error handling, instead.
/// </summary>
public class ErrorController : Controller
{

    public Dictionary<string, string> Errors { get { return new Dictionary<string,string>(); } }

    public ActionResult AccessDenied()
    {
        Response.StatusCode = (int)HttpStatusCode.Forbidden;
        Response.TrySkipIisCustomErrors = true;

        if (Request.IsAjaxRequest())
        {
            Errors.Add("Keyword", "Sorry, pal - members only!");
            Errors.ToArray();

            return Json(new { Errors = Errors }, JsonRequestBehavior.AllowGet);
        }

        return PartialView();
    }

    public ActionResult NotFound()
    {
        Response.StatusCode = (int)HttpStatusCode.NotFound;
        Response.TrySkipIisCustomErrors = true;

        /*
        if (Request.IsAjaxRequest())
        {

            Errors.Add("Keyword", "Ooopss! The requested page could not be found.");
            Errors.ToArray();

            return Json(new { Errors = Errors }, JsonRequestBehavior.AllowGet);
        }
        */
        return PartialView();
    }

    public ActionResult ApplicationError()
    {
        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        Response.TrySkipIisCustomErrors = true;

        if (Request.IsAjaxRequest())
        {
            Errors.Add("Keyword", "Bugger! Something went wrong. But hey, it doesn't seem to be your fault!");
            Errors.ToArray();

            return Json(new { Errors = Errors }, JsonRequestBehavior.AllowGet);
        }

        return PartialView();
    }
}
