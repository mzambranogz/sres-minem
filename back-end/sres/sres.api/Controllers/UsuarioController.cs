using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.api.Controllers
{
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {

        [Route("buscarusuario")]
        [HttpGet]
        public List<UsuarioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<UsuarioBE> lista = null;
            try
            {
                lista = UsuarioLN.BuscarUsuario(busqueda, registros, pagina, columna, orden);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return lista;
        }
    }
}
