CREATE OR REPLACE PACKAGE BODY PKG_SRES_CRITERIO AS
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
                    FROM T_GENM_CONVOCATORIA C
                    WHERE ' ||
                    CASE
                      WHEN PI_NRO_INFORME IS NOT NULL THEN
                      'LOWER(TRANSLATE(C.NRO_INFORME,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_NRO_INFORME ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND '
                    END ||
                    'LOWER(TRANSLATE(C.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_NOMBRE ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND ' ||
                    CASE
                      WHEN PI_FECHA_INICIO IS NOT NULL AND PI_FECHA_FIN IS NOT NULL THEN
                      '(
                      TO_DATE(C.FECHA_INICIO) BETWEEN TO_DATE(''' || TO_CHAR(PI_FECHA_INICIO, 'DD/MM/YYYY') || ''') AND TO_DATE(''' || TO_CHAR(PI_FECHA_FIN, 'DD/MM/YYYY') || ''') OR
                      TO_DATE(C.FECHA_FIN) BETWEEN TO_DATE(''' || TO_CHAR(PI_FECHA_INICIO, 'DD/MM/YYYY') || ''') AND TO_DATE(''' || TO_CHAR(PI_FECHA_FIN, 'DD/MM/YYYY') || ''')
                      ) AND '
                      WHEN PI_FECHA_INICIO IS NOT NULL AND PI_FECHA_FIN IS NULL THEN
                      '(TO_DATE(C.FECHA_INICIO) >= TO_DATE(''' || TO_CHAR(PI_FECHA_INICIO, 'DD/MM/YYYY') || ''') OR TO_DATE(C.FECHA_FIN) >= TO_DATE(''' || TO_CHAR(PI_FECHA_INICIO, 'DD/MM/YYYY') || ''')) AND '
                      WHEN PI_FECHA_INICIO IS NULL AND PI_FECHA_FIN IS NOT NULL THEN
                      '(TO_DATE(C.FECHA_INICIO) <= TO_DATE(''' || TO_CHAR(PI_FECHA_FIN, 'DD/MM/YYYY') || ''') OR TO_DATE(C.FECHA_FIN) <= TO_DATE(''' || TO_CHAR(PI_FECHA_FIN, 'DD/MM/YYYY') || ''')) AND '
                    END ||
                    'C.FLAG_ESTADO = ''1''';
    EXECUTE IMMEDIATE vQUERY_CONT INTO vTOTAL_REG;
    
    vPAGINA_TOTAL := CEIL(TO_NUMBER(vTOTAL_REG) / TO_NUMBER(PI_REGISTROS));
    IF vPAGINA_ACTUAL = 0 THEN
      vPAGINA_ACTUAL := 1;
    END IF;
    IF vPAGINA_ACTUAL > vPAGINA_TOTAL THEN
      vPAGINA_ACTUAL := vPAGINA_TOTAL;
    END IF;

    vPAGINA_INICIAL := vPAGINA_ACTUAL - 1;
    
    vCOLUMNA := 'C.' || PI_COLUMNA;
    
    vQUERY_SELECT := 'SELECT * FROM 
                        (
                          SELECT  C.ID_CONVOCATORIA,
                                  C.NOMBRE,
                                  C.FECHA_INICIO,
                                  C.FECHA_FIN,
                                  C.LIMITE_POSTULANTE,
                                  C.NRO_INFORME,
                                  C.FLAG_ESTADO,
                                  ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                  || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                  || vPAGINA_ACTUAL || ' AS PAGINA,'
                                  || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                  || vTOTAL_REG || ' AS TOTAL_REGISTROS
                          FROM T_GENM_CONVOCATORIA C
                          WHERE ' ||
                          CASE
                            WHEN PI_NRO_INFORME IS NOT NULL THEN
                            'LOWER(TRANSLATE(C.NRO_INFORME,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_NRO_INFORME ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND '
                          END ||
                          'LOWER(TRANSLATE(C.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_NOMBRE ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND ' ||
                          CASE
                            WHEN PI_FECHA_INICIO IS NOT NULL AND PI_FECHA_FIN IS NOT NULL THEN
                            '(
                            TO_DATE(C.FECHA_INICIO) BETWEEN TO_DATE(''' || TO_CHAR(PI_FECHA_INICIO, 'DD/MM/YYYY') || ''') AND TO_DATE(''' || TO_CHAR(PI_FECHA_FIN, 'DD/MM/YYYY') || ''') OR
                            TO_DATE(C.FECHA_FIN) BETWEEN TO_DATE(''' || TO_CHAR(PI_FECHA_INICIO, 'DD/MM/YYYY') || ''') AND TO_DATE(''' || TO_CHAR(PI_FECHA_FIN, 'DD/MM/YYYY') || ''')
                            ) AND '
                            WHEN PI_FECHA_INICIO IS NOT NULL AND PI_FECHA_FIN IS NULL THEN
                            '(TO_DATE(C.FECHA_INICIO) >= TO_DATE(''' || TO_CHAR(PI_FECHA_INICIO, 'DD/MM/YYYY') || ''') OR TO_DATE(C.FECHA_FIN) >= TO_DATE(''' || TO_CHAR(PI_FECHA_INICIO, 'DD/MM/YYYY') || ''')) AND '
                            WHEN PI_FECHA_INICIO IS NULL AND PI_FECHA_FIN IS NOT NULL THEN
                            '(TO_DATE(C.FECHA_INICIO) <= TO_DATE(''' || TO_CHAR(PI_FECHA_FIN, 'DD/MM/YYYY') || ''') OR TO_DATE(C.FECHA_FIN) <= TO_DATE(''' || TO_CHAR(PI_FECHA_FIN, 'DD/MM/YYYY') || ''')) AND '
                          END ||
                          'C.FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_BUSQ_CONVOCATORIA;
  
  PROCEDURE USP_SEL_OBTIENE_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    C.ID_CONVOCATORIA,
    C.NOMBRE,
    C.FECHA_INICIO,
    C.FECHA_FIN,
    C.LIMITE_POSTULANTE,
    C.NRO_INFORME,
    C.FLAG_ESTADO
    FROM T_GENM_CONVOCATORIA C
    WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA;
  END USP_SEL_OBTIENE_CONVOCATORIA;
  
  -- INSCRIPCION
  PROCEDURE USP_MAN_GUARDA_INSCRIPCION(
    PI_ID_INSCRIPCION IN OUT NUMBER,
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSTITUCION NUMBER,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    
    SELECT MAX(ID_INSCRIPCION) INTO vID
    FROM T_GENM_INSCRIPCION
    WHERE ID_INSCRIPCION = PI_ID_INSCRIPCION;
    
    IF vID IS NULL THEN
      PI_ID_INSCRIPCION := SQ_GENM_INSCRIPCION.NEXTVAL();
      
      INSERT INTO T_GENM_INSCRIPCION
      (ID_INSCRIPCION, ID_CONVOCATORIA, ID_INSTITUCION, REG_USUARIO)
      VALUES
      (PI_ID_INSCRIPCION, PI_ID_CONVOCATORIA, PI_ID_INSTITUCION, PI_UPD_USUARIO);
    ELSE
      UPDATE T_GENM_INSCRIPCION I SET
      I.UPD_USUARIO = PI_UPD_USUARIO,
      I.UPD_FECHA = SYSDATE
      WHERE
      I.ID_INSCRIPCION = PI_ID_INSCRIPCION;
    END IF;
    
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_MAN_GUARDA_INSCRIPCION;
  
  PROCEDURE USP_SEL_OBTIENE_INSCRIPCION_CONVOCATORIA_INSTITUCION(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSTITUCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    I.ID_INSCRIPCION,
    I.ID_CONVOCATORIA,
    I.ID_INSTITUCION,
    I.FLAG_ANULAR,
    I.NRO_INFORME_PRELIMINAR,
    I.FLAG_ESTADO
    FROM T_GENM_INSCRIPCION I
    WHERE I.ID_CONVOCATORIA = PI_ID_CONVOCATORIA
    AND I.ID_INSTITUCION = PI_ID_INSTITUCION;
  END USP_SEL_OBTIENE_INSCRIPCION_CONVOCATORIA_INSTITUCION;
  
  -- REQUERIMIENTO
  PROCEDURE USP_SEL_LISTA_INSCRIPCIONREQUERIMIENTO_CONVOCATORIA_INSCRIPCION(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    R.ID_REQUERIMIENTO,
    R.NOMBRE AS "NOMBRE_REQUERIMIENTO",
    R.FLAG_ESTADO AS "FLAG_ESTADO_REQUERIMIENTO",
    IR.ARCHIVO_BASE,
    IR.ARCHIVO_CIFRADO,
    IR.VALIDO AS "VALIDO",
    IR.OBSERVACION,
    I.ID_INSTITUCION
    FROM T_GENM_REQUERIMIENTO R INNER JOIN
    T_GEND_CONVOCATORIA_REQUERIM CR ON R.ID_REQUERIMIENTO = CR.ID_REQUERIMIENTO AND R.FLAG_ESTADO = '1' LEFT JOIN
    T_GEND_INSCRIPCION_REQUERIR IR ON CR.ID_CONVOCATORIA = IR.ID_CONVOCATORIA AND CR.ID_REQUERIMIENTO = IR.ID_REQUERIMIENTO AND IR.ID_INSCRIPCION = PI_ID_INSCRIPCION LEFT JOIN
    T_GENM_INSCRIPCION I ON IR.ID_INSCRIPCION = I.ID_INSCRIPCION
    WHERE CR.ID_CONVOCATORIA = PI_ID_CONVOCATORIA
    AND CR.FLAG_ESTADO = '1';
  END USP_SEL_LISTA_INSCRIPCIONREQUERIMIENTO_CONVOCATORIA_INSCRIPCION;
  
  PROCEDURE USP_SEL_OBTIENE_INSCRIPCIONREQUERIMIENTO(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PI_ID_REQUERIMIENTO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    IR.ID_CONVOCATORIA,
    IR.ID_INSCRIPCION,
    IR.ID_REQUERIMIENTO,
    IR.ARCHIVO_BASE,
    IR.ARCHIVO_CIFRADO,
    IR.VALIDO AS "VALIDO",
    IR.OBSERVACION
    FROM T_GEND_INSCRIPCION_REQUERIR IR
    WHERE
    IR.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND
    IR.ID_INSCRIPCION = PI_ID_INSCRIPCION AND
    IR.ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO;
  END USP_SEL_OBTIENE_INSCRIPCIONREQUERIMIENTO;
  
  PROCEDURE USP_MAN_GUARDA_INSCRIPCIONREQUERIMIENTO(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PI_ID_REQUERIMIENTO NUMBER,
    PI_ARCHIVO_BASE VARCHAR2,
    PI_ARCHIVO_CIFRADO VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
  vRegistros NUMBER;
  BEGIN
    SELECT COUNT(1) INTO vRegistros
    FROM T_GEND_INSCRIPCION_REQUERIR IR
    WHERE
    IR.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND
    IR.ID_INSCRIPCION = PI_ID_INSCRIPCION AND
    IR.ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO;
    
    IF vRegistros = 0 THEN
      INSERT INTO T_GEND_INSCRIPCION_REQUERIR
      (ID_CONVOCATORIA, ID_INSCRIPCION, ID_REQUERIMIENTO, ARCHIVO_BASE, ARCHIVO_CIFRADO, REG_USUARIO)
      VALUES
      (PI_ID_CONVOCATORIA, PI_ID_INSCRIPCION, PI_ID_REQUERIMIENTO, PI_ARCHIVO_BASE, PI_ARCHIVO_CIFRADO, PI_UPD_USUARIO);
    ELSE
      UPDATE T_GEND_INSCRIPCION_REQUERIR IR SET
      IR.ARCHIVO_BASE = PI_ARCHIVO_BASE,
      IR.ARCHIVO_CIFRADO = PI_ARCHIVO_CIFRADO,
      IR.UPD_FECHA = SYSDATE,
      IR.UPD_USUARIO = PI_UPD_USUARIO
      WHERE
      IR.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND
      IR.ID_INSCRIPCION = PI_ID_INSCRIPCION AND
      IR.ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO;
    END IF;
    
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_MAN_GUARDA_INSCRIPCIONREQUERIMIENTO;
  
  PROCEDURE USP_SEL_LISTA_CRITERIO_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    C.ID_CRITERIO,
    C.NOMBRE,
    C.FLAG_ESTADO
    FROM T_GEND_CONVOCATORIA_CRITERIO CR INNER JOIN
    T_GENM_CRITERIO C ON CR.ID_CRITERIO = C.ID_CRITERIO AND CR.FLAG_ESTADO = C.FLAG_ESTADO
    WHERE
    CR.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND
    CR.FLAG_ESTADO = '1';
  END USP_SEL_LISTA_CRITERIO_CONVOCATORIA;
  
  PROCEDURE USP_SEL_OBTIENE_CRITERIO_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_CRITERIO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    C.ID_CRITERIO,
    C.NOMBRE,
    C.FLAG_ESTADO
    FROM T_GEND_CONVOCATORIA_CRITERIO CR INNER JOIN
    T_GENM_CRITERIO C ON CR.ID_CRITERIO = C.ID_CRITERIO AND CR.FLAG_ESTADO = C.FLAG_ESTADO
    WHERE
    CR.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND
    CR.ID_CRITERIO = PI_ID_CRITERIO AND
    CR.FLAG_ESTADO = '1';
  END USP_SEL_OBTIENE_CRITERIO_CONVOCATORIA;
  
  PROCEDURE USP_INS_REGISTRA_INSCRIPCIONTRAZABILIDAD(
    PI_ID_INSCRIPCION NUMBER,
    PI_DESCRIPCION VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    vID := SQ_GEND_INSCRIPCION_TRAZA.NEXTVAL();
    
    INSERT INTO T_GEND_INSCRIPCION_TRAZA
    (ID_INSCRIPCION, ID_TRAZABILIDAD, DESCRIPCION, REG_USUARIO)
    VALUES
    (PI_ID_INSCRIPCION, vID, PI_DESCRIPCION, PI_UPD_USUARIO);
    
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_INS_REGISTRA_INSCRIPCIONTRAZABILIDAD;
  
  PROCEDURE USP_SEL_LISTA_CASO_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    C.ID_CASO,
    C.NOMBRE,
    C.ID_CRITERIO,
    C.FLAG_ESTADO
    FROM T_GENM_CASO C
    WHERE C.ID_CRITERIO = PI_ID_CRITERIO;
  END USP_SEL_LISTA_CASO_CRITERIO;
  
  PROCEDURE USP_SEL_LISTA_COMPONENTE_CASO(
    PI_ID_CASO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    C.ID_COMPONENTE,
    C.NOMBRE,
    C.INCREMENTABLE,
    C.ID_CASO,
    C.ID_CRITERIO,
    C.FLAG_ESTADO
    FROM T_GENM_COMPONENTE C
    WHERE C.ID_CASO = PI_ID_CASO;
  END USP_SEL_LISTA_COMPONENTE_CASO;
  
  PROCEDURE USP_DEL_INDICADOR_DATA(
        PI_ID_CRITERIO IN NUMBER,
        PI_ID_CASO  IN NUMBER,
        PI_ID_COMPONENTE IN NUMBER,
        PI_ID_INSCRIPCION IN NUMBER,
        PI_ELIMINAR_INDICADOR IN VARCHAR2
  )IS
        vSql            VARCHAR2(250);
  BEGIN
        vSql := 'UPDATE T_MAEM_INDICADOR_DATA SET FLAG_ESTADO = ''0'' WHERE ID_CRITERIO ='||PI_ID_CRITERIO||' AND ID_CASO = '||PI_ID_CASO||' AND ID_COMPONENTE = '||PI_ID_COMPONENTE||' AND ID_INSCRIPCION = '||PI_ID_INSCRIPCION||' AND ID_INDICADOR IN ('||PI_ELIMINAR_INDICADOR||')';
        EXECUTE IMMEDIATE vSql;
  END USP_DEL_INDICADOR_DATA;
  
  PROCEDURE USP_SEL_INDICADORDATA_ID(
    PI_ID_CRITERIO NUMBER,
    PI_ID_CASO NUMBER,
    PI_ID_COMPONENTE NUMBER,
    PI_ID_INSCRIPCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  )AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  DISTINCT ID_CRITERIO, ID_CASO, ID_COMPONENTE, ID_INDICADOR
    FROM    T_MAEM_INDICADOR_DATA
    WHERE   ID_CRITERIO = PI_ID_CRITERIO AND ID_CASO = PI_ID_CASO AND ID_COMPONENTE = PI_ID_COMPONENTE AND FLAG_ESTADO = '1'            
    ORDER BY ID_INDICADOR ASC;
  END USP_SEL_INDICADORDATA_ID;
END PKG_SRES_CRITERIO;