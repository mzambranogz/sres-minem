using sres.be.MRV;
using sres.da.MRV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln.MRV
{
    public class InstitucionLN : BaseLN
    {
        InstitucionDA institucionDA = new InstitucionDA();

        public InstitucionBE ObtenerInstitucionPorRuc(string ruc)
        {
            InstitucionBE item = null;

            try
            {
                cn.Open();
                item = institucionDA.ObtenerInstitucionPorRuc(ruc, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
