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
using Oracle.DataAccess.Client;

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

        public List<InstitucionBE> ListarInstitucion()
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();

            try
            {
                cn.Open();
                lista = institucionDA.ListarInstitucion(cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
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
                item.LISTA_CONTACTO = institucionDA.ObtenerListaContacto(idInstitucion, cn);
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
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    seModifico = institucionDA.ModificarDatosInstitucion(institucion, cn);
                    if (seModifico)
                        foreach (InstitucionContactoBE contacto in institucion.LISTA_CONTACTO)
                            if (!(seModifico = institucionDA.GuardarContacto(contacto, cn))) break;

                    if (seModifico) {
                        seModifico = institucionDA.DeleteActividadEconomica(institucion, cn);
                        if (seModifico)
                            foreach (ActividadEconomicaBE ae in institucion.LISTA_ACTIVIDAD)
                                if (!(seModifico = institucionDA.GuardarActividadEconomica(ae, cn))) break;
                    }                        

                    if (seModifico) ot.Commit();
                    else ot.Rollback();
                }                    
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seModifico;
        }
        public bool cambiarPrimerInicio(int idInstitucion)
        {
            bool seModifico = false;
            try
            {
                cn.Open();
                seModifico = institucionDA.cambiarPrimerInicio(idInstitucion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seModifico;
        }

        public List<DepartamentoBE> listarDepartamento()
        {
            List<DepartamentoBE> lista = new List<DepartamentoBE>();
            try
            {
                cn.Open();
                lista = institucionDA.listarDepartamento(cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ProvinciaBE> listarProvincia(string idDepartamento)
        {
            List<ProvinciaBE> lista = new List<ProvinciaBE>();
            try
            {
                cn.Open();
                lista = institucionDA.listarProvincia(idDepartamento ,cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<DistritoBE> listarDistrito(string idProvincia)
        {
            List<DistritoBE> lista = new List<DistritoBE>();
            try
            {
                cn.Open();
                lista = institucionDA.listarDistrito(idProvincia, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<InstitucionBE> ListaBusquedaInstitucion(InstitucionBE entidad)
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();
            try
            {
                cn.Open();
                lista = institucionDA.ListarBusquedaInstitucion(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public InstitucionBE ActualizarInstitucion(InstitucionBE institucion)
        {
            bool seModifico = false;
            int val = 0;
            try
            {
                cn.Open();
                InstitucionBE inst = institucionDA.ObtenerInstitucionPorRuc(institucion.RUC, cn);
                val = inst == null ? 0 : institucion.ID_INSTITUCION == inst.ID_INSTITUCION ? 0 : 1;
                seModifico = inst == null ? true : institucion.ID_INSTITUCION == inst.ID_INSTITUCION ? true : false;
                if (seModifico) seModifico = institucionDA.ActualizarInstitucion(institucion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return new InstitucionBE { VAL = val, OK = seModifico};
        }

        public List<ActividadEconomicaBE> ListarActividad(int id)
        {
            List<ActividadEconomicaBE> lista = new List<ActividadEconomicaBE>();
            try
            {
                cn.Open();
                lista = institucionDA.ListarActividad(id, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

    }
}
