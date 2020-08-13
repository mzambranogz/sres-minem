﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ReconocimientoBE : BaseBE
    {
        public int ID_RECONOCIMIENTO { get; set; }
        public int ID_INSCRIPCION { get; set; }
        public InscripcionBE INSCRIPCION { get; set; }
        public int? ID_INSIGNIA { get; set; }
        public InsigniaBE INSIGNIA { get; set; }
        public int PUNTAJE { get; set; }
        public string FLAG_CATEGORIA { get; set; }
        public int ID_ESTRELLA { get; set; }
        public decimal EMISIONES { get; set; }
        public string FLAG_ESTRELLA { get; set; }
        public string FLAG_MEJORACONTINUA { get; set; }
        public string FLAG_EMISIONESMAX { get; set; }
        public string FLAG_ESTADO { get; set; }
    }
}