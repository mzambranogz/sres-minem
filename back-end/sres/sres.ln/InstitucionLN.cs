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
using System.Web;

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
            catch(Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            if (item != null)
            {
                string pathFormat = AppSettings.Get<string>("Path.Institucion");
                string pathDirectoryRelative = string.Format(pathFormat, item.ID_INSTITUCION);
                string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                string pathFile = Path.Combine(pathDirectory, item.LOGO);
                if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                pathFile = !File.Exists(pathFile) ? null : pathFile;
                if (!string.IsNullOrEmpty(pathFile))
                {
                    item.LOGO_CONTENIDO = File.ReadAllBytes(pathFile);
                    item.LOGO_TIPO = MimeMapping.GetMimeMapping(pathFile);
                }
            }

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
            catch(Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.LOGO))
                {
                    string pathFormat = AppSettings.Get<string>("Path.Institucion");
                    string pathDirectoryRelative = string.Format(pathFormat, item.ID_INSTITUCION);
                    string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                    string pathFile = Path.Combine(pathDirectory, item.LOGO);
                    if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                    pathFile = !File.Exists(pathFile) ? null : pathFile;
                    if (!string.IsNullOrEmpty(pathFile))
                    {
                        item.LOGO_CONTENIDO = File.ReadAllBytes(pathFile);
                        item.LOGO_TIPO = MimeMapping.GetMimeMapping(pathFile);
                    }
                }
            }

            return item;
        }

        public bool ModificarLogoInstitucion(InstitucionBE institucion)
        {
            bool seModifico = false;

            try
            {
                cn.Open();
                seModifico = institucionDA.ModificarLogoInstitucion(institucion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            if(seModifico == true)
            {
                string pathFormat = AppSettings.Get<string>("Path.Institucion");
                string pathDirectoryRelative = string.Format(pathFormat, institucion.ID_INSTITUCION);
                string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                string pathFile = Path.Combine(pathDirectory, institucion.LOGO);
                if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                if (institucion.LOGO_CONTENIDO != null) File.WriteAllBytes(pathFile, institucion.LOGO_CONTENIDO);
            }

            return seModifico;
        }

        public bool ModificarDatosInstitucion(InstitucionBE institucion)
        {
            bool seModifico = false;

            try
            {
                cn.Open();
                seModifico = institucionDA.ModificarDatosInstitucion(institucion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seModifico;
        }
    }
}
