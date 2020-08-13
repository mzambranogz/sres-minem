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
    public class InformePreliminarDA : BaseDA
    {
        public List<InscripcionBE> listaInscripcionConvocatoriaEvaluador(ConvocatoriaBE entidad, OracleConnection db)
        {
            List<InscripcionBE> lista = new List<InscripcionBE>();

            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_LISTA_INSC_CONV_EVA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_USUARIO", entidad.ID_USUARIO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<InscripcionBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<ConvocatoriaCriterioPuntajeInscripBE> obtenerInscripcionEvaluacion(InscripcionBE entidad, OracleConnection db)
        {
            List<ConvocatoriaCriterioPuntajeInscripBE> item = new List<ConvocatoriaCriterioPuntajeInscripBE>();

            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_GET_EVALUACION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<ConvocatoriaCriterioPuntajeInscripBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }
    }
}
