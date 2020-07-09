using sres.be;
using sres.da;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
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
            catch(Exception ex) { throw ex; }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public InstitucionBE ObtenerInstitucion(int idInstitucion)
        {
            InstitucionBE item = null;

            try
            {
                cn.Open();
                item = institucionDA.ObtenerInstitucion(idInstitucion, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
