using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sres.be;
using sres.ln;
using sres.app.Models;

namespace sres.app.Controllers
{
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult TablaMantenimiento()
        {
            return View();
        }
        public ActionResult Criterio(CriterioBE entidad)
        {
            if (entidad.PAGINA == 0)
            {
                entidad = new CriterioBE() { CANTIDAD_REGISTROS = 10, ORDER_BY = "ID_CRITERIO", ORDER_ORDEN = "ASC", PAGINA = 1, BUSCAR = "" };
            }
            CriterioMO modelo = new CriterioMO();
            modelo.lista = CriterioLN.ListaBusquedaCriterio(entidad);
            return View(modelo);
        }


    }
}