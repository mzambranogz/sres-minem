using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ConvocatoriaInsigniaBE : BaseBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public int ID_INSIGNIA { get; set; }
        public string NOMBRE_INSIGNIA { get; set; }
        public string ARCHIVO_BASE { get; set; }
        public int PUNTAJE_MIN { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}

