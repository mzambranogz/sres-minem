using Dapper;
using Oracle.DataAccess.Client;
using sres.be;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.da
{
    public class InscripcionDA : BaseDA
    {
        public InscripcionBE ObtenerInscripcionPorConvocatoriaInstitucion(int idConvocatoria, int idInstitucion, OracleConnection db)
        {
            InscripcionBE item = null;

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_OBTIENE_INSC_CONV_INST";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<InscripcionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public bool GuardarInscripcion(InscripcionBE inscripcion, out int idInscripcion, OracleConnection db)
        {
            bool seGuardo = false;
            idInscripcion = -1;

            try
            {
                string sp = $"{Package.Criterio}USP_MAN_GUARDA_INSCRIPCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", inscripcion.ID_INSCRIPCION, OracleDbType.Int32, ParameterDirection.InputOutput);
                p.Add("PI_ID_CONVOCATORIA", inscripcion.ID_CONVOCATORIA);
                p.Add("PI_ID_INSTITUCION", inscripcion.ID_INSTITUCION);
                p.Add("PI_UPD_USUARIO", inscripcion.UPD_USUARIO);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                idInscripcion = (int)p.Get<dynamic>("PI_ID_INSCRIPCION").Value;
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch(Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public bool GuardarEvaluacionInscripcion(InscripcionBE inscripcion, OracleConnection db)
        {
            bool seGuardo = false;

            try
            {
                string sp = $"{Package.Verificacion}USP_UPD_EVAL_INSCRIPCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", inscripcion.ID_INSCRIPCION, OracleDbType.Int32, ParameterDirection.InputOutput);
                p.Add("PI_ID_TIPO_EVALUACION", inscripcion.ID_TIPO_EVALUACION);
                p.Add("PI_OBSERVACION", inscripcion.OBSERVACION);
                p.Add("PI_UPD_USUARIO", inscripcion.UPD_USUARIO);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public List<InscripcionBE> BuscarInscripcion(int idConvocatoria, int? idInscripcion, string razonSocialInstitucion, string nombresCompletosUsuario, int? idUsuario, int registros, int pagina, string columna, string orden, OracleConnection db)
        {
            List<InscripcionBE> lista = new List<InscripcionBE>();

            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_BUSQ_INSCRIPCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PI_RAZON_SOCIAL_INSTITUCION", razonSocialInstitucion);
                p.Add("PI_NOMBRES_APELLIDOS_USUARIO", nombresCompletosUsuario);
                p.Add("PI_ID_USUARIO", idUsuario);
                p.Add("PI_REGISTROS", registros);
                p.Add("PI_PAGINA", pagina);
                p.Add("PI_COLUMNA", columna);
                p.Add("PI_ORDEN", orden);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure).Select(x => 
                new InscripcionBE
                {
                    ID_INSCRIPCION = (int)x.ID_INSCRIPCION,
                    ID_INSTITUCION = (int)x.ID_INSTITUCION,
                    FLAG_ANULAR = (int)x.FLAG_ANULAR,
                    INSTITUCION = new InstitucionBE { RUC = (string)x.RUC_INSTITUCION, RAZON_SOCIAL = (string)x.RAZON_SOCIAL_INSTITUCION },
                    UPD_USUARIO = (int)x.REG_USUARIO,
                    UPD_FECHA = (DateTime)x.REG_FECHA,
                    USUARIO = new UsuarioBE { NOMBRES = (string)x.NOMBRES_USUARIO, APELLIDOS = (string)x.APELLIDOS_USUARIO, CORREO = (string)x.CORREO_USUARIO },
                    CANTIDADCRITERIOSINGRESADOS = (int)x.CANTIDADCRITERIOSINGRESADOS,
                    PUNTOSACUMULADOS = (int)x.PUNTOSACUMULADOS,
                    ASPIRACION = (int)x.ASPIRACION,
                    CONVOCATORIA = new ConvocatoriaBE { ID_ETAPA = (int?) x.ID_ETAPA_CONVOCATORIA },
                    ROWNUMBER = (int)x.ROWNUMBER,
                    TOTAL_PAGINAS = (int)x.TOTAL_PAGINAS,
                    PAGINA = (int)x.PAGINA,
                    CANTIDAD_REGISTROS = (int)x.CANTIDAD_REGISTROS,
                    TOTAL_REGISTROS = (int)x.TOTAL_REGISTROS
                }).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public bool AnularInscripcion(InscripcionBE inscripcion, OracleConnection db)
        {
            bool seGuardo = false;

            try
            {
                string sp = $"{Package.Verificacion}USP_UPD_ANULAR_INSCRIPCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", inscripcion.ID_INSCRIPCION);
                p.Add("PI_USUARIO_GUARDAR", inscripcion.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public InscripcionBE ObtenerInscripcionPorId(int idInscripcion, OracleConnection db)
        {
            InscripcionBE ins = new InscripcionBE();
            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_GET_ID_INSCRIPCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                ins = db.QueryFirstOrDefault<InscripcionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return ins;
        }

        public InstitucionBE ObtenerInstitucionPorInscripcion(int idInscripcion, OracleConnection db)
        {
            InstitucionBE ins = new InstitucionBE();
            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_GET_INSTITUCION_INSC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                ins = db.QueryFirstOrDefault<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return ins;
        }

        public List<InscripcionTrazabilidadBE> ObtenerInscripcionTrazabilidad(int idInscripcion, OracleConnection db)
        {
            List<InscripcionTrazabilidadBE> ins = new List<InscripcionTrazabilidadBE>();
            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_INSCRIPCION_TRAZABILIDAD";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                //ins = db.Query<InscripcionTrazabilidadBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                ins = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure).Select(x => new InscripcionTrazabilidadBE
                {
                    ID_INSCRIPCION = (int)x.ID_INSCRIPCION,
                    ID_TRAZABILIDAD = (int)x.ID_TRAZABILIDAD,
                    DESCRIPCION = (string)x.DESCRIPCION,
                    FECHA_TRAZA = ((DateTime)x.REG_FECHA).ToString("dd/MM/yyyy HH:mm"),
                    CORREO = (string)x.CORREO,
                    ROL = (string)x.ROL,
                    ID_ROL = (int)x.ID_ROL,
                    ETAPA = (string)x.ETAPA
            }).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }
            return ins;
        }
    }
}
