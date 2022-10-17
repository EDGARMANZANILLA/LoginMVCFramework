using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginMVCFramework.Controllers
{
    public class ErroresController : Controller
    {
        // GET: Errores
        public ActionResult ErrorEnVistaSolicitada()
        {
            return View();
        }
    }
}