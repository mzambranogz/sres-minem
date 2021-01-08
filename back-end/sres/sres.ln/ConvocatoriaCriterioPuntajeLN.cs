using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;
using sres.ut;
using Oracle.DataAccess.Client;
using System.Data;

namespace sres.ln
{
    public class ConvocatoriaCriterioPuntajeLN : BaseLN
    {
        ConvocatoriaCriterioPuntajeDA convcripuntajeDA = new ConvocatoriaCriterioPuntajeDA();

        public List<ConvocatoriaCriterioPuntajeBE> ListaConvocatoriaCriterioPuntaje(int idConvocatoria, int idCriterio)
        {
            List<ConvocatoriaCriterioPuntajeBE> lista = new List<ConvocatoriaCriterioPuntajeBE>();
            try
            {
                cn.Open();
                lista = convcripuntajeDA.listarConvocatoriaCriterioPuntaje(idConvocatoria, idCriterio, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public ConvocatoriaCriterioPuntajeBE ObtenerPuntajeInscripcion(int idConvocatoria, int idInscripcion)
        {
            ConvocatoriaCriterioPuntajeBE item = new ConvocatoriaCriterioPuntajeBE();
            ConvocatoriaCriterioPuntajeBE itempuntajeCE = new ConvocatoriaCriterioPuntajeBE();
            try
            {
                cn.Open();
                item = convcripuntajeDA.ObtenerPuntajeInscripcion(idConvocatoria, idInscripcion, cn);
                itempuntajeCE = convcripuntajeDA.ObtenerPuntajeConsumoEficiencia(idConvocatoria, idInscripcion, cn);
                decimal restante = itempuntajeCE.CONSUMO_ENERGETICO + itempuntajeCE.EFICIENCIA_TRANSPORTE - 60;
                item.PUNTAJE -= restante;
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
