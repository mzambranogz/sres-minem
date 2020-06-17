using Dapper;
using Oracle.DataAccess.Client;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.da.MRV
{
    public class UsuarioDA : BaseDA
    {
        string adminPackage = AppSettings.Get<string>("UserBDMRV") + ".PKG_MRV_ADMIN_SISTEMA.";

        public Dictionary<string, string> ObtenerPassword(string correo)
        {
            Dictionary<string, string> item = null;
            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = adminPackage + "USP_SEL_PASSWORD";
                    var p = new OracleDynamicParameters();
                    p.Add("pUsuarioLogin", correo);
                    p.Add("pRefcursor", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault(sp, p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
            }

            return item;
        }
    }
}
