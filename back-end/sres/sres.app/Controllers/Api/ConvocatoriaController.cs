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
        InformePreliminarLN informeLN = new InformePreliminarLN();

        Mailing mailing = new Mailing();

        [Route("buscarconvocatoria")]
        [HttpGet]
        public DataPaginateBE BuscarConvocatoria(string nroInforme, string nombre, DateTime? fechaDesde, DateTime? fechaHasta, int registros, int pagina, string columna, string orden, int idInstitucion, int idUsuario)
        {
            List<ConvocatoriaBE> convocatoria = convocatoriaLN.BuscarConvocatoria(nroInforme, nombre, fechaDesde, fechaHasta, registros, pagina, columna, orden, idInstitucion, idUsuario);

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
                if (convocatoria.OK && obj.VALIDAR_ETAPA == 1)
                {
                    if ((convocatoria.OK && esNuevoRegistro) || obj.ID_ETAPA == 2 || obj.ID_ETAPA == 4 || obj.ID_ETAPA == 6 || obj.ID_ETAPA == 7 || obj.ID_ETAPA == 10 || obj.ID_ETAPA == 14)
                    {
                        List<dynamic> listaEnvios = new List<dynamic>();
                        List<UsuarioBE> listaUsuario = new List<UsuarioBE>();
                        if (obj.ID_ETAPA == 1 || obj.ID_ETAPA == 2) listaUsuario = usuarioLN.ListarUsuarioPorRol((int)EnumsCustom.Roles.POSTULANTE);
                        else if (obj.ID_ETAPA == 4 || obj.ID_ETAPA == 6 || obj.ID_ETAPA == 7 || obj.ID_ETAPA == 10) listaUsuario = usuarioLN.ListarUsuarioResponsable(obj.ID_CONVOCATORIA);
                        else if (obj.ID_ETAPA == 14) listaUsuario = usuarioLN.ListarUsuarioResponsableAll(obj.ID_CONVOCATORIA);

                        string fieldConvocatoria = "[CONVOCATORIA]", fieldServer = "[SERVER]", fieldNombres = "[NOMBRES]", fieldFecha = "[FECHA]", fieldEntidad = "[ENTIDAD]";
                        string[] fields = new string[] { fieldConvocatoria, fieldServer, fieldNombres, fieldFecha, fieldEntidad };
                        string[] fieldsRequire = new string[] { fieldConvocatoria, fieldServer, fieldNombres, fieldFecha, fieldEntidad };
                        string subject = "";
                        if (obj.ID_ETAPA == 1) subject = $"Informate de la nueva convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 2) subject = $"Inscríbete en la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 4) subject = $"Subsanación de requisitos de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 6) subject = $"Recopilación de información de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 7) subject = $"Coordinación previa a la revisión de la información de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 10) subject = $"Levantamiento de observaciones de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 14) subject = $"Finalización de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        //MailAddressCollection mailTo = new MailAddressCollection();
                        //foreach (UsuarioBE usuario in listaUsuario)
                        //{
                        //    mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRES} {usuario.APELLIDOS}"));
                        //}

                        //if (obj.ID_ETAPA == 7) convocatoria.VAL = convocatoriaLN.TrazabilidadEtapa(obj.ID_CONVOCATORIA, Convert.ToInt16(obj.ID_ETAPA), obj.USUARIO_GUARDAR);
                        //else if (obj.ID_ETAPA == 14) convocatoria.VAL = convocatoriaLN.TrazabilidadEtapa(obj.ID_CONVOCATORIA, Convert.ToInt16(obj.ID_ETAPA), obj.USUARIO_GUARDAR);

                        //if (listaUsuario.Count > 0)
                        //{
                        //    if (obj.ID_ETAPA == 1) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.CreacionConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 2) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.PostulacionConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 4) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.DocSolicitadosConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 6) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.RecopilacionInfConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 7) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.CoordinacionConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 10) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.LevantamientoObsConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 14) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.FinalizacionConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //}             

                        if (listaUsuario.Count > 0)
                        {
                            foreach (UsuarioBE usuario in listaUsuario)
                            {
                                Dictionary<string, string> dataBody = new Dictionary<string, string> {[fieldConvocatoria] = obj.FECHA_INICIO.Year.ToString(),[fieldServer] = AppSettings.Get<string>("Server"),[fieldNombres] = $"{usuario.NOMBRES} {usuario.APELLIDOS}", [fieldFecha] = DateTime.Now.ToString("dd/MM/yyyy"), [fieldEntidad] = usuario.RAZON_SOCIAL };
                                MailAddressCollection mailTo = new MailAddressCollection();
                                mailTo.Add(new MailAddress(usuario.CORREO));
                                var template = Mailing.Templates.CreacionConvocatoria;
                                if (obj.ID_ETAPA == 1) template = Mailing.Templates.CreacionConvocatoria;
                                else if (obj.ID_ETAPA == 2) template = Mailing.Templates.PostulacionConvocatoria;
                                else if (obj.ID_ETAPA == 4) template = Mailing.Templates.DocSolicitadosConvocatoria;
                                else if (obj.ID_ETAPA == 6) template = Mailing.Templates.RecopilacionInfConvocatoria;
                                else if (obj.ID_ETAPA == 7) template = Mailing.Templates.CoordinacionConvocatoria;
                                else if (obj.ID_ETAPA == 10) template = Mailing.Templates.LevantamientoObsConvocatoria;
                                else if (obj.ID_ETAPA == 14) template = Mailing.Templates.FinalizacionConvocatoria;

                                dynamic envio = new
                                {
                                    Template = template,
                                    Databody = dataBody,
                                    Fields = fields,
                                    FieldsRequire = fieldsRequire,
                                    Subject = subject,
                                    MailTo = mailTo
                                };
                                listaEnvios.Add(envio);
                            }
                        }

                        if (obj.ID_ETAPA == 7) convocatoria.VAL = convocatoriaLN.TrazabilidadEtapa(obj.ID_CONVOCATORIA, Convert.ToInt16(obj.ID_ETAPA), obj.USUARIO_GUARDAR);
                        else if (obj.ID_ETAPA == 14) convocatoria.VAL = convocatoriaLN.TrazabilidadEtapa(obj.ID_CONVOCATORIA, Convert.ToInt16(obj.ID_ETAPA), obj.USUARIO_GUARDAR);

                        Task.Factory.StartNew(() =>
                        {
                            if (listaEnvios.Count > 0)
                            {
                                foreach (dynamic item in listaEnvios)
                                {
                                    mailing.SendMail(item.Template, item.Databody, item.Fields, item.FieldsRequire, item.Subject, item.MailTo);
                                }
                            }
                        });
                    }

                    if (obj.ID_ETAPA == 3 || obj.ID_ETAPA == 5 || obj.ID_ETAPA == 8 || obj.ID_ETAPA == 11)
                    {
                        List<dynamic> listaEnvios = new List<dynamic>();
                        List<ConvocatoriaBE> lista = convocatoriaLN.listarConvocatoriaEva(new ConvocatoriaBE() { ID_CONVOCATORIA = obj.ID_CONVOCATORIA });
                        string fieldConvocatoria = "[CONVOCATORIA]", fieldServer = "[SERVER]", fieldNombres = "[NOMBRES]";
                        string[] fields = new string[] { fieldConvocatoria, fieldServer, fieldNombres };
                        string[] fieldsRequire = new string[] { fieldConvocatoria, fieldServer, fieldNombres };
                        //Dictionary<string, string> dataBody = new Dictionary<string, string> {[fieldConvocatoria] = obj.NOMBRE,[fieldServer] = AppSettings.Get<string>("Server") };
                        string subject = "";
                        if (obj.ID_ETAPA == 3) subject = $"Revisión de los requisitos de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 5) subject = $"Filtrado de los participantes de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 8) subject = $"Primera Revisión de la información de los participantes de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 9) subject = $"Informe Preliminar de la información de los participantes de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 11) subject = $"Segunda Revisión de la información de los participantes de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        else if (obj.ID_ETAPA == 12) subject = $"Informe Final de la información de los participantes de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                        //MailAddressCollection mailTo = new MailAddressCollection();
                        //foreach (ConvocatoriaBE usuario in lista)
                        //{
                        //    mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRE}"));
                        //}
                        //if (lista.Count > 0)
                        //{
                        //    if (obj.ID_ETAPA == 3) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.RevisionReqConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 5) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.FiltradoConvocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 8) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.RevisionN1Convocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //    else if (obj.ID_ETAPA == 11) Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.RevisionN2Convocatoria, dataBody, fields, fieldsRequire, subject, mailTo));
                        //}

                        if (lista.Count > 0)
                        {
                            foreach (ConvocatoriaBE usuario in lista)
                            {
                                Dictionary<string, string> dataBody = new Dictionary<string, string> {[fieldConvocatoria] = obj.FECHA_INICIO.Year.ToString(),[fieldServer] = AppSettings.Get<string>("Server"), [fieldNombres] = usuario.NOMBRE };
                                MailAddressCollection mailTo = new MailAddressCollection();
                                mailTo.Add(new MailAddress(usuario.CORREO));
                                var template = Mailing.Templates.RevisionReqConvocatoria;
                                if (obj.ID_ETAPA == 3) template = Mailing.Templates.RevisionReqConvocatoria;
                                else if (obj.ID_ETAPA == 5) template = Mailing.Templates.FiltradoConvocatoria;
                                else if (obj.ID_ETAPA == 8) template = Mailing.Templates.RevisionN1Convocatoria;
                                else if (obj.ID_ETAPA == 9) template = Mailing.Templates.InformePreConvocatoria;
                                else if (obj.ID_ETAPA == 11) template = Mailing.Templates.RevisionN2Convocatoria;
                                else if (obj.ID_ETAPA == 12) template = Mailing.Templates.InformeFinalConvocatoria;
                                //else if (obj.ID_ETAPA == 7) template = Mailing.Templates.CoordinacionConvocatoria;
                                //else if (obj.ID_ETAPA == 10) template = Mailing.Templates.LevantamientoObsConvocatoria;

                                dynamic envio = new
                                {
                                    Template = template,
                                    Databody = dataBody,
                                    Fields = fields,
                                    FieldsRequire = fieldsRequire,
                                    Subject = subject,
                                    MailTo = mailTo
                                };
                                listaEnvios.Add(envio);
                            }

                            Task.Factory.StartNew(() =>
                            {
                                if (listaEnvios.Count > 0)
                                {
                                    foreach (dynamic item in listaEnvios)
                                    {
                                        mailing.SendMail(item.Template, item.Databody, item.Fields, item.FieldsRequire, item.Subject, item.MailTo);
                                    }
                                }
                            });
                        }
                    }

                    if (obj.ID_ETAPA == 13)
                    {
                        List<InscripcionBE> lista = informeLN.listaResultadosParticipantes(obj.ID_CONVOCATORIA);
                        if (lista.Count > 0)
                        {
                            string contenidoInforme = "";
                            List<dynamic> listaEnvios = new List<dynamic>();
                            foreach (InscripcionBE ins in lista)
                            {
                                string resultado = "";
                                ReconocimientoBE recon = informeLN.obtenerReconocimientoInscripcion(ins);
                                if (recon != null)
                                {
                                    List<ReconocimientoMedidaBE> listaRM = informeLN.obtenerReconocimientoInscripcionMedida(recon.ID_RECONOCIMIENTO);
                                    string mejora = recon.FLAG_MEJORACONTINUA == "1" ? "SI" : "NO";
                                    string emisiones = recon.FLAG_EMISIONESMAX == "1" ? "SI" : "NO";
                                    resultado += $"<span><strong>Categorización del sello de reconocimiento: </strong>{recon.CATEGORIA}</span><br>";
                                    resultado += $"<span><strong>Reconocimiento por reducción de emisiones GEI (tCO<sub>2</sub>): </strong>{recon.ESTRELLA}</span><br>";
                                    if (listaRM.Count > 0)
                                    {
                                        resultado += $"<span><strong>Reconocimiento por cada medida que aportan a las NDC: </strong></span><br>";
                                        foreach (ReconocimientoMedidaBE rm in listaRM)
                                        {
                                            if (rm.OBTENIDO == "1")
                                                resultado += $"<span><strong>{rm.NOMBRE_MEDMIT}: </strong>SI</span><br>";
                                            else
                                                resultado += $"<span><strong>{rm.NOMBRE_MEDMIT}: </strong>NO</span><br>";
                                        }
                                    }
                                    else
                                    {
                                        resultado += $"<span><strong>Reconocimiento por cada medida que aportan a las NDC: </strong>NO</span><br>";
                                    }
                                    resultado += $"<span><strong>Reconocimiento destacado por reducción de emisiones GEI (tCO<sub>2</sub>): </strong>{emisiones}</span><br>";
                                    resultado += $"<span><strong>Reconocimiento destacado a la mejora continua de energía sostenible: </strong>{mejora}</span><br>";
                                }
                                else
                                {
                                    resultado += $"<span><strong>Categorización del sello de reconocimiento: </strong>NO</span><br>";
                                    resultado += $"<span><strong>Reconocimiento por reducción de emisiones GEI (tCO<sub>2</sub>): </strong>NO</span><br>";
                                    resultado += $"<span><strong>Reconocimiento por cada medida que aportan a las NDC: </strong>NO</span><br>";
                                    resultado += $"<span><strong>Reconocimiento destacado por reducción de emisiones GEI (tCO<sub>2</sub>): </strong>NO</span><br>";
                                    resultado += $"<span><strong>Reconocimiento destacado a la mejora continua de energía sostenible: </strong>NO</span><br>";
                                }
                                contenidoInforme += $"<tr><td style='padding:5px;'>{resultado}</td></tr>";

                                string fieldConvocatoria = "[CONVOCATORIA]", fieldServer = "[SERVER]", nombres = "[NOMBRES]", contenido = "[CONTENIDO]", entidad = "[ENTIDAD]";
                                string[] fields = new string[] { fieldConvocatoria, fieldServer, nombres, contenido, entidad };
                                string[] fieldsRequire = new string[] { fieldConvocatoria, fieldServer, nombres, contenido, entidad };
                                Dictionary<string, string> dataBody = new Dictionary<string, string> {[fieldConvocatoria] = obj.FECHA_INICIO.Year.ToString(),[fieldServer] = AppSettings.Get<string>("Server"),[nombres] = ins.NOMBRES_USU,[contenido] = contenidoInforme, [entidad] = ins.RAZON_SOCIAL };
                                string subject = $"Resultados de la convocatoria del Reconocimiento de Energía Eficiente y Sostenible por el periodo {obj.FECHA_INICIO.Year.ToString()}";
                                MailAddressCollection mailTo = new MailAddressCollection();
                                mailTo.Add(new MailAddress(ins.CORREO));

                                dynamic envio = new
                                {
                                    Template = Mailing.Templates.ResultadosConvocatoria,
                                    Databody = dataBody,
                                    Fields = fields,
                                    FieldsRequire = fieldsRequire,
                                    Subject = subject,
                                    MailTo = mailTo
                                };

                                listaEnvios.Add(envio);
                            }

                            convocatoria.VAL = convocatoria.VAL = convocatoriaLN.TrazabilidadEtapa(obj.ID_CONVOCATORIA, Convert.ToInt16(obj.ID_ETAPA), obj.USUARIO_GUARDAR);

                            Task.Factory.StartNew(() =>
                            {
                                foreach (dynamic item in listaEnvios)
                                {
                                    mailing.SendMail(item.Template, item.Databody, item.Fields, item.FieldsRequire, item.Subject, item.MailTo);
                                }
                            });
                        }
                    }
                }
                else
                {
                    if (obj.ID_ETAPA == 7) convocatoria.VAL = 1;
                    if (obj.ID_ETAPA == 14) convocatoria.VAL = 1;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return convocatoria;
        }

        [Route("buscarconvocatoria")]
        [HttpGet]
        public List<ConvocatoriaBE> BuscarConvocatoria(string codigo, string nombre, DateTime? fechaDesde, DateTime? fechaHasta, int registros, int pagina, string columna, string orden)
        {
            List<ConvocatoriaBE> lista = new List<ConvocatoriaBE>();
            try
            {
                lista = convocatoriaLN.ListarBusquedaConvocatoria(new ConvocatoriaBE() { CODIGO = codigo == "" ? 0 : Convert.ToInt32(codigo), NOMBRE = nombre, FECHA_DESDE = fechaDesde, FECHA_HASTA = fechaHasta, CANTIDAD_REGISTROS = registros, PAGINA = pagina, ORDER_BY = columna, ORDER_ORDEN = orden });
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

        [Route("listarconvocatoriacridet")]
        [HttpGet]
        public List<CriterioBE> listarConvocatoriaCriDetalle(int id)
        {
            List<CriterioBE> lista = new List<CriterioBE>();
            try
            {
                lista = convocatoriaLN.listarConvocatoriaCriDetalle(new ConvocatoriaBE() { ID_CONVOCATORIA = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return lista;
        }
    }
}
