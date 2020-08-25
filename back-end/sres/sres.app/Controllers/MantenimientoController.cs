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

        public ActionResult Anno()
        {
            return View();
        }

        public ActionResult Rol()
        {
            return View();
        }

        public ActionResult Sector()
        {
            return View();
        }

        public ActionResult Caso()
        {
            return View();
        }

        public ActionResult Componente()
        {
            return View();
        }

        public ActionResult Documento()
        {
            return View();
        }

        public ActionResult Insignia()
        {
            return View();
        }
    }
}