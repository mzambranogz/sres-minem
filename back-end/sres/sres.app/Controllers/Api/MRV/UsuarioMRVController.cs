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
        [Route("verificarruccorreo")]
        [HttpGet]
        public bool VerificarRucCorreo(string ruc, string correo)
        {
            return UsuarioLN.VerificarRucCorreo(ruc, correo);
        }

        [Route("validarloginusuario")]
        [HttpPost]
        public Dictionary<string, object> ValidarLoginUsuario(string ruc, string correo, string contraseña)
        {
            return UsuarioLN.ValidarLoginUsuario(ruc, correo, contraseña);
        }
    }
}
