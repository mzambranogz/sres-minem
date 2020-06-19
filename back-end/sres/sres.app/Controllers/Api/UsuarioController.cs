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
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        [Route("buscarusuario")]
        [HttpGet]
        public List<UsuarioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return UsuarioLN.BuscarUsuario(busqueda, registros, pagina, columna, orden);
        }

        [Route("cambiarestadousuario")]
        [HttpPost]
        public bool CambiarEstadoUsuario(UsuarioBE usuario)
        {
            return UsuarioLN.CambiarEstadoUsuario(usuario);
        }
    }
}
