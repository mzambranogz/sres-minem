﻿using Oracle.DataAccess.Client;
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
        public List<InscripcionBE> generarInformePreliminar(ConvocatoriaBE entidad)
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
                        List<ConvocatoriaCriterioPuntajeInscripBE> listaInsc = informeDA.obtenerInscripcionEvaluacion(ins, 1, cn);
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

        public List<InscripcionBE> generarInformeFinal(ConvocatoriaBE entidad)
        {
            List<InscripcionBE> lista = new List<InscripcionBE>();
            try
            {
                cn.Open();
                lista = informeDA.listaInscripcionConvocatoriaEvaluador(entidad, cn);
                if (lista.Count > 0)
                {
                    string contenidoInforme = "";
                    foreach (InscripcionBE ins in lista)
                    {
                        string empresa = "";
                        List<ConvocatoriaCriterioPuntajeInscripBE> listaInsc = informeDA.obtenerInscripcionEvaluacion(ins, 0, cn);
                        if (listaInsc.Count > 0)
                        {
                            string contenido = "";
                            foreach (ConvocatoriaCriterioPuntajeInscripBE ccpi in listaInsc)
                            {
                                contenido += $"<span><strong>{ccpi.NOMBRE_CRI}</strong></span><br><span><strong>Observación: </strong>{ccpi.OBSERVACION}</span><br>";
                            }
                            empresa = $"<span><strong>Empresa:</strong> {ins.RAZON_SOCIAL}</span>";
                            contenidoInforme += $"<tr><td style='padding:5px;'>{empresa}<br><span><strong>Responsable de la información:</strong> {ins.NOMBRES_USU}</span><br>Detalles de la evaluación:<br>{contenido}</td></tr>";
                        }
                    }
                    contenidoInforme += $"<tr><td style='padding:5px;'>Los resultados obtenidos por las empresas son los siguientes:</td></tr>";
                    foreach (InscripcionBE ins in lista)
                    {
                        string resultado = "";
                        ReconocimientoBE recon = informeDA.obtenerReconocimientoInscripcion(ins, cn);
                        if (recon != null) {
                            List<ReconocimientoMedidaBE> listaRM = informeDA.obtenerReconocimientoInscripcionMedida(recon.ID_RECONOCIMIENTO, cn);
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

                            contenidoInforme += $"<tr><td style='padding:5px;'><span><strong>Empresa:</strong> {recon.RAZON_SOCIAL}</span><br>{resultado}</td></tr>";
                        }                        
                    }

                    UsuarioBE usuario = usuarioDA.getAdministrador(cn);
                    string fieldConvocatoria_ = "[CONTENIDO]", fieldServer_ = "[SERVER]", nombres_ = "[NOMBRES]";
                    string[] fields_ = new string[] { fieldConvocatoria_, fieldServer_, nombres_ };
                    string[] fieldsRequire_ = new string[] { fieldConvocatoria_, fieldServer_, nombres_ };
                    Dictionary<string, string> dataBody_ = new Dictionary<string, string> {[fieldConvocatoria_] = contenidoInforme,[fieldServer_] = AppSettings.Get<string>("Server"),[nombres_] = $"{usuario.NOMBRES} {usuario.APELLIDOS}" };
                    string subject_ = $"Informe Final, convocatoria - {lista[0].NOMBRE_CONV}";
                    MailAddressCollection mailTo_ = new MailAddressCollection();
                    mailTo_.Add(new MailAddress(usuario.CORREO));

                    Task.Factory.StartNew(() =>
                    {                       
                        mailing.SendMail(Mailing.Templates.InformeFinal, dataBody_, fields_, fieldsRequire_, subject_, mailTo_);
                    });
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<InscripcionBE> listaResultadosParticipantes(int idConvocatoria) {
            List<InscripcionBE> lista = new List<InscripcionBE>();
            try
            {
                cn.Open();
                lista = informeDA.listaResultadosParticipantes(idConvocatoria, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public ReconocimientoBE obtenerReconocimientoInscripcion(InscripcionBE entidad)
        {
            ReconocimientoBE item = new ReconocimientoBE();
            try
            {
                cn.Open();
                item = informeDA.obtenerReconocimientoInscripcion(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<ReconocimientoMedidaBE> obtenerReconocimientoInscripcionMedida(int idReconocimiento)
        {
            List<ReconocimientoMedidaBE> lista = new List<ReconocimientoMedidaBE>();
            try
            {
                cn.Open();
                lista = informeDA.obtenerReconocimientoInscripcionMedida(idReconocimiento, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
