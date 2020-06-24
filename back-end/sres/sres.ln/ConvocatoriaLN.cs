﻿using System;
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
    public class ConvocatoriaLN : BaseLN
    {

        public ConvocatoriaDA convocatoriaDA = new ConvocatoriaDA();

        public ConvocatoriaBE RegistroConvocatoria(ConvocatoriaBE entidad)
        {
            ConvocatoriaBE item = null;

            try
            {
                cn.Open();
                using (OracleTransaction ot = cn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    int idConvocatoria = -1;
                    bool seGuardoConvocatoria = false;

                    seGuardoConvocatoria = convocatoriaDA.RegistroConvocatoria(entidad, out idConvocatoria, cn, ot).OK;

                    if (seGuardoConvocatoria)
                    {
                        foreach (var it in entidad.LISTA_REQ)
                        {
                            if (!(seGuardoConvocatoria = convocatoriaDA.GuardarRequerimiento(new RequerimientoBE { ID_REQUERIMIENTO = it.ID_REQUERIMIENTO, FLAG_ESTADO = it.FLAG_ESTADO, USUARIO_GUARDAR = entidad.USUARIO_GUARDAR }, idConvocatoria, cn, ot).OK)) break;
                        }

                        if (seGuardoConvocatoria)
                        {
                            foreach (var it in entidad.LISTA_CRI)
                            {
                                if (!(seGuardoConvocatoria = convocatoriaDA.GuardarCriterio(new CriterioBE { ID_CRITERIO = it.ID_CRITERIO, FLAG_ESTADO = it.FLAG_ESTADO, USUARIO_GUARDAR = entidad.USUARIO_GUARDAR }, idConvocatoria, cn, ot).OK)) break;
                            }

                            if (seGuardoConvocatoria)
                            {
                                foreach (var it in entidad.LISTA_EVA)
                                {
                                    if (!(seGuardoConvocatoria = convocatoriaDA.GuardarEvaluador(new UsuarioBE { ID_USUARIO = it.ID_USUARIO, FLAG_ESTADO = it.FLAG_ESTADO, USUARIO_GUARDAR = entidad.USUARIO_GUARDAR }, idConvocatoria, cn, ot).OK)) break;
                                }
                            }                       

                        }
                    }

                    if (seGuardoConvocatoria) ot.Commit();
                    else ot.Rollback();
                    item.OK = seGuardoConvocatoria;
                }
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }

    }
}
