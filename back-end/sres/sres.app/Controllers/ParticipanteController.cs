using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sres.ln;
using sres.be;

namespace sres.app.Controllers
{
    [RoutePrefix("Participantes")]
    public class ParticipanteController : Controller
    {
        InstitucionLN institucionLN = new InstitucionLN();

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

        [Route("{idInstitucion}/Reconocimiento")]
        public ActionResult Reconocimiento(int idInstitucion)
        {
            InstitucionBE institucion = institucionLN.ObtenerInstitucion(idInstitucion);
            ViewData["institucion"] = institucion;
            return View();
        }
    }
}