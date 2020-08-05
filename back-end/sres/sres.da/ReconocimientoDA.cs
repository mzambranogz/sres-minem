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
    public class ReconocimientoDA : BaseDA
    {
        public ReconocimientoBE ObtenerReconocimientoUltimo(int idInscripcion ,OracleConnection db)
        {
            ReconocimientoBE item = new ReconocimientoBE();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_INSIGNIA";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<ReconocimientoBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }
    }
}
