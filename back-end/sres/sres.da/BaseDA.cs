using sres.ut;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.da
{
    public class BaseDA
    {
        #region Miembros

        private string _CadenaConexion;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la cadena de conexión al catalogo
        /// </summary>       

        public string CadenaConexion
        {
            get { return _CadenaConexion; }
        }


        #endregion
        #region Constructor
        /// <summary>
        /// constructor que genera la cadena de conexion  por defecto
        /// </summary>
        public BaseDA()
        {
            //string nameConnection = AppSettings.Get<string>("localJuanCarlos");
            this._CadenaConexion = ConfigurationManager.ConnectionStrings["localJuanCarlos"].ConnectionString;
        }
        #endregion
    }
}
