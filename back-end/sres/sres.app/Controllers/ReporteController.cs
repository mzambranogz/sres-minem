using sres.app.Controllers._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sres.app.Controllers
{
    public class ReporteController : Controller
    {
        [SesionOut]
        // GET: Reporte
        public ActionResult Index()
        {
            return View();
        }
    }
}