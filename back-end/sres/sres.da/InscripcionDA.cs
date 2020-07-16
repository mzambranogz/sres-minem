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
    public class InscripcionDA : BaseDA
    {
        public InscripcionBE ObtenerInscripcionPorConvocatoriaInstitucion(int idConvocatoria, int idInstitucion, OracleConnection db)
        {
            InscripcionBE item = null;

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_OBTIENE_INSC_CONV_INST";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<InscripcionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public bool GuardarInscripcion(InscripcionBE inscripcion, out int idInscripcion, OracleConnection db)
        {
            bool seGuardo = false;
            idInscripcion = -1;

            try
            {
                string sp = $"{Package.Criterio}USP_MAN_GUARDA_INSCRIPCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", inscripcion.ID_INSCRIPCION, OracleDbType.Int32, ParameterDirection.InputOutput);
                p.Add("PI_ID_CONVOCATORIA", inscripcion.ID_CONVOCATORIA);
                p.Add("PI_ID_INSTITUCION", inscripcion.ID_INSTITUCION);
                p.Add("PI_UPD_USUARIO", inscripcion.UPD_USUARIO);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                idInscripcion = (int)p.Get<dynamic>("PI_ID_INSCRIPCION").Value;
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch(Exception ex) { Log.Error(ex); }

            return seGuardo;
        }
    }
}
