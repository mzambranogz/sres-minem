using sres.be.MRV;
using sres.da.MRV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln.MRV
{
    public class MigrarEmisionesLN : BaseLN
    {
        MigrarEmisionesDA migrarDA = new MigrarEmisionesDA();
        InstitucionDA institucionDA = new InstitucionDA();

        public MigrarEmisionesBE ObtenerMigrarEmisiones(string idIniciativas, string ruc, int idUsuario ) {
            MigrarEmisionesBE migrar = new MigrarEmisionesBE();
            List<MigrarEmisionesBE> lista = new List<MigrarEmisionesBE>();
            try
            {
                cn.Open();
                InstitucionBE inst = institucionDA.ObtenerInstitucionPorRuc(ruc, cn);
                migrar.VALIDACION = inst == null ? 1 : 2;
                migrar.LISTA_MIGRAR = migrar.VALIDACION == 1 ? lista : migrarDA.ObtenerEmisiones(ruc, idIniciativas, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return migrar;
        }
    }
}
