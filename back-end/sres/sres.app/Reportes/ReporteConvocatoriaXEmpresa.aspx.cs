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
    public partial class ReporteConvocatoriaXEmpresa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargaCombos();
            }
        }
        void CargaCombos()
        {
            CargarConvocatoria();
            CargarEmpresa();
        }

        void CargarConvocatoria()
        {
            List<ConvocatoriaBE> listaCombo = new ConvocatoriaLN().ListarConvocatoria();
            ConvocatoriaBE itemTodos = new ConvocatoriaBE() { ID_CONVOCATORIA = -1, NOMBRE = "[TODOS]" };
            if (listaCombo == null)
                listaCombo = new List<ConvocatoriaBE>();
            listaCombo.Insert(0, itemTodos);
            ddlConvocatoria.DataSource = listaCombo;
            ddlConvocatoria.DataBind();
        }

        void CargarEmpresa()
        {
            List<InstitucionBE> listaCombo = new InstitucionLN().ListarInstitucion();
            InstitucionBE itemTodos = new InstitucionBE() { ID_INSTITUCION= -1, RAZON_SOCIAL = "[TODOS]" };
            if (listaCombo == null)
                listaCombo = new List<InstitucionBE>();
            listaCombo.Sort((p, q) => string.Compare(p.RAZON_SOCIAL, q.RAZON_SOCIAL));
            listaCombo.Insert(0, itemTodos);
            ddlEmpresa.DataSource = listaCombo;
            ddlEmpresa.DataBind();
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            int idConvocatoria = int.Parse(ddlConvocatoria.SelectedValue);
            int idEmpresa = int.Parse(ddlEmpresa.SelectedValue);
            string convocatoria = ddlConvocatoria.SelectedItem.Text;
            string empresa = ddlEmpresa.SelectedItem.Text;

            List<ReporteBE.ReporteConvocatoriaEmpresa> dataReporte = new ReporteLN().ListaReporteConvocatoriasXEmpresa(idConvocatoria, idEmpresa);
            ReportDataSource dsReporte = new ReportDataSource("dsConvocatoriaEmpresa", dataReporte);
            rpwReporte.Visible = true;
            rpwReporte.LocalReport.SetParameters(new ReportParameter("Convocatoria", convocatoria));
            rpwReporte.LocalReport.SetParameters(new ReportParameter("Empresa", empresa));
            rpwReporte.LocalReport.DataSources.Clear();
            rpwReporte.LocalReport.DataSources.Add(dsReporte);
        }
    }
}