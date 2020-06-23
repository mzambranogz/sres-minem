﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Oracle.DataAccess.Client;
using System.Data;
using System.Web.Configuration;
using sres.be;
using sres.ut;

namespace sres.da
{
    public class ProcesoDA : BaseDA
    {

        public ProcesoBE ActualizarProceso(ProcesoBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_UPD_PROCESO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PROCESO", entidad.ID_PROCESO);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
                db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return entidad;
        }

        public ProcesoBE getProceso(ProcesoBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            ProcesoBE item = new ProcesoBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_PROCESO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PROCESO", entidad.ID_PROCESO);
                item = db.Query<ProcesoBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public List<ProcesoBE> ListarBusquedaProceso(ProcesoBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            List<ProcesoBE> lista = new List<ProcesoBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_PROCESO";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ProcesoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }
    }
}