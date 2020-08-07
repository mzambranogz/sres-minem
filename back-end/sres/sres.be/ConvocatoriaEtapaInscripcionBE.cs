using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ConvocatoriaEtapaInscripcionBE : BaseBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public int? ID_ETAPA { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public string REALIZADO { get; set; }
        public int ID_TIPO_EVALUACION { get; set; }
        public string OBSERVACION { get; set; }
        public int PUNTAJE { get; set; }
        public decimal EMISIONES_REDUCIDAS { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
