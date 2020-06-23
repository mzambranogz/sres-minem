using sres.be.MRV;
using sres.da.MRV;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln.MRV
{
    public class UsuarioLN : BaseLN
    {
        static UsuarioDA usuarioDA = new UsuarioDA();

        public static bool VerificarRucCorreo(string ruc, string correo)
        {
            return usuarioDA.VerificarRucCorreo(ruc, correo, cn);
        }

        public static Dictionary<string, object> ValidarLoginUsuario(string ruc, string correo, string contraseña)
        {
            UsuarioBE usuario = usuarioDA.ObtenerUsuarioPorRucCorreo(ruc, correo, cn);
            bool contraseñaCorrecta = usuario == null ? false : Seguridad.CompararHashSal(contraseña, usuario.PASSWORD_USUARIO);
            return new Dictionary<string, object>
            {
                ["VALIDO"] = contraseñaCorrecta,
                ["USUARIO"] = usuario != null ? (!contraseñaCorrecta ? null : usuario) : usuario
            };
        }
    }
}
