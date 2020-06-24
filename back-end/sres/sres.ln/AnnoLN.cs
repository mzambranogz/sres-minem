using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.ln;
using sres.da;
using System.Data;
using sres.ut;

namespace sres.ln
{
    public class AnnoLN : BaseLN
    {
        public static AnnoDA AnnoDA = new AnnoDA();

        //public static AnnoBE GuardarAnno(AnnoBE entidad)
        //{
        //    return EtapaDA.GuardarEtapa(entidad);
        //}

        public List<AnnoBE> getAllAnno()
        {
            List<AnnoBE> lista = new List<AnnoBE>();

            try
            {
                cn.Open();
                lista = AnnoDA.getAllAnno(cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
