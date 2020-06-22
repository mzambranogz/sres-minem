using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;

namespace sres.ln
{
    public class EtapaLN
    {
        public static EtapaDA EtapaDA = new EtapaDA();

        public static EtapaBE GuardarEtapa(EtapaBE entidad)
        {
            return EtapaDA.GuardarEtapa(entidad);
        }

        public static EtapaBE getEtapa(EtapaBE entidad)
        {
            return EtapaDA.getEtapa(entidad);
        }

        public static List<EtapaBE> ListaBusquedaEtapa(EtapaBE entidad)
        {
            return EtapaDA.ListarBusquedaEtapa(entidad);
        }
    }
}
