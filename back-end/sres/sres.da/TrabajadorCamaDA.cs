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
    public class TrabajadorCamaDA : BaseDA
    {
        public List<TrabajadorCamaBE> listaSubsectorTipoempresa(int idSector, OracleConnection db)
        {
            List<TrabajadorCamaBE> lista = new List<TrabajadorCamaBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_TRABAJADOR_CAMA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_SUBSECTOR_TIPOEMPRESA", idSector);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<TrabajadorCamaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
