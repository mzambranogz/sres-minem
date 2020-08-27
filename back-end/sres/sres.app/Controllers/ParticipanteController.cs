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

        [Route("PreguntasFrecuentes")]
        public ActionResult PreguntasFrecuentes()
        {
            return View();
        }
    }
}