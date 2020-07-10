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
    public class CriterioLN : BaseLN
    {
        CriterioDA criterioDA = new CriterioDA();
        CasoDA casoDA = new CasoDA();
        ComponenteDA componenteDA = new ComponenteDA();

        //public CriterioBE RegistroCriterio(CriterioBE entidad)
        //{
        //    CriterioBE item = null;

        //    try
        //    {
        //        cn.Open();
        //        item = criterioDA.RegistroCriterio(entidad, cn);
        //    }
        //    finally { if (cn.State == ConnectionState.Open) cn.Close(); }

        //    return item;
        //}

        public CriterioBE GuardarCriterio(CriterioBE entidad)
        {
            CriterioBE item = null;

            try
            {
                cn.Open();
                item = criterioDA.GuardarCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public CriterioBE EliminarCriterio(CriterioBE entidad)
        {
            CriterioBE item = null;

            try
            {
                cn.Open();
                item = criterioDA.EliminarCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public CriterioBE getCriterio(CriterioBE entidad)
        {
            CriterioBE item = null;

            try
            {
                cn.Open();
                item = criterioDA.getCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<CriterioBE> ListaBusquedaCriterio(CriterioBE entidad)
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                cn.Open();
                lista = criterioDA.ListarBusquedaCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<CriterioBE> getAllCriterio()
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                cn.Open();
                lista = criterioDA.getAllCriterio(cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        //public List<CasoBE> BuscarCriterioCaso(CasoBE entidad)
        public List<CasoBE> BuscarCriterioCaso(int idCriterio, int idInscripcion)
        {
            List<CasoBE> lista = new List<CasoBE>();
            try
            {
                cn.Open();
                lista = casoDA.ListarCasoPorCriterio(idCriterio, cn);

                foreach (var caso in lista)
                {
                    caso.LIST_COMPONENTE = criterioDA.BuscarCriterioCaso(idCriterio, caso.ID_CASO, cn);

                    foreach (var componente in caso.LIST_COMPONENTE)
                    {
                        componente.LIST_INDICADOR_HEAD = criterioDA.ArmarIndicador(componente, cn);
                        foreach (var indicador in componente.LIST_INDICADOR_HEAD)
                        {
                            indicador.OBJ_PARAMETRO = criterioDA.ObtenerParametro(indicador, cn);
                        }

                        componente.ID_INSCRIPCION = idInscripcion;
                        var flag_n = criterioDA.VerificarIndicador(componente, cn);

                        componente.LIST_INDICADOR_BODY = criterioDA.ObtenerIndicador(componente, cn);
                        foreach (var ind in componente.LIST_INDICADOR_BODY)
                        {
                            ind.FLAG_NUEVO = flag_n;
                            if (flag_n == 0)
                            {
                                ind.LIST_INDICADORFORM = criterioDA.ArmarIndicadorForm(ind, cn);
                            }
                            else
                            {
                                ind.ID_INSCRIPCION = idInscripcion;
                                ind.LIST_INDICADORDATA = criterioDA.ArmarIndicadorData(ind, cn);
                            }
                        }

                    }
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public CasoBE GuardarCriterioCaso(CasoBE item)
        {

            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    bool seGuardoConvocatoria = true;

                    foreach (var componente in item.LIST_COMPONENTE)
                    {
                        foreach (var indicador in componente.LIST_INDICADOR)
                        {
                            var id_indicador = indicador.ID_INDICADOR == 0 ? criterioDA.ObtenerIdIndicador(componente, cn, ot) : indicador.ID_INDICADOR;
                            foreach (var indicador_data in indicador.LIST_INDICADORDATA)
                            {
                                indicador_data.ID_INDICADOR = id_indicador;
                                if (!(seGuardoConvocatoria = criterioDA.GuardarIndicadorData(indicador_data, item.USUARIO_GUARDAR, cn, ot).OK)) break;
                            }
                            if (!seGuardoConvocatoria) break;
                        }
                        if (!seGuardoConvocatoria) break;
                    }


                    if (seGuardoConvocatoria) ot.Commit();
                    else ot.Rollback();
                    item.OK = seGuardoConvocatoria;
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<CriterioBE> ListarCriterioPorConvocatoria(int idConvocatoria)
        {
            List<CriterioBE> lista = null;

            try
            {
                cn.Open();
                lista = criterioDA.ListarCriterioPorConvocatoria(idConvocatoria, cn);
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public CriterioBE ObtenerCriterioPorConvocatoria(int idConvocatoria, int idCriterio)
        {
            CriterioBE item = null;

            try
            {
                cn.Open();
                item = criterioDA.ObtenerCriterioPorConvocatoria(idConvocatoria, idCriterio, cn);
                //item.LISTA_CASO = casoDA.ListarCasoPorCriterio(idCriterio, cn);
                //if(item.LISTA_CASO != null)
                //{
                //    item.LISTA_CASO.ForEach(x =>
                //    {
                //        x.LIST_COMPONENTE = componenteDA.ListarComponentePorCaso(x.ID_CASO, cn);
                //    });
                //}
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }
    }
}
