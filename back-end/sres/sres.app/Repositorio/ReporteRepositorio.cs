using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using sres.be;
using sres.ln;
using sres.ut;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Configuration;

namespace sres.app.Repositorio
{
    public class ReporteRepositorio
    {
        ReportViewer rvReporte = new ReportViewer();
        ReporteLN rec = new ReporteLN();

        private void ConfigurarReporte()
        {
            rvReporte.ProcessingMode = ProcessingMode.Local;
        }

        public bool DescargarReconocimiento(int idconvocatoria, int idinstitucion, string NombrePDF)
        {
            bool OK = true;
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;
                string rutatarget = WebConfigurationManager.AppSettings["Ruta.Reporte"].ToString();
                ConfigurarReporte();
                rvReporte.LocalReport.ReportPath = string.Format("{0}\\Certificado.rdlc", rutatarget);
                List<ReporteBE.ReporteReconocimiento> lista = rec.DescargarReconocimiento(idconvocatoria, idinstitucion);
                ReportDataSource dataSource = new ReportDataSource("dsreconocimiento", lista);

                rvReporte.LocalReport.DataSources.Clear();
                rvReporte.LocalReport.DataSources.Add(dataSource);
                byte[] bytes = rvReporte.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                using (FileStream fs = new FileStream(NombrePDF, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                OK = false;
                Log.Error(ex);
            }
            return OK;
        }
    }
}