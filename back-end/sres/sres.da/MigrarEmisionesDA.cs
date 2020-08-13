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
    public class MigrarEmisionesDA : BaseDA
    {
        public bool migrarEmisiones(MigrarEmisionesBE entidad, OracleConnection db)
        {
            bool seguardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_MAN_CONVOCATORIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PI_ID_INICIATIVA", entidad.ID_INICIATIVA);
                p.Add("PI_ID_MEDMIT", entidad.ID_MEDMIT);
                p.Add("PI_REDUCIDO", entidad.REDUCIDO);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seguardo = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return seguardo;
        }
    }
}
