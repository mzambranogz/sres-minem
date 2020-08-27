using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sres.app.Controllers
{
    [RoutePrefix("Participantes")]
    public class ParticipanteController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Categoria")]
        public ActionResult Categoria()
        {
            return View();
        }

        [Route("Preguntas-frecuentes")]
        public ActionResult PreguntasFrecuentes()
        {
            return View();
        }

        [Route("Mapa-del-sitio")]
        public ActionResult MapaSitio()
        {
            return View();
        }

        [Route("Terminos-y-condiciones")]
        public ActionResult TerminosCondiciones()
        {
            return View();
        }
    }
}