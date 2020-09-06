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
    public class PremiacionLN : BaseLN
    {
        PremiacionDA premDA = new PremiacionDA();
        public List<PremiacionBE> ListaBusquedaPremiacion(PremiacionBE entidad)
        {
            List<PremiacionBE> lista = new List<PremiacionBE>();
            try
            {
                cn.Open();
                lista = premDA.ListarBusquedaPremiacion(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public PremiacionBE GuardarPremiacion(PremiacionBE entidad)
        {
            bool seGuardo = false;
            int validar = 0;
            try
            {
                int id = -1;
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    validar = premDA.ValidarPremiacion(entidad, cn);
                    seGuardo = validar == 1 ? true : false;
                    if (seGuardo) seGuardo = premDA.GuardarPremiacion(entidad, out id, cn).OK; 
                    if (seGuardo)
                    {
                        if (entidad.ARCHIVO_CONTENIDO != null && entidad.ARCHIVO_CONTENIDO.Length > 0)
                        {
                            string pathFormat = AppSettings.Get<string>("Path.Reconocimiento");
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
            entidad.VAL = validar; entidad.OK = seGuardo;
            return entidad;
        }

        public PremiacionBE getPremiacion(PremiacionBE entidad)
        {
            PremiacionBE item = null;
            try
            {
                cn.Open();
                item = premDA.getPremiacion(entidad, cn);
                if (item != null) {
                    string pathFormat = AppSettings.Get<string>("Path.Reconocimiento");
                    string pathDirectoryRelative = string.Format(pathFormat, item.ID_PREMIACION);
                    string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                    string pathFile = Path.Combine(pathDirectory, item.ARCHIVO_BASE);
                    if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                    pathFile = !File.Exists(pathFile) ? null : pathFile;
                    item.ARCHIVO_CONTENIDO = pathFile == null ? null : File.ReadAllBytes(pathFile);
                }                
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return item;
        }

        public PremiacionBE EliminarPremiacion(PremiacionBE entidad)
        {
            PremiacionBE item = null;
            try
            {
                cn.Open();
                item = premDA.EliminarPremiacion(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<ReconocimientoBE> ListaReconocimiento(int idInstitucion)
        {
            List<ReconocimientoBE> lista = new List<ReconocimientoBE>();
            try
            {
                cn.Open();
                lista = premDA.ListaReconocimiento(idInstitucion, cn);
                if (lista.Count > 0)
                    foreach (ReconocimientoBE r in lista)
                        r.LISTA_REC_MEDMIT = premDA.ListaReconocimientoMedida(r.ID_RECONOCIMIENTO, cn);

            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}
