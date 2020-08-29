using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class FactorBE : BaseBE
    {
        public int ID_FACTOR { get; set; }
        public string NOMBRE { get; set; }
        public string SOBRE_NOMBRE { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
