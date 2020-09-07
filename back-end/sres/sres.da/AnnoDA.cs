
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
    public class AnnoDA : BaseDA
    {

        public AnnoBE GuardarAnno(AnnoBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_ANNO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ANNO", entidad.ID_ANNO);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
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

        public AnnoBE EliminarAnno(AnnoBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_ANNO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ANNO", entidad.ID_ANNO);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
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

        public AnnoBE getAnno(AnnoBE entidad, OracleConnection db)
        {
            AnnoBE item = new AnnoBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_ANNO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ANNO", entidad.ID_ANNO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<AnnoBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public List<AnnoBE> ListarBusquedaAnno(AnnoBE entidad, OracleConnection db)
        {
            List<AnnoBE> lista = new List<AnnoBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_ANNO";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<AnnoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<AnnoBE> getAllAnno(OracleConnection db)
        {
            List<AnnoBE> lista = new List<AnnoBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_ANNO";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<AnnoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
