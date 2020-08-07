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
    [RoutePrefix("api/sector")]
    public class SectorController : ApiController
    {
        SectorLN sectorLN = new SectorLN();

        [Route("buscarsector")]
        [HttpGet]
        public List<SectorBE> BuscarSector(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<SectorBE> lista = new List<SectorBE>();
            try
            {
                lista = sectorLN.ListaBusquedaSector(new SectorBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        [Route("obtenersector")]
        [HttpGet]
        public SectorBE ObtenerSector(int id)
        {
            SectorBE ent = new SectorBE();
            try
            {
                ent = sectorLN.getSector(new SectorBE() { ID_SECTOR = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return ent;
        }

        [Route("guardarsector")]
        public bool GuardarSector(SectorBE sector)
        {
            bool f;
            try
            {
                SectorBE ent = sectorLN.GuardarSector(sector);
                f = ent.OK;

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                f = false;
            }

            return f;
        }

        [Route("cambiarestadosector")]
        [HttpPost]
        public bool CambiarEstadoSector(SectorBE sector)
        {
            bool f;
            try
            {
                SectorBE ent = sectorLN.EliminarSector(sector);
                f = ent.OK;

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                f = false;
            }

            return f;
        }

        [Route("listarsectorporestado")]
        [HttpGet]
        public List<SectorBE> ListarSectorPorEstado(string flagEstado)
        { 
            List<SectorBE> lista = new List<SectorBE>();
            try
            {
                lista = sectorLN.ListarSectorPorEstado(flagEstado);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        [Route("obtenerallsector")]
        [HttpGet]
        public List<SectorBE> ListarSectorConvocatoria()
        {
            List<SectorBE> lista = new List<SectorBE>();
            try
            {
                lista = sectorLN.ListarSectorConvocatoria();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
