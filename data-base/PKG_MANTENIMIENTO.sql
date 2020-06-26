create or replace PACKAGE PKG_SRES_MANTENIMIENTO AS

  --M CRITERIO-------------------------------
  PROCEDURE USP_PRC_MAN_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  );
  
  PROCEDURE USP_DEL_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PI_USUARIO_GUARDAR NUMBER
  );
  
  PROCEDURE USP_SEL_GET_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );

  PROCEDURE USP_SEL_LISTA_BUSQ_CRITERIO(
    PI_BUSCAR VARCHAR2,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  ); 
  
  PROCEDURE USP_SEL_ALL_CRITERIO(
    PO_REF OUT SYS_REFCURSOR
  );

------------------------------------------------------------
  
  -- M USUARIO --------------------------------------
  PROCEDURE USP_SEL_LISTA_BUSQ_USUARIO(
    PI_BUSCAR VARCHAR2,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );

  PROCEDURE USP_UPD_CAMBIA_ESTADO_USUARIO(
    PI_ID_USUARIO IN NUMBER,
    PI_FLAG_ESTADO IN VARCHAR2,
    PI_UPD_USUARIO IN NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );

  PROCEDURE USP_MAN_GUARDA_USUARIO(
    PI_ID_USUARIO IN OUT NUMBER,
    PI_NOMBRES VARCHAR2,
    PI_APELLIDOS VARCHAR2,
    PI_CORREO VARCHAR2,
    PI_CONTRASENA VARCHAR2,
    PI_TELEFONO VARCHAR2,
    PI_ANEXO VARCHAR2,
    PI_CELULAR VARCHAR2,
    PI_ID_INSTITUCION NUMBER,
    PI_ID_ROL NUMBER,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_SEL_OBTIENE_USUARIO(
    PI_ID_USUARIO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );

  PROCEDURE USP_SEL_OBTIENE_USUARIO_INSTITUCION_CORREO(
    PI_ID_INSTITUCION NUMBER,
    PI_CORREO VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );

  -- M ROL --------------------------------------
  PROCEDURE USP_SEL_LISTA_ROL_ESTADO(
    PI_FLAG_ESTADO VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );

  PROCEDURE USP_SEL_ALL_EVALUADOR(
    PO_REF OUT SYS_REFCURSOR
  );

  -- M SECTOR --------------
  PROCEDURE USP_SEL_LISTA_SECTOR_ESTADO(
    PI_FLAG_ESTADO VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );

  -- M REQUERIMIENTO ---------------
  PROCEDURE USP_PRC_MAN_REQUERIMIENTO(
    PI_ID_REQUERIMIENTO NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  );
  
  PROCEDURE USP_DEL_REQUERIMIENTO(
    PI_ID_REQUERIMIENTO NUMBER,
    PI_USUARIO_GUARDAR NUMBER
  );
  
  PROCEDURE USP_SEL_GET_REQUERIMIENTO(
    PI_ID_REQUERIMIENTO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );

  PROCEDURE USP_SEL_LISTA_BUSQ_REQUERIMIENTO(
    PI_BUSCAR VARCHAR2,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  ); 
  
  PROCEDURE USP_SEL_ALL_REQUERIMIENTO(
    PO_REF OUT SYS_REFCURSOR
  );

  -- M PROCESO ---------------
  PROCEDURE USP_UPD_PROCESO(
    PI_ID_PROCESO NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  );
  
  PROCEDURE USP_SEL_GET_PROCESO(
    PI_ID_PROCESO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_LISTA_BUSQ_PROCESO(
    PI_BUSCAR VARCHAR2,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );
  
  -- M ETAPA ---------------
  PROCEDURE USP_UPD_ETAPA(
    PI_ID_ETAPA NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  );
  
  PROCEDURE USP_SEL_GET_ETAPA(
    PI_ID_ETAPA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_LISTA_BUSQ_ETAPA(
    PI_BUSCAR VARCHAR2,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_ALL_ETAPA(
    PO_REF OUT SYS_REFCURSOR
  );

  -- M INSTITUCION ---------------
  PROCEDURE USP_SEL_OBTIENE_INSTITUCION_RUC(
    PI_RUC VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );

  PROCEDURE USP_SEL_OBTIENE_INSTITUCION(
    PI_ID_INSTITUCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );

  -- M ANNO -------------
  PROCEDURE USP_SEL_ALL_ANNO(
    PO_REF OUT SYS_REFCURSOR
  );

  -- M CONVOCATORIA ----------------
  PROCEDURE USP_PRC_MAN_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_GET IN OUT NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_FECHA_INICIO DATE,
    PI_FECHA_FIN DATE,
    PI_LIMITE_POSTULANTE NUMBER,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_SEL_LISTA_BUSQ_CONVOCAT(
    PI_BUSCAR VARCHAR2,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_GET_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_DEL_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_USUARIO_GUARDAR NUMBER
  );
  
  PROCEDURE USP_PRC_CONVOCATORIA_REQ(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_REQUERIMIENTO NUMBER,
    PI_FLAG_ESTADO VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_PRC_CONVOCATORIA_CRI(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_CRITERIO NUMBER,
    PI_FLAG_ESTADO VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_PRC_CONVOCATORIA_EVA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_USUARIO NUMBER,
    PI_FLAG_ESTADO VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_PRC_CONVOCATORIA_ETA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_ETAPA NUMBER,
    PI_DIAS VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
  
  PROCEDURE USP_SEL_LISTA_CONVOCAT_REQ(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_LISTA_CONVOCAT_CRI(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_LISTA_CONVOCAT_EVA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_LISTA_CONVOCAT_ETA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  );
  
-- INSTITUCION|
  PROCEDURE USP_MAN_GUARDA_INSTITUCION(
    PI_ID_INSTITUCION IN OUT NUMBER,
    PI_RUC VARCHAR2,
    PI_RAZON_SOCIAL VARCHAR2,
    PI_DOMICILIO_LEGAL VARCHAR2,
    PI_ID_SECTOR NUMBER,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
END PKG_SRES_MANTENIMIENTO;