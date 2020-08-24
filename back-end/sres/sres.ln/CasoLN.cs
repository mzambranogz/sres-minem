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
    public class CasoLN : BaseLN
    {
        CasoDA casoDA = new CasoDA();
        public List<CasoBE> ObtenerListaCasoCriterioPorConvocatoria(CasoBE entidad)
        {
            List<CasoBE> lista = null;

            try
            {
                cn.Open();
                lista = casoDA.VerificarConvocatoriaCriterioInscripcion(entidad, cn);
                lista = lista.Count == 0 ? casoDA.ObtenerListaCasoCriterioPorConvocatoria(entidad, cn) : lista;
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<CasoBE> ListaBusquedaCaso(CasoBE entidad)
        {
            List<CasoBE> lista = new List<CasoBE>();

            try
            {
                cn.Open();
                lista = casoDA.ListarBusquedaCaso(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public bool GuardarInsignia(CasoBE entidad)
        {
            bool seGuardo = false;
            try
            {
                cn.Open();
                seGuardo = casoDA.GuardarCaso(entidad, cn).OK;
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public CasoBE getCaso(CasoBE entidad)
        {
            CasoBE item = null;
            try
            {
                cn.Open();
                item = casoDA.getCaso(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public CasoBE EliminarCaso(CasoBE entidad)
        {
            CasoBE item = null;
            try
            {
                cn.Open();
                item = casoDA.EliminarCaso(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
