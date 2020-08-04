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
    public class InscripcionDocumentoDA : BaseDA
    {
        public InscripcionDocumentoBE GuardarInscripcionDocumento(InscripcionDocumentoBE inscripcionDoc, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_MAN_DOCUMENTO_DATA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", inscripcionDoc.ID_CONVOCATORIA);
                p.Add("PI_ID_CRITERIO", inscripcionDoc.ID_CRITERIO);
                p.Add("PI_ID_CASO", inscripcionDoc.ID_CASO);
                p.Add("PI_ID_DOCUMENTO", inscripcionDoc.ID_DOCUMENTO);
                p.Add("PI_ID_INSCRIPCION", inscripcionDoc.ID_INSCRIPCION);
                p.Add("PI_ARCHIVO_BASE", inscripcionDoc.ARCHIVO_BASE);
                p.Add("PI_ARCHIVO_TIPO", inscripcionDoc.ARCHIVO_TIPO);
                p.Add("PI_USUARIO_GUARDAR", inscripcionDoc.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                inscripcionDoc.OK = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return inscripcionDoc;
        }

        public InscripcionDocumentoBE ObtenerInscripcionDocumento(DocumentoBE doc, OracleConnection db)
        {
            InscripcionDocumentoBE inscdoc = new InscripcionDocumentoBE();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_DOCUMENTO_DATA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", doc.ID_CONVOCATORIA);
                p.Add("PI_ID_CRITERIO", doc.ID_CRITERIO);
                p.Add("PI_ID_CASO", doc.ID_CASO);
                p.Add("PI_ID_DOCUMENTO", doc.ID_DOCUMENTO);
                p.Add("PI_ID_INSCRIPCION", doc.ID_INSCRIPCION);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                inscdoc = db.Query<InscripcionDocumentoBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex) { Log.Error(ex); }

            return inscdoc;
        }

        public bool GuardarCriterioEvaluacion(InscripcionDocumentoBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Criterio}USP_UPD_EVA_CRI_INSC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_DOCUMENTO", entidad.ID_DOCUMENTO);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PI_ID_TIPO_EVALUACION", entidad.ID_TIPO_EVALUACION);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad.OK;
        }

    }
}
