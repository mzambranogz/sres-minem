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
    public class ConvocatoriaEtapaInscripcionDA : BaseDA
    {
        public ConvocatoriaEtapaInscripcionBE ObtenerConvocatoriaEtapaInscripcion(ConvocatoriaEtapaInscripcionBE entidad, OracleConnection db)
        {
            ConvocatoriaEtapaInscripcionBE item = null;
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_OBTIENE_CONV_ETA_INSC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_ETAPA", entidad.ID_ETAPA);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<ConvocatoriaEtapaInscripcionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }
    }
}
