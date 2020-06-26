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
    [RoutePrefix("api/rol")]
    public class RolController : ApiController
    {
        RolLN rolLN = new RolLN();

        [Route("buscarobjeto")]
        [HttpGet]
        public List<RolBE> BuscarObjeto(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<RolBE> lista = new List<RolBE>();
            try
            {
                lista = rolLN.ListaBusquedaRol(new RolBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("obtenerobjeto")]
        [HttpGet]
        public RolBE ObtenerObjeto(int id)
        {
            RolBE ent = new RolBE();
            try
            {
                ent = rolLN.getRol(new RolBE() { ID_ROL = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return ent;
        }

        [Route("guardarobjeto")]
        public bool GuardarObjeto(RolBE obj)
        {
            bool f;
            try
            {
                RolBE ent = rolLN.GuardarRol(obj);
                f = ent.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                f = false;
            }
            return f;
        }

        [Route("listarrolporestado")]
        [HttpGet]
        public List<RolBE> ListarRolPorEstado(string flagEstado)
        {
            List<RolBE> lista = new List<RolBE>();
            try
            {
                lista = rolLN.ListarRolPorEstado(flagEstado);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }
    }
}
