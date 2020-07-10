﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class CriterioBE : BaseBE
    {
        public int ID_CRITERIO { get; set; }
        public string NOMBRE { get; set; }
        public string FLAG_ESTADO { get; set; }
        public List<RequerimientoBE> LISTA_REQUERIMIENTO { get; set; }
        public List<CasoBE> LISTA_CASO { get; set; }
    }
}
