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
    public class ParametroDetalleRelacionDA : BaseDA
    {
        public List<ParametroDetalleBE> FiltrarParametroDetalle(ParametroDetalleRelacionBE entidad, OracleConnection db)
        {
            List<ParametroDetalleBE> detalle = new List<ParametroDetalleBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_GET_PARAM_DET_REL";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PI_PARAMETROS", entidad.PARAMETROS);
                p.Add("PI_DETALLES", entidad.DETALLES);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                detalle = db.Query<ParametroDetalleBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return detalle;
        }
    }
}
