using Newtonsoft.Json;
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

namespace sres.app.Controllers
{
    public class LoginController : Controller
    {
        UsuarioLN usuarioLN = new UsuarioLN();
        InstitucionLN institucionLN = new InstitucionLN();
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

        [HttpPost]
        public async Task<ActionResult> Validar(string correo, string contraseña, string token = null)
        {
            Dictionary<string, object> response = new Dictionary<string, object> { ["success"] = false, ["message"] = "" };

            UsuarioBE usuario = null;

            bool esValido = usuarioLN.ValidarUsuario(correo, contraseña, out usuario);
            if (esValido)
            {
                usuario.INSTITUCION = institucionLN.ObtenerInstitucion(usuario.ID_INSTITUCION.Value);
                if (usuario.ID_ROL != null) usuario.ROL = rolLN.ObtenerRol(usuario.ID_ROL.Value);
                Session["user"] = usuario;
                response["success"] = true;
                response["message"] = "Validación correcta";
                return Json(response);
            }

            response["success"] = false;
            response["message"] = "contraseña incorrecta";
            if (usuario == null) response["message"] = "Usuario no existe";
            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> ValidarMRV(string correo, string contraseña, string token = null)
        {
            Dictionary<string, object> response = new Dictionary<string, object> { ["success"] = false, ["message"] = "" };

            bool esValido = usuarioLNMRV.ValidarUsuario(correo, contraseña);

            if (esValido)
            {
                bool seMigro = false, existeUsuario = false, existeUsuarioMRV = false, existeInstitucion = false, seGuardoInstitucion = false;

                seMigro = usuarioLN.MigrarUsuario(correo, out existeUsuario, out existeUsuarioMRV, out existeInstitucion, out seGuardoInstitucion);

                UsuarioBE usuario = usuarioLN.ObtenerUsuarioPorCorreo(correo);

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
            return Json(response);
        }

        public ActionResult Salir()
        {
            Session.Remove("user");
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
    }
}