using sres.be;
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
    [RoutePrefix("api/documento")]
    public class DocumentoController : ApiController
    {
        DocumentoLN documentoLN = new DocumentoLN();

        [Route("buscardocumento")]
        [HttpGet]
        public List<DocumentoBE> BuscarCaso(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return documentoLN.ListaBusquedaCaso(new DocumentoBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenerdocumento")]
        [HttpGet]
        public DocumentoBE ObtenerCaso(int idcriterio, int iddocumento)
        {
            return documentoLN.getDocumento(new DocumentoBE() { ID_DOCUMENTO = iddocumento, ID_CRITERIO = idcriterio });
        }

        [Route("guardardocumento")]
        public bool GuardarCaso(DocumentoBE entidad)
        {
            return documentoLN.GuardarDocumento(entidad);
        }

        [Route("cambiarestadodocumento")]
        [HttpPost]
        public bool EliminarDocumento(DocumentoBE obj)
        {
            DocumentoBE c = documentoLN.EliminarDocumento(obj);
            return c.OK;
        }
    }
}
