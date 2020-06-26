using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class RolBE : BaseBE
    {
        public int ID_ROL { get; set; }
        public string NOMBRE { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int UPD_USUARIO { get; set; }
    }
}
