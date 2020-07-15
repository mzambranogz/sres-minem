using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace sres.be
{
    public class InscripcionRequerimientoBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public ConvocatoriaBE CONVOCATORIA { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public InscripcionBE INSCRIPCION { get; set; }
        public int ID_REQUERIMIENTO { get; set; }
        public RequerimientoBE REQUERIMIENTO { get; set; }
        public string ARCHIVO_BASE { get; set; }
        public string ARCHIVO_CIFRADO { get; set; }
        public byte[] ARCHIVO_CONTENIDO { get; set; }
        public string ARCHIVO_TIPO { get; set; }
        public bool? VALIDO { get; set; }
        public string OBSERVACION { get; set; }
        public int? UPD_USUARIO { get; set; }
        public int? ID_INSTITUCION { get; set; }
    }
}
