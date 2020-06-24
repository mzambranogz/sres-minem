using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.be;
using sres.da;
using System.Data;

namespace sres.ln
{
    public class RequerimientoLN : BaseLN
    {
        public RequerimientoDA RequerimientoDA = new RequerimientoDA();

        //public static RequerimientoBE RegistroRequerimiento(RequerimientoBE entidad)
        //{
        //    return RequerimientoDA.RegistroRequerimiento(entidad);
        //}

        //public static RequerimientoBE ActualizarRequerimiento(RequerimientoBE entidad)
        //{
        //    return RequerimientoDA.ActualizarRequerimiento(entidad);
        //}

        public RequerimientoBE GuardarRequerimiento(RequerimientoBE entidad)
        {
            RequerimientoBE item = null;

            try
            {
                cn.Open();
                item = RequerimientoDA.GuardarRequerimiento(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public RequerimientoBE EliminarRequerimiento(RequerimientoBE entidad)
        {
            RequerimientoBE item = null;

            try
            {
                cn.Open();
                item = RequerimientoDA.EliminarRequerimiento(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public RequerimientoBE getRequerimiento(RequerimientoBE entidad)
        {
            RequerimientoBE item = null;

            try
            {
                cn.Open();
                item = RequerimientoDA.getRequerimiento(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

        public List<RequerimientoBE> ListaBusquedaRequerimiento(RequerimientoBE entidad)
        {
            List<RequerimientoBE> lista = new List<RequerimientoBE>();

            try
            {
                cn.Open();
                lista = RequerimientoDA.ListarBusquedaRequerimiento(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public List<RequerimientoBE> getAllRequerimiento()
        {
            List<RequerimientoBE> lista = new List<RequerimientoBE>();

            try
            {
                cn.Open();
                lista = RequerimientoDA.getAllRequerimiento(cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }
    }
}