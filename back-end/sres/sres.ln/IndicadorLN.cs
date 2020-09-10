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
        CriterioDA criDA = new CriterioDA();
        ComponenteDA compDA = new ComponenteDA();

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

                    if (seGuardo)
                        seGuardo = indDA.GuardarFactores(entidad, cn);

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return seGuardo;
        }

        public IndicadorBE ObtenerIndicador(IndicadorBE entidad)
        {
            IndicadorBE item = new IndicadorBE();
            try
            {
                cn.Open();
                item.LISTA_PARAM = indDA.ListarParametro(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return item;
        }

        public List<ComponenteBE> ObtenerIndicadorForm(int idCriterio, int idCaso, int idComponente)
        {
            //List<CasoBE> lista = new List<CasoBE>();
            List<ComponenteBE> listaComponente = new List<ComponenteBE>();
            //ComponenteBE compo = new ComponenteBE{ ID_CRITERIO = idCriterio, ID_CASO = idCaso, ID_COMPONENTE = idComponente, INCREMENTABLE = "0"};
            ComponenteBE compo = new ComponenteBE();
            ParametroDetalleDA paramdetalleDA = new ParametroDetalleDA();
            try
            {
                cn.Open();
                compo = compDA.getComponente(new ComponenteBE { ID_CRITERIO = idCriterio, ID_CASO = idCaso, ID_COMPONENTE = idComponente }, cn);
                compo.LIST_INDICADOR_HEAD = criDA.ArmarIndicador(new ComponenteBE { ID_CRITERIO = idCriterio, ID_CASO = idCaso, ID_COMPONENTE = idComponente }, cn);
                foreach (var indicador in compo.LIST_INDICADOR_HEAD)
                {
                    indicador.OBJ_PARAMETRO = criDA.ObtenerParametro(indicador, cn);
                }

                compo.LIST_INDICADOR_BODY = criDA.ObtenerIndicador(new ComponenteBE { ID_CRITERIO = idCriterio, ID_CASO = idCaso, ID_COMPONENTE = idComponente }, cn);

                foreach (var ind in compo.LIST_INDICADOR_BODY)
                {
                    ind.FLAG_NUEVO = 0;
                    ind.LIST_INDICADORFORM = criDA.ArmarIndicadorForm(ind, cn);
                    if (ind.LIST_INDICADORFORM.Count > 0) foreach (var indForm in ind.LIST_INDICADORFORM) indForm.LIST_PARAMDET = indForm.ID_TIPO_CONTROL == 1 ? paramdetalleDA.ParametroDetalleForm(indForm, cn) : null;
                }
                listaComponente.Add(compo);
            }            
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return listaComponente;
        }

        public bool GuardarIndicadorForm(CasoBE item)
        {
            bool seGuardo = true;
            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    foreach (var componente in item.LIST_COMPONENTE)
                    {
                        foreach (var indicador in componente.LIST_INDICADOR)
                        {
                            foreach (var indicador_form in indicador.LIST_INDICADORFORM)
                            {
                                if (!(seGuardo = indDA.GuardarIndicadorForm(indicador_form, item.USUARIO_GUARDAR, cn, ot).OK)) break;
                            }
                            if (!seGuardo) break;
                        }
                        if (!seGuardo) break;
                    }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return seGuardo;
        }
    }
}
