using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sres.app.Controllers
{
    [RoutePrefix("Convocatoria")]
    public class ConvocatoriaController : Controller
    {
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();

        public ActionResult Index()
        {
            return View();
        }

        [Route("Inscribirme/{idConvocatoria}")]
        public ActionResult Inscribirme(int idConvocatoria)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);

            if (convocatoria == null) return HttpNotFound();

            ViewData["convocatoria"] = convocatoria;

            return View();
        }
    }
}