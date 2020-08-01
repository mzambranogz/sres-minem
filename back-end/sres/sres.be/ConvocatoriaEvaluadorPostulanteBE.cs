using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ConvocatoriaEvaluadorPostulanteBE : BaseBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public int ID_USUARIO { get; set; }
        public int id_INSTITUCION { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}
