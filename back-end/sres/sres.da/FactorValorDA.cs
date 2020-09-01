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
    public class FactorValorDA : BaseDA
    {
        public List<ComponenteBE> ListarBusquedaComponenteFactor(ComponenteBE entidad, OracleConnection db)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_VAL_FACTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ComponenteBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<FactorDataBE> getFactorValor(FactorBE f, OracleConnection db)
        {
            List<FactorDataBE> item = new List<FactorDataBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_GET_LISTA_FACTOR_VALOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", f.ID_FACTOR);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<FactorDataBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return item;
        }

        public FactorDataBE GuardarFactorValor(FactorDataBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_FACTOR_DATA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", entidad.ID_FACTOR);
                p.Add("PI_ID_DETALLE", entidad.ID_DETALLE);
                p.Add("PI_ID_PARAMETROS", entidad.ID_PARAMETROS);
                p.Add("PI_VALORES", entidad.VALORES);
                p.Add("PI_FACTOR", entidad.FACTOR);
                p.Add("PI_UNIDAD", entidad.UNIDAD);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
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

        public bool DeleteFactorData(int idFactor, int usu, OracleConnection db, OracleTransaction ot = null)
        {
            bool seGuardo = false;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_FACTOR_DATA";
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
    }
}
