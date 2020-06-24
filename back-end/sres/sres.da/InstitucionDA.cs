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
    public class InstitucionDA : BaseDA
    {

        public InstitucionBE ObtenerInstitucionPorRuc(string ruc, OracleConnection db)
        {
            InstitucionBE item = null;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_OBTIENE_INSTITUCION_RUC";
                var p = new OracleDynamicParameters();
                p.Add("PI_RUC", ruc);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }

        public InstitucionBE ObtenerInstitucion(int idInstitucion, OracleConnection db)
        {
            InstitucionBE item = null;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_OBTIENE_INSTITUCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public bool GuardarInstitucion(InstitucionBE institucion, OracleConnection db, out int idInstitucion)
        {
            bool seGuardo = false;
            idInstitucion = -1;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_MAN_GUARDA_INSTITUCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", institucion.ID_INSTITUCION, OracleDbType.Int32, ParameterDirection.Output);
                p.Add("PI_RUC", institucion.RUC);
                p.Add("PI_RAZON_SOCIAL", institucion.RAZON_SOCIAL);
                p.Add("PI_DOMICILIO_LEGAL", institucion.DOMICILIO_LEGAL);
                p.Add("PI_ID_SECTOR", institucion.ID_SECTOR);
                p.Add("PI_UPD_USUARIO", institucion.UPD_USUARIO);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                idInstitucion = (int)p.Get<dynamic>("PI_ID_INSTITUCION").Value;
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0 && idInstitucion != -1;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }
    }
}
