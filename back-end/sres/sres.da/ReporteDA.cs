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
        public List<ReporteBE.ReporteEstadisticoTipoEmpresaXConvocatoria> ListarReporteEstadisticoTipoEmpresaXConvocatoria(int idConvocatoria, OracleConnection db)
        {
            List<ReporteBE.ReporteEstadisticoTipoEmpresaXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoTipoEmpresaXConvocatoria>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_ESTADTIPOEMPRESAXCONV";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteEstadisticoTipoEmpresaXConvocatoria>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<ReporteBE.ReporteEstadisticoTipoPostulanteXConvocatoria> ListarReporteEstadisticoTipoPostulanteXConvocatoria(int idConvocatoria, OracleConnection db)
        {
            List<ReporteBE.ReporteEstadisticoTipoPostulanteXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoTipoPostulanteXConvocatoria>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_ESTADTIPOPOSTXCONV";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteEstadisticoTipoPostulanteXConvocatoria>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<ReporteBE.ReporteEstadisticoInsigniaXConvocatoria> ListarReporteEstadisticoInsigniaXConvocatoria(int idConvocatoria, OracleConnection db)
        {
            List<ReporteBE.ReporteEstadisticoInsigniaXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoInsigniaXConvocatoria>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_ESTADRECINSIGXCONV";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteEstadisticoInsigniaXConvocatoria>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<ReporteBE.ReporteEstadisticoEstrellaXConvocatoria> ListarReporteEstadisticoEstrellaXConvocatoria(int idConvocatoria, OracleConnection db)
        {
            List<ReporteBE.ReporteEstadisticoEstrellaXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoEstrellaXConvocatoria>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_ESTADRECESTREXCONV";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteEstadisticoEstrellaXConvocatoria>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<ReporteBE.ReporteEstadisticoMejoraContinuaXConvocatoria> ListarReporteEstadisticoMejoraContinuaXConvocatoria(int idConvocatoria, OracleConnection db)
        {
            List<ReporteBE.ReporteEstadisticoMejoraContinuaXConvocatoria> lista = new List<ReporteBE.ReporteEstadisticoMejoraContinuaXConvocatoria>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_ESTADRECMCXCONV";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteEstadisticoMejoraContinuaXConvocatoria>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<ReporteBE.ReportePostulacionesXSectorSubSector> ListaReportePostulacionesXSectorSubsector(int idSector, int idSubSector, OracleConnection db)
        {
            List<ReporteBE.ReportePostulacionesXSectorSubSector> lista = new List<ReporteBE.ReportePostulacionesXSectorSubSector>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_EVALSECTORSUBSECTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_SECTOR", idSector);
                p.Add("PI_ID_SUBSECTOR", idSubSector);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReportePostulacionesXSectorSubSector>(sp, p, commandType: CommandType.StoredProcedure).ToList();

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public List<ReporteBE.ReporteConvocatoriaEmpresa> ListaReporteConvocatoriasXEmpresa(int idConvocatoria, int idInstitucion, OracleConnection db)
        {
            List<ReporteBE.ReporteConvocatoriaEmpresa> lista = new List<ReporteBE.ReporteConvocatoriaEmpresa>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_CONVOCATORIAEMPRESA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteConvocatoriaEmpresa>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public List<ReporteBE.ReporteReconocimientoEmpresa> ListaReporteReconocimientoEmpresa(int idConvocatoria, int idInstitucion, OracleConnection db)
        {
            List<ReporteBE.ReporteReconocimientoEmpresa> lista = new List<ReporteBE.ReporteReconocimientoEmpresa>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_RECONOCIMIENTOEMPRESA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteReconocimientoEmpresa>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public List<ReporteBE.ReporteReconocimiento> Descargarreconocimiento(int idConvocatoria, int idInstitucion, OracleConnection db)
        {
            List<ReporteBE.ReporteReconocimiento> lista = new List<ReporteBE.ReporteReconocimiento>();

            try
            {
                string sp = $"{Package.Admin}USP_SEL_REP_RECONOCIMIENTOEMPRESA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReporteBE.ReporteReconocimiento>(sp, p, commandType: CommandType.StoredProcedure).ToList();

                if (lista != null)
                    foreach (var item in lista)
                    {
                        if (item.FLAG_MEDIDA == 1 || item.FLAG_MEJORACONTINUA == 1)
                            item.PREMIO_ESPECIAL = "y premiación especial";
                    }

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }
    }
}
