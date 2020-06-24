using Dapper;
using Oracle.DataAccess.Client;
using sres.be.MRV;
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
        public bool VerificarCorreo(string correo, OracleConnection db)
        {
            bool verificacion = false;
            try
            {
                string sp = $"{Package.Admin}USP_SEL_VERIFICAR_EMAIL";
                var p = new OracleDynamicParameters();
                p.Add("pEMAIL_USUARIO", correo);
                p.Add("pVerificar", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int cantidad = (int)p.Get<dynamic>("pVerificar").Value;
                verificacion = cantidad > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return verificacion;
        }

        public UsuarioBE ObtenerUsuarioPorCorreo(string correo, OracleConnection db)
        {
            UsuarioBE item = null;

            try
            {
                string sp = $"{Package.Admin}USP_SEL_OBTIENE_USUARIO_CORREO";
                var p = new OracleDynamicParameters();
                p.Add("PI_CORREO", correo);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public UsuarioBE ObtenerPassword(string correo, OracleConnection db)
        {
            UsuarioBE item = null;
            try
            {
                string sp = $"{Package.Admin}USP_SEL_PASSWORD";
                var p = new OracleDynamicParameters();
                p.Add("pUsuarioLogin", correo);
                p.Add("pRefcursor", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }
    }
}