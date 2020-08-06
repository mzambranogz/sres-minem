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
    public class TrabajadorCamaLN : BaseLN
    {
        TrabajadorCamaDA trabajadorcamaDA = new TrabajadorCamaDA();

        public List<TrabajadorCamaBE> listaSubsectorTipoempresa(int idSubsectorTipoempresa)
        {
            List<TrabajadorCamaBE> lista = new List<TrabajadorCamaBE>();
            try
            {
                cn.Open();
                lista = trabajadorcamaDA.listaSubsectorTipoempresa(idSubsectorTipoempresa, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
