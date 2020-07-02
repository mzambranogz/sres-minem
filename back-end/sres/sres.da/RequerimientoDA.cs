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
    public class RequerimientoDA : BaseDA
    {
        //public RequerimientoBE RegistroRequerimiento(RequerimientoBE entidad, OracleConnection db)
        //{
        // try
        // {
        //     string sp = $"{Package.Mantenimiento}USP_INS_REQUERIMIENTO";
        //     var p = new OracleDynamicParameters();
        //     p.Add("PI_NOMBRE", entidad.NOMBRE);
        //     db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
        //     entidad.OK = true;
        // }
        // catch (Exception ex)
        // {
        //     Log.Error(ex);
        //     entidad.OK = false;
        // }

        //    return entidad;
        //}

        //public RequerimientoBE ActualizarRequerimiento(RequerimientoBE entidad)
        //{
        //    try
        //    {
        //        using (IDbConnection db = new OracleConnection(CadenaConexion))
        //        {
        //            string sp = sPackage + "USP_UPD_REQUERIMIENTO";
        //            var p = new OracleDynamicParameters();
        //            p.Add("PI_ID_REQUERIMIENTO", entidad.ID_REQUERIMIENTO);
        //            p.Add("PI_NOMBRE", entidad.NOMBRE);
        //            db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
        //            entidad.OK = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        entidad.OK = false;
        //    }

        //    return entidad;
        //}

        public RequerimientoBE GuardarRequerimiento(RequerimientoBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_REQUERIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_REQUERIMIENTO", entidad.ID_REQUERIMIENTO);
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

        public RequerimientoBE EliminarRequerimiento(RequerimientoBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_REQUERIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_REQUERIMIENTO", entidad.ID_REQUERIMIENTO);
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

        public RequerimientoBE getRequerimiento(RequerimientoBE entidad, OracleConnection db)
        {
            RequerimientoBE item = new RequerimientoBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_REQUERIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_REQUERIMIENTO", entidad.ID_REQUERIMIENTO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<RequerimientoBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public List<RequerimientoBE> ListarBusquedaRequerimiento(RequerimientoBE entidad, OracleConnection db)
        {
            List<RequerimientoBE> lista = new List<RequerimientoBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_REQUERIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<RequerimientoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        //public List<RequerimientoBE> ListarRequerimientoPorConvocatoria(int idConvocatoria, OracleConnection db)
        //{
        //    List<RequerimientoBE> lista = new List<RequerimientoBE>();

        //    try
        //    {
        //        string sp = $"{Package.Criterio}USP_SEL_LISTA_REQUERIMIENTO_CONVOCATORIA";
        //        var p = new OracleDynamicParameters();
        //        p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
        //        p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
        //        lista = db.Query<RequerimientoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    catch (Exception ex) { Log.Error(ex); }

        //    return lista;
        //}

        public List<RequerimientoBE> getAllRequerimiento(OracleConnection db)
        {
            List<RequerimientoBE> lista = new List<RequerimientoBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_REQUERIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<RequerimientoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

    }
}