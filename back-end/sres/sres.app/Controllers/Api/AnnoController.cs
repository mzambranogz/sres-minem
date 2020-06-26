using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using sres.be;
using sres.ln;
using sres.ut;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/anno")]
    public class AnnoController : ApiController
    {
        AnnoLN annoLN = new AnnoLN();

        [Route("buscaranno")]
        [HttpGet]
        public List<AnnoBE> BuscarAnno(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<AnnoBE> lista = new List<AnnoBE>();
            try
            {
                lista = annoLN.ListaBusquedaAnno(new AnnoBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("obteneranno")]
        [HttpGet]
        public AnnoBE ObtenerAnno(int id)
        {
            AnnoBE ent = new AnnoBE();
            try
            {
                ent = annoLN.getAnno(new AnnoBE() { ID_ANNO = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return ent;
        }

        [Route("guardaranno")]
        public bool GuardarAnno(AnnoBE anno)
        {
            bool f;
            try
            {
                AnnoBE ent = annoLN.GuardarAnno(anno);
                f = ent.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                f = false;
            }
            return f;
        }

        [Route("cambiarestadoanno")]
        [HttpPost]
        public bool CambiarEstadoAnno(AnnoBE anno)
        {
            bool f;
            try
            {
                AnnoBE ent = annoLN.EliminarAnno(anno);
                f = ent.OK;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                f = false;
            }
            return f;
        }

        [Route("obtenerallanno")]
        [HttpGet]
        public List<AnnoBE> ObtenerAnno()
        {
            List<AnnoBE> lista = new List<AnnoBE>();
            try
            {
                lista = annoLN.getAllAnno();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }
    }
}
