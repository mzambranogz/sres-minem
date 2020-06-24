using sres.be.MRV;
using sres.ln.MRV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/mrv/usuario")]
    public class UsuarioMRVController : ApiController
    {
        UsuarioLN usuarioLN = new UsuarioLN();

        [Route("verificarcorreo")]
        [HttpGet]
        public bool VerificarCorreo(string ruc, string correo)
        {
            return usuarioLN.VerificarCorreo(correo);
        }

        [Route("validarloginusuario")]
        [HttpPost]
        public Dictionary<string, object> ValidarLoginUsuario(string correo, string contraseña)
        {
            return usuarioLN.ValidarLoginUsuario(correo, contraseña);
        }
    }
}
