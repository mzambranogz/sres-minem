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
    public class ParametroLN : BaseLN
    {
        ParametroDA paramDA = new ParametroDA();
        public List<ParametroBE> ListaBusquedaParametro(ParametroBE entidad)
        {
            List<ParametroBE> lista = new List<ParametroBE>();
            try
            {
                cn.Open();
                lista = paramDA.ListarBusquedaParametro(entidad, cn);
                if (lista.Count > 0)
                    foreach (ParametroBE p in lista)
                        if (p.ID_TIPO_CONTROL == 1)
                            p.LISTA_DET = paramDA.ListarDetalleParametro(p.ID_PARAMETRO, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return lista;
        }

        public bool GuardarParametro(ParametroBE entidad)
        {
            bool seGuardo = true;
            try
            {
                int id = -1;
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    if (!string.IsNullOrEmpty(entidad.ID_DELETE_DETALLE))
                        if (!paramDA.EliminarDetalle(entidad, cn).OK) seGuardo = false;

                    if (seGuardo) 
                        if (seGuardo = paramDA.RegistroParametro(entidad, out id, cn).OK) {
                            foreach (ParametroDetalleBE pd in entidad.LISTA_DET)
                                if (!(seGuardo = paramDA.RegistroParametroDetalle(pd, id, cn))) break;
                        }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public ParametroBE ObtenerParametro(ParametroBE entidad)
        {
            ParametroBE item = new ParametroBE();
            try
            {
                cn.Open();
                item = paramDA.ObtenerParametro(entidad.ID_PARAMETRO, cn);
                if (item != null)
                    if (item.ID_TIPO_CONTROL == 1) {
                        item.LISTA_DET = paramDA.ListarDetalleParametro(entidad.ID_PARAMETRO, cn);
                        if (item.FILTRO != "0" && !(string.IsNullOrEmpty(item.FILTRO))) {
                            string filtro = item.FILTRO.Replace('|',',');
                            item.LISTA_PARAM = paramDA.ObtenerParametroFiltro(filtro, cn);
                        }                            
                    }
                        
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return item;
        }

        public List<ParametroBE> ObtenerParametroLista()
        {
            List<ParametroBE> lista = new List<ParametroBE>();
            try
            {
                cn.Open();
                lista = paramDA.ObtenerParametroLista(cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return lista;
        }
    }
}
