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
    public class InstitucionLN : BaseLN
    {
        InstitucionDA institucionDA = new InstitucionDA();

        public InstitucionBE ObtenerInstitucionPorRuc(string ruc)
        {
            InstitucionBE item = null;

            try
            {
                cn.Open();
                item = institucionDA.ObtenerInstitucionPorRuc(ruc, cn);
            }
            catch(Exception ex) { throw ex; }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<InstitucionBE> BuscarParticipantes(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();

            try
            {
                cn.Open();
                lista = institucionDA.BuscarParticipantes(busqueda, registros, pagina, columna, orden, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public InstitucionBE ObtenerInstitucion(int idInstitucion)
        {
            InstitucionBE item = null;

            try
            {
                cn.Open();
                item = institucionDA.ObtenerInstitucion(idInstitucion, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
