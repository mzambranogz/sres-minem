using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class EstrellaTrabajadorCamaBE : BaseBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public int ID_ESTRELLA { get; set; }
        public int ID_TRABAJADORES_CAMA { get; set; }
        public decimal EMISIONES_MIN { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
