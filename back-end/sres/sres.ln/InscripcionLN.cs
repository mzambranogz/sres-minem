using Oracle.DataAccess.Client;
using sres.be;
using sres.da;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class InscripcionLN : BaseLN
    {
        InscripcionDA inscripcionDA = new InscripcionDA();
        InscripcionRequerimientoDA inscripcionRequerimientoDA = new InscripcionRequerimientoDA();

        public InscripcionBE ObtenerInscripcionPorConvocatoriaInstitucion(int idConvocatoria, int idInstitucion)
        {
            InscripcionBE item = null;

            try
            {
                cn.Open();
                item = inscripcionDA.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, idInstitucion, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public bool GuardarInscripcion(InscripcionBE inscripcion)
        {
            bool seGuardo = false;

            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction())
                {
                    int idInscripcion = -1;
                    seGuardo = inscripcionDA.GuardarInscripcion(inscripcion, out idInscripcion, cn);

                    if (seGuardo)
                    {
                        if(inscripcion.LISTA_INSCRIPCION_REQUERIMIENTO != null)
                        {
                            foreach (InscripcionRequerimientoBE iInscripcionRequerimiento in inscripcion.LISTA_INSCRIPCION_REQUERIMIENTO)
                            {
                                if (iInscripcionRequerimiento.ID_INSCRIPCION <= 0) iInscripcionRequerimiento.ID_INSCRIPCION = idInscripcion;
                                if (seGuardo)
                                {
                                    if(iInscripcionRequerimiento.ARCHIVO_CONTENIDO != null)
                                    {
                                        string pathFormat = AppSettings.Get<string>("Path.Inscripcion.Requerimiento");
                                        string pathDirectoryRelative = string.Format(pathFormat, inscripcion.ID_INSTITUCION, inscripcion.ID_INSCRIPCION);
                                        string pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryRelative);
                                        string pathFile = Path.Combine(pathDirectory, iInscripcionRequerimiento.ARCHIVO_BASE);
                                        if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
                                        File.WriteAllBytes(pathFile, iInscripcionRequerimiento.ARCHIVO_CONTENIDO);
                                    }
                                    seGuardo = inscripcionRequerimientoDA.GuardarInscripcionRequerimiento(iInscripcionRequerimiento, cn);
                                }
                                else break;
                            }
                        }
                    }

                    if (seGuardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }
    }
}
