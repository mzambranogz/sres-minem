using sres.be;
using sres.da;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class SectorLN : BaseLN
    {
        static SectorDA sectorDA = new SectorDA();

        public static List<SectorBE> ListarSectorPorEstado(string flagEstado)
        {
            return sectorDA.ListarSectorPorEstado(flagEstado, cn);
        }
    }
}
