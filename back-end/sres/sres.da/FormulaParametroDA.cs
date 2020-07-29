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
    public class FormulaParametroDA : BaseDA
    {
        public FormulaParametroBE GetFormulaParametro(FormulaParametroBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_GET_FORMULA_PARAM";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                entidad = db.Query<FormulaParametroBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                //entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                //entidad.OK = false;
            }
            return entidad;
        }
    }
}
