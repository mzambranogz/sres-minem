using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class IndicadorBE
    {
        public ParametroBE OBJ_PARAMETRO { get; set; }
        public int ID_INDICADOR { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public List<IndicadorDataBE> LIST_INDICADORDATA { get; set; }
        public List<IndicadorFormBE> LIST_INDICADORFORM { get; set; }
        public List<IndicadorBE> LISTA_IND { get; set; }
        public List<ParametroBE> LISTA_PARAM { get; set; }
        public string FORMULA { get; set; }
        public string FORMULA_ARMADO { get; set; }
        public string COMPORTAMIENTO { get; set; }
        public int INS { get; set; }
        public string ID_ACTIVO { get; set; }
        public string CRITERIO { get; set; }
        public string CASO { get; set; }
        public string COMPONENTE { get; set; }
        public string ID_FACTORES { get; set; }
        public string INCREMENTABLE { get; set; }
        public int FLAG_NUEVO { get; set; }
    }
}
