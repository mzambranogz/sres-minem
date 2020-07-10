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
    public class ComponenteDA : BaseDA
    {
        public List<ComponenteBE> ListarComponentePorCaso(int idCaso, OracleConnection db)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_LISTA_COMPONENTE_CASO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CASO", idCaso);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ComponenteBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
