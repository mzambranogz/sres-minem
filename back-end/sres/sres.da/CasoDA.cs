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
    public class CasoDA : BaseDA
    {
        //public List<CasoBE> ListarCasoPorCriterio(int idCriterio, OracleConnection db)
        //{
        //    List<CasoBE> lista = new List<CasoBE>();

        //    try
        //    {
        //        string sp = $"{Package.Criterio}USP_SEL_LISTA_CASO_CRITERIO";
        //        var p = new OracleDynamicParameters();
        //        p.Add("PI_ID_CRITERIO", idCriterio);
        //        p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
        //        lista = db.Query<CasoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    catch (Exception ex) { Log.Error(ex); }

        //    return lista;
        //}

        public List<CasoBE> VerificarConvocatoriaCriterioInscripcion(CasoBE entidad, OracleConnection db)
        {
            List<CasoBE> lista = new List<CasoBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_VERF_CONV_CRITERIO_INSC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<CasoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<CasoBE> ObtenerListaCasoCriterioPorConvocatoria(CasoBE entidad, OracleConnection db)
        {
            List<CasoBE> lista = new List<CasoBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_LISTA_CASO_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<CasoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public CasoBE GuardarConvocatoriaCriterioCasoInscripcion(CasoBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Criterio}USP_PRC_CONV_CRI_CAS_INSC_DATA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", entidad.ID_CONVOCATORIA);
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_CASO", entidad.ID_CASO);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return entidad;
        }
    }
}
