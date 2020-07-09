using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ConvocatoriaCriterioRequerimientoBE
    {
        public int? ID_CONVOCATORIA { get; set; }
        public ConvocatoriaBE CONVOCATORIA { get; set; }
        public int? ID_CRITERIO { get; set; }
        public CriterioBE CRITERIO { get; set; }
        public int? ID_REQUERIMIENTO { get; set; }
        public RequerimientoBE REQUERIMIENTO { get; set; }
        public string OBLIGATORIO { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int? UPD_USUARIO { get; set; }
    }
}
