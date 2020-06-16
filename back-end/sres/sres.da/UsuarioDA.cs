using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Oracle.DataAccess.Client;
using System.Web.Configuration;
using sres.be;
using sres.ut;

namespace sres.da
{
    public class UsuarioDA : BaseDA
    {
        private string adminPackage = AppSettings.Get<string>("UserBD") + ".PKG_SRES_ADMIN.";

        public List<UsuarioBE> ListaUsuario()
        {
            List<UsuarioBE> lista = null;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{adminPackage}USP_SEL_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PO", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    lista = db.Query<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
            }

            return lista;
        }

        public UsuarioBE ObtenerUsuarioPorCorreo(string correo)
        {
            UsuarioBE item = null;

            try
            {
                using(IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{adminPackage}USP_SEL_USUARIO_CORREO";
                    OracleDynamicParameters p = new OracleDynamicParameters();
                    p.Add("pCORREO", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: correo);
                    p.Add("pCURSOR", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure);
                }
            }
            catch(Exception ex)
            {
                //Log.Error(ex);
            }

            return item;
        }
    }
}
