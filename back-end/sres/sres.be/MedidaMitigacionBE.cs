using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class MedidaMitigacionBE : BaseBE
    {
        public int ID_MEDMIT { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public string ARCHIVO_BASE { get; set; }
        public string ARCHIVO_CIFRADO { get; set; }
        public byte[] ARCHIVO_CONTENIDO { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
