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
        FactorValorDA facDA = new FactorValorDA();
        FactorDA factorDA = new FactorDA();
        ParametroDetalleDA paramDetDA = new ParametroDetalleDA();
        public List<ComponenteBE> ListaBusquedaComponenteFactor(ComponenteBE entidad)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();
            try
            {
                cn.Open();
                lista = facDA.ListarBusquedaComponenteFactor(entidad, cn);
                if (lista.Count > 0)
                    foreach (ComponenteBE c in lista)
                        if (c.ID_FACTORES != "")
                            c.LISTA_FAC_cOMP = factorDA.listaFactorComponente(c.ID_FACTORES.Replace("|", ","), cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public FactorBE getFactorValor(FactorBE entidad)
        {
            FactorBE fac = new FactorBE();
            List<FactorDataBE> lista = new List<FactorDataBE>();
            List<FactorParametroBE> listaFP = new List<FactorParametroBE>();
            try
            {
                cn.Open();
                listaFP = factorDA.ListaFactorParametro(entidad, cn);
                if (listaFP.Count > 0)
                    foreach (FactorParametroBE p in listaFP)
                        p.LIST_PARAMDET = paramDetDA.ParametroDetalleData(new IndicadorDataBE{ ID_PARAMETRO = p.ID_PARAMETRO}, cn);

                lista = facDA.getFactorValor(entidad, cn);
                fac.LISTA_PARAM_FACTOR = listaFP;
                fac.LISTA_FAC_DATA = lista;

            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return fac;
        }

        public bool GuardarFactorValor(FactorBE entidad)
        {
            bool seGuardo = false;
            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    seGuardo = facDA.DeleteFactorData(entidad.LISTA_FAC_DATA[0].ID_FACTOR, entidad.USUARIO_GUARDAR, cn);
                    if (seGuardo)
                        if (entidad.LISTA_FAC_DATA.Count > 0)
                            foreach (FactorDataBE fd in entidad.LISTA_FAC_DATA)
                                if (!(seGuardo = facDA.GuardarFactorValor(fd, cn).OK)) break;

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }                
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }
    }
}
