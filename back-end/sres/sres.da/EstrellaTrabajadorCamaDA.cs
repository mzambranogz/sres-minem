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
    public class EstrellaTrabajadorCamaDA : BaseDA
    {
        public List<EstrellaTrabajadorCamaBE> listarEstrellaTrabCama(int idConvocatoria, int idTrabCama, OracleConnection db)
        {
            List<EstrellaTrabajadorCamaBE> lista = new List<EstrellaTrabajadorCamaBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_LISTA_ESTR_TRAB_CAMA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_TRABAJADORES_CAMA", idTrabCama);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<EstrellaTrabajadorCamaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
