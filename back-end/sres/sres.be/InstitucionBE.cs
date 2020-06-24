using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class InstitucionBE
    {
        public int ID_INSTITUCION { get; set; }
        public string RUC { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string DOMICILIO_LEGAL { get; set; }
        public int ID_SECTOR { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int? UPD_USUARIO { get; set; }
    }
}
