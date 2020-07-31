using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Oracle.DataAccess.Client;
using System.Data;
using System.Web.Configuration;
using sres.be;
using sres.ut;

namespace sres.da
{
    public class ConvocatoriaCriterioPuntajeDA : BaseDA
    {
        public List<ConvocatoriaCriterioPuntajeBE> listarConvocatoriaCriterioPuntaje(int idConvocatoria, int idCriterio, OracleConnection db)
        {
            List<ConvocatoriaCriterioPuntajeBE> lista = new List<ConvocatoriaCriterioPuntajeBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_CONV_CRI_PUNTAJE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_CRITERIO", idCriterio);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ConvocatoriaCriterioPuntajeBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public List<ConvocatoriaCriterioPuntajeBE> ObtenerCriterioPuntaje(int idCriterio, OracleConnection db)
        {
            List<ConvocatoriaCriterioPuntajeBE> lista = new List<ConvocatoriaCriterioPuntajeBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_CRI_PUNTAJE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", idCriterio);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ConvocatoriaCriterioPuntajeBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public ConvocatoriaCriterioPuntajeBE GuardarConvocatoriaCriterioPuntaje(ConvocatoriaCriterioPuntajeBE entidad, int idConvocatoria, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_CONV_CRI_PUNTAJE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_DETALLE", entidad.ID_DETALLE);
                p.Add("PI_PUNTAJE", entidad.PUNTAJE);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && idConvocatoria != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

    }
}
