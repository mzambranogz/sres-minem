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
    public class InsigniaDA : BaseDA
    {
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

        public List<InsigniaBE> ListarBusquedaInsignia(InsigniaBE entidad, OracleConnection db)
        {
            List<InsigniaBE> lista = new List<InsigniaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_INSIGNIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<InsigniaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public InsigniaBE GuardarInsignia(InsigniaBE entidad, out int id, OracleConnection db)
        {
            id = -1;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_INSIGNIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSIGNIA", entidad.ID_INSIGNIA);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                p.Add("PI_PUNTAJE_MIN", entidad.PUNTAJE_MIN);
                p.Add("PI_ARCHIVO_BASE", entidad.ARCHIVO_BASE);                
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PI_ID_GET", 0, OracleDbType.Int32, ParameterDirection.Output);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
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

        public InsigniaBE getInsignia(InsigniaBE entidad, OracleConnection db)
        {
            InsigniaBE item = new InsigniaBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_INSIGNIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSIGNIA", entidad.ID_INSIGNIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<InsigniaBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public InsigniaBE EliminarInsignia(InsigniaBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_INSIGNIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSIGNIA", entidad.ID_INSIGNIA);
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
    }
}
