using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class CasoBE
    {
        public List<ComponenteBE> LIST_COMPONENTE { get; set; }
        public List<InscripcionDocumentoBE> LIST_DOCUMENTO { get; set; }
        public List<DocumentoBE> LIST_DOC { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public int ID_CONVOCATORIA { get; set; }
        public int ID_ETAPA { get; set; }
        public string NOMBRE_CRI { get; set; }
        public decimal EMISIONES { get; set; }
        public decimal ENERGIA { get; set; }
        public decimal COMBUSTIBLE { get; set; }
    }
}
