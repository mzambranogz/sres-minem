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
    public class FactorValorLN : BaseLN
    {
        FactorValorDA componenteDA = new FactorValorDA();
        FactorDA factorDA = new FactorDA();
        public List<ComponenteBE> ListaBusquedaComponenteFactor(ComponenteBE entidad)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();
            try
            {
                cn.Open();
                lista = componenteDA.ListarBusquedaComponenteFactor(entidad, cn);
                if (lista.Count > 0)
                    foreach (ComponenteBE c in lista)
                        if (c.ID_FACTORES != "")
                            c.LISTA_FAC_cOMP = factorDA.listaFactorComponente(c.ID_FACTORES.Replace("|", ","), cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
