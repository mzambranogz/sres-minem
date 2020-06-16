using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;

namespace sres.ln
{
    public class UsuarioLN
    {
        public static UsuarioDA aDA = new UsuarioDA();

        public static List<UsuarioBE> ListaUsuario()
        {
            return aDA.ListaUsuario();
        }
    }
}
