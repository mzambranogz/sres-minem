using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ut
{
    public static class Extensiones
    {
        public static Dictionary<string, object> ObtenerListaClave(this string s, char caracterInicial, char caracterFinal)
        {
            Dictionary<string, object> valor = new Dictionary<string, object>();

            bool seEncontroCaracterInicial = false, seEncontroCaracterFinal = false;

            string texto = "";

            foreach (char c in s)
            {
                if (!seEncontroCaracterInicial)
                {
                    seEncontroCaracterInicial = c == caracterInicial;
                    continue;
                }

                if (c == caracterInicial)
                {
                    texto = "";
                    continue;
                }

                seEncontroCaracterFinal = c == caracterFinal;

                if (seEncontroCaracterFinal)
                {
                    valor.Add($"{caracterInicial}{texto}{caracterFinal}", texto);
                    seEncontroCaracterInicial = false;
                    seEncontroCaracterFinal = false;
                    texto = "";
                    continue;
                }

                texto += c;
            }

            return valor.Count == 0 ? null : valor;
        }

        public static object ObtenerValorDesdeClave(this object obj, string clave)
        {
            object valor = null;
            string json = JsonConvert.SerializeObject(obj);
            string script = $"({json}).{clave}";
            VsaEngine engine = VsaEngine.CreateEngine();
            valor = Eval.JScriptEvaluate(script, engine);
            return valor;
        }

        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
