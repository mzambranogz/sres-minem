using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.ln;
using sres.da;

namespace sres.ln
{
    public class AnnoLN : BaseLN
    {
        public static AnnoDA AnnoDA = new AnnoDA();

        //public static AnnoBE GuardarAnno(AnnoBE entidad)
        //{
        //    return EtapaDA.GuardarEtapa(entidad);
        //}

        public static List<AnnoBE> getAllAnno()
        {
            return AnnoDA.getAllAnno(cn);
        }
    }
}
