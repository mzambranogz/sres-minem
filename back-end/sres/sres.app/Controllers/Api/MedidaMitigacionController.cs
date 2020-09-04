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
    [RoutePrefix("api/medidamitigacion")]
    public class MedidaMitigacionController : ApiController
    {
        MedidaMitigacionLN medidaLN = new MedidaMitigacionLN();

        [Route("buscarmedidamitigacion")]
        [HttpGet]
        public List<MedidaMitigacionBE> BuscarMedidaMitigacion(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return medidaLN.ListaBusquedaMedidaMitigacion(new MedidaMitigacionBE { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenermedidamitigacion")]
        [HttpGet]
        public MedidaMitigacionBE ObtenerMedidaMitigacion(int idMedida)
        {
            return medidaLN.getMedidaMitigacion(new MedidaMitigacionBE() { ID_MEDMIT = idMedida });
        }

        [Route("guardarmedidamitigacion")]
        public MedidaMitigacionBE GuardarMedidaMitigacion(MedidaMitigacionBE entidad)
        {
            return medidaLN.GuardarMedidaMitigacion(entidad);
        }

        [Route("cambiarestadomedidamitigacion")]
        [HttpPost]
        public bool CambiarEstadoMedidaMitigacion(MedidaMitigacionBE entidad)
        {
            MedidaMitigacionBE c = medidaLN.EliminarMedidaMitigacion(entidad);
            return c.OK;
        }
    }
}
