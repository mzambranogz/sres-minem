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
        string User { get { return AppSettings.Get<string>("UserBD"); } }

        protected dynamic Package
        {
            get
            {
                return new
                {
                    Admin = $"{User}.PKG_SISSELLO_ADMIN.",
                    Mantenimiento = $"{User}.PKG_SISSELLO_MANTENIMIENTO.",
                    Criterio = $"{User}.PKG_SISSELLO_CRITERIO.",
                    Verificacion = $"{User}.PKG_SISSELLO_VERIFICACION."
                };
            }
        }
    }
}
