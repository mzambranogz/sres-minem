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
    public class InsigniaLN : BaseLN
    {
        InsigniaDA insigniaDA = new InsigniaDA();

        public List<InsigniaBE> getAllInsignia()
        {
            List<InsigniaBE> lista = new List<InsigniaBE>();
            try
            {
                cn.Open();
                lista = insigniaDA.getAllInsignia(cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<InsigniaBE> ListaBusquedaInsignia(InsigniaBE entidad)
        {
            List<InsigniaBE> lista = new List<InsigniaBE>();

            try
            {
                cn.Open();
                lista = insigniaDA.ListarBusquedaInsignia(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public bool GuardarInsignia(InsigniaBE entidad)
        {
            bool seGuardo = false;
            try
            {
                int id = -1;
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    seGuardo = insigniaDA.GuardarInsignia(entidad, out id, cn).OK;
                    if (seGuardo) {
                        if (entidad.ARCHIVO_CONTENIDO != null && entidad.ARCHIVO_CONTENIDO.Length > 0)
                        {
                            string pathFormat = AppSettings.Get<string>("Path.Insignia");
                            string pathDirectoryRelative = string.Format(pathFormat, id);
                            string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                            string pathFile = Path.Combine(pathDirectory, entidad.ARCHIVO_BASE);
                            if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                            File.WriteAllBytes(pathFile, entidad.ARCHIVO_CONTENIDO);
                        }
                        else
                            seGuardo = false;
                    }                        

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
                    
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }

        public InsigniaBE getInsignia(InsigniaBE entidad)
        {
            InsigniaBE item = null;
            try
            {
                cn.Open();
                item = insigniaDA.getInsignia(entidad, cn);
                string pathFormat = AppSettings.Get<string>("Path.Insignia");
                string pathDirectoryRelative = string.Format(pathFormat, item.ID_INSIGNIA);
                string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                string pathFile = Path.Combine(pathDirectory, item.ARCHIVO_BASE);
                if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                pathFile = !File.Exists(pathFile) ? null : pathFile;
                item.ARCHIVO_CONTENIDO = pathFile == null ? null : File.ReadAllBytes(pathFile);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public InsigniaBE EliminarInsignia(InsigniaBE entidad)
        {
            InsigniaBE item = null;

            try
            {
                cn.Open();
                item = insigniaDA.EliminarInsignia(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }    
}
