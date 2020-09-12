using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ReporteBE
    {
        public class ReporteEstadisticoXTipoEmpresa
        {
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_TIPOEMPRESA { get; set; }
            public decimal CANTIDAD_INSCRIPCIONES { get; set; }
            public decimal PORCENTAJE_INSCRIPCIONES { get; set; }
        }
}
