using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class SubsectorTipoempresaBE : BaseBE
    {
        public int ID_SUBSECTOR_TIPOEMPRESA { get; set; }
        public int ID_SECTOR { get; set; }
        public string NOMBRE { get; set; }
        public string FLAG_ESTADO { get; set; }
        public List<TrabajadorCamaBE> LISTA_TRAB_CAMA { get; set; }
    }
}
