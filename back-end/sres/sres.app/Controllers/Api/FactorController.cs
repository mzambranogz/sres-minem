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
    [RoutePrefix("api/factor")]
    public class FactorController : ApiController
    {
        FactorLN factorLN = new FactorLN();
        [Route("buscarfactor")]
        [HttpGet]
        public List<FactorBE> BuscarFactor(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return factorLN.ListaBusquedaFactor(new FactorBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("guardarfactor")]
        public bool GuardarFactor(FactorBE entidad)
        {
            return factorLN.GuardarFactor(entidad);
        }

        [Route("obtenerfactor")]
        [HttpGet]
        public FactorBE ObtenerFactor(int idfactor)
        {
            return factorLN.ObtenerFactor(new FactorBE() { ID_FACTOR = idfactor });
        }
    }
}
