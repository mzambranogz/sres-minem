using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;

namespace sres.ln
{
    public class CriterioLN
    {
        public static CriterioDA criterioDA = new CriterioDA();

        public static List<CriterioBE> ListaBusquedaCriterio(CriterioBE entidad)
        {
            return criterioDA.ListarBusquedaCriterio(entidad);
        }
    }
}
