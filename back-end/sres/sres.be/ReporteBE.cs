using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ReporteBE
    {
        public class ReporteEstadisticoTipoEmpresaXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_TIPOEMPRESA { get; set; }
            public decimal CANTIDADTOTAL_INSCRIPCIONES { get; set; }
            public decimal CANTIDAD_INSCRIPCIONES { get; set; }
            public decimal PORCENTAJE_INSCRIPCIONES { get; set; }
        }

        public class ReporteEstadisticoTipoPostulanteXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_TIPOPOSTULANTE { get; set; }
            public decimal CANTIDAD_POSTULANTES { get; set; }
        }

        public class ReporteEstadisticoInsigniaXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_SUBSECTOR { get; set; }
            public string INSIGNIA { get; set; }
            public decimal CANTIDAD { get; set; }
        }

        public class ReporteEstadisticoEstrellaXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_SUBSECTOR { get; set; }
            public string ESTRELLA { get; set; }
            public decimal CANTIDAD { get; set; }
        }

        public class ReporteEstadisticoMejoraContinuaXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_SUBSECTOR { get; set; }
            public string MEJORACONTINUA { get; set; }
            public decimal CANTIDAD { get; set; }
        }
    }
}
