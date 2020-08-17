using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class InstitucionContactoBE : BaseBE
    {
        public int ID_INSTITUCION { get; set; }
        public int ID_CONTACTO { get; set; }
        public string NOMBRE { get; set; }
        public string CARGO { get; set; }
        public string TELEFONO { get; set; }
        public string CORREO { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
