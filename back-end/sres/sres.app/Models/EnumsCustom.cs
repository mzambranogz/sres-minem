using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace sres.app.Models
{
    public class EnumsCustom
    {
        public enum Roles
        {
            ADMINISTRADOR = 1,
            EVALUADOR = 2,
            POSTULANTE = 3
        }

        public enum Etapas
        {
            [Description("Publicación y difusión")]
            PUBLICACIONYDIFUSION = 1,
            [Description("Postulación")]
            POSTULACION = 2,
            [Description("Recopilación de Información")]
            RECOPILACIONDEINFORMACION = 3,
            [Description("Coordinación")]
            COORDINACION = 4,
            [Description("Revisión N1")]
            REVISIONN1 = 5,
            [Description("Informe preliminar")]
            INFORMEPRELIMINAR = 6,
            [Description("Levantamiento de observaciones")]
            LEVANTAMIENTOOBSERVACIONES = 7,
            [Description("Revisión N2")]
            REVISIONN2 = 8,
            [Description("Informe final")]
            INFORMEFINAL = 9
        }
    }
}