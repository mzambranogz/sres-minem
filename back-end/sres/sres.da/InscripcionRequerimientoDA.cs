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
    public class InscripcionRequerimientoDA : BaseDA
    {
        public List<InscripcionRequerimientoBE> ListarInscripcionRequerimientoPorConvocatoriaInscripcion(int idConvocatoria, int? idInscripcion, OracleConnection db)
        {
            List<InscripcionRequerimientoBE> lista = new List<InscripcionRequerimientoBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_LISTA_INSCRIPCIONREQUERIMIENTO_CONVOCATORIA_INSCRIPCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure).Select(x => new InscripcionRequerimientoBE
                {
                    ID_REQUERIMIENTO = (int)x.ID_REQUERIMIENTO,
                    REQUERIMIENTO = new RequerimientoBE { ID_REQUERIMIENTO = (int)x.ID_REQUERIMIENTO, NOMBRE = (string)x.NOMBRE_REQUERIMIENTO, FLAG_ESTADO = (string)x.FLAG_ESTADO_REQUERIMIENTO },
                    ARCHIVO_BASE = (string)x.ARCHIVO_BASE,
                    ARCHIVO_CIFRADO = (string)x.ARCHIVO_CIFRADO,
                    VALIDO = x.VALIDO == null ? null : (bool?)Convert.ToBoolean((int)x.VALIDO),
                    OBSERVACION = (string)x.OBSERVACION
                }).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public InscripcionRequerimientoBE ObtenerInscripcionRequerimiento(int idConvocatoria, int idInscripcion, int idRequerimiento, OracleConnection db)
        {
            InscripcionRequerimientoBE item = null;

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_OBTIENE_INSCRIPCIONREQUERIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PI_ID_REQUERIMIENTO", idRequerimiento);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<InscripcionRequerimientoBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public bool GuardarInscripcionRequerimiento(InscripcionRequerimientoBE inscripcionRequerimiento, OracleConnection db)
        {
            bool seGuardo = false;

            try
            {
                string sp = $"{Package.Criterio}USP_MAN_GUARDA_INSCRIPCIONREQUERIMIENTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", inscripcionRequerimiento.ID_CONVOCATORIA);
                p.Add("PI_ID_INSCRIPCION", inscripcionRequerimiento.ID_INSCRIPCION);
                p.Add("PI_ID_REQUERIMIENTO", inscripcionRequerimiento.ID_REQUERIMIENTO);
                p.Add("PI_ARCHIVO_BASE", inscripcionRequerimiento.ARCHIVO_BASE);
                p.Add("PI_ARCHIVO_CIFRADO", inscripcionRequerimiento.ARCHIVO_CIFRADO);
                p.Add("PI_UPD_USUARIO", inscripcionRequerimiento.UPD_USUARIO);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }
    }
}
