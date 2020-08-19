using sres.app.Models;
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
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/convocatoria")]
    public class ConvocatoriaController : ApiController
    {    
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();
        UsuarioLN usuarioLN = new UsuarioLN();

        Mailing mailing = new Mailing();

        [Route("buscarconvocatoria")]
        [HttpGet]
        public DataPaginateBE BuscarConvocatoria(string nroInforme, string nombre, DateTime? fechaDesde, DateTime? fechaHasta, int registros, int pagina, string columna, string orden, int idInstitucion)
        {
            List<ConvocatoriaBE> convocatoria = convocatoriaLN.BuscarConvocatoria(nroInforme, nombre, fechaDesde, fechaHasta, registros, pagina, columna, orden, idInstitucion);

            DataPaginateBE data = new DataPaginateBE
            {
                DATA = convocatoria,
                PAGINA = convocatoria.Count == 0 ? 0 : convocatoria[0].PAGINA,
                CANTIDAD_REGISTROS = convocatoria.Count == 0 ? 0 : convocatoria[0].CANTIDAD_REGISTROS,
                TOTAL_PAGINAS = convocatoria.Count == 0 ? 0 : convocatoria[0].TOTAL_PAGINAS,
                TOTAL_REGISTROS = convocatoria.Count == 0 ? 0 : convocatoria[0].TOTAL_REGISTROS
            };

            return data;
        }

        [Route("obtenerconvocatoria")]
        [HttpGet]
        public ConvocatoriaBE ObtenerConvocatoria(int idConvocatoria)
        {
            return convocatoriaLN.ObtenerConvocatoria(idConvocatoria);
        }

        [Route("guardarconvocatoria")]
        [HttpPost]
        public ConvocatoriaBE GuardarConvocatoria(ConvocatoriaBE obj)
        {
            ConvocatoriaBE convocatoria = null;
            try
            {
                bool esNuevoRegistro = obj.ID_CONVOCATORIA <= 0;
                convocatoria = convocatoriaLN.RegistroConvocatoria(obj);
                if ((convocatoria.OK && esNuevoRegistro) || obj.ID_ETAPA == 2)
                {
                    List<UsuarioBE> listaUsuario = usuarioLN.ListarUsuarioPorRol((int)EnumsCustom.Roles.POSTULANTE);

                    string fieldConvocatoria = "[CONVOCATORIA]", fieldServer = "[SERVER]";
                    string[] fields = new string[] { fieldConvocatoria, fieldServer };
                    string[] fieldsRequire = new string[] { fieldConvocatoria, fieldServer };
                    Dictionary<string, string> dataBody = new Dictionary<string, string> { [fieldConvocatoria] = obj.NOMBRE, [fieldServer] = AppSettings.Get<string>("Server") };
                    string subject = "";
                    if (obj.ID_ETAPA == 1) subject = $"Informate de la nueva convocatoria {obj.NOMBRE}";
                    else if (obj.ID_ETAPA == 2) subject = $"Inscríbete en la convocatoria {obj.NOMBRE}";
                    //else if (obj.ID_ETAPA == 3) subject = $"Revisión de los requisitos de la convocatoria {obj.NOMBRE}";
                    MailAddressCollection mailTo = new MailAddressCollection();
                    foreach (UsuarioBE usuario in listaUsuario)
                    {
                        mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRES} {usuario.APELLIDOS}"));
                    }
                    if (listaUsuario.Count > 0) {
                        if (obj.ID_ETAPA == 1) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.CreacionConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo)); 
                        else if (obj.ID_ETAPA == 2) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.PostulacionConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                    }
                }

                if (obj.ID_ETAPA == 3) {
                    List<ConvocatoriaBE> lista = convocatoriaLN.listarConvocatoriaEva(new ConvocatoriaBE() { ID_CONVOCATORIA = obj.ID_CONVOCATORIA });
                    string fieldConvocatoria = "[CONVOCATORIA]", fieldServer = "[SERVER]";
                    string[] fields = new string[] { fieldConvocatoria, fieldServer };
                    string[] fieldsRequire = new string[] { fieldConvocatoria, fieldServer };
                    Dictionary<string, string> dataBody = new Dictionary<string, string> {[fieldConvocatoria] = obj.NOMBRE,[fieldServer] = AppSettings.Get<string>("Server") };
                    string subject = "";
                    if (obj.ID_ETAPA == 3) subject = $"Revisión de los requisitos de la convocatoria {obj.NOMBRE}";
                    MailAddressCollection mailTo = new MailAddressCollection();
                    foreach (ConvocatoriaBE usuario in lista)
                    {
                        mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRE}"));
                    }
                    if (lista.Count > 0) {
                        if (obj.ID_ETAPA == 3) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.RevisionReqConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return convocatoria;
        }

        [Route("buscarconvocatoria")]
        [HttpGet]
        public List<ConvocatoriaBE> BuscarConvocatoria(string busqueda, int registros, int pagina, string columna, string orden)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.ListarBusquedaConvocatoria(new ConvocatoriaBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("obtenerconvocatoria")]
        [HttpGet]
        public ConvocatoriaBE GetConvocatoria(int id)
        {
            return convocatoriaLN.getConvocatoria(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
        }

        [Route("cambiarestadoconvocatoria")]
        [HttpPost]
        public bool CambiarEstadoCriterio(ConvocatoriaBE obj)
        {
            ConvocatoriaBE c = convocatoriaLN.EliminarConvocatoria(obj);
            return c.OK;
        }

        [Route("listarconvocatoriareq")]
        [HttpGet]
        public List<ConvocatoriaBE> listarConvocatoriaReq(int id)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaReq(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriacri")]
        [HttpGet]
        public List<CriterioBE> listarConvocatoriaCri(int id)
        {
            List<CriterioBE> lista = new List<CriterioBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaCri(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriaeva")]
        [HttpGet]
        public List<ConvocatoriaBE> listarConvocatoriaEva(int id)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaEva(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriaeta")]
        [HttpGet]
        public List<ConvocatoriaBE> listarConvocatoriaEta(int id)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaEta(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriapos")]
        [HttpGet]
        public List<InstitucionBE> listarConvocatoriaPos(int id)
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaPos(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriainsig")]
        [HttpGet]
        public List<ConvocatoriaInsigniaBE> listarConvocatoriaInsig(int id)
        {
            List<ConvocatoriaInsigniaBE> lista = new List<ConvocatoriaInsigniaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaInsig(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("listarconvocatoriaesttrab")]
        [HttpGet]
        public List<EstrellaTrabajadorCamaBE> listarConvocatoriaEstrellaTrab(int id)
        {
            List<EstrellaTrabajadorCamaBE> lista = new List<EstrellaTrabajadorCamaBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaEstrellaTrab(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("guardarevaluadorpostulante")]
        [HttpPost]
        public bool GuardarEvaluadorPostulante(ConvocatoriaBE obj)
        {
            bool verificar = false;
            try
            {
                verificar = convocatoriaLN.GuardarEvaluadorPostulante(obj);     
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return verificar;
        }

        [Route("guardarconvocatoriaetapainscripcion")]
        [HttpPost]
        public bool GuardarConvocatoriaEtapaInscripcion(ConvocatoriaEtapaInscripcionBE obj)
        {
            bool verificar = false;
            try
            {
                verificar = convocatoriaLN.GuardarConvocatoriaEtapaInscripcion(obj);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return verificar;
        }

        [Route("listarpostulanteevaluador")]
        [HttpGet]
        public List<InstitucionBE> listarPostulanteEvaluador(int idConvocatoria, int idEvaluador)
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();
            try
            {
                lista = convocatoriaLN.listarPostulanteEvaluador(idConvocatoria, idEvaluador);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }

        [Route("deseleccionarpostulante")]
        [HttpPost]
        public bool DeseleccionarPostulante(ConvocatoriaBE obj)
        {
            bool verificar = false;
            try
            {
                verificar = convocatoriaLN.DeseleccionarPostulante(obj);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return verificar;
        }

    }
}
