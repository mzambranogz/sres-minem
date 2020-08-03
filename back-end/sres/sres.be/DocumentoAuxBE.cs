using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class DocumentoBE
    {
        public int ID_CONVOCATORIA { get; set; }
        public int ID_CASO { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public int ID_TIPO_EVALUACION { get; set; }
        public InscripcionDocumentoBE OBJ_INSCDOC { get; set; }
    }
}
