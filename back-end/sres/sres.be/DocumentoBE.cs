using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class DocumentoBE : BaseBE
    {
        public int ID_DOCUMENTO { get; set; }
        public int ID_CRITERIO { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public string OBLIGATORIO { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
