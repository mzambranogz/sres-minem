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
    public class FactorDA : BaseDA
    {
        public List<FactorParametroBE> ListaFactorParametro(FactorBE entidad, OracleConnection db)
        {
            List<FactorParametroBE> lista = new List<FactorParametroBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_FACTOR_PARAMETRO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", entidad.ID_FACTOR);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<FactorParametroBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<FactorDataBE> ListaFactorData(FactorBE entidad, string SQL, OracleConnection db)
        {
            List<FactorDataBE> lista = new List<FactorDataBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_FACTOR_VALOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_FACTOR", entidad.ID_FACTOR);
                p.Add("PI_SQL_WHERE", SQL);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<FactorDataBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
