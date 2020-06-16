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

namespace sres.app.Controllers
{
    public class LoginController : Controller
    {
        [SesionIn]
        public ActionResult Index()
        {
            string keySiteCaptcha = AppSettings.Get<string>("ReCAPTCHA_Site_Key");
            ViewData["keySiteCaptcha"] = keySiteCaptcha;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Validar(string usuario, string contraseña, string token)
        {
            List<UsuarioBE> lista = UsuarioLN.ListaUsuario();
            //

            bool esCaptchaValido = await IsCaptchaValid(token);

            if (!esCaptchaValido)
            {
                TempData["error_message"] = "Captcha inválido";
                return RedirectToAction("Index", "Login");
            }

            bool esValido = usuario == "admin@hotmail.com" && contraseña == "123456";

            if (esValido)
            {
                Session["user"] = new { usuario = usuario };

                return RedirectToAction("Index", "Inicio");
            }

            //string keySecretCaptcha = AppSettings.Get<string>("ReCAPTCHA_Secret_Key");

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
                //Log.Error(ex);
                return false;
            }
        }

        //bool IsCaptchaValid(string response)
        //{
        //    try
        //    {
        //        var secret = AppSettings.Get<string>("ReCAPTCHA_Secret_Key");
        //        using (var client = new WebClient())
        //        {
        //            var values = new Dictionary<string, string>
        //            {
        //                {"secret", secret},
        //                {"response", response},
        //                {"remoteip", Request.UserHostAddress}
        //            };

        //            var content = JsonConvert.SerializeObject(values);
        //            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        //            var verify = client.UploadString("https://www.google.com/recaptcha/api/siteverify", content);
        //            var captchaResponseJson = verify;
        //            var captchaResult = JsonConvert.DeserializeObject<GoogleResponse>(captchaResponseJson);
        //            return captchaResult.success && captchaResult.score > 0.5;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Error(ex);
        //        return false;
        //    }
        //}
    }
}