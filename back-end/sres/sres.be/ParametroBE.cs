﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ParametroBE
    {
        public int ID_PARAMETRO { get; set; }
        public string NOMBRE { get; set; }
        public string ETIQUETA { get; set; }
        public int ID_TIPO_DATO { get; set; }
        public string DECIMAL_V { get; set; }
        public string VERIFICABLE { get; set; }
        public string EDITABLE { get; set; }
        public string OBTENIBLE { get; set; }
        public string ESTATICO { get; set; }
        public string UNIDAD { get; set; }
    }
}