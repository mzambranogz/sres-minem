using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class TrabajadorCamaBE : BaseBE
    {
        public int ID_TRABAJADORES_CAMA { get; set; }
        public int ID_SUBSECTOR_TIPOEMPRESA { get; set; }
        public string NOMBRE { get; set; }
        public string MAYOR_SIGNO { get; set; }
        public string MENOR_SIGNO { get; set; }
        public int MAYOR_VALOR { get; set; }
        public int MENOR_VALOR { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
