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
    public class ComponenteDA : BaseDA
    {
        public List<ComponenteBE> ListarComponentePorCaso(int idCaso, OracleConnection db)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_LISTA_COMPONENTE_CASO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CASO", idCaso);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ComponenteBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<ComponenteBE> ListarBusquedaComponente(ComponenteBE entidad, OracleConnection db)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_COMP";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ComponenteBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public ComponenteBE GuardarComponente(ComponenteBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_COMPONENTE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                p.Add("PI_ETIQUETA", entidad.ETIQUETA);
                p.Add("PI_INCREMENTABLE", entidad.INCREMENTABLE);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public ComponenteBE getComponente(ComponenteBE entidad, OracleConnection db)
        {
            ComponenteBE item = new ComponenteBE();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_COMPONENTE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<ComponenteBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public ComponenteBE EliminarComponente(ComponenteBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_COMPONENTE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
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

        public List<ComponenteBE> ListaComponenteCasoCriterio(ComponenteBE entidad, OracleConnection db)
        {
            List<ComponenteBE> item = new List<ComponenteBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_COMP_CASO_CRI";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<ComponenteBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return item;
        }
    }
}
