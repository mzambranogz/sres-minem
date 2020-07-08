using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class IndicadorDataBE : BaseBE
    {
        public int ID_CRITERIO { get; set; }
        public int ID_CASO { get; set; }
        public int ID_COMPONENTE { get; set; }
        public int ID_PARAMETRO { get; set; }
        public int ID_INDICADOR { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public string VALOR { get; set; }
    }
}
