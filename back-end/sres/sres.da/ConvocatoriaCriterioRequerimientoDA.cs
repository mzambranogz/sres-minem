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
    public class ConvocatoriaCriterioRequerimientoDA : BaseDA
    {
        public bool GuardarConvocatoriaCriterioRequerimiento (ConvocatoriaCriterioRequerimientoBE convocatoriaCriterioRequerimiento, OracleConnection db)
        {
            bool seGuardo = false;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_MAN_GUARDA_CONVOCATORIA_CRITERIO_REQUERIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", convocatoriaCriterioRequerimiento.ID_CONVOCATORIA);
                p.Add("PI_ID_CRITERIO", convocatoriaCriterioRequerimiento.ID_CRITERIO);
                p.Add("PI_ID_REQUERIMIENTO", convocatoriaCriterioRequerimiento.ID_REQUERIMIENTO);
                p.Add("PI_OBLIGATORIO", convocatoriaCriterioRequerimiento.OBLIGATORIO);
                p.Add("PI_UPD_USUARIO", convocatoriaCriterioRequerimiento.UPD_USUARIO);
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
