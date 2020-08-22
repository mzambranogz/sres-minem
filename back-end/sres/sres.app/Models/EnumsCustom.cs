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
            [Description("Revisión requisitos")]
            REVISIONREQUISITO = 3,
            [Description("Documentos solicitados")]
            DOCUMENTOSOLICITADO = 4,
            [Description("Filtrado de participantes")]
            FILTRADOPARTICIPANTE = 5,
            [Description("Recopilación de Información")]
            RECOPILACIONDEINFORMACION = 6,
            [Description("Coordinación")]
            COORDINACION = 7,
            [Description("Revisión N1")]
            REVISIONN1 = 8,
            [Description("Informe preliminar")]
            INFORMEPRELIMINAR = 9,
            [Description("Levantamiento de observaciones")]
            LEVANTAMIENTOOBSERVACIONES = 10,
            [Description("Revisión N2")]
            REVISIONN2 = 11,
            [Description("Informe final")]
            INFORMEFINAL = 12
        }
    }
}