using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;

namespace sres.ln
{
    public class EtapaLN : BaseLN
    {
        public static EtapaDA EtapaDA = new EtapaDA();

        public static EtapaBE ActualizarEtapa(EtapaBE entidad)
        {
            return EtapaDA.ActualizarEtapa(entidad, cn);
        }

        public static EtapaBE getEtapa(EtapaBE entidad)
        {
            return EtapaDA.getEtapa(entidad, cn);
        }

        public static List<EtapaBE> ListaBusquedaEtapa(EtapaBE entidad)
        {
            return EtapaDA.ListarBusquedaEtapa(entidad, cn);
        }
    }
}
