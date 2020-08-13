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
    public class MigrarEmisionesDA : BaseDA
    {
        public List<MigrarEmisionesBE> ObtenerEmisiones(string ruc, string id_iniciativas,OracleConnection db)
        {
            List<MigrarEmisionesBE> lista = new List<MigrarEmisionesBE>();
            try
            {
                string sp = $"{Package.Admin}USP_SEL_REDUCIDOS_MEDIDA";
                var p = new OracleDynamicParameters();
                p.Add("PI_RUC", ruc);
                p.Add("PI_ID_INICIATIVAS", id_iniciativas);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<MigrarEmisionesBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
