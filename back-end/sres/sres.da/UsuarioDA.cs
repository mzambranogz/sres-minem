using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Oracle.DataAccess.Client;
using System.Web.Configuration;
using sres.be;
using sres.ut;
using sres.da.DataBaseHelpers;
using Newtonsoft.Json;

namespace sres.da
{
    public class UsuarioDA : BaseDA
    {
        static string user = AppSettings.Get<string>("UserBD");

        string adminPackage = $"{user}.PKG_SRES_ADMIN.";
        string mantenimientoPackage = $"{user}.PKG_SRES_MANTENIMIENTO.";

        #region PAQUETE ADMIN
        public List<UsuarioBE> ListaUsuario()
        {
            List<UsuarioBE> lista = null;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{adminPackage}USP_SEL_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PO", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    lista = db.Query<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public UsuarioBE ObtenerUsuarioPorCorreo(string correo)
        {
            UsuarioBE item = null;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{adminPackage}USP_SEL_USUARIO_CORREO";
                    OracleDynamicParameters p = new OracleDynamicParameters();
                    p.Add("pCORREO", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: correo);
                    p.Add("pCURSOR", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }
        #endregion

        #region PAQUETE MANTENIMIENTO
        public List<UsuarioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<UsuarioBE> lista = null;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{mantenimientoPackage}USP_SEL_LISTA_BUSQ_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_BUSCAR", busqueda);
                    p.Add("PI_REGISTROS", registros);
                    p.Add("PI_PAGINA", pagina);
                    p.Add("PI_COLUMNA", columna);
                    p.Add("PI_ORDEN", orden);
                    p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    lista = db.Query<dynamic>(sp, p, commandType: CommandType.StoredProcedure)
                        .Select(x => new UsuarioBE
                        {
                            ID_USUARIO = (int)x.ID_USUARIO,
                            NOMBRES = (string)x.NOMBRES,
                            APELLIDOS = (string)x.APELLIDOS,
                            CORREO = (string)x.CORREO,
                            TELEFONO = (string)x.TELEFONO,
                            ANEXO = (string)x.ANEXO,
                            CELULAR = (string)x.CELULAR,
                            ID_INSTITUCION = (int?)x.ID_INSTITUCION,
                            INSTITUCION = x.ID_INSTITUCION == null ? null : new InstitucionBE { ID_INSTITUCION = (int)x.ID_INSTITUCION, RAZON_SOCIAL = (string)x.RAZON_SOCIAL_INSTITUCION, RUC = (string)x.RUC_INSTITUCION },
                            ID_ROL = (int?)x.ID_ROL,
                            ROL = x.ID_ROL == null ? null : new RolBE { ID_ROL = (int)x.ID_ROL, NOMBRE = (string)x.NOMBRE_ROL },
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public UsuarioBE ObtenerUsuario(int idUsuario)
        {
            UsuarioBE item = null;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{mantenimientoPackage}USP_SEL_OBTIENE_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_ID_USUARIO", idUsuario);
                    p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public bool CambiarEstadoUsuario(UsuarioBE usuario)
        {
            bool seActualizo = false;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{mantenimientoPackage}USP_UPD_CAMBIA_ESTADO_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_ID_USUARIO", usuario.ID_USUARIO);
                    p.Add("PI_FLAG_ESTADO", usuario.FLAG_ESTADO);
                    p.Add("PI_UPD_USUARIO", usuario.UPD_USUARIO);
                    p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                    db.Execute(sp, p, commandType: CommandType.StoredProcedure);

                    int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;

                    seActualizo = filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return seActualizo;
        }

        public bool GuardarUsuario(UsuarioBE usuario)
        {
            bool seActualizo = false;

            try
            {
                using (IDbConnection db = new OracleConnection(CadenaConexion))
                {
                    string sp = $"{mantenimientoPackage}USP_MAN_GUARDA_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_ID_USUARIO", usuario.ID_USUARIO, direction: ParameterDirection.InputOutput);
                    p.Add("PI_NOMBRES", usuario.NOMBRES);
                    p.Add("PI_APELLIDOS", usuario.APELLIDOS);
                    p.Add("PI_CORREO", usuario.CORREO);
                    p.Add("PI_TELEFONO", usuario.TELEFONO);
                    p.Add("PI_ANEXO", usuario.ANEXO);
                    p.Add("PI_CELULAR", usuario.CELULAR);
                    p.Add("PI_ID_INSTITUCION", usuario.ID_INSTITUCION);
                    p.Add("PI_ID_ROL", usuario.ID_ROL);
                    //p.Add("PI_FLAG_ESTADO", usuario.FLAG_ESTADO);
                    p.Add("PI_UPD_USUARIO", usuario.UPD_USUARIO);
                    p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                    db.Execute(sp, p, commandType: CommandType.StoredProcedure);

                    int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;

                    seActualizo = filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return seActualizo;
        }
        #endregion
    }
}
