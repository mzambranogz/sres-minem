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
    [RoutePrefix("api/estrella")]
    public class EstrellaController : ApiController
    {
        EstrellaLN estrellaLN = new EstrellaLN();

        [Route("buscarestrella")]
        [HttpGet]
        public List<EstrellaBE> BuscarEstrella(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return estrellaLN.ListaBusquedaEstrella(new EstrellaBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("guardarestrella")]
        public bool GuardarEstrella(EstrellaBE entidad)
        {
            return estrellaLN.GuardarEstrella(entidad).OK;
        }

        [Route("obtenerallestrella")]
        [HttpGet]
        public List<EstrellaBE> BuscarAllEstrella()
        {
            return estrellaLN.listarEstrellas();
        }

        [Route("obtenerestrella")]
        [HttpGet]
        public EstrellaBE ObtenerEstrella(int id)
        {
            return estrellaLN.getEstrella(new EstrellaBE() { ID_ESTRELLA = id });
        }

        [Route("cambiarestadoestrella")]
        [HttpPost]
        public bool EliminarEstrella(EstrellaBE obj)
        {
            EstrellaBE c = estrellaLN.EliminarEstrella(obj);
            return c.OK;
        }

    }
}
