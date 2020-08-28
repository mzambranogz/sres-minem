using Dapper;
using Oracle.DataAccess.Client;
using sres.be;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.da
{
    public class InstitucionDA : BaseDA
    {

        public InstitucionBE ObtenerInstitucionPorRuc(string ruc, OracleConnection db)
        {
            InstitucionBE item = null;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_OBTIENE_INSTITUCION_RUC";
                var p = new OracleDynamicParameters();
                p.Add("PI_RUC", ruc);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return item;
        }

        public InstitucionBE ObtenerInstitucion(int idInstitucion, OracleConnection db)
        {
            InstitucionBE item = null;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_SEL_OBTIENE_INSTITUCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.QueryFirstOrDefault<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public List<InstitucionContactoBE> ObtenerListaContacto(int idInstitucion, OracleConnection db)
        {
            List<InstitucionContactoBE> item = new List<InstitucionContactoBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_OBTIENE_LIST_CONTACTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<InstitucionContactoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public List<InstitucionBE> ListarInstitucion(OracleConnection db)
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_LISTA_INSTITUCION";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<InstitucionBE> BuscarParticipantes(string busqueda, int registros, int pagina, string columna, string orden, OracleConnection db)
        {
            List<InstitucionBE> lista = new List<InstitucionBE>();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_BUSQ_PARTICIPANTES";
                var p = new OracleDynamicParameters();
                p.Add("PI_BUSQUEDA", busqueda);
                p.Add("PI_REGISTROS", registros);
                p.Add("PI_PAGINA", pagina);
                p.Add("PI_COLUMNA", columna);
                p.Add("PI_ORDEN", orden);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public bool GuardarInstitucion(InstitucionBE institucion, OracleConnection db, out int idInstitucion)
        {
            bool seGuardo = false;
            idInstitucion = -1;

            try
            {
                string sp = $"{Package.Mantenimiento}USP_MAN_GUARDA_INSTITUCION";
                var p = new OracleDynamicParameters();
                //p.Add("PI_ID_INSTITUCION", institucion.ID_INSTITUCION, OracleDbType.Int32, ParameterDirection.Output);
                p.Add("PI_ID_INSTITUCION", institucion.ID_INSTITUCION);
                p.Add("PI_RUC", institucion.RUC);
                p.Add("PI_RAZON_SOCIAL", institucion.RAZON_SOCIAL);
                p.Add("PI_DOMICILIO_LEGAL", institucion.DOMICILIO_LEGAL);
                p.Add("PI_ID_SECTOR", institucion.ID_SECTOR);
                p.Add("PI_UPD_USUARIO", institucion.UPD_USUARIO);
                p.Add("PI_ID_GET", 0, OracleDbType.Int32, ParameterDirection.Output);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                idInstitucion = (int)p.Get<dynamic>("PI_ID_GET").Value;
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seGuardo = filasAfectadas > 0 && idInstitucion != -1;
            }
            catch (Exception ex) { Log.Error(ex); }

            return seGuardo;
        }

        public bool ModificarLogoInstitucion(InstitucionBE institucion, OracleConnection db)
        {
            bool seModifico = false;

            try
            {
                string sp = $"{Package.Criterio}USP_UPD_MOD_LOGO_INSTITUCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", institucion.ID_INSTITUCION);
                p.Add("PI_LOGO", institucion.LOGO);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);

                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seModifico = filasAfectadas > 0;

            }
            catch (Exception ex) { Log.Error(ex); }

            return seModifico;
        }

        public bool ModificarDatosInstitucion(InstitucionBE institucion, OracleConnection db)
        {
            bool seModifico = false;

            try
            {
                string sp = $"{Package.Criterio}USP_UPD_MOD_DATOS_INSTITUCION";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", institucion.ID_INSTITUCION);
                p.Add("PI_ID_DEPARTAMENTO", institucion.ID_DEPARTAMENTO);
                p.Add("PI_ID_PROVINCIA", institucion.ID_PROVINCIA);
                p.Add("PI_ID_DISTRITO", institucion.ID_DISTRITO);
                p.Add("PI_CONTRIBUYENTE", institucion.CONTRIBUYENTE);
                p.Add("PI_ID_ACTIVIDAD", institucion.ID_ACTIVIDAD);
                p.Add("PI_NOMBRE_COMERCIAL", institucion.NOMBRE_COMERCIAL);
                p.Add("PI_DESCRIPCION", institucion.DESCRIPCION);
                p.Add("PI_ID_SUBSECTOR_TIPOEMPRESA", institucion.ID_SUBSECTOR_TIPOEMPRESA);
                p.Add("PI_ID_TRABAJADORES_CAMA", institucion.ID_TRABAJADORES_CAMA);
                p.Add("PI_CANTIDAD", institucion.CANTIDAD);
                p.Add("PI_CANTIDAD_MUJERES", institucion.CANTIDAD_MUJERES);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);

                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seModifico = filasAfectadas > 0;

            }
            catch (Exception ex) { Log.Error(ex); }

            return seModifico;
        }

        public InstitucionBE ObtenerInstitucionInscripcion(int idInscripcion, OracleConnection db)
        {
            InstitucionBE item = new InstitucionBE();

            try
            {
                string sp = $"{Package.Criterio}USP_SEL_INSTITUCION_INSC";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSCRIPCION", idInscripcion);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                item = db.Query<InstitucionBE>(sp, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex) { Log.Error(ex); }

            return item;
        }

        public bool cambiarPrimerInicio(int idInstitucion, OracleConnection db)
        {
            bool seModifico = false;
            try
            {
                string sp = $"{Package.Verificacion}USP_UPD_CAMBIAR_PRIMER_INICIO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", idInstitucion);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);
                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seModifico = filasAfectadas > 0;
            }
            catch (Exception ex) { Log.Error(ex); }
            return seModifico;
        }

        public bool GuardarContacto(InstitucionContactoBE contacto, OracleConnection db)
        {
            bool seModifico = false;

            try
            {
                string sp = $"{Package.Criterio}USP_UPD_GUARDAR_CONTACTO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_INSTITUCION", contacto.ID_INSTITUCION);
                p.Add("PI_ID_CONTACTO", contacto.ID_CONTACTO);
                p.Add("PI_NOMBRE", contacto.NOMBRE);
                p.Add("PI_CARGO", contacto.CARGO);
                p.Add("PI_TELEFONO", contacto.TELEFONO);
                p.Add("PI_CORREO", contacto.CORREO);
                p.Add("PI_USUARIO_GUARDAR", contacto.USUARIO_GUARDAR);
                p.Add("PO_ROWAFFECTED", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                db.Execute(sp, p, commandType: CommandType.StoredProcedure);

                int filasAfectadas = (int)p.Get<dynamic>("PO_ROWAFFECTED").Value;
                seModifico = filasAfectadas > 0;

            }
            catch (Exception ex) { Log.Error(ex); }

            return seModifico;
        }

        public List<DepartamentoBE> listarDepartamento(OracleConnection db)
        {
            List<DepartamentoBE> lista = new List<DepartamentoBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_DEPARTAMENTO";
                var p = new OracleDynamicParameters();
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<DepartamentoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<ProvinciaBE> listarProvincia(string idDepartamento, OracleConnection db)
        {
            List<ProvinciaBE> lista = new List<ProvinciaBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_PROVINCIA";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_DEPARTAMENTO", idDepartamento);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<ProvinciaBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }

        public List<DistritoBE> listarDistrito(string idProvincia, OracleConnection db)
        {
            List<DistritoBE> lista = new List<DistritoBE>();
            try
            {
                string sp = $"{Package.Criterio}USP_SEL_DISTRITO";
                var p = new OracleDynamicParameters();
                p.Add("PI_ID_PROVINCIA", idProvincia);
                p.Add("PO_REF", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lista = db.Query<DistritoBE>(sp, p, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { Log.Error(ex); }

            return lista;
        }
    }
}
