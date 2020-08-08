using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class SectorBE : BaseBE
    {
        public int ID_SECTOR { get; set; }
        public string NOMBRE { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int UPD_USUARIO { get; set; }
        public List<SubsectorTipoempresaBE> LISTA_SUBSEC_TIPOEMP { get; set; }
    }
}
