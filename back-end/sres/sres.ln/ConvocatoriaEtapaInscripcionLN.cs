using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;
using sres.ut;
using Oracle.DataAccess.Client;

namespace sres.ln
{
    public class ConvocatoriaEtapaInscripcionLN : BaseLN
    {
        ConvocatoriaEtapaInscripcionDA convetainscDA = new ConvocatoriaEtapaInscripcionDA();

        public ConvocatoriaEtapaInscripcionBE ObtenerConvocatoriaEtapaInscripcion(ConvocatoriaEtapaInscripcionBE entidad)
        {
            ConvocatoriaEtapaInscripcionBE item = null;
            try
            {
                cn.Open();
                item = convetainscDA.ObtenerConvocatoriaEtapaInscripcion(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
