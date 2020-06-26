using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;
using System.Data;

namespace sres.ln
{
    public class EtapaLN : BaseLN
    {
        public EtapaDA EtapaDA = new EtapaDA();

        public EtapaBE GuardarEtapa(EtapaBE entidad)
        {
            EtapaBE item = null;

            try
            {
                cn.Open();
                item = EtapaDA.GuardarEtapa(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public EtapaBE getEtapa(EtapaBE entidad)
        {
            EtapaBE item = null;

            try
            {
                cn.Open();
                item = EtapaDA.getEtapa(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<EtapaBE> ListaBusquedaEtapa(EtapaBE entidad)
        {
            List<EtapaBE> lista = new List<EtapaBE>();

            try
            {
                cn.Open();
                lista = EtapaDA.ListarBusquedaEtapa(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<EtapaBE> getAllEtapa()
        {
            List<EtapaBE> lista = new List<EtapaBE>();

            try
            {
                cn.Open();
                lista = EtapaDA.getAllEtapa(cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
