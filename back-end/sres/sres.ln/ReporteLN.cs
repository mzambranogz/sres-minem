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

        public List<ReporteBE.ReporteEstadisticoTipoEmpresaXConvocatoria> ListarReporteEstadisticoTipoEmpresaXConvocatoria(int idConvocatoria)
        {
            List<ReporteBE.ReporteEstadisticoTipoEmpresaXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoTipoEmpresaXConvocatoria>();

            try
            {
                cn.Open();
                lista = reporteDA.ListarReporteEstadisticoTipoEmpresaXConvocatoria(idConvocatoria, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ReporteBE.ReporteEstadisticoTipoPostulanteXConvocatoria> ListarReporteEstadisticoTipoPostulanteXConvocatoria(int idConvocatoria)
        {
            List<ReporteBE.ReporteEstadisticoTipoPostulanteXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoTipoPostulanteXConvocatoria>();

            try
            {
                cn.Open();
                lista = reporteDA.ListarReporteEstadisticoTipoPostulanteXConvocatoria(idConvocatoria, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ReporteBE.ReporteEstadisticoInsigniaXConvocatoria> ListarReporteEstadisticoInsigniaXConvocatoria(int idConvocatoria)
        {
            List<ReporteBE.ReporteEstadisticoInsigniaXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoInsigniaXConvocatoria>();

            try
            {
                cn.Open();
                lista = reporteDA.ListarReporteEstadisticoInsigniaXConvocatoria(idConvocatoria, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ReporteBE.ReporteEstadisticoEstrellaXConvocatoria> ListarReporteEstadisticoEstrellaXConvocatoria(int idConvocatoria)
        {
            List<ReporteBE.ReporteEstadisticoEstrellaXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoEstrellaXConvocatoria>();

            try
            {
                cn.Open();
                lista = reporteDA.ListarReporteEstadisticoEstrellaXConvocatoria(idConvocatoria, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ReporteBE.ReporteEstadisticoMejoraContinuaXConvocatoria> ListarReporteEstadisticoMejoraContinuaXConvocatoria(int idConvocatoria)
        {
            List<ReporteBE.ReporteEstadisticoMejoraContinuaXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoMejoraContinuaXConvocatoria>();

            try
            {
                cn.Open();
                lista = reporteDA.ListarReporteEstadisticoMejoraContinuaXConvocatoria(idConvocatoria, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ReporteBE.ReportePostulacionesXSectorSubSector> ListaReportePostulacionesXSectorSubsector(int idSector, int idSubSector)
        {
            List<ReporteBE.ReportePostulacionesXSectorSubSector> lista = new List<ReporteBE.ReportePostulacionesXSectorSubSector>();

            try
            {
                cn.Open();
                lista = reporteDA.ListaReportePostulacionesXSectorSubsector(idSector, idSubSector, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ReporteBE.ReporteConvocatoriaEmpresa> ListaReporteConvocatoriasXEmpresa(int idConvocatoria, int idInstitucion)
        {
            List<ReporteBE.ReporteConvocatoriaEmpresa> lista = new List<ReporteBE.ReporteConvocatoriaEmpresa>();

            try
            {
                cn.Open();
                lista = reporteDA.ListaReporteConvocatoriasXEmpresa(idConvocatoria, idInstitucion, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ReporteBE.ReporteReconocimientoEmpresa> ListaReporteReconocimientoEmpresa(int idConvocatoria, int idInstitucion)
        {
            List<ReporteBE.ReporteReconocimientoEmpresa> lista = new List<ReporteBE.ReporteReconocimientoEmpresa>();

            try
            {
                cn.Open();
                lista = reporteDA.ListaReporteReconocimientoEmpresa(idConvocatoria, idInstitucion, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;

        }
    }
}
