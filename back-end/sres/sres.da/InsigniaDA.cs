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
    }
}
