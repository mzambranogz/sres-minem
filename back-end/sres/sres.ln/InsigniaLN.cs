using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;
using System.Data;

namespace sres.ln
{
    public class InsigniaLN : BaseLN
    {
        InsigniaDA insigniaDA = new InsigniaDA();

        public List<InsigniaBE> getAllInsignia()
        {
            List<InsigniaBE> lista = new List<InsigniaBE>();
            try
            {
                cn.Open();
                lista = insigniaDA.getAllInsignia(cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
