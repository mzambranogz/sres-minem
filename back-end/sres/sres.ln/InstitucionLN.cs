using sres.be;
using sres.da;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class InstitucionLN
    {
        static InstitucionDA institucionDA = new InstitucionDA();

        public static InstitucionBE ObtenerInstitucionPorRuc(string ruc)
        {
            return institucionDA.ObtenerInstitucionPorRuc(ruc);
        }

        public static InstitucionBE ObtenerInstitucion(int idInstitucion)
        {
            return institucionDA.ObtenerInstitucion(idInstitucion);
        }
    }
}
