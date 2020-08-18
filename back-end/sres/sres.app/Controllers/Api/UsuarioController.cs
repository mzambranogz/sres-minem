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
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        UsuarioLN usuarioLN = new UsuarioLN();
        InstitucionLN institucionLN = new InstitucionLN();
        SectorLN sectorLN = new SectorLN();

        Mailing mailing = new Mailing();

        [Route("buscarusuario")]
        [HttpGet]
        public List<UsuarioBE> BuscarUsuario(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return usuarioLN.BuscarUsuario(busqueda, registros, pagina, columna, orden);
        }

        [Route("obtenerusuario")]
        [HttpGet]
        public UsuarioBE ObtenerUsuario(int idUsuario)
        {
            return usuarioLN.ObtenerUsuario(idUsuario);
        }

        [Route("obtenerusuarioporinstitucioncorreo")]
        [HttpGet]
        public Dictionary<string, object> ObtenerUsuarioPorInstitucionCorreo(int idInstitucion, string correo)
        {
            UsuarioBE usuario = usuarioLN.ObtenerUsuarioPorInstitucionCorreo(idInstitucion, correo);
            return new Dictionary<string, object>
            {
                ["EXISTE"] = usuario != null,
                ["USUARIO"] = usuario
            };
        }

        [Route("cambiarestadousuario")]
        [HttpPost]
        public bool CambiarEstadoUsuario(UsuarioBE usuario)
        {
            bool habilitar = usuario.FLAG_ESTADO == "1";
            bool seGuardo = usuarioLN.CambiarEstadoUsuario(usuario);

            if (seGuardo)
            {
                string fieldNombres = "[NOMBRES]", fieldApellidos = "[APELLIDOS]", fieldServer = "[SERVER]";
                usuario = usuarioLN.ObtenerUsuario(usuario.ID_USUARIO);

                if (habilitar)
                {
                    string[] fields = new string[] { fieldNombres, fieldApellidos, fieldServer };
                    string[] fieldsRequire = new string[] { fieldNombres, fieldApellidos, fieldServer };
                    Dictionary<string, string> dataBody = new Dictionary<string, string> { [fieldNombres] = usuario.NOMBRES, [fieldApellidos] = usuario.APELLIDOS, [fieldServer] = AppSettings.Get<string>("Server") };
                    string subject = $"{usuario.NOMBRES} {usuario.APELLIDOS}, Su cuenta ha sido aprobada en nuestra plataforma SRES del sector energía";
                    MailAddressCollection mailTo = new MailAddressCollection();
                    mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRES} {usuario.APELLIDOS}"));

                    Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.AprobacionUsuario, dataBody, fields, fieldsRequire, subject, mailTo));
                }
                else
                {
                    string[] fields = new string[] { fieldNombres, fieldApellidos };
                    string[] fieldsRequire = new string[] { fieldNombres, fieldApellidos };
                    Dictionary<string, string> dataBody = new Dictionary<string, string> { [fieldNombres] = usuario.NOMBRES, [fieldApellidos] = usuario.APELLIDOS };
                    string subject = $"{usuario.NOMBRES} {usuario.APELLIDOS}, Su cuenta ha sido deshabilitada en nuestra plataforma SRES del sector energía";
                    MailAddressCollection mailTo = new MailAddressCollection();
                    mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRES} {usuario.APELLIDOS}"));

                    Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.DeshabilitarUsuario, dataBody, fields, fieldsRequire, subject, mailTo));
                }
            }

            return seGuardo;
        }

        [Route("guardarusuario")]
        [HttpPost]
        public bool GuardarUsuario(UsuarioBE usuario)
        {
            bool esRegistroNuevo = usuario.ID_USUARIO < 1;
            bool seGuardo = usuarioLN.GuardarUsuario(usuario);

            if (seGuardo && esRegistroNuevo)
            {
                usuario.INSTITUCION.SECTOR = sectorLN.ObtenerSector(usuario.INSTITUCION.ID_SECTOR);

                string fieldRuc = "[RUC]", fieldDireccion = "[DIRECCION]", fieldSector = "[SECTOR]", fieldNombres = "[NOMBRES]", fieldApellidos = "[APELLIDOS]", fieldEmail = "[EMAIL]", fieldTelefono = "[TELEFONO]", fieldCelular = "[CELULAR]", fieldAnexo = "[ANEXO]";
                string[] fields = new string[] { fieldRuc, fieldDireccion, fieldSector, fieldNombres, fieldApellidos, fieldEmail, fieldTelefono, fieldCelular, fieldAnexo };
                string[] fieldsRequire = new string[] { fieldRuc, fieldDireccion, fieldSector, fieldNombres, fieldApellidos, fieldEmail, fieldCelular };
                Dictionary<string, string> dataBody = new Dictionary<string, string> { [fieldRuc] = usuario.INSTITUCION.RAZON_SOCIAL, [fieldDireccion] = usuario.INSTITUCION.DOMICILIO_LEGAL, [fieldSector] = usuario.INSTITUCION.SECTOR.NOMBRE, [fieldNombres] = usuario.NOMBRES, [fieldApellidos] = usuario.APELLIDOS, [fieldEmail] = usuario.CORREO, [fieldTelefono] = usuario.TELEFONO, [fieldCelular] = usuario.CELULAR, [fieldAnexo] = usuario.ANEXO };
                string subject = $"{usuario.NOMBRES} {usuario.APELLIDOS}, Gracias por registrarte en nuestra plataforma SRES del sector energía";
                MailAddressCollection mailTo = new MailAddressCollection();
                mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRES} {usuario.APELLIDOS}"));

                Task.Factory.StartNew(() => mailing.SendMail(Mailing.Templates.CreacionUsuario, dataBody, fields, fieldsRequire, subject, mailTo));
            }

            return seGuardo;
        }

        [Route("obtenerallevaluador")]
        [HttpGet]
        public List<UsuarioBE> ObtenerEvaluador()
        {
            try
            {
                return usuarioLN.getAllEvaluador();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
