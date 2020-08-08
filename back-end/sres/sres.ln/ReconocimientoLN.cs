using sres.be;
using sres.da;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class ReconocimientoLN : BaseLN
    {
        ReconocimientoDA reconocimientoDA = new ReconocimientoDA();

        public List<ReconocimientoBE> ListarUltimosReconocimientos(int idInstitucion, int cantidadRegistros)
        {
            List<ReconocimientoBE> lista = new List<ReconocimientoBE>();

            try
            {
                cn.Open();
                lista = reconocimientoDA.ListarUltimosReconocimientos(idInstitucion, cantidadRegistros, cn);
                while(lista.Count < cantidadRegistros)
                {
                    lista.Add(null);
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
