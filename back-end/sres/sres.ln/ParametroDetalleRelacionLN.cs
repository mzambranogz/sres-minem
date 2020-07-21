using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;
using Oracle.DataAccess.Client;
using sres.ut;

namespace sres.ln
{
    public class ParametroDetalleRelacionLN : BaseLN
    {
        ParametroDetalleRelacionDA paramdetrelDA = new ParametroDetalleRelacionDA();

        public List<ParametroDetalleBE> FiltrarParametroDetalle(ParametroDetalleRelacionBE entidad)
        {
            List<ParametroDetalleBE> lista = null;
            try
            {
                cn.Open();
                lista = paramdetrelDA.FiltrarParametroDetalle(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
