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
    [RoutePrefix("Mantenimiento")]
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

        [Route("Etapa")]
        public ActionResult Proceso()
        {
            return View();
        }

        [Route("Actividad")]
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

        public ActionResult Parametro()
        {
            return View();
        }

        public ActionResult Factor()
        {
            return View();
        }

        public ActionResult Composicion()
        {
            return View();
        }

        public ActionResult FactorValor()
        {
            return View();
        }

        public ActionResult Estrella()
        {
            return View();
        }

        public ActionResult Reconocimiento()
        {
            return View();
        }

        public ActionResult MedidaMitigacion()
        {
            return View();
        }

        public ActionResult Puntaje()
        {
            return View();
        }

        public ActionResult Institucion()
        {
            return View();
        }
    }
}