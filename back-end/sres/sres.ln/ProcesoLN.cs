using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;

namespace sres.ln
{
    public class ProcesoLN
    {
        public static ProcesoDA ProcesoDA = new ProcesoDA();

        public static ProcesoBE ActualizarProceso(ProcesoBE entidad)
        {
            return ProcesoDA.ActualizarProceso(entidad);
        }

        public static ProcesoBE getProceso(ProcesoBE entidad)
        {
            return ProcesoDA.getProceso(entidad);
        }

        public static List<ProcesoBE> ListaBusquedaProceso(ProcesoBE entidad)
        {
            return ProcesoDA.ListarBusquedaProceso(entidad);
        }
    }
}
