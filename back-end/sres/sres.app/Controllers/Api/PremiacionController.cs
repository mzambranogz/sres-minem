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
    [RoutePrefix("api/premiacion")]
    public class PremiacionController : ApiController
    {
        PremiacionLN premLN = new PremiacionLN();
        [Route("buscarpremiacion")]
        [HttpGet]
        public List<PremiacionBE> BuscarComponentePremiacion(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return premLN.ListaBusquedaPremiacion(new PremiacionBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("guardarpremiacion")]
        public PremiacionBE GuardarPremiacion(PremiacionBE entidad)
        {
            return premLN.GuardarPremiacion(entidad);
        }

        [Route("obtenerpremiacion")]
        [HttpGet]
        public PremiacionBE ObtenerPremiacion(int id)
        {
            return premLN.getPremiacion(new PremiacionBE() { ID_PREMIACION = id });
        }

        [Route("cambiarestadopremiacion")]
        [HttpPost]
        public bool EliminarPremiacion(PremiacionBE obj)
        {
            PremiacionBE c = premLN.EliminarPremiacion(obj);
            return c.OK;
        }

        [Route("listareconocimiento")]
        [HttpGet]
        public List<ReconocimientoBE> ListaReconocimiento(int idInstitucion)
        {
            return premLN.ListaReconocimiento(idInstitucion);
        }
    }
}
