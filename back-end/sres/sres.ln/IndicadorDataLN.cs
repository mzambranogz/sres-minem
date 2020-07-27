using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;
using Oracle.DataAccess.Client;
using System.Globalization;
using Microsoft.JScript.Vsa;
using Microsoft.JScript;
using sres.ut;

namespace sres.ln
{
    public class IndicadorDataLN : BaseLN
    {
        IndicadorDataDA indicadorDataDA = new IndicadorDataDA();

        public List<IndicadorDataBE> Calcular(List<IndicadorDataBE> lista)
        {
            List<IndicadorDataBE> listaTemp = new List<IndicadorDataBE>();

            foreach (IndicadorDataBE item in lista) if (item.OBTENIBLE == "1") if (System.Convert.ToDecimal(item.VALOR) == 0) item.VALOR = VerificaOperando(AppSettings.Get<string>("FactorR"), lista);

            decimal ldecImporte = 0;
            decimal lde_porcentaje = 0;

            foreach (var item in lista)
            {
                if (item.RESULTADO == "1")
                {
                    FormulaParametroBE formulaBE = new FormulaParametroBE() { ID_PARAMETRO = item.ID_PARAMETRO, ID_CRITERIO = item.ID_CRITERIO, ID_CASO = item.ID_CASO, ID_COMPONENTE = item.ID_COMPONENTE };
                    try
                    {
                        cn.Open();
                        formulaBE = new FormulaParametroDA().GetFormulaParametro(formulaBE, cn);
                    }
                    finally { if (cn.State == ConnectionState.Open) cn.Close(); }

                    if (formulaBE != null)
                    {
                        switch (formulaBE.COMPORTAMIENTO)
                        {
                            case "C":
                                ldecImporte = Decimal.Parse(formulaBE.VALOR);
                                break;
                            case "%":
                            case "=":
                                //if (formulaBE.COMPORTAMIENTO == "%")
                                //{
                                //    lde_porcentaje = Decimal.Parse(formulaBE.VALOR) / 100;
                                //}
                                //Analizamos la formula
                                int ll_ancho, ll_x;
                                string lc_dato = "";
                                string ls_formulanew = "", ls_subformula = "";
                                string lstrFormula = formulaBE.FORMULA.Trim();

                                ll_ancho = lstrFormula.Trim().Length;
                                for (ll_x = 0; ll_x < ll_ancho; ll_x++)
                                {
                                    lc_dato = lstrFormula.Substring(ll_x, 1);
                                    switch (lc_dato)
                                    {
                                        case "[":
                                            int ll_finoperando, ll_long;
                                            string ls_operando;

                                            ll_finoperando = lstrFormula.IndexOf("]", ll_x);         //Ubica Posicion Fin del Operando
                                            ll_long = ll_finoperando - ll_x - 1;                     //Determina longitud del Operando
                                            ls_operando = lstrFormula.Substring(ll_x + 1, ll_long);  //Captura Operando
                                            ll_x = ll_finoperando;                                   //Lleva puntero al final de operando
                                                                                                     //Quita Espacios en Blanco dentro del Operando
                                            ls_operando = ls_operando.Replace(" ", "");
                                            //Verificando Operando 
                                            ls_subformula = VerificaOperando(ls_operando, lista);
                                            break;

                                        default:
                                            ls_subformula = lc_dato;
                                            break;
                                    }
                                    ls_formulanew += ls_subformula;
                                }
                                //Si la formula esta vacia
                                if (ls_formulanew == "")
                                {
                                    ls_formulanew = "0";
                                }

                                decimal lde_Aux = 0;

                                lde_Aux = Calculate(ls_formulanew);
                                if (formulaBE.COMPORTAMIENTO == "COMPORTAMIENTO")
                                {
                                    ldecImporte = lde_Aux * lde_porcentaje;
                                }
                                else
                                {
                                    ldecImporte = lde_Aux;
                                }
                                break;
                        }
                        item.VALOR = ldecImporte.ToString();
                    }
                }
            }
            return lista;
        }

        private string VerificaOperando(string istrOperando, List<IndicadorDataBE> listaEntidad)
        {
            string ls_subformula = "";
            decimal lde_importecida = 0;
            string ls_cida = "";

            switch (istrOperando.Substring(0, 1))
            {
                case "C":
                    ls_cida = istrOperando.Substring(1);
                    ls_subformula = ls_cida.Trim();
                    break;
                case "P":
                    //'Captura del Operando el Codigo del Parametro
                    ls_cida = istrOperando.Substring(1, istrOperando.Length - 1);
                    //Captura Importe ingresado para el Parametro
                    IndicadorDataBE item = listaEntidad.Find(A => A.ID_PARAMETRO.Equals(int.Parse(ls_cida)));
                    if (item != null)
                    {
                        lde_importecida = decimal.Parse(item.VALOR);
                    }
                    else
                    {
                        lde_importecida = 0;
                    }
                    ls_subformula = lde_importecida.ToString();
                    break;
                case "F":
                    //Recupero información de factor
                    ls_cida = istrOperando.Substring(1);
                    FactorBE Fentidad = new FactorBE() { ID_FACTOR = int.Parse(ls_cida) };

                    List<FactorParametroBE> lFactorBE;
                    try
                    {
                        cn.Open();
                        lFactorBE = new FactorDA().ListaFactorParametro(Fentidad, cn);
                    }
                    finally { if (cn.State == ConnectionState.Open) cn.Close(); }
                    //List<FactorParametroBE> lFactorBE = new FactorDA().ListaFactorParametro(Fentidad, cn);

                    if (lFactorBE != null)
                    {
                        string SQL = "";
                        string SQL_PARAMETRO = "'";
                        string SQL_VALOR = "'";
                        int I = 1;
                        foreach (FactorParametroBE param in lFactorBE)
                        {
                            IndicadorDataBE data = listaEntidad.Find(A => A.ID_PARAMETRO.Equals(param.ID_PARAMETRO));
                            SQL_PARAMETRO += param.ID_PARAMETRO.ToString() + (I == lFactorBE.Count ? "" : "|");
                            SQL_VALOR += data.VALOR + (I == lFactorBE.Count ? "" : "|");
                            I++;
                        }
                        SQL_PARAMETRO += "'";
                        SQL_VALOR += "'";
                        SQL += "AND F.ID_PARAMETROS = " + SQL_PARAMETRO + " AND  F.VALORES = " + SQL_VALOR;

                        List<FactorDataBE> lFactorDataBE;
                        try
                        {
                            cn.Open();
                            lFactorDataBE = new FactorDA().ListaFactorData(Fentidad, SQL, cn);
                        }
                        finally { if (cn.State == ConnectionState.Open) cn.Close(); }

                        //List<FactorDataBE> lFactorDataBE = new FactorDA().ListaFactorParametroData(Fentidad, SQL);
                        if (lFactorDataBE != null)
                        {
                            if (lFactorDataBE.Count > 0)
                                lde_importecida = lFactorDataBE[0].FACTOR;
                            else
                                lde_importecida = 0;
                        }
                    }
                    else
                    {
                        lde_importecida = 0;
                    }
                    ls_subformula = lde_importecida.ToString();

                    break;
            }
            return ls_subformula;
        }

        private static decimal Calculate(string istrExpresion)
        {
            decimal dblResultado = 0;
            string strExpression = istrExpresion.Trim();

            if (istrExpresion.Trim() != "")
            {
                CultureInfo cul = new CultureInfo("es-PE", true);
                strExpression = strExpression.Replace(cul.NumberFormat.NumberDecimalSeparator, ".");
                dblResultado = EvaluateNumericExpression(strExpression);
            }

            return dblResultado;
        }

        private static decimal EvaluateNumericExpression(string istrExpresion)
        {
            VsaEngine engine = VsaEngine.CreateEngine();
            try
            {
                object resultado = Eval.JScriptEvaluate(istrExpresion, engine);
                return System.Convert.ToDecimal(resultado);
            }
            catch
            {
                return 0;
            }
            engine.Close();
        }

    }
}
