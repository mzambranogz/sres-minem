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
    public class ParametroDA : BaseDA
    {
        public List<ParametroBE> ListarBusquedaParametro(ParametroBE entidad, OracleConnection db)
        {
            List<ParametroBE> lista = new List<ParametroBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_PARAMETRO";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ParametroBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public List<ParametroDetalleBE> ListarDetalleParametro(int id, OracleConnection db)
        {
            List<ParametroDetalleBE> lista = new List<ParametroDetalleBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_DETALLE_PARAMETRO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", id);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ParametroDetalleBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public ParametroBE ObtenerParametro(int id, OracleConnection db)
        {
            ParametroBE item = new ParametroBE();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_GET_PARAMETRO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", id);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<ParametroBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return item;
        }

        public ParametroBE EliminarDetalle(ParametroBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_DETALLE_PARAM";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PI_ID_DELETE_DETALLE", entidad.ID_DELETE_DETALLE);
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

        public ParametroBE RegistroParametro(ParametroBE entidad, out int id, OracleConnection db, OracleTransaction ot = null)
        {
            id = -1;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_PARAMETRO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                p.Add("PI_ETIQUETA", entidad.ETIQUETA);
                p.Add("PI_ID_TIPO_CONTROL", entidad.ID_TIPO_CONTROL);
                p.Add("PI_ID_TIPO_DATO", entidad.ID_TIPO_DATO);
                p.Add("PI_ESTATICO", entidad.ESTATICO);
                p.Add("PI_EDITABLE", entidad.EDITABLE);
                p.Add("PI_VERIFICABLE", entidad.VERIFICABLE);
                //p.Add("PI_FILTRO", entidad.FILTRO);
                p.Add("PI_DECIMAL_V", entidad.DECIMAL_V);
                p.Add("PI_RESULTADO", entidad.RESULTADO);
                p.Add("PI_EMISIONES", entidad.EMISIONES);
                p.Add("PI_AHORRO", entidad.AHORRO);
                p.Add("PI_DESCRIPCION", entidad.DESCRIPCION);
                p.Add("PI_UNIDAD", entidad.UNIDAD);
                p.Add("PI_TAMANO", entidad.TAMANO);
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

        public bool RegistroParametroDetalle(ParametroDetalleBE entidad, int id, OracleConnection db, OracleTransaction ot = null)
        {
            bool guardo = false;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_DETALLE_PARAM";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", id);
                p.Add("PI_ID_DETALLE", entidad.ID_DETALLE);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
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
    }
}
