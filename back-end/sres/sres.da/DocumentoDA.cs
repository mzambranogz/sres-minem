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
    public class DocumentoDA : BaseDA
    {
        public List<DocumentoBE> BuscarCriterioCasoDocumento(CasoBE entidad, OracleConnection db)
        {
            List<DocumentoBE> lista = new List<DocumentoBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_CONV_CRI_CASO_DOC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<DocumentoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return lista;
        }

        public List<DocumentoBE> ObtenerCriterioDocumento(CriterioBE entidad, OracleConnection db)
        {
            List<DocumentoBE> lista = new List<DocumentoBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_CRITERIO_DOC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<DocumentoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public DocumentoBE GuardarConvocatoriaCriterioCasoDoc(DocumentoBE entidad, int idConvocatoria, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_CONV_CRI_CASO_DOC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_DOCUMENTO", entidad.ID_DOCUMENTO);
                p.Add("PI_FLAG_ESTADO", entidad.FLAG_ESTADO);
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

        public List<DocumentoBE> listarConvocatoriaCriCasoDoc(CasoBE entidad, OracleConnection db)
        {
            List<DocumentoBE> lista = new List<DocumentoBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LIST_CONV_CRI_CASO_DOC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<DocumentoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }
    }
}
