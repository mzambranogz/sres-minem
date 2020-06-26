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
        public DateTime FECHA_INICIO { get; set; }
        public DateTime FECHA_FIN { get; set; }
        public int LIMITE_POSTULANTE { get; set; }        
        public string FLAG_ESTADO { get; set; }

    }
}
