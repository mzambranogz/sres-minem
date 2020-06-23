using sres.ut;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.da.MRV
{
    public class BaseDA
    {
        string User { get { return AppSettings.Get<string>("UserBDMRV"); } }

        protected dynamic Package
        {
            get
            {
                return new
                {
                    Admin = $"{User}.PKG_MRV_ADMIN_SISTEMA."
                };
            }
        }
    }
}
