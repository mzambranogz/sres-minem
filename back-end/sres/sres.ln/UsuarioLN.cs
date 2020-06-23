using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;
using sres.ut;
using Oracle.DataAccess.Client;

namespace sres.ln
{
    public class UsuarioLN : BaseLN
    {
        static UsuarioDA usuarioDA = new UsuarioDA();
        static InstitucionDA institucionDA = new InstitucionDA();

        public static List<UsuarioBE> ListaUsuario()
        {
            return usuarioDA.ListaUsuario(cn);
        }

        public static UsuarioBE ObtenerUsuarioPorCorreo(string correo)
        {
            return usuarioDA.ObtenerUsuarioPorCorreo(correo, cn);
        }

        public static List<UsuarioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return usuarioDA.BuscarUsuario(busqueda, registros, pagina, columna, orden, cn);
        }

        public static UsuarioBE ObtenerUsuario(int idUsuario)
        {
            return usuarioDA.ObtenerUsuario(idUsuario, cn);
        }

        public static UsuarioBE ObtenerUsuarioPorInstitucionCorreo(int idInstitucion, string correo)
        {
            return usuarioDA.ObtenerUsuarioPorInstitucionCorreo(idInstitucion, correo, cn);
        }

        public static bool CambiarEstadoUsuario(UsuarioBE usuario)
        {
            return usuarioDA.CambiarEstadoUsuario(usuario, cn);
        }

        public static bool GuardarUsuario(UsuarioBE usuario)
        {
            return usuarioDA.GuardarUsuario(usuario, cn);
        }

        public static bool ValidarUsuario(string correo, string contraseña, out UsuarioBE outUsuario)
        {
            outUsuario = null;

            bool esValido = false;

            outUsuario = usuarioDA.ObtenerUsuarioPorCorreo(correo, cn);

            esValido = outUsuario != null;

            if(esValido) esValido = Seguridad.CompararHashSal(contraseña, outUsuario.CONTRASENA);

            return esValido;
        }

        public static bool RegistrarUsuario(UsuarioBE usuario)
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
                        usuario.ID_INSTITUCION = idInstitucion;
                        seGuardo = usuarioDA.GuardarUsuario(usuario, cn, ot);
                    }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
                cn.Close();
            }
            catch (Exception ex) { Log.Error(ex); }


            return seGuardo;
        }

        public static List<UsuarioBE> getAllEvaluador()
        {
            return usuarioDA.getAllEvaluador(cn);
        }
    }
}
