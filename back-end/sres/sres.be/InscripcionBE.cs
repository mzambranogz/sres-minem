using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class InscripcionBE
    {
        public int ID_INSCRIPCION { get; set; }
        public int? ID_CONVOCATORIA { get; set; }
        public ConvocatoriaBE CONVOCATORIA { get; set; }
        public int? ID_INSTITUCION { get; set; }
        public InstitucionBE INSTITUCION { get; set; }
        public List<InscripcionRequerimientoBE> LISTA_INSCRIPCION_REQUERIMIENTO { get; set; }
        public bool FLAG_ANULAR { get; set; }
        public string NRO_INFORME_PRELIMINAR { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int? UPD_USUARIO { get; set; }
    }
}
