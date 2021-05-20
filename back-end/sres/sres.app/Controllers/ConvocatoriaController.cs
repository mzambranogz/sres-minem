using sres.app.Controllers._Base;
using sres.app.Models;
using sres.be;
using sres.ln;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sres.app.Repositorio;

namespace sres.app.Controllers
{
    [RoutePrefix("Convocatoria")]
    public class ConvocatoriaController : BaseController
    {
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();
        InscripcionLN inscripcionLN = new InscripcionLN();
        InscripcionRequerimientoLN inscripcionRequerimientoLN = new InscripcionRequerimientoLN();
        CriterioLN criterioLN = new CriterioLN();
        CasoLN casoLN = new CasoLN();
        ConvocatoriaCriterioPuntajeLN convcripuntajeLN = new ConvocatoriaCriterioPuntajeLN();
        ConvocatoriaEtapaInscripcionLN convetainscLN = new ConvocatoriaEtapaInscripcionLN();
        ReconocimientoLN reconocimientoLN = new ReconocimientoLN();
        MigrarEmisionesLN migrarLN = new MigrarEmisionesLN();
        PuntajeLN puntajeLN = new PuntajeLN();

        [Route("{idConvocatoria}/Informacion")]
        public ActionResult Informacion(int idConvocatoria)
        {
            ConvocatoriaBE convocatoria = idConvocatoria == 0 ? convocatoriaLN.ObtenerUltimaConvocatoria() : convocatoriaLN.ObtenerConvocatoria(idConvocatoria);
            ViewData["convocatoria"] = convocatoria;
            return View();
        }

        [SesionOut]
        public ActionResult Index()
        {
            UsuarioBE usuario = ObtenerUsuarioLogin();

            if (usuario.ID_ROL != (int)EnumsCustom.Roles.POSTULANTE)
            {
                ViewData["convocatoria"] = convocatoriaLN.ObtenerUltimaConvocatoria();
            }
            else if(usuario.ID_ROL == (int)EnumsCustom.Roles.POSTULANTE)
            {
                int cantidadUltimosReconocimientos = AppSettings.Get<int>("Bandeja.Postulante.CantidadUltimosReconocimientos");
                ViewData["ultimos_reconocimientos"] = reconocimientoLN.ListarUltimosReconocimientos(usuario.ID_INSTITUCION.Value, cantidadUltimosReconocimientos).ToList();
            }

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
            ConvocatoriaEtapaInscripcionBE convetainsc = convetainscLN.ObtenerConvocatoriaEtapaInscripcion(new ConvocatoriaEtapaInscripcionBE { ID_CONVOCATORIA = convocatoria.ID_CONVOCATORIA, ID_ETAPA = convocatoria.ID_ETAPA, ID_INSCRIPCION = inscripcion == null ? 0 : inscripcion.ID_INSCRIPCION });
            //int? idInscripcion = inscripcion == null ? null : (int?)inscripcion.ID_INSCRIPCION;

            //List<InscripcionRequerimientoBE> inscripcionRequerimiento = inscripcionRequerimientoLN.ListarInscripcionRequerimientoPorConvocatoriaInscripcion(idConvocatoria, idInscripcion);

            ViewData["convocatoria"] = convocatoria;
            ViewData["inscripcion"] = inscripcion;
            ViewData["convetainsc"] = convetainsc;
            //ViewData["inscripcionRequerimiento"] = inscripcionRequerimiento;

            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Criterios")]
        public ActionResult Criterios(int idConvocatoria)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);
            //if (convocatoria == null) return HttpNotFound();
            UsuarioBE usuario = ObtenerUsuarioLogin();
            InscripcionBE inscripcion = inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, usuario.ID_INSTITUCION.Value);
            //if (inscripcion == null) return HttpNotFound();
            List<CriterioBE> listaCriterio = criterioLN.ListarCriterioPorConvocatoria(idConvocatoria, inscripcion.ID_INSCRIPCION);
            ConvocatoriaEtapaInscripcionBE convetainsc = convetainscLN.ObtenerConvocatoriaEtapaInscripcion(new ConvocatoriaEtapaInscripcionBE { ID_CONVOCATORIA = convocatoria.ID_CONVOCATORIA, ID_ETAPA = convocatoria.ID_ETAPA, ID_INSCRIPCION = inscripcion.ID_INSCRIPCION});
            //MigrarEmisionesBE migrar = migrarLN.obtenerIdIniciativasEmisiones(inscripcion.ID_INSCRIPCION);
            MigrarEmisionesBE migrar = new MigrarEmisionesBE { INICIATIVAS = "0"};
            PuntajeBE puntaje = puntajeLN.getPuntajePosible(idConvocatoria);

            ViewData["usuario"] = usuario;
            ViewData["convocatoria"] = convocatoria;
            ViewData["inscripcion"] = inscripcion;
            ViewData["listaCriterio"] = listaCriterio;
            ViewData["convetainsc"] = convetainsc;
            ViewData["migrar"] = migrar;
            ViewData["puntaje"] = puntaje;

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
            List<CasoBE> listaCaso = casoLN.ObtenerListaCasoCriterioPorConvocatoria(new CasoBE { ID_CONVOCATORIA = idConvocatoria, ID_CRITERIO = idCriterio, ID_INSCRIPCION = inscripcion.ID_INSCRIPCION });
            if (listaCaso == null) return HttpNotFound();

            ViewData["convocatoria"] = convocatoria;
            ViewData["inscripcion"] = inscripcion;
            ViewData["criterio"] = criterio;
            ViewData["listaCaso"] = listaCaso;

            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/BandejaParticipantes")]
        public ActionResult BandejaParticipantes(int idConvocatoria)
        {
            ViewData["convocatoria"] = convocatoriaLN.ObtenerConvocatoria(idConvocatoria); ;
            //ConvocatoriaBE conv = convocatoriaLN.ObtenerUltimaConvocatoria();
            //ViewData["convocatoria"] = idConvocatoria == conv.ID_CONVOCATORIA ? conv : convocatoriaLN.ObtenerConvocatoria(idConvocatoria); ;
            //ViewData["convocatoria"] = convocatoriaLN.ObtenerUltimaConvocatoria();
            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Institucion/{idInstitucion}/Evaluar")]
        public ActionResult EvaluarInstitucion(int idConvocatoria, int idInstitucion)
        {
            //ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerUltimaConvocatoria();
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);
            //if (convocatoria == null) return HttpNotFound();
            ViewData["convocatoria"] = convocatoria;
            InscripcionBE inscripcion = inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, idInstitucion);
            ConvocatoriaEtapaInscripcionBE convetainsc = convetainscLN.ObtenerConvocatoriaEtapaInscripcion(new ConvocatoriaEtapaInscripcionBE { ID_CONVOCATORIA = convocatoria.ID_CONVOCATORIA, ID_ETAPA = convocatoria.ID_ETAPA, ID_INSCRIPCION = inscripcion == null ? 0 : inscripcion.ID_INSCRIPCION });

            //if (inscripcion == null) return HttpNotFound();
            ViewData["convetainsc"] = convetainsc;
            ViewData["inscripcion"] = inscripcion;
            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Inscripcion/{idInscripcion}/EvaluacionCriterios")]
        public ActionResult EvaluacionCriterios(int idConvocatoria, int idInscripcion)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);
            //if (convocatoria == null) return HttpNotFound();
            UsuarioBE usuario = ObtenerUsuarioLogin();
            List<CriterioBE> listaCriterio = criterioLN.ListarCriterioPorConvocatoria(idConvocatoria, idInscripcion);
            ConvocatoriaEtapaInscripcionBE convetainsc = convetainscLN.ObtenerConvocatoriaEtapaInscripcion(new ConvocatoriaEtapaInscripcionBE { ID_CONVOCATORIA = convocatoria.ID_CONVOCATORIA, ID_ETAPA = convocatoria.ID_ETAPA, ID_INSCRIPCION = idInscripcion });
            ConvocatoriaCriterioPuntajeBE convcripunt = convcripuntajeLN.ObtenerPuntajeInscripcion(idConvocatoria, idInscripcion);

            ViewData["convocatoria"] = convocatoria;
            ViewData["listaCriterio"] = listaCriterio;
            ViewData["convetainsc"] = convetainsc;
            ViewData["convcripuntaje"] = convcripunt;
            ViewData["idinscripcion"] = idInscripcion;

            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Inscripcion/{idInscripcion}/EvaluacionCriterio/{idCriterio}")]
        public ActionResult EvaluacionCriterio(int idConvocatoria, int idCriterio, int idInscripcion)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);

            if (convocatoria == null) return HttpNotFound();

            UsuarioBE usuario = ObtenerUsuarioLogin();

            //InscripcionBE inscripcion = inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, usuario.ID_INSTITUCION.Value);

            //if (inscripcion == null) return HttpNotFound();

            CriterioBE criterio = criterioLN.ObtenerCriterioPorConvocatoria(idConvocatoria, idCriterio);

            if (criterio == null) return HttpNotFound();

            List<CasoBE> listaCaso = casoLN.ObtenerListaCasoCriterioPorConvocatoria(new CasoBE { ID_CONVOCATORIA = idConvocatoria, ID_CRITERIO = idCriterio, ID_INSCRIPCION = idInscripcion });

            if (listaCaso == null) return HttpNotFound();

            List<ConvocatoriaCriterioPuntajeBE> listaConvCriPuntaje = convcripuntajeLN.ListaConvocatoriaCriterioPuntaje(idConvocatoria, idCriterio);

            ViewData["convocatoria"] = convocatoria;
            ViewData["idinscripcion"] = idInscripcion;
            ViewData["criterio"] = criterio;
            ViewData["listaCaso"] = listaCaso;
            ViewData["listaConvCriPuntaje"] = listaConvCriPuntaje;

            return View();
        }

        [SesionOut]
        [Route("Cambiar-contrasena")]
        public ActionResult CambiarClave()
        {
            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Seguimiento/{idInscripcion}")]
        public ActionResult Seguimiento(int idConvocatoria, int idInscripcion)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);
            if (idInscripcion == 0) {
                UsuarioBE usuario = ObtenerUsuarioLogin();
                InscripcionBE inscripcion = inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, usuario.ID_INSTITUCION.Value);
                ViewData["inscripcion"] = inscripcion;
            }
            else
                ViewData["inscripcion"] = new InscripcionBE { ID_INSCRIPCION = idInscripcion };
            ViewData["convocatoria"] = convocatoria;
            

            return View();
        }

        [SesionOut]
        [Route("{idConvocatoria}/Detalle")]
        public ActionResult ConvocatoriaDetalle(int idConvocatoria)
        {
            ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(idConvocatoria);
            ViewData["convocatoria"] = convocatoria;

            return View();
        }

        [SesionOut]
        [Route("{0}/Reconocimiento")]
        public ActionResult DescargarReconocimiento(ConvocatoriaBE entidad)
        {
            string nombreArchivo = Guid.NewGuid() + ".pdf";
            string nombrePDF = nombrePDF = AppSettings.Get<string>("Ruta.Ficha") + nombreArchivo;
            bool success = new ReporteRepositorio().DescargarReconocimiento(entidad.ID_CONVOCATORIA, entidad.ID_INSTITUCION, nombrePDF);
            if (success)
            {
                reconocimientoLN.NombreFicha(new ConvocatoriaBE() { ID_CONVOCATORIA = entidad.ID_CONVOCATORIA, ID_INSTITUCION = entidad.ID_INSTITUCION, NOMBRE_FICHA = nombreArchivo });
            }
            return Json(new { success = true, message = nombreArchivo });
        }

        public ActionResult CriterioIngresar()
        {
            return View();
        }
        
    }
}