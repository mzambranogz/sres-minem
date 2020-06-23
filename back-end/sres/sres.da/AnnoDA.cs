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
    public class AnnoDA : BaseDA
    {
        private string sPackage = AppSettings.Get<string>("UserBD") + ".PKG_SRES_MANTENIMIENTO.";
        public List<AnnoBE> getAllAnno(OracleConnection db, OracleTransaction ot = null)
        {
            List<AnnoBE> lista = new List<AnnoBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_ANNO";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<AnnoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
