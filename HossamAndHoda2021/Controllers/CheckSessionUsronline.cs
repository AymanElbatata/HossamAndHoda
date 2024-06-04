using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HossamAndHoda2021.Controllers
{
    public class CheckSessionUsronline : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int currenuser = Convert.ToInt32(CurrentUserLoginData.getcurrentusrdataAuth(1));

            //if Session == null => Login page
            if (currenuser != 0)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "UserProfile", controller = "PRVEMP", area = "" }));

            }
            base.OnActionExecuting(filterContext);
        }
    }

}