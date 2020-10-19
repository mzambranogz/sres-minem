using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class PuntajeBE : BaseBE
    {
        public int ID_CRITERIO { get; set; }
        public int ID_DETALLE { get; set; }
        public string DESCRIPCION { get; set; }
        public decimal PUNTAJE { get; set; }
        public string CRITERIO { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
