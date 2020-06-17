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
    public class CriterioDA : BaseDA
    {
        private string sPackage = AppSettings.Get<string>("UserBD") + ".PKG_SRES_MANTENIMIENTO.";

        public List<CriterioBE> ListarBusquedaCriterio(CriterioBE entidad)
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = sPackage + "USP_SEL_LISTA_BUSQ_CRITERIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_BUSCAR", entidad.BUSCAR);
                    p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                    p.Add("PI_PAGINA", entidad.PAGINA);
                    p.Add("PI_COLUMNA", entidad.ORDER_BY);
                    p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                    p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    lista = db.Query<CriterioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
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
