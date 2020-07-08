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
    public class ConvocatoriaTrazabilidadDA : BaseDA
    {
        public bool RegistrarConvocatoriaTrazabilidad(ConvocatoriaTrazabilidadBE inscripcionTrazabilidad, OracleConnection db)
        {
            bool seGuardo = false;

            try
            {
                string sp = $"{Package.Criterio}USP_INS_REGISTRA_CONVOCATORIATRAZABILIDAD";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", inscripcionTrazabilidad.ID_CONVOCATORIA);
                p.Add("PI_DESCRIPCION", inscripcionTrazabilidad.DESCRIPCION);
                p.Add("PI_UPD_USUARIO", inscripcionTrazabilidad.UPD_USUARIO);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }
    }
}
