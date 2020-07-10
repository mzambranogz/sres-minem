using sres.app.Controllers._Base;
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
        CriterioLN criterioLN = new CriterioLN();

        public ActionResult Index()
        {
            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Inscribirme")]
        public ActionResult Inscribirme(int idConvocatoria)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);

            if (convocatoria == null) return HttpNotFound();

            UsuarioBE usuario = ObtenerUsuarioLogin();

            InscripcionBE inscripcion = usuario == null ? null : inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, usuario.ID_INSTITUCION.Value);

            //int? idInscripcion = inscripcion == null ? null : (int?)inscripcion.ID_INSCRIPCION;

            //List<InscripcionRequerimientoBE> inscripcionRequerimiento = inscripcionRequerimientoLN.ListarInscripcionRequerimientoPorConvocatoriaInscripcion(idConvocatoria, idInscripcion);

            ViewData["convocatoria"] = convocatoria;
            ViewData["inscripcion"] = inscripcion;
            //ViewData["inscripcionRequerimiento"] = inscripcionRequerimiento;

            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Criterios")]
        public ActionResult Criterios(int idConvocatoria)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);

            if (convocatoria == null) return HttpNotFound();

            UsuarioBE usuario = ObtenerUsuarioLogin();

            InscripcionBE inscripcion = inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, usuario.ID_INSTITUCION.Value);

            if (inscripcion == null) return HttpNotFound();

            List<CriterioBE> listaCriterio = criterioLN.ListarCriterioPorConvocatoria(idConvocatoria);

            ViewData["convocatoria"] = convocatoria;
            ViewData["inscripcion"] = inscripcion;
            ViewData["listaCriterio"] = listaCriterio;

            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Criterio/{idCriterio}")]
        public ActionResult Criterio(int idConvocatoria, int idCriterio)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);

            if (convocatoria == null) return HttpNotFound();

            UsuarioBE usuario = ObtenerUsuarioLogin();

            InscripcionBE inscripcion = inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, usuario.ID_INSTITUCION.Value);

            if (inscripcion == null) return HttpNotFound();

            CriterioBE criterio = criterioLN.ObtenerCriterioPorConvocatoria(idConvocatoria, idCriterio);

            if (criterio == null) return HttpNotFound();

            ViewData["convocatoria"] = convocatoria;
            ViewData["inscripcion"] = inscripcion;
            ViewData["criterio"] = criterio;

            return View();
        }

        public ActionResult CriterioIngresar()
        {
            return View();
        }
        
    }
}