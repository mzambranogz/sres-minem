using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class EtapaBE : BaseBE
    {
        public int ID_ETAPA { get; set; }
        public string NOMBRE { get; set; }
        public int ID_PROCESO { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
