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
    [RoutePrefix("api/reconocimiento")]
    public class ReconocimientoController : ApiController
    {
        ReconocimientoLN reconocimientoLN = new ReconocimientoLN();

        [Route("buscarparticipantes")]
        [HttpGet]
        public DataPaginateBE BuscarParticipantes(string razonSocialInstitucion, int? idTipoEmpresa, int? idCriterio, int? idMedMit, int? añoInicioConvocatoria, int? idInsignia, int? idEstrella, int? idSector, int? idEspecial, int registros, int pagina, string columna, string orden)
        {
            List<ReconocimientoBE> reconocimiento = reconocimientoLN.BuscarParticipantes(razonSocialInstitucion, idTipoEmpresa, idCriterio, idMedMit, añoInicioConvocatoria, idInsignia, idEstrella, idSector, idEspecial, registros, pagina, columna, orden);

            DataPaginateBE data = new DataPaginateBE
            {
                DATA = reconocimiento,
                PAGINA = reconocimiento.Count == 0 ? 0 : reconocimiento[0].PAGINA,
                CANTIDAD_REGISTROS = reconocimiento.Count == 0 ? 0 : reconocimiento[0].CANTIDAD_REGISTROS,
                TOTAL_PAGINAS = reconocimiento.Count == 0 ? 0 : reconocimiento[0].TOTAL_PAGINAS,
                TOTAL_REGISTROS = reconocimiento.Count == 0 ? 0 : reconocimiento[0].TOTAL_REGISTROS
            };

            return data;
        }
    }
}
