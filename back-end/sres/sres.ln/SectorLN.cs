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
