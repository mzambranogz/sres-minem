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
    public class EtapaDA : BaseDA
    {

        public EtapaBE GuardarEtapa(EtapaBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_UPD_ETAPA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ETAPA", entidad.ID_ETAPA);
                p.Add("PI_NOMBRE", entidad.ETAPA);
                p.Add("PI_ID_PROCESO", entidad.ID_PROCESO);
                p.Add("PI_USUARIO_GUARDAR", entidad.USUARIO_GUARDAR);
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

        public EtapaBE getEtapa(EtapaBE entidad, OracleConnection db)
        {
            EtapaBE item = new EtapaBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_ETAPA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_ETAPA", entidad.ID_ETAPA);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<EtapaBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public List<EtapaBE> ListarBusquedaEtapa(EtapaBE entidad, OracleConnection db)
        {
            List<EtapaBE> lista = new List<EtapaBE>();

            try
            {
                    string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_ETAPA";
                    var p = new OracleDynamicParameters();
                    p.Add("PI_BUSCAR", entidad.BUSCAR);
                    p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                    p.Add("PI_PAGINA", entidad.PAGINA);
                    p.Add("PI_COLUMNA", entidad.ORDER_BY);
                    p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                    p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                    lista = db.Query<EtapaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                    entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<EtapaBE> getAllEtapa(OracleConnection db)
        {
            List<EtapaBE> lista = new List<EtapaBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_ETAPA";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<EtapaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
