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
    public partial class ReportePostulacionesXSectorSubSector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarCombos();
            }
        }

        void CargarCombos()
        {
            CargarSector();
            CargarSubSector();
        }

        void CargarSector()
        {
            List<SectorBE> listaCombo = new SectorLN().ListarSectorPorEstado("1");
            SectorBE itemTodos = new SectorBE() { ID_SECTOR = -1, NOMBRE = "[TODOS]" };
            if (listaCombo == null)
                listaCombo = new List<SectorBE>();
            listaCombo.Insert(0, itemTodos);
            ddlSector.DataSource = listaCombo;
            ddlSector.DataBind();
        }
        void CargarSubSector()
        {
            List<SubsectorTipoempresaBE> listaCombo = new SubsectorTipoempresaLN().listaSubsectorTipoempresa(int.Parse(ddlSector.SelectedValue));
            SubsectorTipoempresaBE itemTodos = new SubsectorTipoempresaBE() { ID_SECTOR = -1, ID_SUBSECTOR_TIPOEMPRESA = -1, NOMBRE = "[TODOS]" };
            if (listaCombo == null)
                listaCombo = new List<SubsectorTipoempresaBE>();
            listaCombo.Insert(0, itemTodos);
            ddlSubSector.DataSource = listaCombo;
            ddlSubSector.DataBind();
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            int idSector = int.Parse(ddlSector.SelectedValue);
            int idSubSector = int.Parse(ddlSubSector.SelectedValue);

            List<ReporteBE.ReportePostulacionesXSectorSubSector> dataReporte = new ReporteLN().ListaReportePostulacionesXSectorSubsector(idSector, idSubSector);

            ReportDataSource dsReporte = new ReportDataSource("dsPostulacionesSectorSubsector", dataReporte);
            rpwReporte.Visible = true;
            rpwReporte.LocalReport.DataSources.Clear();
            rpwReporte.LocalReport.DataSources.Add(dsReporte);
            rpwReporte.LocalReport.Refresh();
        }

        protected void ddlSector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSector.SelectedValue))
            {
                CargarSubSector();
            }
        }
    }
}