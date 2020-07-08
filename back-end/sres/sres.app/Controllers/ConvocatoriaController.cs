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
    public class ConvocatoriaController : BaseController
    {
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();
        InscripcionLN inscripcionLN = new InscripcionLN();
        InscripcionRequerimientoLN inscripcionRequerimientoLN = new InscripcionRequerimientoLN();

        public ActionResult Index()
        {
            return View();
        }

        [Route("Inscribirme/{idConvocatoria}")]
        public ActionResult Inscribirme(int idConvocatoria)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);

            if (convocatoria == null) return HttpNotFound();

            UsuarioBE usuario = ObtenerUsuarioLogin();

            InscripcionBE inscripcion = usuario == null ? null : inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, usuario.ID_INSTITUCION.Value);

            int? idInscripcion = inscripcion == null ? null : (int?)inscripcion.ID_INSCRIPCION;

            //List<InscripcionRequerimientoBE> inscripcionRequerimiento = inscripcionRequerimientoLN.ListarInscripcionRequerimientoPorConvocatoriaInscripcion(idConvocatoria, idInscripcion);

            ViewData["convocatoria"] = convocatoria;
            ViewData["inscripcion"] = inscripcion;
            //ViewData["inscripcionRequerimiento"] = inscripcionRequerimiento;

            return View();
        }
    }
}