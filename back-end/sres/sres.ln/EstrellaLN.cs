using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;
using Oracle.DataAccess.Client;
using sres.ut;

namespace sres.ln
{
    public class EstrellaLN : BaseLN
    {
        EstrellaDA estrellaDA = new EstrellaDA();
        public List<EstrellaBE> listarEstrellas()
        {
            List<EstrellaBE> lista = new List<EstrellaBE>();

            try
            {
                cn.Open();
                lista = estrellaDA.listarEstrellas(cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
