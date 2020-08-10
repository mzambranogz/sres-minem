using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ConvocatoriaCriterioPuntajeInscripBE : BaseBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public int ID_CRITERIO { get; set; }
        public int ID_DETALLE { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public int ID_TIPO_EVALUACION { get; set; }
        public decimal EMISIONES_REDUCIDAS { get; set; }
        public string OBSERVACION { get; set; }
        public string NOMBRE_CRI { get; set; }
        public List<InscripcionDocumentoBE> LIST_INSCDOC { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
