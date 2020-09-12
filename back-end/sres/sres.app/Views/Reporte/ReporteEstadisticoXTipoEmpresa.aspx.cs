using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sres.app.Views.Reporte
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();
        ReporteLN reporteLN = new ReporteLN();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            int idConvocatoria = int.Parse(ddlConvocatoria.SelectedValue);

            List<ReporteBE.ReporteEstadisticoXTipoEmpresa> data = reporteLN.ListarReporteEstadisticoXTipoEmpresa(idConvocatoria);
        }
    }
}