using sres.be.MRV;
using sres.da.MRV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln.MRV
{
    public class InstitucionLN : BaseLN
    {
        static InstitucionDA institucionDA = new InstitucionDA();

        public static InstitucionBE ObtenerInstitucionPorRuc(string ruc)
        {
            return institucionDA.ObtenerInstitucionPorRuc(ruc, cn);
        }
    }
}
