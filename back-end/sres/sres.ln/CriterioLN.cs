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

        public static CriterioBE RegistroCriterio(CriterioBE entidad)
        {
            return criterioDA.RegistroCriterio(entidad);
        }

        public static CriterioBE GuardarCriterio(CriterioBE entidad)
        {
            return criterioDA.GuardarCriterio(entidad);
        }

        public static CriterioBE EliminarCriterio(CriterioBE entidad)
        {
            return criterioDA.EliminarCriterio(entidad);
        }

        public static CriterioBE getCriterio(CriterioBE entidad)
        {
            return criterioDA.getCriterio(entidad);
        }

        public static List<CriterioBE> ListaBusquedaCriterio(CriterioBE entidad)
        {
            return criterioDA.ListarBusquedaCriterio(entidad);
        }
    }
}
