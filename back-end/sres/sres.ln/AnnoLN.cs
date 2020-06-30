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

        public AnnoBE GuardarAnno(AnnoBE entidad)
        {
            AnnoBE item = null;

            try
            {
                cn.Open();
                item = AnnoDA.GuardarAnno(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public AnnoBE EliminarAnno(AnnoBE entidad)
        {
            AnnoBE item = null;

            try
            {
                cn.Open();
                item = AnnoDA.EliminarAnno(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public AnnoBE getAnno(AnnoBE entidad)
        {
            AnnoBE item = null;

            try
            {
                cn.Open();
                item = AnnoDA.getAnno(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<AnnoBE> ListaBusquedaAnno(AnnoBE entidad)
        {
            List<AnnoBE> lista = new List<AnnoBE>();

            try
            {
                cn.Open();
                lista = AnnoDA.ListarBusquedaAnno(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

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
