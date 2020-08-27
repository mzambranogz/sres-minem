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
    public class IndicadorLN : BaseLN
    {
        IndicadorDA indDA = new IndicadorDA();

        public List<IndicadorBE> ListaBusquedaIndicador(IndicadorBE entidad)
        {
            List<IndicadorBE> lista = new List<IndicadorBE>();
            try
            {
                cn.Open();
                lista = indDA.ListarBusquedaIndicador(entidad, cn);
                if (lista.Count > 0)
                    foreach (IndicadorBE p in lista)
                            p.LISTA_PARAM = indDA.ListarParametro(p, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return lista;
        }

        public bool GuardarIndicador(IndicadorBE entidad)
        {
            bool seGuardo = true;
            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    foreach (IndicadorBE ind in entidad.LISTA_IND)
                        if (!(seGuardo = indDA.GuardarIndicador(ind, cn))) break;

                    if (seGuardo)
                        if (!string.IsNullOrEmpty(entidad.ID_ACTIVO))
                            seGuardo = indDA.EliminarIndicador(entidad, cn);

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return seGuardo;
        }
    }
}
