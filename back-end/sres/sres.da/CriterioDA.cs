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
    public class CriterioDA : BaseDA
    {
        public CriterioBE RegistroCriterio(CriterioBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_INS_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_NOMBRE", entidad.NOMBRE);
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

        public CriterioBE GuardarCriterio(CriterioBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_UPD_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
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

        public CriterioBE EliminarCriterio(CriterioBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
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

        public CriterioBE getCriterio(CriterioBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            CriterioBE item = new CriterioBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<CriterioBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public List<CriterioBE> ListarBusquedaCriterio(CriterioBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
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
    }
}