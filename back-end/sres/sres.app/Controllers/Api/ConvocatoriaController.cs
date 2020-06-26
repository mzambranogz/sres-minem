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
    [RoutePrefix("api/convocatoria")]
    public class ConvocatoriaController : ApiController
    {    
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();

        [Route("guardarconvocatoria")]
        [HttpPost]
        public ConvocatoriaBE GuardarConvocatoria(ConvocatoriaBE obj)
        {
            try
            {
                return convocatoriaLN.RegistroConvocatoria(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("buscarconvocatoria")]
        [HttpGet]
        public List<ConvocatoriaBE> BuscarConvocatoria(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.ListarBusquedaConvocatoria(new ConvocatoriaBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("obtenerconvocatoria")]
        [HttpGet]
        public ConvocatoriaBE ObtenerConvocatoria(int id)
        {
            return convocatoriaLN.getConvocatoria(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
        }

        [Route("cambiarestadoconvocatoria")]
        [HttpPost]
        public bool CambiarEstadoCriterio(ConvocatoriaBE obj)
        {
            ConvocatoriaBE c = convocatoriaLN.EliminarConvocatoria(obj);
            return c.OK;
        }

        [Route("listarconvocatoriareq")]
        [HttpGet]
        public List<ConvocatoriaBE> listarConvocatoriaReq(int id)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaReq(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriacri")]
        [HttpGet]
        public List<ConvocatoriaBE> listarConvocatoriaCri(int id)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaCri(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriaeva")]
        [HttpGet]
        public List<ConvocatoriaBE> listarConvocatoriaEva(int id)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaEva(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriaeta")]
        [HttpGet]
        public List<ConvocatoriaBE> listarConvocatoriaEta(int id)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaEta(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

    }
}
