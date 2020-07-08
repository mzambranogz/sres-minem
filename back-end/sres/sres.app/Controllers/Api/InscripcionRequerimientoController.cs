using sres.be;
using sres.ln;
using sres.ut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/inscripcionrequerimiento")]
    public class InscripcionRequerimientoController : ApiController
    {
        InscripcionRequerimientoLN inscripcionRequerimientoLN = new InscripcionRequerimientoLN();

        [Route("listarinscripcionrequerimientoporconvocatoriainscripcion/{idConvocatoria}/{idInscripcion?}")]
        [HttpGet]
        public List<InscripcionRequerimientoBE> ListarInscripcionRequerimientoPorConvocatoriaInscripcion(int idConvocatoria, int? idInscripcion = null)
        {
            return inscripcionRequerimientoLN.ListarInscripcionRequerimientoPorConvocatoriaInscripcion(idConvocatoria, idInscripcion);
        }

        [Route("obtenerarchivo/{idConvocatoria}/{idInscripcion}/{idInstitucion}/{idRequerimiento}")]
        [HttpGet]
        public HttpResponseMessage ObtnerArchivo(int idConvocatoria, int idInscripcion, int idInstitucion, int idRequerimiento)
        {
            string pathFile = inscripcionRequerimientoLN.ObtenerRutaArchivoRequerimiento(idConvocatoria, idInscripcion, idInstitucion, idRequerimiento);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound);

            if (string.IsNullOrEmpty(pathFile)) return response;

            //string contentFile = File.ReadAllText(pathFile);
            byte[] byteFile = File.ReadAllBytes(pathFile);
            string contentTypeFile = MimeMapping.GetMimeMapping(pathFile);
            //Stream stream = new MemoryStream(byteFile);
            //StreamReader sr = new StreamReader(stream);
            //Encoding encoding = sr.CurrentEncoding;
            //sr.Close();
            //sr.Dispose();

            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(byteFile));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = Path.GetFileName(pathFile);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentTypeFile);

            return response;
        }
    }
}
