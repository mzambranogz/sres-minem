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
    public class RolDA : BaseDA
    {
        public List<RolBE> ListarRolPorEstado(string flagEstado, OracleConnection db)
        {
            List<RolBE> lista = null;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_ROL_ESTADO";
                var p = new OracleDynamicParameters();
                p.Add("PI_FLAG_ESTADO", flagEstado);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<RolBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
