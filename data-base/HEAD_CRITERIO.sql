--------------------------------------------------------
-- Archivo creado  - martes-julio-21-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Package PKG_SRES_CRITERIO
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE "SRES"."PKG_SRES_CRITERIO" AS
  -- CONVOCATORIA
  PROCEDURE USP_SEL_BUSQ_CONVOCATORIA(
    PI_NRO_INFORME VARCHAR2,
    PI_NOMBRE VARCHAR2,
    PI_FECHA_INICIO DATE,
    PI_FECHA_FIN DATE,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_OBTIENE_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  -- INSCRIPCION
  PROCEDURE USP_MAN_GUARDA_INSCRIPCION(
    PI_ID_INSCRIPCION IN OUT NUMBER,
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSTITUCION NUMBER,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_SEL_OBTIENE_INSC_CONV_INST(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSTITUCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  -- REQUERIMIENTO
  PROCEDURE USP_SEL_LISTA_INSCRQ_CONV_INSC(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_OBTIENE_INSCRQ(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PI_ID_REQUERIMIENTO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_MAN_GUARDA_INSCRQ(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PI_ID_REQUERIMIENTO NUMBER,
    PI_ARCHIVO_BASE VARCHAR2,
    PI_ARCHIVO_CIFRADO VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_SEL_LISTA_CRI_CONV(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_OBTIENE_CRI_CONV(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_CRITERIO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_INS_REGISTRA_INSCTRAZA(
    PI_ID_INSCRIPCION NUMBER,
    PI_DESCRIPCION VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_SEL_LISTA_CASO_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PI_ID_CONVOCATORIA NUMBER, --ADD
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_LISTA_COMPONENTE_CASO(
    PI_ID_CASO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_INDICADORDATA_ID(
    PI_ID_CRITERIO NUMBER,
    PI_ID_CASO NUMBER,
    PI_ID_COMPONENTE NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_DEL_INDICADOR_DATA(
        PI_ID_CRITERIO IN NUMBER,
        PI_ID_CASO  IN NUMBER,
        PI_ID_COMPONENTE IN NUMBER,
        PI_ID_INSCRIPCION IN NUMBER,
        PI_ELIMINAR_INDICADOR IN VARCHAR2
  );
  
  PROCEDURE USP_SEL_FORM_PARAM_DET(
    PI_ID_PARAMETRO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_DATA_PARAM_DET(
    PI_ID_PARAMETRO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_CONV_CRI_CASO_DOC(
    PI_ID_CRITERIO NUMBER,
    PI_ID_CASO NUMBER,
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_GET_FORMULA_PARAM(
    PI_ID_CRITERIO NUMBER,
    PI_ID_CASO NUMBER,
    PI_ID_COMPONENTE NUMBER,
    PI_ID_PARAMETRO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_GET_PARAM_DET_REL(
    PI_ID_PARAMETRO NUMBER,
    PI_PARAMETROS VARCHAR2,
    PI_DETALLES VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_PRC_MAN_DOCUMENTO_DATA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_CRITERIO NUMBER,
    PI_ID_CASO NUMBER,
    PI_ID_DOCUMENTO NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PI_ARCHIVO_BASE VARCHAR2,
    PI_ARCHIVO_TIPO VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_SEL_DOCUMENTO_DATA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_CRITERIO NUMBER,
    PI_ID_CASO NUMBER,
    PI_ID_DOCUMENTO NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_PRC_CONV_CRI_CAS_INSC_DATA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_CRITERIO NUMBER,
    PI_ID_CASO NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_SEL_VERF_CONV_CRITERIO_INSC(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_CRITERIO NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
END PKG_SRES_CRITERIO;

/
