using sres.be;
using sres.ln;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/trabajadorcama")]
    public class TrabajadorCamaController : ApiController
    {
        TrabajadorCamaLN trabajadorcamaLN = new TrabajadorCamaLN();

        [Route("listatrabajadorcama")]
        [HttpGet]
        public List<TrabajadorCamaBE> listaTrabajadorCama(int idSubsectorTipoempresa)
        {
            List<TrabajadorCamaBE> lista = new List<TrabajadorCamaBE>();
            try
            {
                lista = trabajadorcamaLN.listaSubsectorTipoempresa(idSubsectorTipoempresa);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
