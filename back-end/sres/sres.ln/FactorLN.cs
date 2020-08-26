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
    public class FactorLN : BaseLN
    {
        FactorDA factorDA = new FactorDA();
        public List<FactorBE> ListaBusquedaFactor(FactorBE entidad)
        {
            List<FactorBE> lista = new List<FactorBE>();
            try
            {
                cn.Open();
                lista = factorDA.ListarBusquedaFactor(entidad, cn);
                if (lista.Count > 0)
                    foreach (FactorBE p in lista)
                            p.LISTA_PARAM_FACTOR = factorDA.ListarFactorParametro(p.ID_FACTOR, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return lista;
        }

        public bool GuardarFactor(FactorBE entidad)
        {
            bool seGuardo = true;
            try
            {
                int id = -1;
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    seGuardo = factorDA.GuardarFactor(entidad, out id, cn).OK;
                    if (seGuardo)
                        if (entidad.LISTA_PARAM_FACTOR != null)
                            if (entidad.LISTA_PARAM_FACTOR.Count > 0) {
                                seGuardo = factorDA.DeleteFactorParametro(id, entidad.USUARIO_GUARDAR, cn);
                                if (seGuardo)
                                    foreach (FactorParametroBE fp in entidad.LISTA_PARAM_FACTOR)
                                        if (!(seGuardo = factorDA.GuardarFactorParametro(fp, id, cn).OK)) break;
                            }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public FactorBE ObtenerFactor(FactorBE entidad)
        {
            FactorBE item = new FactorBE();
            try
            {
                cn.Open();
                item = factorDA.ObtenerFactor(entidad.ID_FACTOR, cn);
                if (item != null)
                    item.LISTA_PARAM_FACTOR = factorDA.ListarFactorParametro(entidad.ID_FACTOR, cn);

            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return item;
        }
    }
}
