using Microsoft.Reporting.WebForms;
using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sres.app.Reportes
{
    public partial class ReporteEstadisticoReconocimientoSySSXConvocatoria : System.Web.UI.Page
    {
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();
        ReporteLN reporteLN = new ReporteLN();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarCombos();
            }
        }

        void CargarCombos()
        {
            CargarComboConvocatoria();
        }

        void CargarComboConvocatoria()
        {
            List<ConvocatoriaBE> listaCombo = convocatoriaLN.ListarConvocatoria();
            ddlConvocatoria.DataSource = listaCombo;
            ddlConvocatoria.DataBind();
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            int idConvocatoria = int.Parse(ddlConvocatoria.SelectedValue);

            List<ReporteBE.ReporteEstadisticoInsigniaXConvocatoria> dataInsignia = reporteLN.ListarReporteEstadisticoInsigniaXConvocatoria(idConvocatoria);
            List<ReporteBE.ReporteEstadisticoEstrellaXConvocatoria> dataEstrella = reporteLN.ListarReporteEstadisticoEstrellaXConvocatoria(idConvocatoria);
            List<ReporteBE.ReporteEstadisticoMejoraContinuaXConvocatoria> dataMejoraContinua = reporteLN.ListarReporteEstadisticoMejoraContinuaXConvocatoria(idConvocatoria);

            ReportDataSource dsInsignia = new ReportDataSource("dsReporteInsignia", dataInsignia);
            ReportDataSource dsEstrella = new ReportDataSource("dsReporteEstrella", dataEstrella);
            ReportDataSource dsMejoraContinua = new ReportDataSource("dsReporteMejoraContinua", dataMejoraContinua);
            rpwReporte.Visible = true;
            rpwReporte.LocalReport.DataSources.Clear();
            rpwReporte.LocalReport.DataSources.Add(dsInsignia);
            rpwReporte.LocalReport.DataSources.Add(dsEstrella);
            rpwReporte.LocalReport.DataSources.Add(dsMejoraContinua);
        }
    }
}