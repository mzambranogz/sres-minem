using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;
using Oracle.DataAccess.Client;
using sres.ut;
using System.IO;

namespace sres.ln
{
    public class MedidaMitigacionLN : BaseLN
    {
        MedidaMitigacionDA medidaDA = new MedidaMitigacionDA();

        public List<MedidaMitigacionBE> ListaBusquedaMedidaMitigacion(MedidaMitigacionBE entidad)
        {
            List<MedidaMitigacionBE> lista = new List<MedidaMitigacionBE>();
            try
            {
                cn.Open();
                lista = medidaDA.ListarBusquedaMedidaMitigacion(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public MedidaMitigacionBE GuardarMedidaMitigacion(MedidaMitigacionBE entidad)
        {
            //CriterioBE item = null;
            bool seGuardoConvocatoria = false;
            int validar = 0;
            try
            {
                int id = -1;
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    validar = entidad.VAL == -1 ? medidaDA.ValidarMedidaMitigacion(entidad, cn) : 1;
                    seGuardoConvocatoria = validar == 1 ? true : false;
                    if (seGuardoConvocatoria) seGuardoConvocatoria = medidaDA.GuardarMedidaMitigacion(entidad, out id, cn).OK;
                    if (seGuardoConvocatoria)
                    {
                        if (entidad.ARCHIVO_CONTENIDO != null && entidad.ARCHIVO_CONTENIDO.Length > 0)
                        {
                            string pathFormat = AppSettings.Get<string>("Path.MedidaMitigacion");
                            string pathDirectoryRelative = string.Format(pathFormat, id);
                            string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                            string pathFile = Path.Combine(pathDirectory, entidad.ARCHIVO_BASE);
                            if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                            File.WriteAllBytes(pathFile, entidad.ARCHIVO_CONTENIDO);
                        }
                        else
                            seGuardoConvocatoria = false;
                    }
                    if (seGuardoConvocatoria) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            entidad.VAL = validar; entidad.OK = seGuardoConvocatoria;
            return entidad;
        }

        public MedidaMitigacionBE getMedidaMitigacion(MedidaMitigacionBE entidad)
        {
            MedidaMitigacionBE item = null;
            try
            {
                cn.Open();
                item = medidaDA.getMedidaMitigacion(entidad, cn);
                string pathFormat = AppSettings.Get<string>("Path.MedidaMitigacion");
                string pathDirectoryRelative = string.Format(pathFormat, item.ID_MEDMIT);
                string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                string pathFile = Path.Combine(pathDirectory, item.ARCHIVO_BASE);
                if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                pathFile = !File.Exists(pathFile) ? null : pathFile;
                item.ARCHIVO_CONTENIDO = pathFile == null ? null : File.ReadAllBytes(pathFile);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public MedidaMitigacionBE EliminarMedidaMitigacion(MedidaMitigacionBE entidad)
        {
            MedidaMitigacionBE item = null;
            try
            {
                cn.Open();
                item = medidaDA.EliminarMedidaMitigacion(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
