using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class ComponenteBE : BaseBE
    {
        public int ID_CASO { get; set; }        
        public int ID_CRITERIO { get; set; }
        public int ID_COMPONENTE { get; set; }
        public string NOMBRE { get; set; }
        public string INCREMENTABLE { get; set; }
        public string ID_FACTORES { get; set; }
    }
}
