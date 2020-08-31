using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class ComponenteBE
    {
        public List<IndicadorBE> LIST_INDICADOR_HEAD { get; set; }
        public List<IndicadorBE> LIST_INDICADOR_BODY { get; set; }
        public List<IndicadorBE> LIST_INDICADOR { get; set; }
        public string ELIMINAR_INDICADOR { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public string CASO { get; set; }
        public string CRITERIO { get; set; }
        public List<FactorBE> LISTA_FAC_cOMP { get; set; }
    }
}
