using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Oracle.DataAccess.Client;
using System.Data;
using System.Web.Configuration;
using sres.be;
using sres.ut;

namespace sres.da
{
    public class ConvocatoriaDA : BaseDA
    {
        public List<ConvocatoriaBE> BuscarConvocatoria(string nroInforme, string nombre, DateTime? fechaDesde, DateTime? fechaHasta, int registros, int pagina, string columna, string orden, int idInstitucion, int idUsuario, OracleConnection db)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_BUSQ_CONVOCATORIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_NRO_INFORME", nroInforme);
                p.Add("PI_NOMBRE", nombre);
                p.Add("PI_FECHA_INICIO", fechaDesde);
                p.Add("PI_FECHA_FIN", fechaHasta);
                p.Add("PI_REGISTROS", registros);
                p.Add("PI_PAGINA", pagina);
                p.Add("PI_COLUMNA", columna);
                p.Add("PI_ORDEN", orden);
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PI_ID_USUARIO", idUsuario);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure).Select(x => new ConvocatoriaBE
                {
                    ID_CONVOCATORIA = (int)x.ID_CONVOCATORIA,
                    NOMBRE = (string)x.NOMBRE,
                    FECHA_INICIO = (DateTime)x.FECHA_INICIO,
                    FECHA_FIN = (DateTime)x.FECHA_FIN,
                    LIMITE_POSTULANTE = (int)x.LIMITE_POSTULANTE,
                    NRO_INFORME = (string)x.NRO_INFORME,
                    ID_ETAPA = (int?)x.ID_ETAPA,
                    ETAPA = x.ID_ETAPA == null ? null : new EtapaBE { ID_ETAPA = (int)x.ID_ETAPA, NOMBRE = (string)x.NOMBRE_ETAPA },
                    FLAG_ESTADO = (string)x.FLAG_ESTADO,
                    ROWNUMBER = (int)x.ROWNUMBER,
                    TOTAL_PAGINAS = (int)x.TOTAL_PAGINAS,
                    PAGINA = (int)x.PAGINA,
                    CANTIDAD_REGISTROS = (int)x.CANTIDAD_REGISTROS,
                    TOTAL_REGISTROS = (int)x.TOTAL_REGISTROS,
                    VALIDAR_EVALUADOR = (int)x.VALIDAR_EVALUADOR,
                    FLAG_ANULAR = idInstitucion == 0 ? 0 : (int)x.FLAG_ANULAR
                }).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public ConvocatoriaBE ObtenerConvocatoria(int idConvocatoria, OracleConnection db)
        {
            ConvocatoriaBE item = null;

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_OBTIENE_CONVOCATORIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<ConvocatoriaBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public ConvocatoriaBE ObtenerUltimaConvocatoria(OracleConnection db)
        {
            ConvocatoriaBE item = null;

            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_OBTIENE_ULT_CONV";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<ConvocatoriaBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public ConvocatoriaBE RegistroConvocatoria(ConvocatoriaBE entidad, out int idConvocatoria, OracleConnection db, OracleTransaction ot = null)
        {
            idConvocatoria = -1;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_CONVOCATORIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_GET", 0, OracleDbType.Int32, ParameterDirection.Output);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                p.Add("PI_DESCRIPCION", entidad.DESCRIPCION);
                p.Add("PI_FECHA_INICIO", entidad.FECHA_INICIO);
                p.Add("PI_FECHA_FIN", entidad.FECHA_FIN);
                p.Add("PI_LIMITE_POSTULANTE", entidad.LIMITE_POSTULANTE);
                p.Add("PI_ID_ETAPA", entidad.ID_ETAPA);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                idConvocatoria = (int)p.Get<dynamic>("PI_ID_GET").Value;
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && idConvocatoria != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public List<ConvocatoriaBE> ListarBusquedaConvocatoria(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_CONVOCAT";
                var p = new OracleDynamicParameters();
                //p.Add("PI_BUSCAR", entidad.BUSCAR);
                //p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                //p.Add("PI_PAGINA", entidad.PAGINA);
                //p.Add("PI_COLUMNA", entidad.ORDER_BY);
                //p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PI_CODIGO", entidad.CODIGO);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                p.Add("PI_FECHA_INICIO", entidad.FECHA_DESDE);
                p.Add("PI_FECHA_FIN", entidad.FECHA_HASTA);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ConvocatoriaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public ConvocatoriaBE getConvocatoria(ConvocatoriaBE entidad, OracleConnection db)
        {
            ConvocatoriaBE item = new ConvocatoriaBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_CONVOCATORIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<ConvocatoriaBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public ConvocatoriaBE EliminarConvocatoria(ConvocatoriaBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_CONVOCATORIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
                entidad.OK = true;

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public RequerimientoBE GuardarRequerimiento(RequerimientoBE entidad, int idConvocatoria, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_CONVOCATORIA_REQ";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_REQUERIMIENTO", entidad.ID_REQUERIMIENTO);
                p.Add("PI_FLAG_ESTADO", entidad.FLAG_ESTADO);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && idConvocatoria != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public CriterioBE GuardarCriterio(CriterioBE entidad, int idConvocatoria, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_CONVOCATORIA_CRI";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_FLAG_ESTADO", entidad.FLAG_ESTADO);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && idConvocatoria != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public UsuarioBE GuardarEvaluador(UsuarioBE entidad, int idConvocatoria, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_CONVOCATORIA_EVA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_USUARIO", entidad.ID_USUARIO);
                p.Add("PI_FLAG_ESTADO", entidad.FLAG_ESTADO);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, ot, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && idConvocatoria != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public EtapaBE GuardarEtapa(EtapaBE entidad, int idConvocatoria, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_CONVOCATORIA_ETA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_ETAPA", entidad.ID_ETAPA);
                //p.Add("PI_DIAS", entidad.DIAS);
                p.Add("PI_FECHA_ETAPA", entidad.FECHA_ETAPA);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, ot, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && idConvocatoria != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public List<ConvocatoriaBE> listarConvocatoriaReq(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_CONVOCAT_REQ";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ConvocatoriaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<CriterioBE> listarConvocatoriaCri(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_CONVOCAT_CRI";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<CriterioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<ConvocatoriaBE> listarConvocatoriaEva(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_CONVOCAT_EVA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ConvocatoriaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<ConvocatoriaBE> listarConvocatoriaEta(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_CONVOCAT_ETA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                //lista = db.Query<ConvocatoriaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                lista = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure).Select(x => new ConvocatoriaBE
                {
                    ID_ETAPA = (int)x.ID_ETAPA,
                    FECHA_ETAPA = (DateTime)x.FECHA_ETAPA,
                    FECHA_ETAPA_CONV = ((DateTime)x.FECHA_ETAPA).ToString("yyyy-MM-dd") == "0001-01-01" ? "" : ((DateTime)x.FECHA_ETAPA).ToString("yyyy-MM-dd"),
                    FECHA_ETAPA_DET= ((DateTime)x.FECHA_ETAPA).ToString("yyyy-MM-dd") == "0001-01-01" ? "" : ((DateTime)x.FECHA_ETAPA).ToString("dd-MM-yyyy").Replace(".", ""),
                    NOMBRE_ETAPA = (string)x.NOMBRE_ETAPA,
                    PROCESO = (string)x.PROCESO,
                    FLAG_ESTADO = (string)x.FLAG_ESTADO
                }).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<InstitucionBE> listarConvocatoriaPos(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_CONVOCAT_POS";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public bool GuardarEvaluadorPostulante(InstitucionBE entidad, OracleConnection db)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_CONV_EVA_POS";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_USUARIO", entidad.ID_USUARIO);
                p.Add("PI_ID_INSTITUCION", entidad.ID_INSTITUCION);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public ConvocatoriaEvaluadorPostulanteBE ObtenerConvEvaluadorPostulante(InstitucionBE entidad, OracleConnection db)
        {
            ConvocatoriaEvaluadorPostulanteBE item = new ConvocatoriaEvaluadorPostulanteBE();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_CONV_EVA_POS";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_INSTITUCION", entidad.ID_INSTITUCION);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<ConvocatoriaEvaluadorPostulanteBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }

        public bool GuardarConvocatoriaEtapaInscripcion(ConvocatoriaEtapaInscripcionBE entidad, OracleConnection db)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_CONV_ETA_INSC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_ETAPA", entidad.ID_ETAPA);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PI_ID_TIPO_EVALUACION", entidad.ID_TIPO_EVALUACION);
                p.Add("PI_OBSERVACION", entidad.OBSERVACION);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public InsigniaBE GuardarInsignia(InsigniaBE entidad, int idConvocatoria, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_CONVOCATORIA_INSIG";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSIGNIA", entidad.ID_INSIGNIA);
                p.Add("PI_PUNTAJE_MIN", entidad.PUNTAJE_MIN);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, ot, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && idConvocatoria != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public List<ConvocatoriaInsigniaBE> listarConvocatoriaInsig(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<ConvocatoriaInsigniaBE> lista = new List<ConvocatoriaInsigniaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_CONVOCAT_INSIG";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ConvocatoriaInsigniaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public bool GuardarResultadoReconocimiento(ReconocimientoBE entidad, OracleConnection db)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_RECONOCIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PI_ID_INSIGNIA", entidad.ID_INSIGNIA);
                p.Add("PI_PUNTAJE", entidad.PUNTAJE);
                p.Add("PI_ID_ESTRELLA", entidad.ID_ESTRELLA);
                p.Add("PI_EMISIONES", entidad.EMISIONES);
                p.Add("PI_FLAG_MEJORACONTINUA", entidad.FLAG_MEJORACONTINUA);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public EstrellaTrabajadorCamaBE GuardarEstrellaTrabajadorCama(EstrellaTrabajadorCamaBE entidad, int idConvocatoria, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_CONV_ESTREL_TRAB";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_ESTRELLA", entidad.ID_ESTRELLA);
                p.Add("PI_ID_TRABAJADORES_CAMA", entidad.ID_TRABAJADORES_CAMA);
                p.Add("PI_EMISIONES_MIN", entidad.EMISIONES_MIN);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, ot, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && idConvocatoria != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public List<EstrellaTrabajadorCamaBE> listarConvocatoriaEstrellaTrab(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<EstrellaTrabajadorCamaBE> lista = new List<EstrellaTrabajadorCamaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_CONV_EST_TRAB";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<EstrellaTrabajadorCamaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public bool GuardarReconocimientoMedida(int idInscripcion, int usuario, OracleConnection db)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_RECONOCIMIENTO_MEDIDA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PI_USUARIO_GUARDAR", usuario);
                //p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                //int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                //seGuardo = filasAfectadas > 0;
                seGuardo = true;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public bool GuardarReconocimientoEmisionesMedida(int idInscripcion, int usuario, OracleConnection db)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_RECON_MEDIDA_RESULT";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PI_USUARIO_GUARDAR", usuario);
                //p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                //int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                //seGuardo = filasAfectadas > 0;
                seGuardo = true;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public bool validarResultadoMedida(int idConvocatoria, int idInscripcion, int usuario, OracleConnection db)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_MEDIDA_RESULTADO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PI_USUARIO_GUARDAR", usuario);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                seGuardo = true;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public List<InstitucionBE> listarPostulanteEvaluador(int idConvocatoria, int idEvaluador, OracleConnection db)
        {
            List<InstitucionBE> item = new List<InstitucionBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_POS_EVA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_USUARIO", idEvaluador);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }

        public bool DeseleccionarPostulante(InstitucionBE entidad, OracleConnection db)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_UPD_CONV_EVA_POS";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_USUARIO", entidad.ID_USUARIO);
                p.Add("PI_ID_INSTITUCION", entidad.ID_INSTITUCION);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public bool TrazabilidadEtapa(int idConvocatoria, int idEtapa, int idUsuario, string descripcion, OracleConnection db)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_INS_TRAZABILIDAD_ETAPA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_ETAPA", idEtapa);
                p.Add("PI_DESCRIPCION", descripcion);
                p.Add("PI_USUARIO_GUARDAR", idUsuario);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                seGuardo = true;
            }
            catch (Exception ex) { Log.Error(ex); seGuardo = false; }
            return seGuardo;
        }

        public List<InsigniaBE> getAllInsignia(OracleConnection db)
        {
            List<InsigniaBE> lista = new List<InsigniaBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_INSIGNIA";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<InsigniaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
