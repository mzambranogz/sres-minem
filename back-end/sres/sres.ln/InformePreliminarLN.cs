using Oracle.DataAccess.Client;
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
using System.Net.Mail;

namespace sres.ln
{
    public class InformePreliminarLN : BaseLN
    {
        InformePreliminarDA informeDA = new InformePreliminarDA();
        UsuarioDA usuarioDA = new UsuarioDA();
        Mailing mailing = new Mailing();
        Mailing mailing_ = new Mailing();
        public List<InscripcionBE> listaInscripcionConvocatoriaEvaluador(ConvocatoriaBE entidad)
        {
            List<InscripcionBE> lista = new List<InscripcionBE>();
            try
            {
                cn.Open();
                lista = informeDA.listaInscripcionConvocatoriaEvaluador(entidad, cn);
                if (lista.Count > 0)
                {
                    string contenidoInforme = "";
                    List<dynamic> listaEnvios = new List<dynamic>();

                    foreach (InscripcionBE ins in lista)
                    {
                        List<ConvocatoriaCriterioPuntajeInscripBE> listaInsc = informeDA.obtenerInscripcionEvaluacion(ins, cn);
                        if (listaInsc.Count > 0)
                        {
                            string contenido = "";
                            string contenidoTemp = "";
                            foreach (ConvocatoriaCriterioPuntajeInscripBE ccpi in listaInsc)
                            {
                                contenido += $"<tr><td style='padding:5px;'><span><strong>{ccpi.NOMBRE_CRI}</strong></span><br><span><strong>Observación: </strong>{ccpi.OBSERVACION}</span></td></tr>";
                                contenidoTemp += $"<span><strong>{ccpi.NOMBRE_CRI}</strong></span><br><span><strong>Observación: </strong>{ccpi.OBSERVACION}</span></br>";
                            }
                            contenidoInforme += $"<tr><td style='padding:5px;'><span>La entidad <strong>{ins.RAZON_SOCIAL}</strong> con el responsable de la información </span><strong>{ins.NOMBRES_USU}</strong>, se identificaron las siguientes observaciones:<br>{contenidoTemp}</td></tr>";
                            string fieldConvocatoria = "[CONTENIDO]", fieldServer = "[SERVER]", nombres = "[NOMBRES]";
                            string[] fields = new string[] { fieldConvocatoria, fieldServer, nombres };
                            string[] fieldsRequire = new string[] { fieldConvocatoria, fieldServer, nombres };
                            Dictionary<string, string> dataBody = new Dictionary<string, string> {[fieldConvocatoria] = contenido,[fieldServer] = AppSettings.Get<string>("Server"),[nombres] = ins.NOMBRES_USU };
                            string subject = $"Levantamiento de observaciones, convocatoria - {ins.NOMBRE_CONV}";
                            MailAddressCollection mailTo = new MailAddressCollection();
                            mailTo.Add(new MailAddress(ins.CORREO));

                            dynamic envio = new
                            {
                                Template = Mailing.Templates.LevantamientoObservacion,
                                Databody = dataBody,
                                Fields = fields,
                                FieldsRequire = fieldsRequire,
                                Subject = subject,
                                MailTo = mailTo
                            };

                            listaEnvios.Add(envio);
                        }
                    }

                    Task.Factory.StartNew(() =>
                    {
                        foreach (dynamic item in listaEnvios)
                        {
                            mailing.SendMail(item.Template, item.Databody, item.Fields, item.FieldsRequire, item.Subject, item.MailTo);
                        }

                        UsuarioBE usuario = usuarioDA.getAdministrador(cn);
                        string fieldConvocatoria_ = "[CONTENIDO]", fieldServer_ = "[SERVER]", nombres_ = "[NOMBRES]";
                        string[] fields_ = new string[] { fieldConvocatoria_, fieldServer_, nombres_ };
                        string[] fieldsRequire_ = new string[] { fieldConvocatoria_, fieldServer_, nombres_ };
                        Dictionary<string, string> dataBody_ = new Dictionary<string, string> {[fieldConvocatoria_] = contenidoInforme,[fieldServer_] = AppSettings.Get<string>("Server"),[nombres_] = "Ja" };
                        string subject_ = $"Informe Preliminar, convocatoria - {lista[0].NOMBRE_CONV}";
                        MailAddressCollection mailTo_ = new MailAddressCollection();
                        mailTo_.Add(new MailAddress(usuario.CORREO));
                        mailing.SendMail(Mailing.Templates.InformePreliminar, dataBody_, fields_, fieldsRequire_, subject_, mailTo_);
                    });
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
