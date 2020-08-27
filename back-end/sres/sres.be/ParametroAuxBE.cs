using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class ParametroBE
    {
        public List<ParametroDetalleBE> LISTA_DET { get; set; }
        public List<ParametroBE> LISTA_PARAM { get; set; }
        public string ID_DELETE_DETALLE { get; set; }
        public string TIPO_CONTROL { get; set; }
        public string FORMULA { get; set; }
    }
}
