using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/insignia")]
    public class InsigniaController : ApiController
    {
        InsigniaLN insigniaLN = new InsigniaLN();

        [Route("obtenerallinsignia")]
        [HttpGet]
        public List<InsigniaBE> ObtenerAllInsignia()
        {
            return insigniaLN.getAllInsignia();
        }

        [Route("buscarinsignia")]
        [HttpGet]
        public List<InsigniaBE> BuscarInsignia(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return insigniaLN.ListaBusquedaInsignia(new InsigniaBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenerinsignia")]
        [HttpGet]
        public InsigniaBE ObtenerInsignia(int id)
        {
            return insigniaLN.getInsignia(new InsigniaBE() { ID_INSIGNIA = id });
        }

        [Route("guardarinsignia")]
        public bool GuardarCriterio(InsigniaBE criterio)
        {
            return insigniaLN.GuardarInsignia(criterio);
        }

        [Route("cambiarestadoinsignia")]
        [HttpPost]
        public bool EliminarInsignia(InsigniaBE obj)
        {
            InsigniaBE c = insigniaLN.EliminarInsignia(obj);
            return c.OK;
        }
    }
}
