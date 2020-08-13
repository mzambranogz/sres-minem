using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class InscripcionBE : BaseBE
    {
        public int ID_INSCRIPCION { get; set; }
        public int? ID_CONVOCATORIA { get; set; }
        public ConvocatoriaBE CONVOCATORIA { get; set; }
        public int? ID_INSTITUCION { get; set; }
        public InstitucionBE INSTITUCION { get; set; }
        public List<InscripcionRequerimientoBE> LISTA_INSCRIPCION_REQUERIMIENTO { get; set; }
        public bool FLAG_ANULAR { get; set; }
        public string NRO_INFORME_PRELIMINAR { get; set; }
        public int? ID_TIPO_EVALUACION { get; set; }
        public string OBSERVACION { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int? UPD_USUARIO { get; set; }
        public DateTime UPD_FECHA { get; set; }
        public UsuarioBE USUARIO { get; set; }
        public int CANTIDADCRITERIOSINGRESADOS { get; set; }
        public int PUNTOSACUMULADOS { get; set; }
        public string NOMBRE_CONV { get; set; }
        public string CORREO { get; set; }
        public string NOMBRES_USU { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public List<InsigniaBE> ASPIRACIONES { get; set; }
    }
}
