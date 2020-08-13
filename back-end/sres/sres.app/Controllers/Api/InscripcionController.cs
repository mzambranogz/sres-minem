using sres.be;
using sres.ln;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/inscripcion")]
    public class InscripcionController : ApiController
    {
        UsuarioLN usuarioLN = new UsuarioLN();
        InscripcionLN inscripcionLN = new InscripcionLN();
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();

        Mailing mailing = new Mailing();

        [Route("existeinscripcion")]
        [HttpGet]
        public bool ExisteInscripcion(int idConvocatoria, int idInstitucion)
        {
            InscripcionBE inscripcion = inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, idInstitucion);
            return inscripcion != null;
        }

        [Route("guardarinscripcion")]
        [HttpPost]
        public HttpResponseMessage GuardarInscripcion(InscripcionBE inscripcion)
        {
            int idInscripcion = 0;
            bool seGuardo = inscripcionLN.GuardarInscripcion(inscripcion, out idInscripcion);

            if (seGuardo)
            {
                ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(!inscripcion.ID_CONVOCATORIA.HasValue ? 0 : inscripcion.ID_CONVOCATORIA.Value);
                UsuarioBE usuario = usuarioLN.ObtenerUsuario(!inscripcion.UPD_USUARIO.HasValue ? 0 : inscripcion.UPD_USUARIO.Value);
                string fieldNombres = "[NOMBRES]", fieldApellidos = "[APELLIDOS]", fieldConvocatoria = "[CONVOCATORIA]";
                string[] fields = new string[] { fieldNombres, fieldApellidos, fieldConvocatoria };
                string[] fieldsRequire = new string[] { fieldNombres, fieldApellidos, fieldConvocatoria };
                Dictionary<string, string> dataBody = new Dictionary<string, string> { [fieldNombres] = usuario.NOMBRES, [fieldApellidos] = usuario.APELLIDOS, [fieldConvocatoria] = convocatoria.NOMBRE };
                string subject = $"{usuario.NOMBRES} {usuario.APELLIDOS}, Su inscripción fue satisfactoria";
                MailAddressCollection mailTo = new MailAddressCollection();
                mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRES} {usuario.APELLIDOS}"));
                Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.InscripcionConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { success = seGuardo, id = idInscripcion });

            return response;
        }

        [Route("evaluarinscripcion")]
        [HttpPost]
        public bool EvaluarInscripcion(InscripcionBE inscripcion)
        {
            bool seGuardo = inscripcionLN.GuardarEvaluacionInscripcion(inscripcion);

            if (seGuardo)
            {
                ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(!inscripcion.ID_CONVOCATORIA.HasValue ? 0 : inscripcion.ID_CONVOCATORIA.Value);
                //UsuarioBE usuario = usuarioLN.ObtenerUsuario(!inscripcion.UPD_USUARIO.HasValue ? 0 : inscripcion.UPD_USUARIO.Value);
                //string fieldNombres = "[NOMBRES]", fieldApellidos = "[APELLIDOS]", fieldConvocatoria = "[CONVOCATORIA]";
                //string[] fields = new string[] { fieldNombres, fieldApellidos, fieldConvocatoria };
                //string[] fieldsRequire = new string[] { fieldNombres, fieldApellidos, fieldConvocatoria };
                //Dictionary<string, string> dataBody = new Dictionary<string, string> { [fieldNombres] = usuario.NOMBRES, [fieldApellidos] = usuario.APELLIDOS, [fieldConvocatoria] = convocatoria.NOMBRE };
                //string subject = $"{usuario.NOMBRES} {usuario.APELLIDOS}, Su inscripción fue satisfactoria";
                //MailAddressCollection mailTo = new MailAddressCollection();
                //mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRES} {usuario.APELLIDOS}"));
                //Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.InscripcionConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
            }

            return seGuardo;
        }

        [Route("buscarinscripcion")]
        [HttpGet]
        public DataPaginateBE BuscarInscripcion(int idConvocatoria, int? idInscripcion, string razonSocialInstitucion, string nombresCompletosUsuario, int? idUsuario, int registros, int pagina, string columna, string orden)
        {
            List<InscripcionBE> inscripcion = inscripcionLN.BuscarInscripcion(idConvocatoria, idInscripcion, razonSocialInstitucion, nombresCompletosUsuario, idUsuario, registros, pagina, columna, orden);

            DataPaginateBE data = new DataPaginateBE
            {
                DATA = inscripcion,
                PAGINA = inscripcion.Count == 0 ? 0 : inscripcion[0].PAGINA,
                CANTIDAD_REGISTROS = inscripcion.Count == 0 ? 0 : inscripcion[0].CANTIDAD_REGISTROS,
                TOTAL_PAGINAS = inscripcion.Count == 0 ? 0 : inscripcion[0].TOTAL_PAGINAS,
                TOTAL_REGISTROS = inscripcion.Count == 0 ? 0 : inscripcion[0].TOTAL_REGISTROS
            };

            return data;
        }
    }
}
