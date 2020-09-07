using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ReconocimientoMedidaBE : BaseBE
    {
        public int ID_RECONOCIMIENTO { get; set; }
        public int ID_MEDMIT { get; set; }
        public string NOMBRE_MEDMIT { get; set; }
        public decimal REDUCIDO { get; set; }
        public string OBTENIDO { get; set; }
        public string ARCHIVO_BASE { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
