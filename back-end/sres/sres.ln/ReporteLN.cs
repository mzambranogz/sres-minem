using sres.be;
using sres.da;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class ReporteLN : BaseLN
    {
        ReporteDA reporteDA = new ReporteDA();

        public List<ReporteBE.ReporteEstadisticoXTipoEmpresa> ListarReporteEstadisticoXTipoEmpresa(int idConvocatoria)
        {
            List<ReporteBE.ReporteEstadisticoXTipoEmpresa> lista = new List<ReporteBE.ReporteEstadisticoXTipoEmpresa>();

            try
            {
                cn.Open();
                lista = reporteDA.ListarReporteEstadisticoXTipoEmpresa(idConvocatoria, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
