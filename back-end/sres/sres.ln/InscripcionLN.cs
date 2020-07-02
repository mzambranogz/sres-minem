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
    public class InscripcionLN : BaseLN
    {
        InscripcionDA inscripcionDA = new InscripcionDA();

        public InscripcionBE ObtenerInscripcionPorConvocatoriaInstitucion(int idConvocatoria, int idInstitucion)
        {
            InscripcionBE item = null;

            try
            {
                cn.Open();
                item = inscripcionDA.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, idInstitucion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public bool GuardarInscripcion(InscripcionBE inscripcion)
        {
            bool seGuardo = false;

            try
            {
                cn.Open();
                seGuardo = inscripcionDA.GuardarInscripcion(inscripcion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }
    }
}
