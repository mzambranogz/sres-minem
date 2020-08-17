using sres.be;
using sres.ln;
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
