using sres.be.MRV;
using sres.ln.MRV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api.MRV
{
    [RoutePrefix("api/mrv/migraremisiones")]
    public class MigrarEmisionesMRVController : ApiController
    {
        MigrarEmisionesLN migrarLN = new MigrarEmisionesLN();
        sres.ln.MedidaMitigacionLN medidaLN = new sres.ln.MedidaMitigacionLN();

        [Route("obtenermigraremisiones")]
        [HttpGet]
        public MigrarEmisionesBE ObtenerMigrarEmisiones(string idIniciativas, string rucLogin, int idUsuarioLogin)
        {
            MigrarEmisionesBE emisiones = migrarLN.ObtenerMigrarEmisiones(idIniciativas, rucLogin, idUsuarioLogin);
            if (emisiones.LISTA_MIGRAR.Count > 0)
                foreach (MigrarEmisionesBE m in emisiones.LISTA_MIGRAR)
                    m.ARCHIVO_BASE = medidaLN.getMedidaMitigacion(new sres.be.MedidaMitigacionBE { ID_MEDMIT = m.ID_MEDMIT}).ARCHIVO_BASE;

            if (emisiones.LISTA_MIGRAR.Count > 0)
                    emisiones.LISTA_MIGRAR = medidaLN.actualizarValoresEmisiones(emisiones.LISTA_MIGRAR);
            return emisiones;
            //return migrarLN.ObtenerMigrarEmisiones(idIniciativas, rucLogin, idUsuarioLogin);
        }
    }
}
