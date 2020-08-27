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
    public class ComponenteLN : BaseLN
    {
        ComponenteDA componenteDA = new ComponenteDA();
        public List<ComponenteBE> ListaBusquedaComponente(ComponenteBE entidad)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();
            try
            {
                cn.Open();
                lista = componenteDA.ListarBusquedaComponente(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public bool GuardarComponente(ComponenteBE entidad)
        {
            bool seGuardo = false;
            try
            {
                cn.Open();
                seGuardo = componenteDA.GuardarComponente(entidad, cn).OK;
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public ComponenteBE getComponente(ComponenteBE entidad)
        {
            ComponenteBE item = null;
            try
            {
                cn.Open();
                item = componenteDA.getComponente(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public ComponenteBE EliminarComponente(ComponenteBE entidad)
        {
            ComponenteBE item = null;
            try
            {
                cn.Open();
                item = componenteDA.EliminarComponente(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<ComponenteBE> ListaComponenteCasoCriterio(ComponenteBE entidad)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();
            try
            {
                cn.Open();
                lista = componenteDA.ListaComponenteCasoCriterio(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
