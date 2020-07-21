using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class InscripcionDocumentoBE : BaseBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public int ID_CRITERIO { get; set; }
        public int ID_CASO { get; set; }
        public int ID_DOCUMENTO { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public string ARCHIVO_BASE { get; set; }
        public string ARCHIVO_CIFRADO { get; set; }
        public byte[] ARCHIVO_CONTENIDO { get; set; }
        public string ARCHIVO_TIPO { get; set; }
    }
}
