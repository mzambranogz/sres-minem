using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ReconocimientoBE : BaseBE
    {
        public int ID_RECONOCIMIENTO { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public InscripcionBE INSCRIPCION { get; set; }
        public int? ID_INSIGNIA { get; set; }
        public InsigniaBE INSIGNIA { get; set; }
        public EstrellaBE ESTRELLA_E { get; set; }
        public List<ReconocimientoMedidaBE> LISTA_REC_MEDMIT { get; set; }
        public decimal PUNTAJE { get; set; }
        public string FLAG_CATEGORIA { get; set; }
        public int? ID_ESTRELLA { get; set; }
        public decimal EMISIONES { get; set; }
        public decimal ENERGIA { get; set; }
        public decimal COMBUSTIBLE { get; set; }
        public string FLAG_ESTRELLA { get; set; }
        public string FLAG_MEJORACONTINUA { get; set; }
        public string FLAG_EMISIONESMAX { get; set; }
        public string CATEGORIA { get; set; }
        public string ESTRELLA { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public int ID_PREMIACION { get; set; }
        public string ARCHIVO_BASE { get; set; }
        public string FECHA_CONVOCATORIA { get; set; }
        public string MES_CONVOCATORIA { get; set; }
        public string ANIO_CONVOCATORIA { get; set; }
        public DateTime FECHA_INICIO { get; set; }
        public string DESCRIPCION { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int PREMIO_MEDMIT { get; set; }        
    }
}
