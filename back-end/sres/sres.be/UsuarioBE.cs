using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class UsuarioBE
    {
        public int ID_USUARIO { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string CORREO { get; set; }
        public string CONTRASENA { get; set; }
        public string TELEFONO { get; set; }
        public string ANEXO { get; set; }
        public string CELULAR { get; set; }
        public int? ID_INSTITUCION { get; set; }
        public InstitucionBE INSTITUCION { get; set; }
        public int? ID_ROL { get; set; }
        public RolBE ROL { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int UPD_USUARIO { get; set; }
    }
}
