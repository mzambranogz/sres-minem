using Oracle.DataAccess.Client;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class BaseLN
    {

        static string nameConnection = AppSettings.Get<string>("NombreConexion");
        static string cadenaConexion = ConfigurationManager.ConnectionStrings[nameConnection].ConnectionString;

        protected OracleConnection cn = new OracleConnection(cadenaConexion);
    }
}
