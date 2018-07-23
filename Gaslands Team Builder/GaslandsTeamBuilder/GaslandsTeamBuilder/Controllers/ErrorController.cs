using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GaslandsTeamBuilder.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ViewResult Index()
        {
            Response.StatusCode = 500;
            return View("Error");
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}