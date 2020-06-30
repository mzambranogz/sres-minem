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
    public class SectorDA : BaseDA
    {
        public SectorBE GuardarSector(SectorBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_SECTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_SECTOR", entidad.ID_SECTOR);
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

        public SectorBE EliminarSector(SectorBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_SECTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_SECTOR", entidad.ID_SECTOR);
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

        public SectorBE getSector(SectorBE entidad, OracleConnection db)
        {
            SectorBE item = new SectorBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_SECTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_SECTOR", entidad.ID_SECTOR);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<SectorBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public List<SectorBE> ListarBusquedaSector(SectorBE entidad, OracleConnection db)
        {
            List<SectorBE> lista = new List<SectorBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_SECTOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<SectorBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<SectorBE> ListarSectorPorEstado(string flagEstado, OracleConnection db)
        {
            List<SectorBE> lista = new List<SectorBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_SECTOR_ESTADO";
                var p = new OracleDynamicParameters();
                p.Add("PI_FLAG_ESTADO", flagEstado);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<SectorBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
