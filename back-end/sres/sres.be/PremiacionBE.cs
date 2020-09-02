using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class PremiacionBE : BaseBE
    {
        public int ID_PREMIACION { get; set; }
        public int ID_INSIGNIA { get; set; }
        public int ID_ESTRELLA { get; set; }
        public string INSIGNIA { get; set; }
        public string ESTRELLA { get; set; }
        public string ARCHIVO_BASE { get; set; }
        public string ARCHIVO_CIFRADO { get; set; }
        public byte[] ARCHIVO_CONTENIDO { get; set; }
        public string FLAG_ESTADO { get; set; }

    }
}
