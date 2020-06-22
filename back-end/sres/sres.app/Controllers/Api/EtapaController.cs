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
    [RoutePrefix("api/etapa")]
    public class EtapaController : ApiController
    {
        [Route("buscarobjeto")]
        [HttpGet]
        public List<EtapaBE> BuscarObjeto(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return EtapaLN.ListaBusquedaEtapa(new EtapaBE() { CANTIDAD_REGISTROS = 10, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
        }

        [Route("obtenerobjeto")]
        [HttpGet]
        public EtapaBE ObtenerObjeto(int id)
        {
            return EtapaLN.getEtapa(new EtapaBE() { ID_ETAPA = id });
        }

        [Route("guardarobjeto")]
        public bool GuardarObjeto(EtapaBE obj)
        {
            EtapaBE c = EtapaLN.GuardarEtapa(obj);
            return c.OK;
        }
    }
}
