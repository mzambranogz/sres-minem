using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class DataPaginateBE
    {
        public object DATA { get; set; }
        public int CANTIDAD_REGISTROS { get; set; }
        public int TOTAL_PAGINAS { get; set; }
        public int PAGINA { get; set; }
        public int TOTAL_REGISTROS { get; set; }
    }
}
