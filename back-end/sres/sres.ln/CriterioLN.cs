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
using System.IO;

namespace sres.ln
{
    public class CriterioLN : BaseLN
    {
        CriterioDA criterioDA = new CriterioDA();
        CasoDA casoDA = new CasoDA();
        ComponenteDA componenteDA = new ComponenteDA();
        IndicadorDataDA indicadorDataDA = new IndicadorDataDA();
        ParametroDetalleDA paramdetalleDA = new ParametroDetalleDA();
        DocumentoDA documentoDA = new DocumentoDA();
        InscripcionDocumentoDA inscripcionDocDA = new InscripcionDocumentoDA();
        ConvocatoriaCriterioPuntajeDA convcripuntDA = new ConvocatoriaCriterioPuntajeDA();
        InscripcionTrazabilidadDA inscripcionTrazabilidadDA = new InscripcionTrazabilidadDA();

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

        public bool GuardarCriterio(CriterioBE entidad)
        {
            //CriterioBE item = null;
            bool seGuardoConvocatoria = false;
            try
            {
                int id = -1;
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    seGuardoConvocatoria = criterioDA.GuardarCriterio(entidad, out id, cn).OK;
                    if (seGuardoConvocatoria) {
                        if (entidad.ARCHIVO_CONTENIDO != null && entidad.ARCHIVO_CONTENIDO.Length > 0)
                        {
                            string pathFormat = AppSettings.Get<string>("Path.Criterio");
                            string pathDirectoryRelative = string.Format(pathFormat, id);
                            string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                            string pathFile = Path.Combine(pathDirectory, entidad.ARCHIVO_BASE);
                            if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                            File.WriteAllBytes(pathFile, entidad.ARCHIVO_CONTENIDO);
                        }
                        else
                            seGuardoConvocatoria = false;
                    }
                    if (seGuardoConvocatoria) ot.Commit();
                    else ot.Rollback();
                }                     
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardoConvocatoria;
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
                string pathFormat = AppSettings.Get<string>("Path.Criterio");
                string pathDirectoryRelative = string.Format(pathFormat, item.ID_CRITERIO);
                string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                string pathFile = Path.Combine(pathDirectory, item.ARCHIVO_BASE);
                if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                pathFile = !File.Exists(pathFile) ? null : pathFile;
                item.ARCHIVO_CONTENIDO = pathFile == null ? null : File.ReadAllBytes(pathFile);
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
                if (lista != null)
                {
                    foreach (CriterioBE cri in lista)
                    {
                        cri.LISTA_CASO = casoDA.ObtenerCriterioCaso(cri, cn);
                        cri.LISTA_DOCUMENTO = documentoDA.ObtenerCriterioDocumento(cri, cn);
                        cri.LISTA_CONVCRIPUNT = convcripuntDA.ObtenerCriterioPuntaje(cri.ID_CRITERIO, cn);
                    }
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        //public List<CasoBE> BuscarCriterioCaso(CasoBE entidad)
        public List<ComponenteBE> BuscarCriterioCaso(int idCriterio, int idInscripcion, int idCaso)
        {
            //List<CasoBE> lista = new List<CasoBE>();
            List<ComponenteBE> listaComponente = new List<ComponenteBE>();
            //int id_caso = 0;
            try
            {
                cn.Open();
                listaComponente = criterioDA.BuscarCriterioCaso(idCriterio, idCaso, cn);

                foreach (var componente in listaComponente)
                {
                    componente.LIST_INDICADOR_HEAD = criterioDA.ArmarIndicador(componente, cn);
                    foreach (var indicador in componente.LIST_INDICADOR_HEAD)
                    {
                        indicador.OBJ_PARAMETRO = criterioDA.ObtenerParametro(indicador, cn);
                    }

                    componente.ID_INSCRIPCION = idInscripcion;
                    var flag_n = componente.INCREMENTABLE == "1" ? criterioDA.VerificarIndicador(componente, cn) : 0;

                    if (componente.INCREMENTABLE == "1" && flag_n > 0)
                        componente.LIST_INDICADOR_BODY = indicadorDataDA.ObtenerIndicadorData(componente, cn);
                    else
                        componente.LIST_INDICADOR_BODY = criterioDA.ObtenerIndicador(componente, cn);

                    foreach (var ind in componente.LIST_INDICADOR_BODY)
                    {
                        ind.FLAG_NUEVO = flag_n;
                        if (flag_n == 0)
                        {
                            ind.ID_INSCRIPCION = idInscripcion;
                            ind.LIST_INDICADORFORM = criterioDA.ArmarIndicadorForm(ind, cn);
                            if (ind.LIST_INDICADORFORM.Count > 0) foreach (var indForm in ind.LIST_INDICADORFORM) indForm.LIST_PARAMDET = indForm.ID_TIPO_CONTROL == 1 ? paramdetalleDA.ParametroDetalleForm(indForm, cn) : null;
                            ind.LIST_INDICADORDATA = componente.INCREMENTABLE == "0" ? criterioDA.ArmarIndicadorData(ind, cn) : null; //add
                            if (componente.INCREMENTABLE == "0" && ind.LIST_INDICADORDATA != null) foreach (var indData in ind.LIST_INDICADORDATA) indData.LIST_PARAMDET = indData.ID_TIPO_CONTROL == 1 ? paramdetalleDA.ParametroDetalleData(indData, cn) : null;
                        }
                        else
                        {
                            ind.ID_INSCRIPCION = idInscripcion;
                            ind.LIST_INDICADORDATA = criterioDA.ArmarIndicadorData(ind, cn);
                            if (ind.LIST_INDICADORDATA != null) foreach (var indData in ind.LIST_INDICADORDATA) indData.LIST_PARAMDET = indData.ID_TIPO_CONTROL == 1 ? paramdetalleDA.ParametroDetalleData(indData, cn) : null;
                        }
                    }

                }

                //}
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return listaComponente;
        }

        public List<DocumentoBE> BuscarCriterioCasoDocumento(int idCriterio, int idCaso, int idConvocatoria, int id_inscripcion)
        {
            List<DocumentoBE> lista = new List<DocumentoBE>();
            try
            {
                cn.Open();
                lista = documentoDA.BuscarCriterioCasoDocumento(new CasoBE { ID_CONVOCATORIA = idConvocatoria, ID_CRITERIO = idCriterio, ID_CASO = idCaso }, cn);
                if (lista != null)
                {
                    foreach (DocumentoBE item in lista)
                    {
                        item.ID_INSCRIPCION = id_inscripcion;
                        item.OBJ_INSCDOC = inscripcionDocDA.ObtenerInscripcionDocumento(item, cn);
                        if (item.OBJ_INSCDOC != null)
                        {
                            string pathFormat = AppSettings.Get<string>("Path.Inscripcion.Documento");
                            string pathDirectoryRelative = string.Format(pathFormat, id_inscripcion, idCriterio, idCaso, item.ID_DOCUMENTO);
                            string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                            string pathFile = Path.Combine(pathDirectory, item.OBJ_INSCDOC.ARCHIVO_BASE);
                            if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                            pathFile = !File.Exists(pathFile) ? null : pathFile;
                            item.OBJ_INSCDOC.ARCHIVO_CONTENIDO = pathFile == null ? null : File.ReadAllBytes(pathFile);
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

                    seGuardoConvocatoria = casoDA.GuardarConvocatoriaCriterioCasoInscripcion(item, cn).OK ? true : false;
                    if (seGuardoConvocatoria)
                    {
                        //==============================================================
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
                            if (!string.IsNullOrEmpty(componente.ELIMINAR_INDICADOR)) if (!(seGuardoConvocatoria = indicadorDataDA.EliminarIndicadorData(componente, cn, ot).OK)) break;
                        }
                        //==============================================================
                        if (item.LIST_DOCUMENTO != null)
                        {
                            foreach (InscripcionDocumentoBE iDoc in item.LIST_DOCUMENTO)
                            {
                                //if (iDoc.ID_INSCRIPCION <= 0) iInscripcionRequerimiento.ID_INSCRIPCION = idInscripcion;
                                if (seGuardoConvocatoria)
                                {
                                    if (iDoc.ARCHIVO_CONTENIDO != null && iDoc.ARCHIVO_CONTENIDO.Length > 0)
                                    {
                                        string pathFormat = AppSettings.Get<string>("Path.Inscripcion.Documento");
                                        string pathDirectoryRelative = string.Format(pathFormat, iDoc.ID_INSCRIPCION, iDoc.ID_CRITERIO, iDoc.ID_CASO, iDoc.ID_DOCUMENTO);
                                        string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                                        string pathFile = Path.Combine(pathDirectory, iDoc.ARCHIVO_BASE);
                                        if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                                        File.WriteAllBytes(pathFile, iDoc.ARCHIVO_CONTENIDO);
                                    }
                                    seGuardoConvocatoria = inscripcionDocDA.GuardarInscripcionDocumento(iDoc, cn).OK;

                                }
                                else break;
                            }
                        }
                        //=======================================================================
                    }

                    if (seGuardoConvocatoria) {
                        string registrocriteriodescripcion = AppSettings.Get<string>("Trazabilidad.Convocatoria.RegistrarCriterio");
                        registrocriteriodescripcion = registrocriteriodescripcion.Replace("{CRITERIO.NOMBRE}", item.NOMBRE_CRI);
                        InscripcionTrazabilidadBE inscripcionTrazabilidad = new InscripcionTrazabilidadBE
                        {
                            ID_INSCRIPCION = item.ID_INSCRIPCION,
                            DESCRIPCION = registrocriteriodescripcion,
                            ID_ETAPA = item.ID_ETAPA,
                            UPD_USUARIO = item.USUARIO_GUARDAR
                        };

                        seGuardoConvocatoria = inscripcionTrazabilidadDA.RegistrarInscripcionTrazabilidad(inscripcionTrazabilidad, cn);
                    }

                    if (seGuardoConvocatoria) ot.Commit();
                    else ot.Rollback();
                    item.OK = seGuardoConvocatoria;
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public ComponenteBE FilaCriterioCaso(int idCriterio, int idCaso, int idComponente)
        {
            ComponenteBE comp = new ComponenteBE();
            comp.LIST_INDICADOR_BODY = criterioDA.ObtenerIndicador(new ComponenteBE { ID_CRITERIO = idCriterio, ID_CASO = idCaso, ID_COMPONENTE = idComponente }, cn);
            foreach (var ind in comp.LIST_INDICADOR_BODY)
            {
                ind.LIST_INDICADORFORM = criterioDA.ArmarIndicadorForm(ind, cn);
                foreach (var indForm in ind.LIST_INDICADORFORM) indForm.LIST_PARAMDET = indForm.ID_TIPO_CONTROL == 1 ? paramdetalleDA.ParametroDetalleForm(indForm, cn) : null;
            }
            return comp;
        }

        public List<CriterioBE> ListarCriterioPorConvocatoria(int idConvocatoria, int idInscripcion)
        {
            List<CriterioBE> lista = null;

            try
            {
                cn.Open();
                lista = criterioDA.ListarCriterioPorConvocatoria(idConvocatoria, idInscripcion, cn);
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

        public ConvocatoriaCriterioPuntajeInscripBE ObtenerConvCriPuntInscrip(int idConvocatoria, int idCriterio, int idInscripcion)
        {
            return criterioDA.ObtenerConvCriPuntajeInsc(idConvocatoria, idCriterio, idInscripcion, cn);
        }

        public bool GuardarEvaluacionCriterio(ConvocatoriaCriterioPuntajeInscripBE entidad)
        {
            bool seGuardoConvocatoria = false;
            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    seGuardoConvocatoria = criterioDA.GuardarEvaluacionCriterio(entidad, cn).OK;
                    if (seGuardoConvocatoria)
                        foreach (InscripcionDocumentoBE i in entidad.LIST_INSCDOC)
                            if (!(seGuardoConvocatoria = inscripcionDocDA.GuardarCriterioEvaluacion(i, cn))) break;

                    if (seGuardoConvocatoria)
                    {
                        string evaluacioncriteriodescripcion = AppSettings.Get<string>("Trazabilidad.Convocatoria.EvaluarCriterio");
                        evaluacioncriteriodescripcion = evaluacioncriteriodescripcion.Replace("{CRITERIO.NOMBRE}", entidad.NOMBRE_CRI);
                        InscripcionTrazabilidadBE inscripcionTrazabilidad = new InscripcionTrazabilidadBE
                        {
                            ID_INSCRIPCION = entidad.ID_INSCRIPCION,
                            DESCRIPCION = evaluacioncriteriodescripcion,
                            ID_ETAPA = entidad.ID_ETAPA,
                            UPD_USUARIO = entidad.USUARIO_GUARDAR
                        };

                        seGuardoConvocatoria = inscripcionTrazabilidadDA.RegistrarInscripcionTrazabilidad(inscripcionTrazabilidad, cn);
                    }

                    if (seGuardoConvocatoria) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return seGuardoConvocatoria;
        }

        public bool GuardarEvaluacionCriterioInscripcion(ConvocatoriaCriterioPuntajeInscripBE entidad)
        {
            return criterioDA.GuardarEvaluacionCriterioInscripcion(entidad, cn).OK;
        }

        public string ObtenerRutaDocumento(int idConvocatoria, int idCriterio, int idCaso, int idInscripcion, int idDocumento)
        {
            InscripcionDocumentoBE item = null;
            try
            {
                cn.Open();
                item = inscripcionDocDA.ObtenerInscripcionDocumento(new DocumentoBE { ID_CONVOCATORIA = idConvocatoria, ID_CRITERIO = idCriterio, ID_CASO = idCaso, ID_INSCRIPCION = idInscripcion, ID_DOCUMENTO = idDocumento }, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            if (item == null) return null;

            string pathFormat = AppSettings.Get<string>("Path.Inscripcion.Documento");
            string pathDirectoryRelative = string.Format(pathFormat, idInscripcion, idCriterio, idCaso, idDocumento);
            string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
            string pathFile = Path.Combine(pathDirectory, item.ARCHIVO_BASE);
            if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
            pathFile = !File.Exists(pathFile) ? null : pathFile;
            return pathFile;
        }
    }
}
