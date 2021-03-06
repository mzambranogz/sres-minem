﻿using Newtonsoft.Json;
using sres.app.Controllers._Base;
using sres.app.Response;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using sres.be;
using sres.ln;
using System.Net.Mail;

namespace sres.app.Controllers
{
    [RoutePrefix("Login")]
    public class LoginController : Controller
    {
        UsuarioLN usuarioLN = new UsuarioLN();
        InstitucionLN institucionLN = new InstitucionLN();
        SectorLN sectorLN = new SectorLN();
        RolLN rolLN = new RolLN();
        ln.MRV.UsuarioLN usuarioLNMRV = new ln.MRV.UsuarioLN();

        bool AuthEnabled = AppSettings.Get<bool>("Auth_Enabled");
        string AuthUsuario = AppSettings.Get<string>("Auth_Usuario");
        string AuthContraseña = AppSettings.Get<string>("Auth_Contraseña");

        [SesionIn]
        public async Task<ActionResult> Index()
        {
            //if (AuthEnabled)
            //{
            //    ActionResult content = await Validar(AuthUsuario, AuthContraseña);
                
            //    return 
            //}

            string keySiteCaptcha = AppSettings.Get<string>("ReCAPTCHA_Site_Key");
            ViewData["keySiteCaptcha"] = keySiteCaptcha;
            return View();
        }

        [HttpGet]
        public ActionResult RefrescarDatosSession()
        {
            if (Session["user"] == null) return Json(new { success = false, message = "La sesión no existe" });

            UsuarioBE usuario = (UsuarioBE)Session["user"];
            usuario = usuarioLN.ObtenerUsuario(usuario.ID_USUARIO);
            usuario.INSTITUCION = institucionLN.ObtenerInstitucion(usuario.ID_INSTITUCION.Value);
            usuario.INSTITUCION.SECTOR = sectorLN.ObtenerSector(usuario.INSTITUCION.ID_SECTOR);
            if (usuario.ID_ROL != null) usuario.ROL = rolLN.ObtenerRol(usuario.ID_ROL.Value);
            Session["user"] = usuario;
            return Json(new { success = true, message = "Se refrescaron los datos de la sesión correctamente" });
        }

        [HttpPost]
        public async Task<ActionResult> Validar(string correo, string contraseña, string token = null)
        {
            Dictionary<string, object> response = new Dictionary<string, object> { ["success"] = false, ["message"] = "" };
            UsuarioBE usuario = null;
            try
            {
                bool esValido = usuarioLN.ValidarUsuario(correo, contraseña, out usuario);
                if (esValido)
                {
                    if (usuario.FLAG_ESTADO == "0" || usuario.FLAG_ESTADO == "2")
                    {
                        response["success"] = false;
                        response["message"] = "Usuario no se encuentra habilitado";
                        return Json(response);
                    }
                    usuario.INSTITUCION = institucionLN.ObtenerInstitucion(usuario.ID_INSTITUCION.Value);
                    usuario.INSTITUCION.SECTOR = sectorLN.ObtenerSector(usuario.INSTITUCION.ID_SECTOR);
                    if (usuario.ID_ROL != null) usuario.ROL = rolLN.ObtenerRol(usuario.ID_ROL.Value);
                    Session["user"] = usuario;
                    response["success"] = true;
                    response["message"] = "Validación correcta";
                    return Json(response);
                }
                response["success"] = false;
                response["message"] = "Contraseña incorrecta";
                if (usuario == null) response["message"] = "Usuario no existe";
            }
            catch (Exception ex){
                Log.Error(ex);
            }            
            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> ValidarMRV(string correo, string contraseña, string token = null)
        {
            Dictionary<string, object> response = new Dictionary<string, object> { ["success"] = false, ["message"] = "" };
            try
            {
                bool esValido = usuarioLNMRV.ValidarUsuario(correo, contraseña);
                if (esValido)
                {
                    bool seMigro = false, existeUsuario = false, existeUsuarioMRV = false, existeInstitucion = false, seGuardoInstitucion = false;
                    seMigro = usuarioLN.MigrarUsuario(correo, out existeUsuario, out existeUsuarioMRV, out existeInstitucion, out seGuardoInstitucion);
                    UsuarioBE usuario = usuarioLN.ObtenerUsuarioPorCorreo(correo);
                    usuario.INSTITUCION = institucionLN.ObtenerInstitucion(usuario.ID_INSTITUCION.Value);
                    usuario.INSTITUCION.SECTOR = sectorLN.ObtenerSector(usuario.INSTITUCION.ID_SECTOR);
                    if (usuario.ID_ROL != null) usuario.ROL = rolLN.ObtenerRol(usuario.ID_ROL.Value);

                    if (usuario != null)
                    {
                        Session["user"] = usuario;
                        response["success"] = true;
                        response["message"] = $"Validación correcta{(!existeUsuario ? ". Se migró correctamente los datos del usuario MRV" : "")}";

                        return Json(response);
                    }
                }
                response["success"] = false;
                response["message"] = "Las credenciales MRV no son válidas";
            }
            catch (Exception ex) {
                Log.Error(ex);
            }            
            return Json(response);
        }

        public ActionResult Salir()
        {
            try
            {
                Session.Remove("user");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }            
            return RedirectToAction("Index", "Login");
        }

        async Task<bool> IsCaptchaValid(string response)
        {
            try
            {
                var secret = AppSettings.Get<string>("ReCAPTCHA_Secret_Key");
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        {"secret", secret},
                        {"response", response},
                        {"remoteip", Request.UserHostAddress}
                    };

                    var content = new FormUrlEncodedContent(values);
                    var verify = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                    var captchaResponseJson = await verify.Content.ReadAsStringAsync();
                    var captchaResult = JsonConvert.DeserializeObject<GoogleResponse>(captchaResponseJson);
                    return captchaResult.success && captchaResult.score > 0.5;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public ActionResult Registrarme()
        {
            return View();
        }

        public ActionResult RecuperarContraseña()
        {
            return View();
        }

        [Route("CambiarContraseña/{idUsuario}")]
        public ActionResult CambiarContraseña(int idUsuario)
        {
            UsuarioBE usuario = usuarioLN.ObtenerUsuario(idUsuario);

            if (usuario == null) return HttpNotFound();

            return View("CambiarClave");
        }
    }
}