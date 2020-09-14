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
    public class PuntajeDA : BaseDA
    {
        public List<PuntajeBE> ListarBusquedaPuntaje(PuntajeBE entidad, OracleConnection db)
        {
            List<PuntajeBE> lista = new List<PuntajeBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_PUNTAJE";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<PuntajeBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public PuntajeBE GuardarPuntaje(PuntajeBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_PUNTAJE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_DETALLE", entidad.ID_DETALLE);
                p.Add("PI_DESCRIPCION", entidad.DESCRIPCION);
                p.Add("PI_PUNTAJE", entidad.PUNTAJE);
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

        public PuntajeBE getPuntaje(PuntajeBE entidad, OracleConnection db)
        {
            PuntajeBE item = new PuntajeBE();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_PUNTAJE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_DETALLE", entidad.ID_DETALLE);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<PuntajeBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public PuntajeBE EliminarPuntaje(PuntajeBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_PUNTAJE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_DETALLE", entidad.ID_DETALLE);
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

        public PuntajeBE getPuntajePosible(int convocatoria, OracleConnection db)
        {
            PuntajeBE item = new PuntajeBE();
            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_PUNTAJE_POSIBLE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", convocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<PuntajeBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }
    }
}
