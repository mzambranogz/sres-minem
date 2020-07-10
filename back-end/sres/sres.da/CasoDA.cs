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
    public class CasoDA : BaseDA
    {
        public List<CasoBE> ListarCasoPorCriterio(int idCriterio, OracleConnection db)
        {
            List<CasoBE> lista = new List<CasoBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_LISTA_CASO_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", idCriterio);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<CasoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
