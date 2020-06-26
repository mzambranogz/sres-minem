using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using sres.be;
using sres.ln;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/anno")]
    public class AnnoController : ApiController
    {
        AnnoLN annoLN = new AnnoLN();

        [Route("buscaranno")]
        [HttpGet]
        public List<AnnoBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return annoLN.ListaBusquedaAnno(new AnnoBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
        }

        [Route("obteneranno")]
        [HttpGet]
        public AnnoBE ObtenerAnno(int id)
        {
            return annoLN.getAnno(new AnnoBE() { ID_ANNO = id });
        }

        [Route("guardaranno")]
        public bool GuardarAnno(AnnoBE anno)
        {
            AnnoBE c = annoLN.GuardarAnno(anno);
            return c.OK;
        }

        [Route("cambiarestadoanno")]
        [HttpPost]
        public bool CambiarEstadoAnno(AnnoBE anno)
        {
            AnnoBE c = annoLN.EliminarAnno(anno);
            return c.OK;
        }

        [Route("obtenerallanno")]
        [HttpGet]
        public List<AnnoBE> ObtenerAnno()
        {
            try
            {
                return annoLN.getAllAnno();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
