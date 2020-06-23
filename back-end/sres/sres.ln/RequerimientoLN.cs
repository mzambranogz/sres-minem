using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;

namespace sres.ln
{
    public class RequerimientoLN : BaseLN
    {
        public static RequerimientoDA RequerimientoDA = new RequerimientoDA();

        //public static RequerimientoBE RegistroRequerimiento(RequerimientoBE entidad)
        //{
        //    return RequerimientoDA.RegistroRequerimiento(entidad);
        //}

        //public static RequerimientoBE ActualizarRequerimiento(RequerimientoBE entidad)
        //{
        //    return RequerimientoDA.ActualizarRequerimiento(entidad);
        //}

        public static RequerimientoBE GuardarRequerimiento(RequerimientoBE entidad)
        {
            return RequerimientoDA.GuardarRequerimiento(entidad, cn);
        }

        public static RequerimientoBE EliminarRequerimiento(RequerimientoBE entidad)
        {
            return RequerimientoDA.EliminarRequerimiento(entidad, cn);
        }

        public static RequerimientoBE getRequerimiento(RequerimientoBE entidad)
        {
            return RequerimientoDA.getRequerimiento(entidad, cn);
        }

        public static List<RequerimientoBE> ListaBusquedaRequerimiento(RequerimientoBE entidad)
        {
            return RequerimientoDA.ListarBusquedaRequerimiento(entidad, cn);
        }
    }
}