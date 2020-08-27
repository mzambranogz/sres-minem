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
    public class IndicadorDA : BaseDA
    {
        public List<IndicadorBE> ListarBusquedaIndicador(IndicadorBE entidad, OracleConnection db)
        {
            List<IndicadorBE> lista = new List<IndicadorBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_IND";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<IndicadorBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<ParametroBE> ListarParametro(IndicadorBE entidad, OracleConnection db)
        {
            List<ParametroBE> lista = new List<ParametroBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_PARAM_INDICADOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ParametroBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public bool GuardarIndicador(IndicadorBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            bool guardo = false;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_INDICADOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PI_ORDEN", entidad.ORDEN);
                p.Add("PI_FORMULA", entidad.FORMULA);
                p.Add("PI_FORMULA_ARMADO", entidad.FORMULA_ARMADO);
                p.Add("PI_INS", entidad.INS);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                guardo = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return guardo;
        }

        public bool EliminarIndicador(IndicadorBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            bool guardo = false;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_INDICADOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_ACTIVO", entidad.ID_ACTIVO);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                guardo = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return guardo;
        }
    }
}
