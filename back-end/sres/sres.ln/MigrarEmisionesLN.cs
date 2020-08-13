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
                        if (entidad.LISTA_MIGRAR.Count > 0) {
                            if (migrarDA.reestablecerEmisiones(entidad.ID_INSCRIPCION, cn))
                                foreach (MigrarEmisionesBE m in entidad.LISTA_MIGRAR)
                                    if (!(seguardo = migrarDA.migrarEmisiones(m, cn))) break;
                        }                        
                    }
                    if (seguardo) ot.Commit();
                    else ot.Rollback();
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return seguardo;
        }

        public List<MigrarEmisionesBE> mostrarSeleccionados(int idInscripcion)
        {
            List<MigrarEmisionesBE> lista = new List<MigrarEmisionesBE>();
            try
            {
                cn.Open();
                lista = migrarDA.mostrarSeleccionados(idInscripcion, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return lista;
        }

        public MigrarEmisionesBE obtenerIdIniciativasEmisiones(int idInscripcion)
        {
            MigrarEmisionesBE item = new MigrarEmisionesBE();
            List<MigrarEmisionesBE> lista = new List<MigrarEmisionesBE>();
            try
            {
                cn.Open();
                lista = migrarDA.obtenerIdIniciativasEmisiones(idInscripcion, cn);
                if (lista.Count > 0)
                {
                    foreach (MigrarEmisionesBE m in lista)
                        item.INICIATIVAS += m.ID_INICIATIVA + ",";
                    item.INICIATIVAS = item.INICIATIVAS.Substring(0, item.INICIATIVAS.Length - 1);
                }
                else
                    item.INICIATIVAS = "0";
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }
            return item;
        }
    }
}
