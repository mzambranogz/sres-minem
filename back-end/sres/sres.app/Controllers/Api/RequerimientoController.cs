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
    [RoutePrefix("api/requerimiento")]
    public class RequerimientoController : ApiController
    {

        [Route("buscarobjeto")]
        [HttpGet]
        public List<RequerimientoBE> BuscarObjeto(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return RequerimientoLN.ListaBusquedaRequerimiento(new RequerimientoBE() { CANTIDAD_REGISTROS = 10, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
        }

        [Route("obtenerobjeto")]
        [HttpGet]
        public RequerimientoBE ObtenerObjeto(int id)
        {
            return RequerimientoLN.getRequerimiento(new RequerimientoBE() { ID_REQUERIMIENTO = id });
        }

        [Route("guardarobjeto")]
        public bool GuardarObjeto(RequerimientoBE obj)
        {
            RequerimientoBE c = RequerimientoLN.GuardarRequerimiento(obj);
            return c.OK;
        }

        [Route("cambiarestadoobjeto")]
        [HttpPost]
        public bool CambiarEstadoObjeto(RequerimientoBE obj)
        {
            RequerimientoBE c = RequerimientoLN.EliminarRequerimiento(obj);
            return c.OK;
        }

    }
}
