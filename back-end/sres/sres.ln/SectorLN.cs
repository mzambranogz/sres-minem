using sres.be;
using sres.da;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class SectorLN : BaseLN
    {
        SectorDA sectorDA = new SectorDA();

        public SectorBE GuardarSector(SectorBE entidad)
        {
            SectorBE item = null;

            try
            {
                cn.Open();
                item = sectorDA.GuardarSector(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public SectorBE EliminarSector(SectorBE entidad)
        {
            SectorBE item = null;

            try
            {
                cn.Open();
                item = sectorDA.EliminarSector(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public SectorBE getSector(SectorBE entidad)
        {
            SectorBE item = null;

            try
            {
                cn.Open();
                item = sectorDA.getSector(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<SectorBE> ListaBusquedaSector(SectorBE entidad)
        {
            List<SectorBE> lista = new List<SectorBE>();

            try
            {
                cn.Open();
                lista = sectorDA.ListarBusquedaSector(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<SectorBE> ListarSectorPorEstado(string flagEstado)
        {
            List<SectorBE> lista = new List<SectorBE>();

            try
            {
                cn.Open();
                lista = sectorDA.ListarSectorPorEstado(flagEstado, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
