﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;
using sres.ut;

namespace sres.ln
{
    public class UsuarioLN
    {
        public static UsuarioDA usuarioDA = new UsuarioDA();

        public static List<UsuarioBE> ListaUsuario()
        {
            return usuarioDA.ListaUsuario();
        }

        public static UsuarioBE ObtenerUsuarioPorCorreo(string correo)
        {
            return usuarioDA.ObtenerUsuarioPorCorreo(correo);
        }

        public static bool ValidarUsuario(string correo, string contraseña, out UsuarioBE outUsuario)
        {
            outUsuario = null;

            bool esValido = false;

            outUsuario = usuarioDA.ObtenerUsuarioPorCorreo(correo);

            bool esContraseñaValida = Seguridad.CompararHashSal(contraseña, outUsuario.CONTRASENA);

            return esValido;
        }
    }
}
