using sres.app.ServicioPIDE;
using sres.be;
using sres.ln;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/institucion")]
    public class InstitucionController : ApiController
    {
        InstitucionLN institucionLN = new InstitucionLN();

        [Route("obtenerinstitucionporrucservicio")]
        [HttpGet]
        public InstitucionBE ObtenerInstitucionPorRucPide(string ruc)
        {
            InstitucionBE Entidad = new InstitucionBE();
            try
            {
                IsrvConsultasClient clientePIDE = new IsrvConsultasClient();
                BEDatosPrincipalesRUC empresa = clientePIDE.ConsultaRUC_PIDE(ruc);
                if (empresa.OK)
                {
                    Entidad = new InstitucionBE();
                    Entidad.RAZON_SOCIAL = empresa.Nombre;
                    Entidad.DOMICILIO_LEGAL = empresa.DomicilioLegal;
                    Entidad.CONTRIBUYENTE = empresa.TipoPersona == "PERSONA JURIDICA" ? "1" : "2";
                    Entidad.ID_DEPARTAMENTO = empresa.CodigoDpto;
                    Entidad.ID_PROVINCIA = empresa.CodigoProv;
                    Entidad.ID_DISTRITO = empresa.CodigoDist;
                    Entidad.OK = true;
                }
                else
                {
                    Entidad.OK = false;
                    Entidad.VAL = 1;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Entidad.OK = false;
                Entidad.VAL = 2;
            }

            //DATOS PRUEBA
            //Entidad.RAZON_SOCIAL = "LAS RATAS";
            //Entidad.DOMICILIO_LEGAL = "Los tusilagos";
            //Entidad.CONTRIBUYENTE = "2";
            //Entidad.ID_DEPARTAMENTO = "15";
            //Entidad.ID_PROVINCIA = "1501";
            //Entidad.ID_DISTRITO = "150130";
            //Entidad.OK = true;
            return Entidad;
        }

        [Route("obtenerinstitucionporruc")]
        [HttpGet]
        public InstitucionBE ObtenerInstitucionPorRuc(string ruc)
        {
            return institucionLN.ObtenerInstitucionPorRuc(ruc);
        }

        [Route("obtenerinstitucion")]
        [HttpGet]
        public InstitucionBE ObtenerInstitucion(int idInstitucion)
        {
            return institucionLN.ObtenerInstitucion(idInstitucion);
        }

        [Route("modificarlogoinstitucion")]
        [HttpPut]
        public bool ModificarLogoInstitucion(InstitucionBE institucion)
        {
            return institucionLN.ModificarLogoInstitucion(institucion);
        }

        [Route("modificardatosinstitucion")]
        [HttpPut]
        public bool ModificarDatosInstitucion(InstitucionBE institucion)
        {
            return institucionLN.ModificarDatosInstitucion(institucion);
        }

        [Route("cambiarprimerinicio")]
        [HttpGet]
        public bool cambiarPrimerInicio(int idInstitucion)
        {
            return institucionLN.cambiarPrimerInicio(idInstitucion);
        }

        [Route("listadepartamento")]
        [HttpGet]
        public List<DepartamentoBE> listarDepartamento()
        {
            return institucionLN.listarDepartamento();
        }

        [Route("listaprovincia")]
        [HttpGet]
        public List<ProvinciaBE> listarProvincia(string idDepartamento)
        {
            return institucionLN.listarProvincia(idDepartamento);
        }

        [Route("listadistrito")]
        [HttpGet]
        public List<DistritoBE> listarDistrito(string idProvincia)
        {
            return institucionLN.listarDistrito(idProvincia);
        }

        [Route("buscarinstitucion")]
        [HttpGet]
        public List<InstitucionBE> BuscarInstitucion(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return institucionLN.ListaBusquedaInstitucion(new InstitucionBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("actualizarinstitucion")]
        [HttpPut]
        public InstitucionBE ActualizarInstitucion(InstitucionBE institucion)
        {
            return institucionLN.ActualizarInstitucion(institucion);
        }

        [Route("listaractividad")]
        [HttpGet]
        public List<ActividadEconomicaBE> ListarActividad(int id)
        {
            return institucionLN.ListarActividad(id);
        }

        //[Route("buscarparticipantes")]
        //[HttpGet]
        //public DataPaginateBE BuscarParticipantes(string busqueda, int registros, int pagina, string columna, string orden)
        //{
        //    List<InstitucionBE> institucion = insc.BuscarConvocatoria(nroInforme, nombre, fechaDesde, fechaHasta, registros, pagina, columna, orden);

        //    DataPaginateBE data = new DataPaginateBE
        //    {
        //        DATA = convocatoria,
        //        PAGINA = convocatoria.Count == 0 ? 0 : convocatoria[0].PAGINA,
        //        CANTIDAD_REGISTROS = convocatoria.Count == 0 ? 0 : convocatoria[0].CANTIDAD_REGISTROS,
        //        TOTAL_PAGINAS = convocatoria.Count == 0 ? 0 : convocatoria[0].TOTAL_PAGINAS,
        //        TOTAL_REGISTROS = convocatoria.Count == 0 ? 0 : convocatoria[0].TOTAL_REGISTROS
        //    };

        //    return data;

        //    return institucionLN.BuscarParticipantes(busqueda, registros, pagina, columna, orden);
        //}
    }
}
