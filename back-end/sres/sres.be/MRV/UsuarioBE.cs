using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be.MRV
{
    public class UsuarioBE
    {
        public int ID_USUARIO { get; set; }
        public string NOMBRES_USUARIO { get; set; }
        public string APELLIDOS_USUARIO { get; set; }
        public string EMAIL_USUARIO { get; set; }
        public string PASSWORD_USUARIO { get; set; }
        public string TELEFONO_USUARIO { get; set; }
        public string ANEXO_USUARIO { get; set; }
        public string CELULAR_USUARIO { get; set; }
        public string FLG_ESTADO { get; set; }
        public int ID_INSTITUCION { get; set; }
        public  InstitucionBE INSTITUCION { get; set; }
    }
}
