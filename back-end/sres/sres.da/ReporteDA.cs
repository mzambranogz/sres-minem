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
    public class ReporteDA : BaseDA
    {
        public List<ReporteBE.ReporteEstadisticoXTipoEmpresa> ListarReporteEstadisticoXTipoEmpresa(int idConvocatoria, OracleConnection db)
        {
            List<ReporteBE.ReporteEstadisticoXTipoEmpresa> lista = new List<ReporteBE.ReporteEstadisticoXTipoEmpresa>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_ESTADXTIPOEMPRESA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteEstadisticoXTipoEmpresa>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
