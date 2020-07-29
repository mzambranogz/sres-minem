using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sres.ln;
using sres.be;
using System.Collections.Generic;
using System.Net.Mail;
using sres.ut;

namespace sres.app.test
{
    [TestClass]
    public class MailingTest
    {
        UsuarioLN usuarioLN = new UsuarioLN();
        InstitucionLN institucionLN = new InstitucionLN();
        SectorLN sectorLN = new SectorLN();

        Mailing mailing = new Mailing();

        [TestMethod]
        public void EnvioCorreo()
        {
            UsuarioBE usuario = usuarioLN.ObtenerUsuario(1);
            usuario.INSTITUCION = institucionLN.ObtenerInstitucion(usuario.ID_INSTITUCION.Value);
            usuario.INSTITUCION.SECTOR = sectorLN.ObtenerSector(usuario.INSTITUCION.ID_SECTOR);


            string fieldRuc = "[RUC]", fieldDireccion = "[DIRECCION]", fieldSector = "[SECTOR]", fieldNombres = "[NOMBRES]", fieldApellidos = "[APELLIDOS]", fieldEmail = "[EMAIL]", fieldTelefono = "[TELEFONO]", fieldCelular = "[CELULAR]", fieldAnexo = "[ANEXO]";
            string[] fields = new string[] { fieldRuc, fieldDireccion, fieldSector, fieldNombres, fieldApellidos, fieldEmail, fieldTelefono, fieldCelular, fieldAnexo };
            string[] fieldsRequire = new string[] { fieldRuc, fieldDireccion, fieldSector, fieldNombres, fieldApellidos, fieldEmail, fieldCelular };
            Dictionary<string, string> dataBody = new Dictionary<string, string> { [fieldRuc] = usuario.INSTITUCION.RAZON_SOCIAL, [fieldDireccion] = usuario.INSTITUCION.DOMICILIO_LEGAL, [fieldSector] = usuario.INSTITUCION.SECTOR.NOMBRE, [fieldNombres] = usuario.NOMBRES, [fieldApellidos] = usuario.APELLIDOS, [fieldEmail] = usuario.CORREO, [fieldTelefono] = usuario.TELEFONO, [fieldCelular] = usuario.CELULAR, [fieldAnexo] = usuario.ANEXO };
            string subject = $"{usuario.NOMBRES} {usuario.APELLIDOS}, Gracias por registrarte en nuestra plataforma SRES del sector energía";
            MailAddressCollection mailTo = new MailAddressCollection();
            mailTo.Add(new MailAddress(usuario.CORREO, $"{usuario.NOMBRES} {usuario.APELLIDOS}"));

            mailing.SendMail(Mailing.Templates.CreacionUsuario, dataBody, fields, fieldsRequire, subject, mailTo);
        }
    }
}
