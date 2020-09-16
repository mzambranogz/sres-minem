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
    public class MigrarEmisionesDA : BaseDA
    {
        public bool migrarEmisiones(MigrarEmisionesBE entidad, OracleConnection db)
        {
            bool seguardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_MIGRAR_EMISIONES";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PI_ID_INICIATIVA", entidad.ID_INICIATIVA);
                p.Add("PI_ID_MEDMIT", entidad.ID_MEDMIT);
                p.Add("PI_REDUCIDO", entidad.REDUCIDO);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seguardo = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return seguardo;
        }

        public bool reestablecerEmisiones(int idInscripcion, OracleConnection db)
        {
            bool seguardo = false;
            try
            {
                string sp = $"{Package.Criterio}USP_UDP_REESTABLECER_EMISIONES";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
                seguardo = true;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seguardo;
        }

        public List<MigrarEmisionesBE> mostrarSeleccionados(int idInscripcion, OracleConnection db)
        {
            List<MigrarEmisionesBE> lista = new List<MigrarEmisionesBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_INSCRIPCION_EMISIONES";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<MigrarEmisionesBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<MigrarEmisionesBE> obtenerIdIniciativasEmisiones(int idInscripcion, OracleConnection db)
        {
            List<MigrarEmisionesBE> lista = new List<MigrarEmisionesBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_GET_ID_INICIATIVA_EMI";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<MigrarEmisionesBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public MigrarEmisionesBE actualizarValoresEmisiones(int idIniciativa, int idMedmit, OracleConnection db)
        {
            MigrarEmisionesBE migracion = new MigrarEmisionesBE();
            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_EMISION_INICIATIVA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INICIATIVA", idIniciativa);
                p.Add("PI_ID_MEDMIT", idMedmit);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                migracion = db.Query<MigrarEmisionesBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex) { Log.Error(ex); }

            return migracion;
        }
    }
}
