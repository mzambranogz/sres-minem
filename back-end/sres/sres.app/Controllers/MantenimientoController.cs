using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sres.be;
using sres.ln;
//using sres.app.Models;
using sres.app.Controllers._Base;

namespace sres.app.Controllers
{
    [SesionOut]
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult TablaMantenimiento()
        {
            return View();
        }
        public ActionResult Criterio()
        {
            return View();
        }

        public ActionResult Usuario()
        {
            return View();
        }

        public ActionResult Requerimiento()
        {
            return View();
        }

        public ActionResult Proceso()
        {
            return View();
        }

        public ActionResult Etapa()
        {
            return View();
        }

        public ActionResult Convocatoria()
        {
            return View();
        }
    }
}