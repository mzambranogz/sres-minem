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
    public class EstrellaDA : BaseDA
    {
        public List<EstrellaBE> listarEstrellas(OracleConnection db)
        {
            List<EstrellaBE> lista = new List<EstrellaBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_ESTRELLA";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<EstrellaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }
    }
}
