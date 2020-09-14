using Oracle.DataAccess.Client;
using sres.be;
using sres.da;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class InscripcionLN : BaseLN
    {
        InscripcionDA inscripcionDA = new InscripcionDA();
        InscripcionRequerimientoDA inscripcionRequerimientoDA = new InscripcionRequerimientoDA();
        InstitucionDA institucionDA = new InstitucionDA();
        InscripcionTrazabilidadDA inscripcionTrazabilidadDA = new InscripcionTrazabilidadDA();
        ConvocatoriaDA convocatoriaDA = new ConvocatoriaDA();
        PremiacionDA premDA = new PremiacionDA();

        public InscripcionBE ObtenerInscripcionPorConvocatoriaInstitucion(int idConvocatoria, int idInstitucion)
        {
            InscripcionBE item = null;

            try
            {
                cn.Open();
                item = inscripcionDA.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, idInstitucion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public bool GuardarInscripcion(InscripcionBE inscripcion, out int outIdInscripcion, UsuarioBE usuario = null, InstitucionBE institucion = null)
        {
            outIdInscripcion = 0;
            bool seGuardo = false;

            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction())
                {
                    seGuardo = inscripcionDA.GuardarInscripcion(inscripcion, out outIdInscripcion, cn);

                    if (seGuardo)
                    {
                        string trazabilidadDescripcionRegistrarInscripcion = AppSettings.Get<string>("Trazabilidad.Convocatoria.RegistrarInscripcion");

                        if (!string.IsNullOrEmpty(trazabilidadDescripcionRegistrarInscripcion))
                        {
                            if (institucion == null && inscripcion.ID_INSTITUCION != null) institucion = institucionDA.ObtenerInstitucion(inscripcion.ID_INSTITUCION.Value, cn);

                            Dictionary<string, object> dataTrazabilidad = new Dictionary<string, object>
                            {
                                ["INSTITUCION"] = institucion
                            };

                            Dictionary<string, object> claves = trazabilidadDescripcionRegistrarInscripcion.ObtenerListaClave('{', '}');

                            foreach (KeyValuePair<string, object> item in claves)
                            {
                                string valor = ((object)dataTrazabilidad).ObtenerValorDesdeClave(item.Value.ToString()).ToString();
                                trazabilidadDescripcionRegistrarInscripcion = trazabilidadDescripcionRegistrarInscripcion.Replace(item.Key, valor);
                            }

                            string descripcion = inscripcion.ID_ETAPA == 2 ? trazabilidadDescripcionRegistrarInscripcion : AppSettings.Get<string>("Trazabilidad.Convocatoria.DocumentosSolicitados");
                            InscripcionTrazabilidadBE inscripcionTrazabilidad = new InscripcionTrazabilidadBE
                            {
                                ID_INSCRIPCION = outIdInscripcion,
                                DESCRIPCION = descripcion,
                                ID_ETAPA = inscripcion.ID_ETAPA,
                                UPD_USUARIO = inscripcion.UPD_USUARIO
                            };

                            seGuardo = inscripcionTrazabilidadDA.RegistrarInscripcionTrazabilidad(inscripcionTrazabilidad, cn);
                            if (seGuardo)
                                seGuardo = convocatoriaDA.GuardarConvocatoriaEtapaInscripcion(new ConvocatoriaEtapaInscripcionBE { ID_CONVOCATORIA = Convert.ToInt16(inscripcion.ID_CONVOCATORIA), ID_ETAPA = inscripcion.ID_ETAPA, ID_INSCRIPCION = outIdInscripcion }, cn);
                        }

                        if (inscripcion.LISTA_INSCRIPCION_REQUERIMIENTO != null)
                        {
                            foreach (InscripcionRequerimientoBE iInscripcionRequerimiento in inscripcion.LISTA_INSCRIPCION_REQUERIMIENTO)
                            {
                                //if (iInscripcionRequerimiento.ID_INSCRIPCION <= 0)
                                iInscripcionRequerimiento.ID_INSCRIPCION = outIdInscripcion;
                                if (seGuardo)
                                {

                                    if(iInscripcionRequerimiento.ARCHIVO_CONTENIDO != null)
                                    {
                                        string pathFormat = AppSettings.Get<string>("Path.Inscripcion.Requerimiento");
                                        string pathDirectoryRelative = string.Format(pathFormat, inscripcion.ID_INSTITUCION, outIdInscripcion);
                                        string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                                        string pathFile = Path.Combine(pathDirectory, iInscripcionRequerimiento.ARCHIVO_BASE);
                                        if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                                        File.WriteAllBytes(pathFile, iInscripcionRequerimiento.ARCHIVO_CONTENIDO);
                                    }
                                    seGuardo = inscripcionRequerimientoDA.GuardarInscripcionRequerimiento(iInscripcionRequerimiento, cn);
                                }
                                else break;
                            }
                        }                        
                    }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public bool GuardarEvaluacionInscripcion(InscripcionBE inscripcion)
        {
            bool seGuardo = false;

            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction())
                {
                    seGuardo = inscripcionDA.GuardarEvaluacionInscripcion(inscripcion, cn);
                    if (seGuardo)
                        seGuardo = convocatoriaDA.GuardarConvocatoriaEtapaInscripcion(new ConvocatoriaEtapaInscripcionBE { ID_CONVOCATORIA = Convert.ToInt16(inscripcion.ID_CONVOCATORIA), ID_ETAPA = inscripcion.ID_ETAPA, ID_INSCRIPCION = inscripcion.ID_INSCRIPCION, OBSERVACION = inscripcion.OBSERVACION, ID_TIPO_EVALUACION = Convert.ToInt16(inscripcion.ID_TIPO_EVALUACION) }, cn);

                    if (seGuardo)
                    {
                        if (inscripcion.LISTA_INSCRIPCION_REQUERIMIENTO != null)
                        {
                            foreach (InscripcionRequerimientoBE iInscripcionRequerimiento in inscripcion.LISTA_INSCRIPCION_REQUERIMIENTO)
                            {
                                if (seGuardo)
                                {
                                    seGuardo = inscripcionRequerimientoDA.ActualizarEvaluacionInscripcionRequerimiento(iInscripcionRequerimiento, cn);                                    
                                }
                                else break;
                            }
                        }
                    }

                    if (seGuardo)
                    {
                        string descripcion = inscripcion.ID_ETAPA == 3 ? AppSettings.Get<string>("Trazabilidad.Convocatoria.EvaluarInscripcion") : AppSettings.Get<string>("Trazabilidad.Convocatoria.FiltradoParticipantes");
                        InscripcionTrazabilidadBE inscripcionTrazabilidad = new InscripcionTrazabilidadBE
                        {
                            ID_INSCRIPCION = inscripcion.ID_INSCRIPCION,
                            DESCRIPCION = descripcion,
                            ID_ETAPA = inscripcion.ID_ETAPA,
                            UPD_USUARIO = inscripcion.USUARIO_GUARDAR
                        };
                        seGuardo = inscripcionTrazabilidadDA.RegistrarInscripcionTrazabilidad(inscripcionTrazabilidad, cn);
                    }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public List<InscripcionBE> BuscarInscripcion(int idConvocatoria, int? idInscripcion, string razonSocialInstitucion, string nombresCompletosUsuario, int? idUsuario, int registros, int pagina, string columna, string orden)
        {
            List<InscripcionBE> lista = new List<InscripcionBE>();

            try
            {
                cn.Open();
                lista = inscripcionDA.BuscarInscripcion(idConvocatoria, idInscripcion, razonSocialInstitucion, nombresCompletosUsuario, idUsuario, registros, pagina, columna, orden, cn);
                if (lista.Count > 0)
                    foreach (InscripcionBE ins in lista)
                        ins.ARCHIVO_BASE = premDA.getPremiacion(new PremiacionBE { ID_PREMIACION = ins.ASPIRACION }, cn).ARCHIVO_BASE;
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public bool AnularInscripcion(InscripcionBE inscripcion)
        {
            bool seGuardo = false;

            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction())
                {
                    seGuardo = inscripcionDA.GuardarEvaluacionInscripcion(inscripcion, cn);
                    if (seGuardo)
                        seGuardo = convocatoriaDA.GuardarConvocatoriaEtapaInscripcion(new ConvocatoriaEtapaInscripcionBE { ID_CONVOCATORIA = Convert.ToInt16(inscripcion.ID_CONVOCATORIA), ID_ETAPA = inscripcion.ID_ETAPA, ID_INSCRIPCION = inscripcion.ID_INSCRIPCION, OBSERVACION = inscripcion.OBSERVACION, ID_TIPO_EVALUACION = Convert.ToInt16(inscripcion.ID_TIPO_EVALUACION) }, cn);

                    if (seGuardo)
                    {
                        if (inscripcion.LISTA_INSCRIPCION_REQUERIMIENTO != null)
                        {
                            foreach (InscripcionRequerimientoBE iInscripcionRequerimiento in inscripcion.LISTA_INSCRIPCION_REQUERIMIENTO)
                            {
                                if (seGuardo)
                                {
                                    seGuardo = inscripcionRequerimientoDA.ActualizarEvaluacionInscripcionRequerimiento(iInscripcionRequerimiento, cn);
                                }
                                else break;
                            }
                        }
                    }

                    if (seGuardo) seGuardo = inscripcionDA.AnularInscripcion(inscripcion, cn);
                    //if (seGuardo)
                    //    seGuardo = convocatoriaDA.GuardarConvocatoriaEtapaInscripcion(new ConvocatoriaEtapaInscripcionBE { ID_CONVOCATORIA = Convert.ToInt16(inscripcion.ID_CONVOCATORIA), ID_ETAPA = inscripcion.ID_ETAPA, ID_INSCRIPCION = inscripcion.ID_INSCRIPCION, OBSERVACION = inscripcion.OBSERVACION, ID_TIPO_EVALUACION = Convert.ToInt16(inscripcion.ID_TIPO_EVALUACION) }, cn);

                    if (seGuardo) seGuardo = convocatoriaDA.GuardarResultadoReconocimiento(new ReconocimientoBE { ID_INSCRIPCION = inscripcion.ID_INSCRIPCION, ID_INSIGNIA = 1, PUNTAJE = 0, ID_ESTRELLA = 1, EMISIONES = 0, FLAG_MEJORACONTINUA = "0", USUARIO_GUARDAR = inscripcion.USUARIO_GUARDAR }, cn);
                    if (seGuardo)
                    {
                        string descripcion = AppSettings.Get<string>("Trazabilidad.Convocatoria.AnulacionParticipante");
                        InscripcionTrazabilidadBE inscripcionTrazabilidad = new InscripcionTrazabilidadBE
                        {
                            ID_INSCRIPCION = inscripcion.ID_INSCRIPCION,
                            DESCRIPCION = descripcion,
                            ID_ETAPA = inscripcion.ID_ETAPA,
                            UPD_USUARIO = inscripcion.USUARIO_GUARDAR
                        };
                        seGuardo = inscripcionTrazabilidadDA.RegistrarInscripcionTrazabilidad(inscripcionTrazabilidad, cn);
                    }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public InscripcionBE ObtenerInscripcionPorId(int idInscripcion) {
            InscripcionBE ins = new InscripcionBE();
            try
            {
                cn.Open();
                ins = inscripcionDA.ObtenerInscripcionPorId(idInscripcion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return ins;
        }

        public InstitucionBE InscripcionTrazabilidad(int idInscripcion) {
            InstitucionBE inst = null;
            try
            {
                cn.Open();
                inst = inscripcionDA.ObtenerInstitucionPorInscripcion(idInscripcion, cn);
                if (inst != null) inst.LISTA_INSC_TRAZ = inscripcionDA.ObtenerInscripcionTrazabilidad(idInscripcion, cn);                              
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return inst;
        }
    }
}