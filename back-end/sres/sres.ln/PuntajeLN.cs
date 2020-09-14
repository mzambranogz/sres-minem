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
    public class PuntajeLN : BaseLN
    {
        PuntajeDA puntajeDA = new PuntajeDA();
        public List<PuntajeBE> ListaBusquedaPuntaje(PuntajeBE entidad)
        {
            List<PuntajeBE> lista = new List<PuntajeBE>();

            try
            {
                cn.Open();
                lista = puntajeDA.ListarBusquedaPuntaje(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public bool GuardarPuntaje(PuntajeBE entidad)
        {
            bool seGuardo = false;
            try
            {
                cn.Open();
                seGuardo = puntajeDA.GuardarPuntaje(entidad, cn).OK;
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public PuntajeBE getPuntaje(PuntajeBE entidad)
        {
            PuntajeBE item = null;
            try
            {
                cn.Open();
                item = puntajeDA.getPuntaje(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public PuntajeBE EliminarPuntaje(PuntajeBE entidad)
        {
            PuntajeBE item = null;

            try
            {
                cn.Open();
                item = puntajeDA.EliminarPuntaje(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public PuntajeBE getPuntajePosible(int convocatoria)
        {
            PuntajeBE item = null;
            try
            {
                cn.Open();
                item = puntajeDA.getPuntajePosible(convocatoria, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
