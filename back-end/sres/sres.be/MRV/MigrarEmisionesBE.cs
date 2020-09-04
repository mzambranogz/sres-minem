using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be.MRV
{
    public class MigrarEmisionesBE : BaseBE
    {
        public int ID_INICIATIVA { get; set; }
        public string DESC_INICIATIVA { get; set; }
        public string RUC { get; set; }
        public decimal REDUCIDO { get; set; }
        public int ID_MEDMIT { get; set; }
        public string NOMBRE_MEDMIT { get; set; }
        public int VALIDACION { get; set; }
        public string ARCHIVO_BASE { get; set; }
        public List<MigrarEmisionesBE> LISTA_MIGRAR { get; set; }
    }
}
