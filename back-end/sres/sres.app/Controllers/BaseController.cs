using sres.be;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sres.app.Controllers
{
    public class BaseController : Controller
    {
        protected UsuarioBE ObtenerUsuarioLogin()
        {
            string keySession = "user";
            UsuarioBE usuario = Session[keySession] == null ? null : (UsuarioBE)Session[keySession];
            return usuario;
        }
    }
}