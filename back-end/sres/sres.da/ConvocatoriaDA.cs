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
    public class ConvocatoriaDA :BaseDA
    {
        public List<ConvocatoriaBE> BuscarConvocatoria(string nroInforme, string nombre, DateTime? fechaDesde, DateTime? fechaHasta, int registros, int pagina, string columna, string orden, OracleConnection db)
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
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ConvocatoriaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
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

        public ConvocatoriaBE RegistroConvocatoria(ConvocatoriaBE entidad, out int idConvocatoria, OracleConnection db, OracleTransaction ot = null)
        {
            idConvocatoria = -1;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_CONVOCATORIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA, OracleDbType.Int32, ParameterDirection.Output);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                p.Add("PI_LIMITE_POSTULANTE", entidad.LIMITE_POSTULANTE);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                idConvocatoria = (int)p.Get<dynamic>("PI_ID_CONVOCATORIA").Value;
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

    }
}
