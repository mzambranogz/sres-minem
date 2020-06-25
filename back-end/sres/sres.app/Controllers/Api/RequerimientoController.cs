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
        RequerimientoLN requerimientoLN = new RequerimientoLN();

        [Route("buscarobjeto")]
        [HttpGet]
        public List<RequerimientoBE> BuscarObjeto(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return requerimientoLN.ListaBusquedaRequerimiento(new RequerimientoBE() { CANTIDAD_REGISTROS = 10, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
        }

        [Route("obtenerobjeto")]
        [HttpGet]
        public RequerimientoBE ObtenerObjeto(int id)
        {
            return requerimientoLN.getRequerimiento(new RequerimientoBE() { ID_REQUERIMIENTO = id });
        }

        [Route("guardarobjeto")]
        public bool GuardarObjeto(RequerimientoBE obj)
        {
            RequerimientoBE c = requerimientoLN.GuardarRequerimiento(obj);
            return c.OK;
        }

        [Route("cambiarestadoobjeto")]
        [HttpPost]
        public bool CambiarEstadoObjeto(RequerimientoBE obj)
        {
            RequerimientoBE c = requerimientoLN.EliminarRequerimiento(obj);
            return c.OK;
        }

        [Route("obtenerallrequerimiento")]
        [HttpGet]
        public List<RequerimientoBE> ObtenerRequerimiento()
        {
            try
            {
                return requerimientoLN.getAllRequerimiento();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
