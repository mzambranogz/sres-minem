using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class FactorDataBE : BaseBE
    {
        public int ID_FACTOR { get; set; }
        public int ID_DETALLE { get; set; }
        public string ID_PARAMETROS { get; set; }
        public string VALORES { get; set; }
        public decimal FACTOR { get; set; }
        public string UNIDAD { get; set; }
        public int NUMERO_PARAMETROS { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
