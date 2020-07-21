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
    public class ParametroDetalleDA : BaseDA
    {
        public List<ParametroDetalleBE> ParametroDetalleForm(IndicadorFormBE entidad, OracleConnection db)
        {
            List<ParametroDetalleBE> lista = new List<ParametroDetalleBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_FORM_PARAM_DET";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ParametroDetalleBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return lista;
        }

        public List<ParametroDetalleBE> ParametroDetalleData(IndicadorDataBE entidad, OracleConnection db)
        {
            List<ParametroDetalleBE> lista = new List<ParametroDetalleBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_DATA_PARAM_DET";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ParametroDetalleBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
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
