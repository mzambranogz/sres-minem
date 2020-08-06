using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public partial class InstitucionBE : BaseBE
    {
        public int ID_INSTITUCION { get; set; }
        public string RUC { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string NOMBRE_COMERCIAL { get; set; }
        public string DESCRIPCION { get; set; }
        public string DOMICILIO_LEGAL { get; set; }
        public int ID_SECTOR { get; set; }
        public SectorBE SECTOR { get; set; }
        public string LOGO { get; set; }
        public byte[] LOGO_CONTENIDO { get; set; }
        public string LOGO_TIPO { get; set; }
        public int ID_SUBSECTOR_TIPOEMPRESA { get; set; }
        public int ID_TRABAJADORES_CAMA { get; set; }
        public int CANTIDAD { get; set; }
        public string FLAG_ESTADO { get; set; }
        public int? UPD_USUARIO { get; set; }
    }
}
