using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace sres.ut
{
    public class Mailing
    {
        public enum Templates {
            [Description("CreacionUsuario.html")]
            CreacionUsuario,
            [Description("AprobacionUsuario.html")]
            AprobacionUsuario,
            [Description("DeshabilitarUsuario.html")]
            DeshabilitarUsuario,
            RecuperacionClave,
            [Description("CreacionConvocatoria.html")]
            CreacionConvocatoria,
            [Description("InscripcionConvocatoria.html")]
            InscripcionConvocatoria,
            [Description("LevantamientoObservacion.html")]
            LevantamientoObservacion,
            [Description("InformePreliminar.html")]
            InformePreliminar
        }

        static string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        static string templateDirectory = AppSettings.Get<string>("Mailing.TemplateDirectory");
        static string imagesDirectory = AppSettings.Get<string>("Mailing.ImagesDirectory");
        static string displayNameFrom = AppSettings.Get<string>("Mailing.Mail.From.DisplayName");
        static string addressFrom = AppSettings.Get<string>("Mailing.Mail.From.Address");
        static string host = AppSettings.Get<string>("Mailing.Smtp.Host");
        static int port = AppSettings.Get<int>("Mailing.Smtp.Port");
        static string credencialCorreo = AppSettings.Get<string>("Mailing.Smtp.Credentials.UserName");
        static string credencialContraseña = AppSettings.Get<string>("Mailing.Smtp.Credentials.Password");
        static bool enableSsl = AppSettings.Get<bool>("Mailing.Smtp.EnableSsl");
        static bool useDefaultCredentials = AppSettings.Get<bool>("Mailing.Smtp.UseDefaultCredentials");

        SmtpClient smtp = new SmtpClient(host, port);

        public Mailing()
        {
            smtp.EnableSsl = enableSsl;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = useDefaultCredentials;
            smtp.Credentials = new NetworkCredential(credencialCorreo, credencialContraseña);
        }


        void AddAddressCollection(MailAddressCollection addressCollectionField, MailAddressCollection addressCollection)
        {
            if (addressCollection != null)
            {
                foreach (MailAddress cc in addressCollection)
                {
                    addressCollectionField.Add(cc);
                }
            }
        }

        void ValidateDataBody(string templateBody, Dictionary<string, string> dataBody, string[] fields, string[] fieldsRequire, out string outTemplateBody, out string[] outFieldsNotExists, out string[] outFieldsRequireWithNullValue)
        {
            List<string> keysFieldNotExists = new List<string>();
            List<string> keysFieldRequireWithNullValue = new List<string>();

            outFieldsNotExists = keysFieldNotExists.ToArray();
            outFieldsRequireWithNullValue = keysFieldRequireWithNullValue.ToArray();
            outTemplateBody = templateBody;

            foreach (string key in fields)
            {
                if (!dataBody.ContainsKey(key)) keysFieldNotExists.Add(key);
                else
                {
                    if (fieldsRequire.Contains(key) && dataBody[key] == null) keysFieldRequireWithNullValue.Add(key);
                    else outTemplateBody = outTemplateBody.Replace(key, string.IsNullOrEmpty(dataBody[key]) ? "" : dataBody[key]);
                }
            }
        }

        void AddImages(AlternateView av, Dictionary<string, Dictionary<string, string>> images)
        {
            if (images != null)
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> image in images)
                {
                    string directoryImages = Path.Combine(imagesDirectory);
                    string imageSource = $"{Path.Combine(basePath, directoryImages, image.Value.Keys.First())}";
                    if (!File.Exists(imageSource)) continue;
                    LinkedResource img = new LinkedResource(imageSource, image.Value.Values.First());
                    img.ContentId = image.Key;
                    av.LinkedResources.Add(img);
                }
            }
        }

        public bool SendMail(Templates template, Dictionary<string, string> dataBody, string[] fields, string[] fieldsRequire, string subject, MailAddressCollection addressTo, MailAddressCollection addressCC = null, MailAddressCollection addressCCO = null)
        {
            bool sendMail = false;

            try
            {
                string directoryTemplate = Path.Combine(templateDirectory);
                string fileName = template.GetAttributeOfType<DescriptionAttribute>().Description;
                string fullPathFile = Path.Combine(basePath, directoryTemplate, fileName);

                if (!File.Exists(fullPathFile)) throw new Exception($"No se encontró la ruta especificada: {fullPathFile}");

                string templateBody = File.ReadAllText(fullPathFile);

                string[] fieldsNotExists, fieldsRequireWithNullValue;

                ValidateDataBody(templateBody, dataBody, fields, fieldsRequire, out templateBody, out fieldsNotExists, out fieldsRequireWithNullValue);

                if (fieldsNotExists.Length > 0) throw new Exception($"No se encontraron los siguientes campos: {string.Join(", ", fieldsNotExists)}");
                if (fieldsRequireWithNullValue.Length > 0) throw new Exception($"Los siguientes campos requeridos no tienen valor: {string.Join(", ", fieldsRequireWithNullValue)}");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(addressFrom, displayNameFrom);

                if (addressTo == null) throw new Exception("No existe datos del destinatario");
                if (addressTo.Count == 0) throw new Exception("No existe datos del destinatario");

                AddAddressCollection(mail.To, addressTo);
                AddAddressCollection(mail.CC, addressCC);
                AddAddressCollection(mail.Bcc, addressCCO);

                mail.Subject = subject;

                AlternateView viewHtml = AlternateView.CreateAlternateViewFromString(templateBody, Encoding.UTF8, MediaTypeNames.Text.Html);

                Dictionary<string, Dictionary<string, string>> images = new Dictionary<string, Dictionary<string, string>>
                {
                    ["imagenMEM"] = new Dictionary<string, string> { ["logo-minem.jpg"] = MediaTypeNames.Image.Jpeg },
                    ["imagenBanner"] = new Dictionary<string, string> { ["sres-logo.png"] = "image/png" },
                    ["imagenGEF"] = new Dictionary<string, string> { ["logo_gef.jpg"] = MediaTypeNames.Image.Jpeg },
                    ["imagenPNUD"] = new Dictionary<string, string> { ["logo_pnud.jpg"] = MediaTypeNames.Image.Jpeg }
                };

                AddImages(viewHtml, images);

                mail.AlternateViews.Add(viewHtml);

                smtp.Send(mail);
                sendMail = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return sendMail;
        }

        //private string CuerpoRecuperarClave(UsuarioBE entidad, string server)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
        //    sb.Append("<head>");
        //    sb.Append("<title>");
        //    sb.Append("</title></head>");
        //    sb.Append("<body>");
        //    sb.Append("<div style=\"font-family: Roboto;font-size:13px;margin:0 auto;margin-top:50px;width:650px;\" ><img src=\"cid:imagenBanner\" width=\"150\" />");
        //    sb.Append("     <div style=\"border-bottom: 1px solid #ededed;\"></div><br/><br/>");
        //    sb.Append("     <div style=\"border-left:1px solid #ededed;margin:10px;padding:10px;\">");
        //    sb.Append("         <table style=\"font-family: Roboto;font-size:13px;\">");
        //    sb.Append("             <tr>");
        //    sb.Append("                 <td style=\"padding:5px;\"><strong> Estimado Usuario: &nbsp;</strong><span>" + entidad.NOMBRES + " , podrá reestablecer su contraseña a través de este link: </span><br/><span><a href=\"" + server + "Portal/ReestablecerClave/" + entidad.ID_USUARIO + "\">" + server + "Portal/ReestablecerClave/" + entidad.ID_USUARIO + "</a></span></td>");
        //    sb.Append("             </tr>");
        //    sb.Append("         </table>");
        //    sb.Append("     <div style=\"border-left:1px solid #ededed;margin-top:50px;\">");
        //    sb.Append("         <table style=\"font-family: Roboto; font-size:13px;\" >");
        //    sb.Append("             <tr>");
        //    sb.Append("                 <td><img src=\"cid:imagenMEM\" height=\"40\" style=\"margin-right:7px;\" /><img src=\"cid:imagenGEF\" height=\"45\" style =\"margin-right:7px;\" /><img src=\"cid:imagenPNUD\" height =\"51\" style=\"margin-right:7px;\" /></td>");
        //    sb.Append("             </tr>");
        //    sb.Append("             <tr>");
        //    sb.Append("                 <td style=\"text-align: justify;\" ><br/>");
        //    sb.Append("                     <p> AVISO DE CONFIDENCIALIDAD</p>");
        //    sb.Append("                     <p> Esta Dirección de correo y sus anexos son de propiedad del Ministerio de Energía y Minas y pueden contener información confidencial e información privilegiada.Si no es el destinatario, por favor notifique al remitente inmediatamente retornando el e - mail, eliminar este correo electrónico y destruir todas las copias.Toda difusión o la utilización de esta información por una persona distinta del destinatario no están autorizado y puede ser ilegal.</p><br/>");
        //    sb.Append("                     <p> CONFIDENTIALITY STATEMENT </p>");
        //    sb.Append("                     <p> This e-mail and its attachments are owned by the Ministry of Energy and Mines and may contain confidential and privileged information.If you are not the intended recipient, please notify the sender immediately, return e - mail, delete this e - mail and destroy all copies. Any dissemination or use of this information by a person other than the recipient is not authorized and may be unlawful.</p>");
        //    sb.Append("                 </td>");
        //    sb.Append("             </tr>");
        //    sb.Append("         </table>");
        //    sb.Append("     </div>");
        //    sb.Append("</div>");
        //    sb.Append("</body>");
        //    sb.Append("</html>");

        //    return sb.ToString();
        //}

    }
}
