using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class InstitucionBE
    {
        public int? ID_RECONOCIMIENTO { get; set; }
        public int? ID_INSCRIPCION { get; set; }
        public int? ID_CONVOCATORIA { get; set; }
        public int? ID_USUARIO { get; set; }
        public string PRIMER_INICIO { get; set; }
        public string NOMBRE_SECTOR { get; set; }
        public int VALIDARPIDE { get; set; }
        public List<ActividadEconomicaBE> LISTA_ACTIVIDAD { get; set; }
        public List<InstitucionContactoBE> LISTA_CONTACTO { get; set; }
        public List<InscripcionTrazabilidadBE> LISTA_INSC_TRAZ { get; set; }
        public ConvocatoriaEvaluadorPostulanteBE CONV_EVA_POS { get; set; }
    }
}
