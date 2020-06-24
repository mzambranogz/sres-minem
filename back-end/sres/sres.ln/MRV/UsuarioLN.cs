using sres.be.MRV;
using sres.da.MRV;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln.MRV
{
    public class UsuarioLN : BaseLN
    {
        UsuarioDA usuarioDA = new UsuarioDA();

        public bool VerificarCorreo(string correo)
        {
            bool valor = false;

            try
            {
                cn.Open();
                valor = usuarioDA.VerificarCorreo(correo, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return valor;
        }

        public Dictionary<string, object> ValidarLoginUsuario(string correo, string contraseña)
        {
            Dictionary<string, object> valor = null;

            try
            {
                cn.Open();
                UsuarioBE usuario = usuarioDA.ObtenerUsuarioPorCorreo(correo, cn);
                bool contraseñaCorrecta = usuario == null ? false : Seguridad.CompararHashSal(contraseña, usuario.PASSWORD_USUARIO);

                valor = new Dictionary<string, object>
                {
                    ["VALIDO"] = contraseñaCorrecta,
                    ["USUARIO"] = usuario != null ? (!contraseñaCorrecta ? null : usuario) : usuario
                };
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return valor;
        }
    }
}