using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;
using sres.ut;
using Oracle.DataAccess.Client;

namespace sres.ln
{
    public class ConvocatoriaLN : BaseLN
    {

        ConvocatoriaDA convocatoriaDA = new ConvocatoriaDA();
        RequerimientoDA requerimientoDA = new RequerimientoDA();
        ConvocatoriaCriterioRequerimientoDA convocatoriaCriterioRequerimientoDA = new ConvocatoriaCriterioRequerimientoDA();
        CriterioDA criterioDA = new CriterioDA();
        CasoDA casoDA = new CasoDA();
        DocumentoDA documentoDA = new DocumentoDA();
        ConvocatoriaCriterioPuntajeDA convcripuntDA = new ConvocatoriaCriterioPuntajeDA();

        public List<ConvocatoriaBE> BuscarConvocatoria(string nroInforme, string nombre, DateTime? fechaDesde, DateTime? fechaHasta, int registros, int pagina, string columna, string orden)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                cn.Open();
                lista = convocatoriaDA.BuscarConvocatoria(nroInforme, nombre, fechaDesde, fechaHasta, registros, pagina, columna, orden, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public ConvocatoriaBE ObtenerConvocatoria(int idConvocatoria)
        {
            ConvocatoriaBE item = null;

            try
            {
                cn.Open();
                item = convocatoriaDA.ObtenerConvocatoria(idConvocatoria, cn);
                //item.LISTA_REQ = requerimientoDA.ListarRequerimientoPorConvocatoria(idConvocatoria, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public ConvocatoriaBE ObtenerUltimaConvocatoria()
        {
            ConvocatoriaBE item = null;

            try
            {
                cn.Open();
                item = convocatoriaDA.ObtenerUltimaConvocatoria(cn);
                //item.LISTA_REQ = requerimientoDA.ListarRequerimientoPorConvocatoria(idConvocatoria, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public ConvocatoriaBE RegistroConvocatoria(ConvocatoriaBE entidad)
        {
            ConvocatoriaBE item = new ConvocatoriaBE();

            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    int idConvocatoria = -1;
                    bool seGuardoConvocatoria = false;

                    seGuardoConvocatoria = convocatoriaDA.RegistroConvocatoria(entidad, out idConvocatoria, cn, ot).OK;

                    if (seGuardoConvocatoria)
                    {
                        if (entidad.LISTA_CONVOCATORIA_CRITERIO_REQUERIMIENTO != null)
                        {
                            foreach (ConvocatoriaCriterioRequerimientoBE iCCR in entidad.LISTA_CONVOCATORIA_CRITERIO_REQUERIMIENTO)
                            {
                                iCCR.ID_CONVOCATORIA = idConvocatoria;
                                seGuardoConvocatoria = convocatoriaCriterioRequerimientoDA.GuardarConvocatoriaCriterioRequerimiento(iCCR, cn);
                                if (!seGuardoConvocatoria) break;
                            }
                        }
                    }
                    if (seGuardoConvocatoria)
                    {
                        foreach (var it in entidad.LISTA_REQ)
                        {
                            if (!(seGuardoConvocatoria = convocatoriaDA.GuardarRequerimiento(new RequerimientoBE { ID_REQUERIMIENTO = it.ID_REQUERIMIENTO, FLAG_ESTADO = it.FLAG_ESTADO, USUARIO_GUARDAR = entidad.USUARIO_GUARDAR }, idConvocatoria, cn, ot).OK)) break;
                        }
                    }
                    if (seGuardoConvocatoria)
                    {
                        foreach (CriterioBE it in entidad.LISTA_CRI)
                        {
                            if (!(seGuardoConvocatoria = convocatoriaDA.GuardarCriterio(new CriterioBE { ID_CRITERIO = it.ID_CRITERIO, FLAG_ESTADO = it.FLAG_ESTADO, USUARIO_GUARDAR = entidad.USUARIO_GUARDAR }, idConvocatoria, cn, ot).OK)) break;
                            foreach (CasoBE c in it.LISTA_CASO)
                            {
                                if (!(seGuardoConvocatoria = casoDA.GuardarConvocatoriaCriterioCaso(c, idConvocatoria, cn).OK)) break;
                                foreach (DocumentoBE d in c.LIST_DOC)
                                {
                                    if (!(seGuardoConvocatoria = documentoDA.GuardarConvocatoriaCriterioCasoDoc(d, idConvocatoria, cn).OK)) break;
                                }
                                if (!seGuardoConvocatoria) break;
                            }
                            if (!seGuardoConvocatoria) break;

                            foreach (ConvocatoriaCriterioPuntajeBE p in it.LISTA_CONVCRIPUNT)
                            {
                                if (!(seGuardoConvocatoria = convcripuntDA.GuardarConvocatoriaCriterioPuntaje(p, idConvocatoria, cn).OK)) break;
                            }
                            if (!seGuardoConvocatoria) break;
                        }
                    }
                    if (seGuardoConvocatoria)
                    {
                        foreach (var it in entidad.LISTA_EVA)
                        {
                            if (!(seGuardoConvocatoria = convocatoriaDA.GuardarEvaluador(new UsuarioBE { ID_USUARIO = it.ID_USUARIO, FLAG_ESTADO = it.FLAG_ESTADO, USUARIO_GUARDAR = entidad.USUARIO_GUARDAR }, idConvocatoria, cn, ot).OK)) break;
                        }
                    }
                    if (seGuardoConvocatoria)
                    {
                        foreach (var it in entidad.LISTA_ETA)
                        {
                            if (!(seGuardoConvocatoria = convocatoriaDA.GuardarEtapa(new EtapaBE { ID_ETAPA = it.ID_ETAPA, DIAS = it.DIAS, USUARIO_GUARDAR = entidad.USUARIO_GUARDAR }, idConvocatoria, cn, ot).OK)) break;
                        }
                    }

                    if (seGuardoConvocatoria) ot.Commit();
                    else ot.Rollback();
                    item.OK = seGuardoConvocatoria;
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<ConvocatoriaBE> ListarBusquedaConvocatoria(ConvocatoriaBE entidad)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                cn.Open();
                lista = convocatoriaDA.ListarBusquedaConvocatoria(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public ConvocatoriaBE getConvocatoria(ConvocatoriaBE entidad)
        {
            ConvocatoriaBE item = null;

            try
            {
                cn.Open();
                item = convocatoriaDA.getConvocatoria(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public ConvocatoriaBE EliminarConvocatoria(ConvocatoriaBE entidad)
        {
            ConvocatoriaBE item = null;

            try
            {
                cn.Open();
                item = convocatoriaDA.EliminarConvocatoria(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<ConvocatoriaBE> listarConvocatoriaReq(ConvocatoriaBE entidad)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                cn.Open();
                lista = convocatoriaDA.listarConvocatoriaReq(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<CriterioBE> listarConvocatoriaCri(ConvocatoriaBE entidad)
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                cn.Open();
                lista = convocatoriaDA.listarConvocatoriaCri(entidad, cn);
                if (lista.Count > 0)
                {
                    foreach (CriterioBE cr in lista)
                    {
                        cr.LISTA_CASO = casoDA.listarConvocatoriaCriCaso(cr, cn);
                        if (cr.LISTA_CASO.Count > 0)
                        {
                            foreach (CasoBE c in cr.LISTA_CASO)
                            {
                                c.LIST_DOC = documentoDA.listarConvocatoriaCriCasoDoc(c, cn);
                            }
                        }
                        cr.LISTA_CONVCRIPUNT = convcripuntDA.listarConvocatoriaCriterioPuntaje(cr.ID_CONVOCATORIA, cr.ID_CRITERIO, cn);
                    }
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ConvocatoriaBE> listarConvocatoriaEva(ConvocatoriaBE entidad)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                cn.Open();
                lista = convocatoriaDA.listarConvocatoriaEva(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ConvocatoriaBE> listarConvocatoriaEta(ConvocatoriaBE entidad)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                cn.Open();
                lista = convocatoriaDA.listarConvocatoriaEta(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<InstitucionBE> listarConvocatoriaPos(ConvocatoriaBE entidad)
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();
            try
            {
                cn.Open();
                lista = convocatoriaDA.listarConvocatoriaPos(entidad, cn);
                if (lista.Count > 0) 
                    foreach (InstitucionBE ins in lista) 
                        ins.CONV_EVA_POS = convocatoriaDA.ObtenerConvEvaluadorPostulante(ins, cn);   
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return lista;
        }

        public bool GuardarEvaluadorPostulante(ConvocatoriaBE entidad)
        {
            bool seGuardoConvocatoria = false;
            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {   
                    foreach (InstitucionBE i in entidad.LIST_INSTITUCION)
                        if (i.ID_USUARIO > 0)
                            if (!(seGuardoConvocatoria = convocatoriaDA.GuardarEvaluadorPostulante(i, cn))) break;

                    if (seGuardoConvocatoria) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return seGuardoConvocatoria;
        }

        public bool GuardarConvocatoriaEtapaInscripcion(ConvocatoriaEtapaInscripcionBE entidad)
        {
            bool seGuardoConvocatoria = false;
            try
            {
                cn.Open();
                seGuardoConvocatoria = convocatoriaDA.GuardarConvocatoriaEtapaInscripcion(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return seGuardoConvocatoria;
        }

    }
}