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
    [RoutePrefix("api/proceso")]
    public class ProcesoController : ApiController
    {
        ProcesoLN procesoLN = new ProcesoLN();

        [Route("buscarobjeto")]
        [HttpGet]
        public List<ProcesoBE> BuscarObjeto(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return procesoLN.ListaBusquedaProceso(new ProcesoBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
        }

        [Route("obtenerobjeto")]
        [HttpGet]
        public ProcesoBE ObtenerObjeto(int id)
        {
            return procesoLN.getProceso(new ProcesoBE() { ID_PROCESO = id });
        }

        [Route("guardarobjeto")]
        public bool GuardarObjeto(ProcesoBE obj)
        {
            ProcesoBE c = procesoLN.GuardarProceso(obj);
            return c.OK;
        }

        [Route("obtenerallproceso")]
        [HttpGet]
        public List<ProcesoBE> ObtenerAllProceso()
        {
            return procesoLN.getAllProceso();
        }

    }
}
