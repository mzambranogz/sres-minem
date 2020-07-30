using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;
using sres.ut;
using Oracle.DataAccess.Client;
using System.Data;

namespace sres.ln
{
    public class UsuarioLN : BaseLN
    {
        UsuarioDA usuarioDA = new UsuarioDA();
        InstitucionDA institucionDA = new InstitucionDA();

        da.MRV.UsuarioDA usuarioDAMRV = new da.MRV.UsuarioDA();
        da.MRV.InstitucionDA institucionDAMRV = new da.MRV.InstitucionDA();

        public List<UsuarioBE> ListaUsuario()
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();

            try
            {
                cn.Open();
                lista = usuarioDA.ListaUsuario(cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public UsuarioBE ObtenerUsuarioPorCorreo(string correo)
        {
            UsuarioBE item = null;

            try
            {
                cn.Open();
                item = usuarioDA.ObtenerUsuarioPorCorreo(correo, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<UsuarioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();

            try
            {
                cn.Open();
                lista = usuarioDA.BuscarUsuario(busqueda, registros, pagina, columna, orden, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<UsuarioBE> ListarUsuarioPorRol(int idRol)
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();

            try
            {
                cn.Open();
                lista = usuarioDA.ListarUsuarioPorRol(idRol, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public UsuarioBE ObtenerUsuario(int idUsuario)
        {
            UsuarioBE item = null;

            try
            {
                cn.Open();
                item = usuarioDA.ObtenerUsuario(idUsuario, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public UsuarioBE ObtenerUsuarioPorInstitucionCorreo(int idInstitucion, string correo)
        {
            UsuarioBE item = null;

            try
            {
                cn.Open();
                item = usuarioDA.ObtenerUsuarioPorInstitucionCorreo(idInstitucion, correo, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public bool CambiarEstadoUsuario(UsuarioBE usuario)
        {
            bool valor = false;

            try
            {
                cn.Open();
                valor = usuarioDA.CambiarEstadoUsuario(usuario, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return valor;
        }

        public bool GuardarUsuario(UsuarioBE usuario)
        {
            bool seGuardo = false;

            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    int idInstitucion = -1;
                    bool seGuardoInstitucion = false;

                    seGuardoInstitucion = usuario.INSTITUCION == null ? true : institucionDA.GuardarInstitucion(usuario.INSTITUCION, cn, out idInstitucion);

                    if (seGuardoInstitucion)
                    {
                        usuario.ID_INSTITUCION = usuario.INSTITUCION == null ? null : (int?)idInstitucion;
                        usuario.CONTRASENA = string.IsNullOrEmpty(usuario.CONTRASENA) ? null : Seguridad.hashSal(usuario.CONTRASENA);
                        seGuardo = usuarioDA.GuardarUsuario(usuario, cn);
                    }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }


            return seGuardo;
        }

        public bool ValidarUsuario(string correo, string contraseña, out UsuarioBE outUsuario)
        {
            outUsuario = null;

            bool esValido = false;

            try
            {
                cn.Open();
                outUsuario = usuarioDA.ObtenerUsuarioPorCorreo(correo, cn);

                esValido = outUsuario != null;

                if (esValido) esValido = Seguridad.CompararHashSal(contraseña, outUsuario.CONTRASENA);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return esValido;
        }
        
        public List<UsuarioBE> getAllEvaluador()
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();

            try
            {
                cn.Open();
                lista = usuarioDA.getAllEvaluador(cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public bool MigrarUsuario(string correo, out bool existeUsuario, out bool existeUsuarioMRV, out bool existeInstitucion, out bool seGuardoInstitucion)
        {
            bool seMigro = false;
            existeUsuario = false;
            existeUsuarioMRV = false;
            existeInstitucion = false;
            seGuardoInstitucion = false;

            try
            {
                cn.Open();
                OracleTransaction ot = cn.BeginTransaction();

                UsuarioBE usuario = usuarioDA.ObtenerUsuarioPorCorreo(correo, cn);

                existeUsuario = usuario != null;

                if (!existeUsuario)
                {
                    be.MRV.UsuarioBE usuarioMRV = usuarioDAMRV.ObtenerUsuarioPorCorreo(correo, cn);

                    existeUsuarioMRV = usuarioMRV != null;

                    if (existeUsuarioMRV)
                    {
                        be.MRV.InstitucionBE institucionMRV = institucionDAMRV.ObtenerInstitucion(usuarioMRV.ID_INSTITUCION, cn);

                        be.InstitucionBE institucion = institucionDA.ObtenerInstitucionPorRuc(institucionMRV.RUC_INSTITUCION, cn);

                        existeInstitucion = institucion != null;

                        int idInstitucion = -1;

                        seGuardoInstitucion = false;

                        if (!existeInstitucion)
                        {
                            institucion = new InstitucionBE
                            {
                                ID_INSTITUCION = idInstitucion,
                                RUC = institucionMRV.RUC_INSTITUCION,
                                RAZON_SOCIAL = institucionMRV.NOMBRE_INSTITUCION,
                                DOMICILIO_LEGAL = institucionMRV.DIRECCION_INSTITUCION,
                                ID_SECTOR = institucionMRV.ID_SECTOR_INSTITUCION,
                                FLAG_ESTADO = institucionMRV.FLAG_ESTADO
                            };

                            seGuardoInstitucion = institucionDA.GuardarInstitucion(institucion, cn, out idInstitucion);
                        }

                        usuario = new UsuarioBE
                        {
                            ID_USUARIO = -1,
                            NOMBRES = usuarioMRV.NOMBRES_USUARIO,
                            APELLIDOS = usuarioMRV.APELLIDOS_USUARIO,
                            CORREO = usuarioMRV.EMAIL_USUARIO,
                            CONTRASENA = usuarioMRV.PASSWORD_USUARIO,
                            TELEFONO = usuarioMRV.TELEFONO_USUARIO,
                            ANEXO = usuarioMRV.ANEXO_USUARIO,
                            CELULAR = usuarioMRV.CELULAR_USUARIO,
                            FLAG_ESTADO = "1",
                            ID_INSTITUCION = existeInstitucion ? institucion.ID_INSTITUCION : (seGuardoInstitucion ? (int?)idInstitucion : null),
                            ID_ROL = 3
                        };

                        if (seGuardoInstitucion)
                        {
                            seMigro = usuarioDA.GuardarUsuario(usuario, cn);
                        }
                    }
                }

                if (seMigro) ot.Commit();
                else ot.Rollback();
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seMigro;
        }
    }
}
