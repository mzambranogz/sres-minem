using sres.be;
using sres.da;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class InscripcionRequerimientoLN : BaseLN
    {
        InscripcionRequerimientoDA inscripcionRequerimientoDA = new InscripcionRequerimientoDA();

        public List<InscripcionRequerimientoBE> ListarInscripcionRequerimientoPorConvocatoriaInscripcion(int idConvocatoria, int? idInscripcion)
        {
            List<InscripcionRequerimientoBE> lista = new List<InscripcionRequerimientoBE>();

            try
            {
                cn.Open();
                lista = inscripcionRequerimientoDA.ListarInscripcionRequerimientoPorConvocatoriaInscripcion(idConvocatoria, idInscripcion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public InscripcionRequerimientoBE ObtenerInscripcionRequerimiento(int idConvocatoria, int idInscripcion, int idRequerimiento)
        {
            InscripcionRequerimientoBE item = null;

            try
            {
                cn.Open();
                item = inscripcionRequerimientoDA.ObtenerInscripcionRequerimiento(idConvocatoria, idInscripcion, idRequerimiento, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public string ObtenerRutaArchivoRequerimiento(int idConvocatoria, int idInscripcion, int idInstitucion, int idRequerimiento)
        {
            InscripcionRequerimientoBE item = null;

            try
            {
                cn.Open();
                item = inscripcionRequerimientoDA.ObtenerInscripcionRequerimiento(idConvocatoria, idInscripcion, idRequerimiento, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            if (item == null) return null;

            string pathFormat = AppSettings.Get<string>("Path.Inscripcion.Requerimiento");
            string pathDirectoryRelative = string.Format(pathFormat, idInstitucion, idInscripcion);
            string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
            string pathFile = Path.Combine(pathDirectory, item.ARCHIVO_BASE);
            if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
            pathFile = !File.Exists(pathFile) ? null : pathFile;
            return pathFile;
        }
    }
}
