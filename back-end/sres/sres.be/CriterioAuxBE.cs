using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class CriterioBE
    {
        public List<RequerimientoBE> LISTA_REQUERIMIENTO { get; set; }
        public List<CasoBE> LISTA_CASO { get; set; }
        public List<DocumentoBE> LISTA_DOCUMENTO { get; set; }
        public List<ConvocatoriaCriterioPuntajeBE> LISTA_CONVCRIPUNT { get; set; }
        public int ID_CONVOCATORIA { get; set; }
        public int INGRESO_DATOS { get; set; }
    }
}
