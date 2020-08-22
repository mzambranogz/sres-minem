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
        #region PAQUETE ADMIN
        public List<UsuarioBE> ListaUsuario(OracleConnection db)
        {
            List<UsuarioBE> lista = null;

            try
            {
                    string sp = $"{Package.Admin}USP_SEL_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PO", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    lista = db.Query<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public UsuarioBE ObtenerUsuarioPorCorreo(string correo, OracleConnection db)
        {
            UsuarioBE item = null;

            try
            {
                    string sp = $"{Package.Admin}USP_SEL_USUARIO_CORREO";
                    OracleDynamicParameters p = new OracleDynamicParameters();
                    p.Add("pCORREO", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: correo);
                    p.Add("pCURSOR", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }
        #endregion

        #region PAQUETE MANTENIMIENTO
        public List<UsuarioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden, OracleConnection db)
        {
            List<UsuarioBE> lista = null;

            try
            {
                    string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_USUARIO";
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
                            FLAG_ESTADO = (string)x.FLAG_ESTADO
                        })
                        .ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<UsuarioBE> ListarUsuarioPorRol(int idRol, OracleConnection db)
        {
            List<UsuarioBE> lista = null;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_USUARIO_ROL";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ROL", idRol);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public UsuarioBE ObtenerUsuario(int idUsuario, OracleConnection db)
        {
            UsuarioBE item = null;

            try
            {
                    string sp = $"{Package.Mantenimiento}USP_SEL_OBTIENE_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_ID_USUARIO", idUsuario);
                    p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public UsuarioBE ObtenerUsuarioPorInstitucionCorreo(int idInstitucion, string correo, OracleConnection db)
        {
            UsuarioBE item = null;

            try
            {
                    string sp = $"{Package.Mantenimiento}USP_SEL_OBTIENE_USUARIO_INSTITUCION_CORREO";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_ID_INSTITUCION", idInstitucion);
                    p.Add("PI_CORREO", correo);
                    p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    item = db.QueryFirstOrDefault<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public bool CambiarEstadoUsuario(UsuarioBE usuario, OracleConnection db)
        {
            bool seActualizo = false;

            try
            {
                    string sp = $"{Package.Mantenimiento}USP_UPD_CAMBIA_ESTADO_USUARIO";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_ID_USUARIO", usuario.ID_USUARIO);
                    p.Add("PI_FLAG_ESTADO", usuario.FLAG_ESTADO);
                    p.Add("PI_UPD_USUARIO", usuario.UPD_USUARIO);
                    p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                    db.Execute(sp, p, commandType: CommandType.StoredProcedure);

                    int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;

                    seActualizo = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seActualizo;
        }

        public bool GuardarUsuario(UsuarioBE usuario, OracleConnection db)
        {
            bool seActualizo = false;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_MAN_GUARDA_USUARIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_USUARIO", usuario.ID_USUARIO, direction: ParameterDirection.InputOutput);
                p.Add("PI_NOMBRES", usuario.NOMBRES);
                p.Add("PI_APELLIDOS", usuario.APELLIDOS);
                p.Add("PI_CORREO", usuario.CORREO);
                p.Add("PI_CONTRASENA", usuario.CONTRASENA);
                p.Add("PI_TELEFONO", usuario.TELEFONO);
                p.Add("PI_ANEXO", usuario.ANEXO);
                p.Add("PI_CELULAR", usuario.CELULAR);
                p.Add("PI_ID_INSTITUCION", usuario.ID_INSTITUCION);
                p.Add("PI_ID_ROL", usuario.ID_ROL);
                p.Add("PI_FLAG_ESTADO", usuario.FLAG_ESTADO);
                p.Add("PI_UPD_USUARIO", usuario.UPD_USUARIO);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);

                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;

                seActualizo = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return seActualizo;
        }

        public List<UsuarioBE> getAllEvaluador(OracleConnection db)
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_EVALUADOR";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        public UsuarioBE getAdministrador(OracleConnection db)
        {
            UsuarioBE usuario = new UsuarioBE();
            try
            {
                string sp = $"{Package.Verificacion}USP_SEL_GET_ADMIN";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                usuario = db.Query<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return usuario;
        }

        public List<UsuarioBE> ListarUsuarioResponsable(int idConvocatoria, OracleConnection db)
        {
            List<UsuarioBE> lista = null;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_USUARIO_RESP";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<UsuarioBE> ListarUsuarioResponsableAll(int idConvocatoria, OracleConnection db)
        {
            List<UsuarioBE> lista = null;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_USUARIO_RESP_ALL";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CONVOCATORIA", idConvocatoria);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<UsuarioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
        #endregion
    }
}
