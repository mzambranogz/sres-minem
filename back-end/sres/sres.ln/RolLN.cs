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
    public class RolLN : BaseLN
    {
        RolDA rolDA = new RolDA();

        public List<RolBE> ListarRolPorEstado(string flagEstado)
        {
            List<RolBE> lista = new List<RolBE>();

            try
            {
                cn.Open();
                lista = rolDA.ListarRolPorEstado(flagEstado, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
