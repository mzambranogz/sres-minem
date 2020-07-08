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
    public class CriterioDA : BaseDA
    {
        //public CriterioBE RegistroCriterio(CriterioBE entidad, OracleConnection db)
        //{
        //    try
        //    {
        //        string sp = $"{Package.Mantenimiento}USP_INS_CRITERIO";
        //        var p = new OracleDynamicParameters();
        //        p.Add("PI_NOMBRE", entidad.NOMBRE);
        //        db.ExecuteScalar(sp, p, commandType: CommandType.StoredProcedure);
        //        entidad.OK = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        entidad.OK = false;
        //    }

        //    return entidad;
        //}

        public CriterioBE GuardarCriterio(CriterioBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_NOMBRE", entidad.NOMBRE);
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

        public CriterioBE EliminarCriterio(CriterioBE entidad, OracleConnection db)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_DEL_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
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

        public CriterioBE getCriterio(CriterioBE entidad, OracleConnection db)
        {
            CriterioBE item = new CriterioBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<CriterioBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                item.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                item.OK = false;
            }

            return item;
        }

        public List<CriterioBE> ListarBusquedaCriterio(CriterioBE entidad, OracleConnection db)
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_LISTA_BUSQ_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSCAR", entidad.BUSCAR);
                p.Add("PI_REGISTROS", entidad.CANTIDAD_REGISTROS);
                p.Add("PI_PAGINA", entidad.PAGINA);
                p.Add("PI_COLUMNA", entidad.ORDER_BY);
                p.Add("PI_ORDEN", entidad.ORDER_ORDEN);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<CriterioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<CriterioBE> getAllCriterio(OracleConnection db)
        {
            List<CriterioBE> lista = new List<CriterioBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_ALL_CRITERIO";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<CriterioBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }

        //=========================================================================================
        public List<ComponenteBE> BuscarCriterioCaso(CasoBE entidad, OracleConnection db)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_CRI_CAS_COMPONENTE";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ComponenteBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public List<IndicadorBE> ArmarIndicador(ComponenteBE entidad, OracleConnection db)
        {
            List<IndicadorBE> lista = new List<IndicadorBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_INDICADOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<IndicadorBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }

            return lista;
        }

        public ParametroBE ObtenerParametro(IndicadorBE entidad, OracleConnection db)
        {
            ParametroBE param = new ParametroBE();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_GET_PARAMETRO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                param = db.Query<ParametroBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return param;
        }

        public List<IndicadorBE> ObtenerIndicador(ComponenteBE entidad, OracleConnection db)
        {
            List<IndicadorBE> lista = new List<IndicadorBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_INDICADOR_ID";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                //p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<IndicadorBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return lista;
        }

        public List<IndicadorFormBE> ArmarIndicadorForm(IndicadorBE entidad, OracleConnection db)
        {
            List<IndicadorFormBE> lista = new List<IndicadorFormBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_INDICADOR_FORM";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_INDICADOR", entidad.ID_INDICADOR);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<IndicadorFormBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return lista;
        }

        public List<IndicadorDataBE> ArmarIndicadorData(IndicadorBE entidad, OracleConnection db)
        {
            List<IndicadorDataBE> lista = new List<IndicadorDataBE>();

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_INDICADOR_DATA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_INDICADOR", entidad.ID_INDICADOR);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<IndicadorDataBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
                entidad.OK = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return lista;
        }

        public IndicadorDataBE GuardarIndicadorData(IndicadorDataBE entidad, int usuario, OracleConnection db, OracleTransaction ot = null)
        {
            try
            {
                string sp = $"{Package.Mantenimiento}USP_PRC_MAN_INDICADOR_DATA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_INDICADOR", entidad.ID_INDICADOR);
                p.Add("PI_ID_PARAMETRO", entidad.ID_PARAMETRO);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PI_VALOR", entidad.VALOR);
                p.Add("PI_USUARIO_GUARDAR", usuario);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                entidad.OK = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return entidad;
        }

        public int ObtenerIdIndicador(ComponenteBE entidad, OracleConnection db, OracleTransaction ot = null)
        {
            int indicador = 0;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_NEXT_ID_INDICADOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PO_ID_INDICADOR", 0, OracleDbType.Int32, ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                indicador = (int)p.Get<dynamic>("PO_ID_INDICADOR").Value;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return indicador;
        }

        public int VerificarIndicador(ComponenteBE entidad, OracleConnection db)
        {
            int indicador = 0;
            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_VERIFICAR_INDICADOR";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_CRITERIO", entidad.ID_CRITERIO);
                p.Add("PI_ID_cASO", entidad.ID_CASO);
                p.Add("PI_ID_COMPONENTE", entidad.ID_COMPONENTE);
                p.Add("PI_ID_INSCRIPCION", entidad.ID_INSCRIPCION);
                p.Add("PO_ROW", 0, OracleDbType.Int32, ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                indicador = (int)p.Get<dynamic>("PO_ROW").Value;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                entidad.OK = false;
            }
            return indicador;
        }
    }
}