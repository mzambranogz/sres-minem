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
    public class IndicadorDataDA : BaseDA
    {
        public List<IndicadorBE> ObtenerIndicadorData(ComponenteBE entidad, OracleConnection db)
        {
            List<IndicadorBE> lista = new List<IndicadorBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_INDICADORDATA_ID";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<IndicadorBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return lista;
        }

        public ComponenteBE EliminarIndicadorData(ComponenteBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Criterio}USP_DEL_INDICADOR_DATA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PI_ELIMINAR_INDICADOR", entidad.ELIMINAR_INDICADOR);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
                entidad.OK = true;
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
