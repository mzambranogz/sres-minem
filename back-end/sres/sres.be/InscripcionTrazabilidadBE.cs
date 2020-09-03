using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class InscripcionTrazabilidadBE : BaseBE
    {
        public int ID_INSCRIPCION { get; set; }
        public int ID_TRAZABILIDAD { get; set; }
        public string DESCRIPCION { get; set; }
        public int ID_ETAPA { get; set; }
        public int ID_ROL { get; set; }
        public string ETAPA { get; set; }
        public string CORREO { get; set; }
        public string ROL { get; set; }
        public string FECHA_TRAZA { get; set; }
        public int? UPD_USUARIO { get; set; }
    }
}
