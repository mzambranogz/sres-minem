﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class IndicadorDataBE
    {
        public int ID_TIPO_CONTROL { get; set; }
        public int ID_TIPO_DATO { get; set; }
        public string DECIMAL_V { get; set; }
        public string VERIFICABLE { get; set; }
        public string EDITABLE { get; set; }
        public string OBTENIBLE { get; set; }
        public string ESTATICO { get; set; }
        public string NOMBRE { get; set; }
        public string UNIDAD { get; set; }
        public string FILTRO { get; set; }
        public string RESULTADO { get; set; }
        public string EMISIONES { get; set; }
        public decimal ENERGIA { get; set; }
        public decimal COMBUSTIBLE { get; set; }
        public decimal CAMBIO_MATRIZ { get; set; }
        public string AHORRO { get; set; }
        public int TAMANO { get; set; }
        public string DESCRIPCION { get; set; }
        public string VISIBLE { get; set; }
        public List<ParametroDetalleBE> LIST_PARAMDET { get; set; }
    }
}
