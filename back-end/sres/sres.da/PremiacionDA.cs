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
    public class PremiacionDA : BaseDA
    {
        public List<PremiacionBE> ListarBusquedaPremiacion(PremiacionBE entidad, OracleConnection db)
        {
            List<PremiacionBE> lista = new List<PremiacionBE>();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_PREMIAC";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<PremiacionBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return lista;
        }

        public PremiacionBE GuardarPremiacion(PremiacionBE entidad, out int id, OracleConnection db)
        {
            id = -1;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_PREMIACION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PREMIACION", entidad.ID_PREMIACION);
                p.Add("PI_ID_INSIGNIA", entidad.ID_INSIGNIA);
                p.Add("PI_ID_ESTRELLA", entidad.ID_ESTRELLA);
                p.Add("PI_ARCHIVO_BASE", entidad.ARCHIVO_BASE);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
                p.Add("PI_ID_GET", 0, OracleDbType.Int32, ParameterDirection.Output);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
                id = (int)p.Get<dynamic>("PI_ID_GET").Value;
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0 && id != -1;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return entidad;
        }

        public PremiacionBE getPremiacion(PremiacionBE entidad, OracleConnection db)
        {
            PremiacionBE item = new PremiacionBE();
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_PREMIACION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PREMIACION", entidad.ID_PREMIACION);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<PremiacionBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return item;
        }

        public int ValidarPremiacion(PremiacionBE entidad, OracleConnection db)
        {
            int id = 0;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_VALIDACION_PREMIACION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PREMIACION", entidad.ID_PREMIACION);
                p.Add("PI_ID_INSIGNIA", entidad.ID_INSIGNIA);
                p.Add("PI_ID_ESTRELLA", entidad.ID_ESTRELLA);
                p.Add("PI_ID_GET", 0, OracleDbType.Int32, ParameterDirection.Output);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
                id = (int)p.Get<dynamic>("PI_ID_GET").Value;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return id;
        }

        public PremiacionBE EliminarPremiacion(PremiacionBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_PREMIACION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PREMIACION", entidad.ID_PREMIACION);
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

        public PremiacionBE getPremiacionInsigniaEstrella(int idInsignia, int idEstrella, OracleConnection db)
        {
            PremiacionBE item = new PremiacionBE();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_GET_PREM_INSIG_ESTRE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSIGNIA", idInsignia);
                p.Add("PI_ID_ESTRELLA", idEstrella);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<PremiacionBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return item;
        }

        public List<ReconocimientoBE> ListaReconocimiento(int idInstitucion, OracleConnection db)
        {
            List<ReconocimientoBE> lista = new List<ReconocimientoBE>();
            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_LISTA_RECONOCIM";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                //lista = db.Query<ReconocimientoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                lista = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure).Select(x => new ReconocimientoBE
                {
                    ID_RECONOCIMIENTO = (int)x.ID_RECONOCIMIENTO,
                    ID_INSCRIPCION = (int)x.ID_INSCRIPCION,
                    PUNTAJE = (int)x.PUNTAJE,
                    EMISIONES = (decimal)x.EMISIONES,
                    ENERGIA = (decimal)x.ENERGIA,
                    COMBUSTIBLE = (decimal)x.COMBUSTIBLE,
                    FECHA_INICIO = (DateTime)x.FECHA_INICIO,
                    MES_CONVOCATORIA = ((DateTime)x.FECHA_INICIO).ToString("MMMM").Replace(".", "").ToUpper(),
                    ANIO_CONVOCATORIA = ((DateTime)x.FECHA_INICIO).ToString("yyyy"),
                    DESCRIPCION = (string)x.DESCRIPCION,
                    FLAG_MEJORACONTINUA = (string)x.FLAG_MEJORACONTINUA,
                    FLAG_EMISIONESMAX = (string)x.FLAG_EMISIONESMAX,
                    INSIGNIA = x.ID_INSIGNIA == null ? null : new InsigniaBE { ID_INSIGNIA = (int)x.ID_INSIGNIA, NOMBRE = (string)x.NOMBRE_INSIG },
                    ESTRELLA_E = x.ID_ESTRELLA == null ? null : new EstrellaBE { ID_ESTRELLA = (int)x.ID_ESTRELLA, NOMBRE = (string)x.NOMBRE_ESTRELLA },
                    ID_PREMIACION = (int)x.ID_PREMIACION,
                    ARCHIVO_BASE = (string)x.ARCHIVO_BASE_PRE,
                    FLAG_ESTADO = (string)x.FLAG_ESTADO
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        public List<ReconocimientoMedidaBE> ListaReconocimientoMedida(int idReconocimiento, OracleConnection db)
        {
            List<ReconocimientoMedidaBE> lista = new List<ReconocimientoMedidaBE>();
            try
            {
                string sp = $"{Package.Verificacion}USP_GET_RECONOC_MEDIDA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_RECONOCIMIENTO", idReconocimiento);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ReconocimientoMedidaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }
    }
}
