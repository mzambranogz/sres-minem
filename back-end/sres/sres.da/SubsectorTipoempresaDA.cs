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
    public class SubsectorTipoempresaDA : BaseDA
    {
        public List<SubsectorTipoempresaBE> listaSubsectorTipoempresa(int idSector, OracleConnection db)
        {
            List<SubsectorTipoempresaBE> lista = new List<SubsectorTipoempresaBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_SUBSECTOR_TIPOEMP";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_SECTOR", idSector);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<SubsectorTipoempresaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
