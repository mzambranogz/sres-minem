using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class ConvocatoriaBE
    {
        public List<RequerimientoBE> LISTA_REQ { get; set; }
        public List<CriterioBE> LISTA_CRI { get; set; }
        public List<UsuarioBE> LISTA_EVA { get; set; }
        public List<EtapaBE> LISTA_ETA { get; set; }
        public List<InstitucionBE> LIST_INSTITUCION { get; set; }
        public List<InsigniaBE> LISTA_INSIG { get; set; }
        public List<EstrellaTrabajadorCamaBE> LISTA_ESTRELLA_TRAB { get; set; }
        public List<ConvocatoriaCriterioRequerimientoBE> LISTA_CONVOCATORIA_CRITERIO_REQUERIMIENTO { get; set; }
        public string TXT_FECHA_INICIO { get; set; }
        public string TXT_FECHA_FIN { get; set; }
        public int ID_REQUERIMIENTO { get; set; }
        public int ID_CRITERIO { get; set; }
        public int ID_USUARIO { get; set; }
        public string NOMBRE_ETAPA { get; set; }
        public int DIAS { get; set; }
        public string CORREO { get; set; }
        public int VALIDAR_ETAPA { get; set; }
        public int CODIGO { get; set; }
        public int FLAG_ANULAR { get; set; }
    }
}
