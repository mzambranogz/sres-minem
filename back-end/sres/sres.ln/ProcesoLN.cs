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
    public class ProcesoLN : BaseLN
    {
        public ProcesoDA ProcesoDA = new ProcesoDA();

        public ProcesoBE GuardarProceso(ProcesoBE entidad)
        {
            ProcesoBE item = null;

            try
            {
                cn.Open();
                item = ProcesoDA.GuardarProceso(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public ProcesoBE getProceso(ProcesoBE entidad)
        {
            ProcesoBE item = null;

            try
            {
                cn.Open();
                item = ProcesoDA.getProceso(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<ProcesoBE> ListaBusquedaProceso(ProcesoBE entidad)
        {
            List<ProcesoBE> lista = new List<ProcesoBE>();

            try
            {
                cn.Open();
                lista = ProcesoDA.ListarBusquedaProceso(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
