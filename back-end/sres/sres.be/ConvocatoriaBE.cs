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
        public string DESCRIPCION { get; set; }
        public DateTime FECHA_INICIO { get; set; }
        public DateTime FECHA_FIN { get; set; }
        public int LIMITE_POSTULANTE { get; set; }
        public string NRO_INFORME { get; set; }
        public string REQUERIMIENTO { get; set; }
        public string CRITERIO { get; set; }
        public string EVALUADOR { get; set; }
        public int? ID_ETAPA { get; set; }
        public EtapaBE ETAPA { get; set; }
        public string FLAG_ESTADO { get; set; }

    }
}
