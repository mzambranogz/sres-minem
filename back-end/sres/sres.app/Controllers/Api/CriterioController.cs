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
    [RoutePrefix("api/criterio")]
    public class CriterioController : ApiController
    {
        CriterioLN criterioLN = new CriterioLN();

        [Route("buscarcriterio")]
        [HttpGet]
        public List<CriterioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return criterioLN.ListaBusquedaCriterio(new CriterioBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
        }

        [Route("obtenercriterio")]
        [HttpGet]
        public CriterioBE ObtenerCriterio(int idCriterio)
        {
            return criterioLN.getCriterio(new CriterioBE() { ID_CRITERIO = idCriterio });
        }

        [Route("guardarcriterio")]
        public bool GuardarCriterio(CriterioBE criterio)
        {
            CriterioBE c = criterioLN.GuardarCriterio(criterio);
            return c.OK;
        }

        [Route("cambiarestadocriterio")]
        [HttpPost]
        public bool CambiarEstadoCriterio(CriterioBE criterio)
        {
            CriterioBE c = criterioLN.EliminarCriterio(criterio);
            return c.OK;
        }

        [Route("obtenerallcriterio")]
        [HttpGet]
        public List<CriterioBE> ObtenerCriterio()
        {
            try
            {
                return criterioLN.getAllCriterio();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
