using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ActividadEconomicaBE : BaseBE
    {
        public int ID_INSTITUCION { get; set; }
        public int ID_ACTIVIDAD { get; set; }
        public string NOMBRE_ACTIVIDAD { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
