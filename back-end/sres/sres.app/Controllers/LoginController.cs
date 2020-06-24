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

        bool AuthEnabled = AppSettings.Get<bool>("Auth_Enabled");
        string AuthUsuario = AppSettings.Get<string>("Auth_Usuario");
        string AuthContraseña = AppSettings.Get<string>("Auth_Contraseña");

        [SesionIn]
        public async Task<ActionResult> Index()
        {
            if (AuthEnabled)
            {
                return await Validar(AuthUsuario, AuthContraseña);
            }

            string keySiteCaptcha = AppSettings.Get<string>("ReCAPTCHA_Site_Key");
            ViewData["keySiteCaptcha"] = keySiteCaptcha;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Validar(string correo, string contraseña, string token = null)
        {
            if (!AuthEnabled)
            {
                bool esCaptchaValido = await IsCaptchaValid(token);

                if (!esCaptchaValido)
                {
                    TempData["error_message"] = "Captcha inválido";
                    return RedirectToAction("Index", "Login");
                }
            }

            UsuarioBE usuario = null;

            bool esValido = usuarioLN.ValidarUsuario(correo, contraseña, out usuario);
            //esValido = true; //QUITAR ESTA LINEA SOLO PRUEBA
            if (esValido)
            {
                Session["user"] = usuario;
                //Session["user"] = 1; //QUITAR SOLO PRUEBA

                return RedirectToAction("Index", "Inicio");
            }

            TempData["error_message"] = "Usuario y/o contraseña incorrecto";
            return RedirectToAction("Index", "Login");
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