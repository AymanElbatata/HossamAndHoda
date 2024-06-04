using System;
using System.Web;
using System.Web.Mvc;

namespace HossamAndHoda2021.Controllers
{
    public class BaseController : Controller
    {
        //var ctx = filterContext.HttpContext;

        int currenuser = Convert.ToInt32(CurrentUserLoginData.getcurrentusrdataAuth(1));


        protected override void OnActionExecuting(ActionExecutingContext filterContext)

        {
            if (currenuser == 0)
            {
                // return   RedirectToAction("Login", "HomeAccount",new {area="" });
                filterContext.Result = new RedirectResult("~/Home/InvalidNotAllowed");
                return;
            }
        }


    }
}