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
    public class FactorDA : BaseDA
    {
        public List<FactorParametroBE> ListaFactorParametro(FactorBE entidad, OracleConnection db)
        {
            List<FactorParametroBE> lista = new List<FactorParametroBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_FACTOR_PARAMETRO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", entidad.ID_FACTOR);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<FactorParametroBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<FactorDataBE> ListaFactorData(FactorBE entidad, string SQL, OracleConnection db)
        {
            List<FactorDataBE> lista = new List<FactorDataBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_FACTOR_VALOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", entidad.ID_FACTOR);
                p.Add("PI_SQL_WHERE", SQL);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<FactorDataBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<FactorBE> ListarBusquedaFactor(FactorBE entidad, OracleConnection db)
        {
            List<FactorBE> lista = new List<FactorBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_FACTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<FactorBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public List<FactorParametroBE> ListarFactorParametro(int idFactor, OracleConnection db)
        {
            List<FactorParametroBE> lista = new List<FactorParametroBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_FACTOR_PARAM";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", idFactor);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<FactorParametroBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public FactorBE GuardarFactor(FactorBE entidad, out int id, OracleConnection db, OracleTransaction ot = null)
        {
            id = -1;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_FACTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", entidad.ID_FACTOR);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                p.Add("PI_SOBRE_NOMBRE", entidad.SOBRE_NOMBRE);
                p.Add("PI_ID_GET", 0, OracleDbType.Int32, ParameterDirection.Output);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                id = (int)p.Get<dynamic>("PI_ID_GET").Value;
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && id != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return entidad;
        }

        public bool DeleteFactorParametro(int idFactor, int usu, OracleConnection db, OracleTransaction ot = null)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_FACTOR_PARAMETRO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", idFactor);
                p.Add("PI_USUARIO_GUARDAR", usu);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                seGuardo = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return seGuardo;
        }

        public FactorParametroBE GuardarFactorParametro(FactorParametroBE entidad, int id, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_FACTOR_PARAM";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", id);
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PI_ID_DETALLE", entidad.ID_DETALLE);
                p.Add("PI_NOMBRE_DETALLE", entidad.NOMBRE_DETALLE);
                p.Add("PI_ORDEN", entidad.ORDEN);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return entidad;
        }

        public FactorBE ObtenerFactor(int id, OracleConnection db)
        {
            FactorBE item = new FactorBE();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_GET_FACTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", id);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<FactorBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return item;
        }
    }
}
