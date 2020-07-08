using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class IndicadorBE
    {
        public ParametroBE OBJ_PARAMETRO { get; set; }
        public int ID_INDICADOR {get; set;}
        public int ID_INSCRIPCION { get; set; }
        public List<IndicadorDataBE> LIST_INDICADORDATA { get; set; }
        public List<IndicadorFormBE> LIST_INDICADORFORM { get; set; }
        public int FLAG_NUEVO { get; set; }
    }
}
