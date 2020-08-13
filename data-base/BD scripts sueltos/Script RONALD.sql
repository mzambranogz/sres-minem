--PACKAGE VERIFICACION

  PROCEDURE USP_SEL_BUSQ_INSCRIPCION (
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PI_RAZON_SOCIAL_INSTITUCION VARCHAR2,
    PI_NOMBRES_APELLIDOS_USUARIO VARCHAR2,
    PI_ID_USUARIO NUMBER,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  ) AS
    vTOTAL_REG INTEGER;
    vPAGINA_TOTAL INTEGER;
    vPAGINA_ACTUAL INTEGER := PI_PAGINA;
    vPAGINA_INICIAL INTEGER := 0;
    vQUERY_CONT VARCHAR2(10000) := '';
    vQUERY_SELECT VARCHAR2(10000) := '';
    vCOLUMNA VARCHAR2(200);
  BEGIN
    vQUERY_CONT := 'SELECT  COUNT(1)
                    FROM
                    T_GENM_INSCRIPCION INSC INNER JOIN
                    T_GEND_CONV_EVA_INST CI ON INSC.ID_CONVOCATORIA = CI.ID_CONVOCATORIA AND INSC.ID_INSTITUCION = CI.ID_INSTITUCION INNER JOIN
                    T_GENM_CONVOCATORIA C ON INSC.ID_CONVOCATORIA = C.ID_CONVOCATORIA INNER JOIN
                    T_GENM_USUARIO U ON INSC.REG_USUARIO = U.ID_USUARIO INNER JOIN
                    T_GENM_INSTITUCION INST ON INSC.ID_INSTITUCION = INST.ID_INSTITUCION
                    WHERE
                    CI.ID_USUARIO = ' || PI_ID_USUARIO || ' AND
                    INSC.ID_CONVOCATORIA = ' || PI_ID_CONVOCATORIA || ' AND ' ||
                    CASE
                      WHEN PI_ID_INSCRIPCION IS NOT NULL THEN
                      'INSC.ID_INSCRIPCION = ' || PI_ID_INSCRIPCION || ' AND '
                    END ||
                    'LOWER(TRANSLATE(INST.RAZON_SOCIAL, ''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%'' || LOWER(TRANSLATE(''' || PI_RAZON_SOCIAL_INSTITUCION || ''', ''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) || ''%'' AND ' ||
                    'LOWER(TRANSLATE(U.NOMBRES || '' '' || U.APELLIDOS, ''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%'' || LOWER(TRANSLATE(''' || PI_NOMBRES_APELLIDOS_USUARIO || ''', ''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) || ''%'' AND ' ||
                    'INSC.FLAG_ESTADO = ''1''';
    EXECUTE IMMEDIATE vQUERY_CONT INTO vTOTAL_REG;

    vPAGINA_TOTAL := CEIL(TO_NUMBER(vTOTAL_REG) / TO_NUMBER(PI_REGISTROS));
    IF vPAGINA_ACTUAL = 0 THEN
      vPAGINA_ACTUAL := 1;
    END IF;
    IF vPAGINA_ACTUAL > vPAGINA_TOTAL THEN
      vPAGINA_ACTUAL := vPAGINA_TOTAL;
    END IF;

    vPAGINA_INICIAL := vPAGINA_ACTUAL - 1;

    vCOLUMNA := 'INSC.' || PI_COLUMNA;

    vQUERY_SELECT := 'SELECT * FROM
                        (
                          SELECT
                                  INSC.ID_INSCRIPCION,
                                  INSC.ID_INSTITUCION,
                                  INST.RUC AS "RUC_INSTITUCION",
                                  INST.RAZON_SOCIAL AS "RAZON_SOCIAL_INSTITUCION",
                                  INSC.REG_USUARIO,
                                  INSC.REG_FECHA,
                                  C.ID_ETAPA AS "ID_ETAPA_CONVOCATORIA",
                                  U.NOMBRES AS "NOMBRES_USUARIO",
                                  U.APELLIDOS AS "APELLIDOS_USUARIO",
                                  U.CORREO AS "CORREO_USUARIO",
                                  0 AS "CANTIDADCRITERIOSINGRESADOS",
                                  0 AS "PUNTOSACUMULADOS",
                                  ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                  || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                  || vPAGINA_ACTUAL || ' AS PAGINA,'
                                  || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                  || vTOTAL_REG || ' AS TOTAL_REGISTROS
                          FROM
                          T_GENM_INSCRIPCION INSC INNER JOIN
                          T_GEND_CONV_EVA_INST CI ON INSC.ID_CONVOCATORIA = CI.ID_CONVOCATORIA AND INSC.ID_INSTITUCION = CI.ID_INSTITUCION INNER JOIN
                          T_GENM_CONVOCATORIA C ON INSC.ID_CONVOCATORIA = C.ID_CONVOCATORIA INNER JOIN
                          T_GENM_USUARIO U ON INSC.REG_USUARIO = U.ID_USUARIO INNER JOIN
                          T_GENM_INSTITUCION INST ON INSC.ID_INSTITUCION = INST.ID_INSTITUCION
                          WHERE
                          CI.ID_USUARIO = ' || PI_ID_USUARIO || ' AND
                          INSC.ID_CONVOCATORIA = ' || PI_ID_CONVOCATORIA || ' AND ' ||
                          CASE
                            WHEN PI_ID_INSCRIPCION IS NOT NULL THEN
                            'INSC.ID_INSCRIPCION = ' || PI_ID_INSCRIPCION || ' AND '
                          END ||
                          'LOWER(TRANSLATE(INST.RAZON_SOCIAL, ''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%'' || LOWER(TRANSLATE(''' || PI_RAZON_SOCIAL_INSTITUCION || ''', ''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) || ''%'' AND ' ||
                          'LOWER(TRANSLATE(U.NOMBRES || '' '' || U.APELLIDOS, ''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%'' || LOWER(TRANSLATE(''' || PI_NOMBRES_APELLIDOS_USUARIO || ''', ''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) || ''%'' AND ' ||
                          'INSC.FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));

    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_BUSQ_INSCRIPCION;
  
  PROCEDURE USP_UPD_EVAL_INSCRIPCION (
    PI_ID_INSCRIPCION NUMBER,
    PI_ID_TIPO_EVALUACION NUMBER,
    PI_OBSERVACION VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
  BEGIN
    UPDATE T_GENM_INSCRIPCION I SET
    I.ID_TIPO_EVALUACION = PI_ID_TIPO_EVALUACION,
    I.OBSERVACION = PI_OBSERVACION,
    I.UPD_USUARIO = PI_UPD_USUARIO,
    I.UPD_FECHA = SYSDATE
    WHERE
    I.ID_INSCRIPCION = PI_ID_INSCRIPCION;
    
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_UPD_EVAL_INSCRIPCION;
  
  PROCEDURE USP_UPD_EVAL_INSC_REQ (
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PI_ID_REQUERIMIENTO NUMBER,
    PI_VALIDO NUMBER,
    PI_OBSERVACION VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
  BEGIN
    UPDATE T_GEND_INSCRIPCION_REQUERIR IR SET
    IR.VALIDO = PI_VALIDO,
    IR.OBSERVACION = PI_OBSERVACION,
    IR.UPD_USUARIO = PI_UPD_USUARIO,
    IR.UPD_FECHA = SYSDATE
    WHERE
    IR.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND
    IR.ID_INSCRIPCION = PI_ID_INSCRIPCION AND
    IR.ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO;
    
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_UPD_EVAL_INSC_REQ;


create table SISSELLO.T_GENM_RECONOCIMIENTO
(
  id_reconocimiento   NUMBER not null,
  id_inscripcion      NUMBER,
  flag_categoria      VARCHAR2(1),
  id_insignia         NUMBER,
  puntaje_categoria   NUMBER,
  flag_estrella       VARCHAR2(1),
  id_estrella         NUMBER,
  emisiones_estrella  NUMBER,
  flag_mejoracontinua VARCHAR2(1),
  flag_cantidadmaxima VARCHAR2(1),
  flag_estado         VARCHAR2(1),
  reg_usuario         NUMBER,
  reg_fecha           DATE default SYSDATE,
  upd_usuario         NUMBER,
  upd_fecha           DATE
)