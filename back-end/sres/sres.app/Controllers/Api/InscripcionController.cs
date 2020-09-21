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

            if (seGuardo && inscripcion.ID_ETAPA == 2)
            {
                ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(!inscripcion.ID_CONVOCATORIA.HasValue ? 0 : inscripcion.ID_CONVOCATORIA.Value);
                UsuarioBE usuario = usuarioLN.ObtenerUsuario(!inscripcion.UPD_USUARIO.HasValue ? 0 : inscripcion.UPD_USUARIO.Value);
                string fieldNombres = "[NOMBRES]", fieldApellidos = "[APELLIDOS]", fieldConvocatoria = "[CONVOCATORIA]", fieldNombreConv = "[NOMBRE_CONV]", fieldServer = "[SERVER]";
                string[] fields = new string[] { fieldNombres, fieldApellidos, fieldConvocatoria, fieldNombreConv, fieldServer };
                string[] fieldsRequire = new string[] { fieldNombres, fieldApellidos, fieldConvocatoria, fieldNombreConv, fieldServer };
                Dictionary<string, string> dataBody = new Dictionary<string, string> { [fieldNombres] = usuario.NOMBRES, [fieldApellidos] = usuario.APELLIDOS, [fieldConvocatoria] = convocatoria.FECHA_INICIO.Year.ToString(),[fieldNombreConv] = convocatoria.NOMBRE,[fieldServer] = AppSettings.Get<string>("Server") };
                string subject = $"Inscripción a la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {convocatoria.FECHA_INICIO.Year.ToString()}";
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
                //ConvocatoriaBE convocatoria = convocatoriaLN.ObtenerConvocatoria(!inscripcion.ID_CONVOCATORIA.HasValue ? 0 : inscripcion.ID_CONVOCATORIA.Value);
                InscripcionBE insc = inscripcionLN.ObtenerInscripcionPorId(inscripcion.ID_INSCRIPCION);
                if (insc != null)
                {
                    string fieldNombres = "[NOMBRES]", fieldConvocatoria = "[CONVOCATORIA]", fieldObservacion = "[OBSERVACION]", fieldEntidad = "[ENTIDAD]", fieldNombreConv = "[NOMBRE_CONV]", fieldServer = "[SERVER]";
                    string[] fields = new string[] { fieldNombres, fieldConvocatoria, fieldObservacion, fieldEntidad, fieldNombreConv, fieldServer };
                    string[] fieldsRequire = new string[] { fieldNombres, fieldConvocatoria, fieldObservacion, fieldEntidad, fieldNombreConv, fieldServer };
                    Dictionary<string, string> dataBody = new Dictionary<string, string> {[fieldNombres] = insc.NOMBRES_USU,[fieldConvocatoria] = insc.FECHA_INICIO.Year.ToString(),[fieldObservacion] = inscripcion.OBSERVACION, [fieldEntidad] = insc.RAZON_SOCIAL, [fieldNombreConv] = insc.NOMBRE_CONV,[fieldServer] = AppSettings.Get<string>("Server") };
                    string subject = "";
                    if (inscripcion.ID_ETAPA == 3) subject = $"Observación de los requisitos de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {insc.FECHA_INICIO.Year.ToString()}";
                    else if (inscripcion.ID_ETAPA == 5) subject = $"Aprobación de los requisitos de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {insc.FECHA_INICIO.Year.ToString()}";
                    MailAddressCollection mailTo = new MailAddressCollection();
                    mailTo.Add(new MailAddress(insc.CORREO));
                    if (inscripcion.ID_ETAPA == 3) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.ObservacionRequisitos, dataBody, fields, fieldsRequire, subject, mailTo));
                    else if (inscripcion.ID_ETAPA == 5) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.AprobacionRequesitos, dataBody, fields, fieldsRequire, subject, mailTo));
                }
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

        [Route("anularinscripcion")]
        [HttpPost]
        public bool AnularInscripcion(InscripcionBE inscripcion)
        {
            bool seGuardo = inscripcionLN.AnularInscripcion(inscripcion);

            if (seGuardo)
            {
                InscripcionBE insc = inscripcionLN.ObtenerInscripcionPorId(inscripcion.ID_INSCRIPCION);
                if (insc != null) {
                    string fieldNombres = "[NOMBRES]", fieldConvocatoria = "[CONVOCATORIA]", fieldObservacion = "[OBSERVACION]", fieldServer = "[SERVER]";
                    string[] fields = new string[] { fieldNombres, fieldConvocatoria, fieldObservacion, fieldServer };
                    string[] fieldsRequire = new string[] { fieldNombres, fieldConvocatoria, fieldObservacion, fieldServer };
                    Dictionary<string, string> dataBody = new Dictionary<string, string> {[fieldNombres] = insc.NOMBRES_USU,[fieldConvocatoria] = insc.FECHA_INICIO.Year.ToString(),[fieldObservacion] = inscripcion.OBSERVACION,[fieldServer] = AppSettings.Get<string>("Server") };
                    string subject = $"{insc.NOMBRES_USU}, su inscripción fue anulada";
                    MailAddressCollection mailTo = new MailAddressCollection();
                    mailTo.Add(new MailAddress(insc.CORREO));
                    Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.InscripcionAnulacion, dataBody, fields, fieldsRequire, subject, mailTo));
                }                
            }

            return seGuardo;
        }

        [Route("inscripciontrazabilidad")]
        [HttpGet]
        public InstitucionBE InscripcionTrazabilidad(int idInscripcion)
        {            
            return inscripcionLN.InscripcionTrazabilidad(idInscripcion);
        }
    }
}
