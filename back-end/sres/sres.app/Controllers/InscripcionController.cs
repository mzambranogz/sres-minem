using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sres.app.Controllers
{
    [RoutePrefix("Inscripcion")]
    public class InscripcionController : BaseController
    {
        [Route("{idConvocatoria}/{idInscripcion}")]
        public ActionResult Index(int idConvocatoria, int idInscripcion)
        {
            return View();
        }
    }
}