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
    public class ComponenteLN : BaseLN
    {
        ComponenteDA componenteDA = new ComponenteDA();
        public List<ComponenteBE> ListaBusquedaComponente(ComponenteBE entidad)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();
            try
            {
                cn.Open();
                lista = componenteDA.ListarBusquedaComponente(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
