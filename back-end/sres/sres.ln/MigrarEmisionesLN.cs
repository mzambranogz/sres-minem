using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;
using sres.ut;
using Oracle.DataAccess.Client;

namespace sres.ln
{
    public class MigrarEmisionesLN : BaseLN
    {
        MigrarEmisionesDA migrarDA = new MigrarEmisionesDA();

        public bool migrarEmisiones(MigrarEmisionesBE entidad) {
            bool seguardo = true;
            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    if (entidad.LISTA_MIGRAR != null)
                    {
                        foreach (MigrarEmisionesBE m in entidad.LISTA_MIGRAR)
                            if (!(seguardo = migrarDA.migrarEmisiones(m, cn))) break;
                    }
                    if (seguardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return seguardo;
        }
    }
}
