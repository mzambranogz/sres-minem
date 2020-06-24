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
    public class SectorDA : BaseDA
    {
        public List<SectorBE> ListarSectorPorEstado(string flagEstado, OracleConnection db)
        {
            List<SectorBE> lista = new List<SectorBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_SECTOR_ESTADO";
                var p = new OracleDynamicParameters();
                p.Add("PI_FLAG_ESTADO", flagEstado);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<SectorBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
