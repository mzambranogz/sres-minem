using sres.be;
using sres.da;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.ln
{
    public class ReconocimientoLN : BaseLN
    {
        ReconocimientoDA reconocimientoDA = new ReconocimientoDA();
        PremiacionDA premDA = new PremiacionDA();
        InsigniaDA insigDA = new InsigniaDA();

        public List<ReconocimientoBE> ListarUltimosReconocimientos(int idInstitucion, int cantidadRegistros)
        {
            List<ReconocimientoBE> lista = new List<ReconocimientoBE>();

            try
            {
                cn.Open();
                lista = reconocimientoDA.ListarUltimosReconocimientos(idInstitucion, cantidadRegistros, cn);
                if (lista.Count > 0) {
                    foreach(ReconocimientoBE r in lista)
                        //r.ARCHIVO_BASE = premDA.getPremiacion(new PremiacionBE { ID_PREMIACION = r.ID_PREMIACION }, cn).ARCHIVO_BASE;
                        r.ARCHIVO_BASE = insigDA.getInsignia(new InsigniaBE { ID_INSIGNIA = Convert.ToInt16(r.ID_INSIGNIA) }, cn).ARCHIVO_BASE;
                }
                //int idPremiacion = premDA.getPremiacionInsigniaEstrella(1, 1, cn).ID_PREMIACION;
                int idInsignia = 1;
                while (lista.Count < cantidadRegistros)
                {                    
                    lista.Add(new ReconocimientoBE {
                        //ID_PREMIACION = idPremiacion,
                        ID_INSIGNIA = idInsignia,
                        //ARCHIVO_BASE = premDA.getPremiacion(new PremiacionBE { ID_PREMIACION = idPremiacion }, cn).ARCHIVO_BASE,
                        ARCHIVO_BASE = insigDA.getInsignia(new InsigniaBE { ID_INSIGNIA = idInsignia }, cn).ARCHIVO_BASE,
                        FECHA_CONVOCATORIA = "-------",
                        VAL = 0
                    });
                    //lista.Add(null);
                }
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<ReconocimientoBE> BuscarParticipantes(string razonSocialInstitucion, int? idTipoEmpresa, int? idCriterio, int? idMedMit, int? añoInicioConvocatoria, int? idInsignia, int? idEstrella, int? idSector, int? idEspecial, int registros, int pagina, string columna, string orden)
        {
            List<ReconocimientoBE> lista = new List<ReconocimientoBE>();

            try
            {
                cn.Open();
                lista = reconocimientoDA.BuscarParticipantes(razonSocialInstitucion, idTipoEmpresa, idCriterio, idMedMit, añoInicioConvocatoria, idInsignia, idEstrella, idSector, idEspecial, registros, pagina, columna, orden, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }        

        public bool NombreFicha(ConvocatoriaBE ent)
        {
            bool seGuardo = false;
            try
            {
                cn.Open();
                reconocimientoDA.NombreFicha(ent, cn);
            }
            catch (Exception ex) { Log.Error(ex); }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return seGuardo;
        }
    }
}
