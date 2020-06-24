﻿using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/rol")]
    public class RolController : ApiController
    {
        RolLN rolLN = new RolLN();

        [Route("listarrolporestado")]
        [HttpGet]
        public List<RolBE> ListarRolPorEstado(string flagEstado)
        {
            return rolLN.ListarRolPorEstado(flagEstado);
        }
    }
}
