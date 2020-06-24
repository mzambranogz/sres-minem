using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class ConvocatoriaBE : BaseBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public string NOMBRE { get; set; }
        public int LIMITE_POSTULANTE { get; set; }
        public string REQUERIMIENTO { get; set; }
        public string CRITERIO { get; set; }
        public string EVALUADOR { get; set; }
        public string FLAG_ESTADO { get; set; }

    }
}
