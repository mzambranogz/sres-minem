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
    public class InstitucionDA : BaseDA
    {
        public InstitucionBE ObtenerInstitucionPorRuc(string ruc, OracleConnection db, OracleTransaction ot = null)
        {
            InstitucionBE item = null;

            try
            {
                string sp = $"{Package.Admin}USP_SEL_OBTIENE_INSTITUCION_RUC";
                var p = new OracleDynamicParameters();
                p.Add("PI_RUC", ruc);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }
    }
}