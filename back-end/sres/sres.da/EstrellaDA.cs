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
    public class EstrellaDA : BaseDA
    {
        public List<EstrellaBE> listarEstrellas(OracleConnection db)
        {
            List<EstrellaBE> lista = new List<EstrellaBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_ESTRELLA";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<EstrellaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public List<EstrellaBE> ListarBusquedaEstrella(EstrellaBE entidad, OracleConnection db)
        {
            List<EstrellaBE> lista = new List<EstrellaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_ESTRELLA";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<EstrellaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public EstrellaBE GuardarEstrella(EstrellaBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_ESTRELLA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ESTRELLA", entidad.ID_ESTRELLA);
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

        public EstrellaBE getEstrella(EstrellaBE entidad, OracleConnection db)
        {
            EstrellaBE item = new EstrellaBE();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_ESTRELLA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ESTRELLA", entidad.ID_ESTRELLA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<EstrellaBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public EstrellaBE EliminarEstrella(EstrellaBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_ESTRELLA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ESTRELLA", entidad.ID_ESTRELLA);
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
