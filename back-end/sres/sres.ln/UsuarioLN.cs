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

                    seGuardoInstitucion = usuario.INSTITUCION == null ? true : institucionDA.GuardarInstitucion(usuario.INSTITUCION, cn, out idInstitucion, ot);

                    if (seGuardoInstitucion)
                    {
                        usuario.ID_INSTITUCION = usuario.INSTITUCION == null ? null : (int?)idInstitucion;
                        usuario.CONTRASENA = string.IsNullOrEmpty(usuario.CONTRASENA) ? null : Seguridad.hashSal(usuario.CONTRASENA);
                        seGuardo = usuarioDA.GuardarUsuario(usuario, cn, ot);
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
    }
}
