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
    [RoutePrefix("api/puntaje")]
    public class PuntajeController : ApiController
    {
        PuntajeLN puntajeLN = new PuntajeLN();
        [Route("buscarcriteriopuntaje")]
        [HttpGet]
        public List<PuntajeBE> BuscarPuntaje(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return puntajeLN.ListaBusquedaPuntaje(new PuntajeBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenercriteriopuntaje")]
        [HttpGet]
        public PuntajeBE ObtenerPuntaje(int idcriterio, int iddetalle)
        {
            return puntajeLN.getPuntaje(new PuntajeBE() { ID_DETALLE = iddetalle, ID_CRITERIO = idcriterio });
        }

        [Route("guardarcriteriopuntaje")]
        public bool GuardarPuntaje(PuntajeBE entidad)
        {
            return puntajeLN.GuardarPuntaje(entidad);
        }

        [Route("cambiarestadocriteriopuntaje")]
        [HttpPost]
        public bool EliminarPuntaje(PuntajeBE obj)
        {
            PuntajeBE c = puntajeLN.EliminarPuntaje(obj);
            return c.OK;
        }
    }
}
