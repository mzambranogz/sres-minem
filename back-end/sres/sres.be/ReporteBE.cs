﻿using sres.ut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sres.be
{
    public class ReporteBE
    {
        public class ReporteEstadisticoTipoEmpresaXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_TIPOEMPRESA { get; set; }
            public decimal CANTIDADTOTAL_INSCRIPCIONES { get; set; }
            public decimal CANTIDAD_INSCRIPCIONES { get; set; }
            public decimal PORCENTAJE_INSCRIPCIONES { get; set; }
        }

        public class ReporteEstadisticoTipoPostulanteXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_TIPOPOSTULANTE { get; set; }
            public decimal CANTIDAD_POSTULANTES { get; set; }
        }

        public class ReporteEstadisticoInsigniaXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_SUBSECTOR { get; set; }
            public string INSIGNIA { get; set; }
            public decimal CANTIDAD { get; set; }
        }

        public class ReporteEstadisticoEstrellaXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_SUBSECTOR { get; set; }
            public string ESTRELLA { get; set; }
            public decimal CANTIDAD { get; set; }
        }

        public class ReporteEstadisticoMejoraContinuaXConvocatoria
        {
            public string NOMBRE_CONVOCATORIA { get; set; }
            public string NOMBRE_SECTOR { get; set; }
            public string NOMBRE_SUBSECTOR { get; set; }
            public string MEJORACONTINUA { get; set; }
            public decimal CANTIDAD { get; set; }
        }

        public class ReportePostulacionesXSectorSubSector
        {
            public string PERIODO { get; set; }
            public string RAZON_SOCIAL { get; set; }
            public string RESPONSABLE { get; set; }
            public string CATEGORIA { get; set; }
            public string ESTRELLA { get; set; }
            public int PUNTAJE { get; set; }
            public double EMISIONES { get; set; }
            public double ENERGIA { get; set; }
            public double COMBUSTIBLE { get; set; }
            public DateTime REG_FECHA { get; set; }
            public string SECTOR { get; set; }
            public string SUBSECTOR { get; set; }
            public int NUMERO_ESTRELLA { get; set; }
            public int ID_SECTOR { get; set; }
            public int ID_SUBSECTOR_TIPOEMPRESA { get; set; }
        }

        public class ReporteConvocatoriaEmpresa
        {
            public string CRITERIO { get; set; }
            public string CRITERIO_PUNTAJE { get; set; }
            public int PUNTAJE { get; set; }
            public double EMISIONES_REDUCIDAS { get; set; }
            public double ENERGIA { get; set; }
            public double COMBUSTIBLE { get; set; }
            public string OBSERVACION { get; set; }
            public int ID_CONVOCATORIA { get; set; }
            public int ID_INSTITUCION { get; set; }
            public string CONVOCATORIA { get; set; }
            public string RAZON_SOCIAL { get; set; }
        }

        public class ReporteReconocimientoEmpresa
        {
            public string PERIODO { get; set; }
            public string RAZON_SOCIAL { get; set; }
            public string CONVOCATORIA { get; set; }
            public string CATEGORIA { get; set; }
            public string ESTRELLA { get; set; }
            public int PUNTAJE { get; set; }
            public double EMISIONES { get; set; }
            public double ENERGIA { get; set; }
            public double COMBUSTIBLE { get; set; }
            public int ID_CONVOCATORIA { get; set; }
            public int ID_INSTITUCION { get; set; }
            public int ID_PREMIACION { get; set; }
            public string ARCHIVO_BASE_PRE { get; set; }
            public string RUTA_IMAGEN
            {
                get
                {
                    string rutaReconocimiento = AppSettings.Get<string>("Path.Reconocimiento").Replace("/", "\\").Replace("{0}", ID_PREMIACION.ToString());
                    string rutaFisicaServidor = AppSettings.Get<string>("ServerFisico");
                    string rutaImagen = rutaFisicaServidor + rutaReconocimiento + "\\" + ARCHIVO_BASE_PRE;
                    return rutaImagen;
                }
            }
        }

        public class ReporteReconocimiento
        {
            public int ID_RECONOCIMIENTO { get; set; }
            public string PERIODO { get; set; }
            public string PERIODO_ACTUAL { get; set; }
            public string RAZON_SOCIAL { get; set; }
            public string CONVOCATORIA { get; set; }
            public string CATEGORIA { get; set; }
            public string ESTRELLA { get; set; }
            public int PUNTAJE { get; set; }
            public double EMISIONES { get; set; }
            public double ENERGIA { get; set; }
            public double COMBUSTIBLE { get; set; }
            public int ID_CONVOCATORIA { get; set; }
            public int ID_INSTITUCION { get; set; }
            public int ID_PREMIACION { get; set; }
            public string ARCHIVO_BASE_PRE { get; set; }
            public int FLAG_MEDIDA { get; set; }
            public int FLAG_MEJORACONTINUA { get; set; }
            public string PREMIO_ESPECIAL { get; set; }
            public int MES { get; set; }
            public string DIA { get; set; }
            public string NOMBRE_MES
            {
                get
                {
                    string mes = "";
                    if (MES == 1) mes = "enero";
                    else if (MES == 2) mes = "febrero";
                    else if (MES == 3) mes = "marzo";
                    else if (MES == 4) mes = "abril";
                    else if (MES == 5) mes = "mayo";
                    else if (MES == 6) mes = "junio";
                    else if (MES == 7) mes = "julio";
                    else if (MES == 8) mes = "agosto";
                    else if (MES == 9) mes = "setiembre";
                    else if (MES == 10) mes = "octubre";
                    else if (MES == 11) mes = "noviembre";
                    else if (MES == 12) mes = "diciembre";
                    return mes;
                }
            }
            public string RUTA_IMAGEN
            {
                get
                {
                    string rutaReconocimiento = AppSettings.Get<string>("Path.Reconocimiento").Replace("/", "\\").Replace("{0}", ID_PREMIACION.ToString());
                    string rutaFisicaServidor = AppSettings.Get<string>("ServerFisico");
                    string rutaImagen = rutaFisicaServidor + rutaReconocimiento + "\\" + ARCHIVO_BASE_PRE;
                    return rutaImagen;
                }
            }

            public byte[] IMAGEN
            {
                get
                {
                    string rutaReconocimiento = AppSettings.Get<string>("Path.Reconocimiento").Replace("/", "\\").Replace("{0}", ID_PREMIACION.ToString());
                    string rutaFisicaServidor = AppSettings.Get<string>("ServerFisico");
                    string rutaImagen = rutaFisicaServidor + rutaReconocimiento + "\\" + ARCHIVO_BASE_PRE;
                    byte[] imagen = File.ReadAllBytes(rutaImagen);
                    return imagen;
                }
            }

            public byte[] IMAGEN_PREMIO_ESPECIAL
            {
                get
                {
                    string rutaFisicaServidor = AppSettings.Get<string>("ServerFisico");
                    string rutaImagen = rutaFisicaServidor + "\\Assets\\images\\gei.png";
                    byte[] imagen = File.ReadAllBytes(rutaImagen);
                    return imagen;
                }
            }

            public int PREMIO_LOGRADO
            {
                get
                {
                    if (FLAG_MEDIDA == 1 || FLAG_MEJORACONTINUA == 1)
                        return 1;
                    else
                        return 0;
                }
            }

            public string PERIODO_1
            {
                get
                {
                    int periodo = Convert.ToInt16(PERIODO);
                    string periodo_1 = Convert.ToString(periodo + 1);
                    return periodo_1;
                }
            }

            public string PERIODO_2
            {
                get
                {
                    int periodo = Convert.ToInt16(PERIODO);
                    string periodo_2 = Convert.ToString(periodo + 2);
                    return periodo_2;
                }
            }
        }

    }
}
