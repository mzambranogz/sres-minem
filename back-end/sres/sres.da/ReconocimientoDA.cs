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
    public class ReconocimientoDA : BaseDA
    {
        public ReconocimientoBE ObtenerReconocimientoUltimo(int idInscripcion ,OracleConnection db)
        {
            ReconocimientoBE item = new ReconocimientoBE();
            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_RECONOCIMIENTO_MEJORA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<ReconocimientoBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }

        public List<ReconocimientoBE> ListarUltimosReconocimientos(int idInstitucion, int cantidadRegistros, OracleConnection db)
        {
            List<ReconocimientoBE> lista = new List<ReconocimientoBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_ULT_RECONOC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PI_CANTIDAD", cantidadRegistros);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure).Select(x =>
                    new ReconocimientoBE
                    {
                        ID_RECONOCIMIENTO = (int)x.ID_RECONOCIMIENTO,
                        ID_INSCRIPCION = (int)x.ID_INSCRIPCION,
                        INSCRIPCION = new InscripcionBE { CONVOCATORIA = new ConvocatoriaBE { FECHA_INICIO = (DateTime)x.FECHA_INICIO_CONVOCATORIA } },
                        ID_INSIGNIA = (int?)x.ID_INSIGNIA,
                        INSIGNIA = !((int?)x.ID_INSIGNIA).HasValue ? null : new InsigniaBE { ID_INSIGNIA = (int)x.ID_INSIGNIA, NOMBRE = (string)x.NOMBRE_INSIGNIA },
                        ID_PREMIACION = (int)x.ID_PREMIACION,
                        FECHA_CONVOCATORIA = ((DateTime)x.FECHA_INICIO_CONVOCATORIA).Year.ToString(),
                        VAL = (int)x.VAL
                    }).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<ReconocimientoBE> BuscarParticipantes(string razonSocialInstitucion, int? idTipoEmpresa, int? idCriterio, int? idMedMit, int? añoInicioConvocatoria, int? idInsignia, int? idEstrella, int registros, int pagina, string columna, string orden, OracleConnection db)
        {
            List<ReconocimientoBE> lista = new List<ReconocimientoBE>();

            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_BUSQ_PARTICIPANTES";
                var p = new OracleDynamicParameters();
                p.Add("PI_DESCRIPCION", razonSocialInstitucion);
                p.Add("PI_ID_TIPOEMPRESA", idTipoEmpresa);
                p.Add("PI_ID_CRITERIO", idCriterio);
                p.Add("PI_ID_MEDMIT", idMedMit);
                p.Add("PI_PERIODO", añoInicioConvocatoria);
                p.Add("PI_ID_INSIGNIA", idInsignia);
                p.Add("PI_ID_ESTRELLA", idEstrella);
                p.Add("PI_REGISTROS", registros);
                p.Add("PI_PAGINA", pagina);
                p.Add("PI_COLUMNA", columna);
                p.Add("PI_ORDEN", orden);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure).Select(x =>
                    new ReconocimientoBE
                    {
                        ID_RECONOCIMIENTO = (int)x.ID_RECONOCIMIENTO,
                        ID_INSCRIPCION = (int)x.ID_INSCRIPCION,
                        INSCRIPCION = new InscripcionBE { INSTITUCION = new InstitucionBE { RAZON_SOCIAL = (string)x.RAZON_SOCIAL_INSTITUCION, LOGO = (string)x.LOGO_INSTITUCION } },
                        ID_INSIGNIA = (int?)x.ID_INSIGNIA,
                        INSIGNIA = !((int?)x.ID_INSIGNIA).HasValue ? null : new InsigniaBE { ID_INSIGNIA = (int)x.ID_INSIGNIA, ARCHIVO_BASE = (string)x.ARCHIVO_BASE_INSIGNIA },
                        ID_ESTRELLA = (int?)x.ID_ESTRELLA,
                        ESTRELLA = (string)x.NOMBRE_ESTRELLA,
                        PUNTAJE = (int)x.PUNTAJE,
                        EMISIONES = (decimal)x.EMISIONES,
                        ROWNUMBER = (int)x.ROWNUMBER,
                        TOTAL_PAGINAS = (int)x.TOTAL_PAGINAS,
                        PAGINA = (int)x.PAGINA,
                        CANTIDAD_REGISTROS = (int)x.CANTIDAD_REGISTROS,
                        TOTAL_REGISTROS = (int)x.TOTAL_REGISTROS
                    }).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
