﻿using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/criterio")]
    public class CriterioController : ApiController
    {
        CriterioLN criterioLN = new CriterioLN();

        [Route("buscarcriterio")]
        [HttpGet]
        public List<CriterioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return criterioLN.ListaBusquedaCriterio(new CriterioBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenercriterio")]
        [HttpGet]
        public CriterioBE ObtenerCriterio(int idCriterio)
        {
            return criterioLN.getCriterio(new CriterioBE() { ID_CRITERIO = idCriterio });
        }

        [Route("guardarcriterio")]
        public bool GuardarCriterio(CriterioBE criterio)
        {
            return criterioLN.GuardarCriterio(criterio);
        }

        [Route("cambiarestadocriterio")]
        [HttpPost]
        public bool CambiarEstadoCriterio(CriterioBE criterio)
        {
            CriterioBE c = criterioLN.EliminarCriterio(criterio);
            return c.OK;
        }

        [Route("obtenerallcriterio")]
        [HttpGet]
        public List<CriterioBE> ObtenerCriterio()
        {
            try
            {
                return criterioLN.getAllCriterio();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("buscarcriteriocaso")]
        [HttpGet]
        public List<ComponenteBE> BuscarCriterioCaso(int id_criterio, int id_inscripcion, int id_caso)
        {
            return criterioLN.BuscarCriterioCaso(id_criterio, id_inscripcion, id_caso);
        }

        [Route("buscarcriteriocasodocumento")]
        [HttpGet]
        public List<DocumentoBE> BuscarCriterioCasoDocumento(int id_criterio, int id_caso, int id_convocatoria, int id_inscripcion)
        {
            return criterioLN.BuscarCriterioCasoDocumento(id_criterio, id_caso, id_convocatoria, id_inscripcion);
        }

        [Route("guardarcriteriocaso")]
        [HttpPost]
        public bool GuardarCriterioCaso(CasoBE entidad)
        {
            return criterioLN.GuardarCriterioCaso(entidad).OK;
        }

        [Route("filacriteriocaso")]
        [HttpGet]
        //public List<CasoBE> BuscarCriterioCaso(int id_criterio, int id_caso, int id_inscripcion)
        public ComponenteBE FilaCriterioCaso(int id_criterio, int id_caso, int id_componente)
        {
            return criterioLN.FilaCriterioCaso(id_criterio, id_caso, id_componente);
        }

        [Route("obtenerconvcriteriopuntajeinscripcion")]
        [HttpGet]
        public ConvocatoriaCriterioPuntajeInscripBE ObtenerConvCriPuntajeInsc(int idCriterio, int idInscripcion, int idConvocatoria)
        {
            return criterioLN.ObtenerConvCriPuntInscrip(idConvocatoria, idCriterio, idInscripcion);
        }

        [Route("obtenerdocumento/{idConvocatoria}/{idCriterio}/{idCaso}/{idInscripcion}/{idDocumento}")]
        [HttpGet]
        public HttpResponseMessage ObtnerDocumento(int idConvocatoria, int idCriterio, int idCaso, int idInscripcion, int idDocumento)
        {
            string pathFile = criterioLN.ObtenerRutaDocumento(idConvocatoria, idCriterio, idCaso, idInscripcion, idDocumento);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound);

            if (string.IsNullOrEmpty(pathFile)) return response;

            byte[] byteFile = File.ReadAllBytes(pathFile);
            string contentTypeFile = MimeMapping.GetMimeMapping(pathFile);

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(byteFile));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = Path.GetFileName(pathFile);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentTypeFile);

            return response;
        }

        [Route("guardarevaluacioncriterio")]
        [HttpPost]
        public bool GuardarEvaluacionCriterio(ConvocatoriaCriterioPuntajeInscripBE entidad)
        {
            return criterioLN.GuardarEvaluacionCriterio(entidad);
        }

        [Route("guardarevaluacioncriterios")]
        [HttpPost]
        public bool GuardarEvaluacionCriteriosInscripcion(ConvocatoriaCriterioPuntajeInscripBE entidad)
        {
            return criterioLN.GuardarEvaluacionCriterioInscripcion(entidad);
        }

        [Route("obtenerallcategoria")]
        [HttpGet]
        public List<CategoriaBE> ObtenerCategoria()
        {
            try
            {
                return criterioLN.getAllCategoria();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[Route("listarcriterioporconvocatoria")]
        //[HttpGet]
        //public List<CriterioBE> ListarCriterioPorConvocatoria(int idConvocatoria)
        //{
        //    return criterioLN.ListarCriterioPorConvocatoria(idConvocatoria);
        //}
    }
}
