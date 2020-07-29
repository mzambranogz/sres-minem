using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class FactorParametroBE : BaseBE
    {
        public int ID_FACTOR { get; set; }
        public int ID_DETALLE { get; set; }
        public string NOMBRE_DETALLE { get; set; }
        public int ID_PARAMETRO { get; set; }
        public int ORDEN { get; set; }
        public int NUMERO_PARAMETROS { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
