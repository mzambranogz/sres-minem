using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;

namespace sres.ln
{
    public class CriterioLN : BaseLN
    {
        public CriterioDA criterioDA = new CriterioDA();

        public CriterioBE RegistroCriterio(CriterioBE entidad)
        {
            CriterioBE item = null;

            try
            {
                cn.Open();
                item = criterioDA.RegistroCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public CriterioBE GuardarCriterio(CriterioBE entidad)
        {
            CriterioBE item = null;

            try
            {
                cn.Open();
                item = criterioDA.GuardarCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public CriterioBE EliminarCriterio(CriterioBE entidad)
        {
            CriterioBE item = null;

            try
            {
                cn.Open();
                item = criterioDA.EliminarCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public CriterioBE getCriterio(CriterioBE entidad)
        {
            CriterioBE item = null;

            try
            {
                cn.Open();
                item = criterioDA.getCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<CriterioBE> ListaBusquedaCriterio(CriterioBE entidad)
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                cn.Open();
                lista = criterioDA.ListarBusquedaCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<CriterioBE> getAllCriterio()
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                cn.Open();
                lista = criterioDA.getAllCriterio(cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
