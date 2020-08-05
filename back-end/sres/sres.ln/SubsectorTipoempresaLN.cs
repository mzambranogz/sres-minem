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
    public class SubsectorTipoempresaLN : BaseLN
    {
        SubsectorTipoempresaDA subsectipoemp = new SubsectorTipoempresaDA();

        public List<SubsectorTipoempresaBE> listaSubsectorTipoempresa(int idSector) {
            List<SubsectorTipoempresaBE> lista = new List<SubsectorTipoempresaBE>();
            try
            {
                cn.Open();
                lista = subsectipoemp.listaSubsectorTipoempresa(idSector, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
