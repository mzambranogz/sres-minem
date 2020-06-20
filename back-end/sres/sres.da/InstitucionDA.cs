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
    public class InstitucionDA : BaseDA
    {
        static string user = AppSettings.Get<string>("UserBD");

        string mantenimientoPackage = $"{user}.PKG_SRES_MANTENIMIENTO.";

        public InstitucionBE ObtenerInstitucionPorRuc(string ruc)
        {
            InstitucionBE item = null;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{mantenimientoPackage}USP_SEL_OBTIENE_INSTITUCION_RUC";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_RUC", ruc);
                    p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }

        public InstitucionBE ObtenerInstitucion(int idInstitucion)
        {
            InstitucionBE item = null;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{mantenimientoPackage}USP_SEL_OBTIENE_INSTITUCION";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_ID_INSTITUCION", idInstitucion);
                    p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }
    }
}
