using sres.be;
using sres.da;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class RolLN : BaseLN
    {
        static RolDA rolDA = new RolDA();

        public static List<RolBE> ListarRolPorEstado(string flagEstado)
        {
            return rolDA.ListarRolPorEstado(flagEstado, cn);
        }
    }
}
