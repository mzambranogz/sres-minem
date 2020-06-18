create or replace PACKAGE BODY PKG_SRES_MANTENIMIENTO AS

  --M CRITERIO-------------------------------
  PROCEDURE USP_INS_CRITERIO(
    PI_NOMBRE VARCHAR2
  ) AS
    vID_CRITERIO NUMBER;
  BEGIN
    SELECT SQ_GENM_CRITERIO.NEXTVAL INTO vID_CRITERIO FROM DUAL;
    INSERT INTO T_GENM_CRITERIO (ID_CRITERIO, NOMBRE) VALUES (vID_CRITERIO, PI_NOMBRE);
  END USP_INS_CRITERIO;

  PROCEDURE USP_UPD_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PI_NOMBRE VARCHAR2
  ) AS
  BEGIN
    UPDATE T_GENM_CRITERIO 
    SET NOMBRE = PI_NOMBRE 
    WHERE ID_CRITERIO = PI_ID_CRITERIO;
  END USP_UPD_CRITERIO;
  
  PROCEDURE USP_DEL_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PI_NOMBRE VARCHAR2
  ) AS
  BEGIN
    UPDATE T_GENM_CRITERIO 
    SET FLAG_ESTADO = '0' 
    WHERE ID_CRITERIO = PI_ID_CRITERIO;
  END USP_DEL_CRITERIO;

  PROCEDURE USP_SEL_GET_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_CRITERIO,
            NOMBRE
    FROM  T_GENM_CRITERIO
    WHERE ID_CRITERIO = PI_ID_CRITERIO; 
  END USP_SEL_GET_CRITERIO;

  PROCEDURE USP_SEL_LISTA_BUSQ_CRITERIO(
    PI_BUSCAR VARCHAR2,
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
                    FROM T_GENM_CRITERIO C
                    WHERE LOWER(TRANSLATE(C.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                    FLAG_ESTADO = ''1''';
    EXECUTE IMMEDIATE vQUERY_CONT INTO vTOTAL_REG;
    
    vPAGINA_TOTAL := CEIL(TO_NUMBER(vTOTAL_REG) / TO_NUMBER(PI_REGISTROS));
    IF vPAGINA_ACTUAL = 0 THEN
      vPAGINA_ACTUAL := 1;
    END IF;
    IF vPAGINA_ACTUAL > vPAGINA_TOTAL THEN
      vPAGINA_ACTUAL := vPAGINA_TOTAL;
    END IF;

    vPAGINA_INICIAL := vPAGINA_ACTUAL - 1;
    vCOLUMNA := PI_COLUMNA;
    
    vQUERY_SELECT := 'SELECT * FROM 
                        (
                        SELECT  C.ID_CRITERIO,
                                C.NOMBRE,
                                ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                || vPAGINA_ACTUAL || ' AS PAGINA,'
                                || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                || vTOTAL_REG || ' AS TOTAL_REGISTROS
                        FROM T_GENM_CRITERIO C
                        WHERE LOWER(TRANSLATE(C.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_CRITERIO;
  
  ------------------------------------------------------------
  
  -- M USUARIO --------------------------------------
  PROCEDURE USP_SEL_LISTA_BUSQ_USUARIO(
    PI_BUSCAR VARCHAR2,
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
                    FROM T_GENM_USUARIO U
                    WHERE (
                    LOWER(TRANSLATE(U.NOMBRES,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                    LOWER(TRANSLATE(U.APELLIDOS,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                    LOWER(TRANSLATE(U.CORREO,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                    LOWER(TRANSLATE(U.CELULAR,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%''
                    ) AND
                    FLAG_ESTADO = ''1''';
    EXECUTE IMMEDIATE vQUERY_CONT INTO vTOTAL_REG;

    vPAGINA_TOTAL := CEIL(TO_NUMBER(vTOTAL_REG) / TO_NUMBER(PI_REGISTROS));
    IF vPAGINA_ACTUAL = 0 THEN
      vPAGINA_ACTUAL := 1;
    END IF;
    IF vPAGINA_ACTUAL > vPAGINA_TOTAL THEN
      vPAGINA_ACTUAL := vPAGINA_TOTAL;
    END IF;

    vPAGINA_INICIAL := vPAGINA_ACTUAL - 1;
    vCOLUMNA := PI_COLUMNA;

    vQUERY_SELECT := 'SELECT * FROM
                        (
                        SELECT  U.ID_USUARIO,
                                U.NOMBRES,
                                U.APELLIDOS,
                                U.CORREO,
                                U.TELEFONO,
                                U.ANEXO,
                                U.CELULAR,
                                ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                || vPAGINA_ACTUAL || ' AS PAGINA,'
                                || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                || vTOTAL_REG || ' AS TOTAL_REGISTROS
                        FROM T_GENM_USUARIO U
                        WHERE (
                        LOWER(TRANSLATE(U.NOMBRES,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                        LOWER(TRANSLATE(U.APELLIDOS,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                        LOWER(TRANSLATE(U.CORREO,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                        LOWER(TRANSLATE(U.CELULAR,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%''
                        ) AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));

    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_USUARIO;

  -- M REQUERIMIENTO ---------------
  PROCEDURE USP_INS_REQUERIMIENTO(
    PI_NOMBRE VARCHAR2
  ) AS
    vID_REQUERIMIENTO NUMBER;
  BEGIN
    SELECT SQ_GENM_REQUERIMIENTO.NEXTVAL INTO vID_REQUERIMIENTO FROM DUAL;
    INSERT INTO T_GENM_REQUERIMIENTO (ID_REQUERIMIENTO, NOMBRE) VALUES (vID_REQUERIMIENTO, PI_NOMBRE);
  END USP_INS_REQUERIMIENTO;

  PROCEDURE USP_UPD_REQUERIMIENTO(
    PI_ID_REQUERIMIENTO NUMBER,
    PI_NOMBRE VARCHAR2
  ) AS
  BEGIN
    UPDATE T_GENM_REQUERIMIENTO 
    SET NOMBRE = PI_NOMBRE 
    WHERE ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO;
  END USP_UPD_REQUERIMIENTO;
  
  PROCEDURE USP_DEL_REQUERIMIENTO(
    PI_ID_REQUERIMIENTO NUMBER,
    PI_NOMBRE VARCHAR2
  ) AS
  BEGIN
    UPDATE T_GENM_REQUERIMIENTO
    SET FLAG_ESTADO = '0' 
    WHERE ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO;
  END USP_DEL_REQUERIMIENTO;

  PROCEDURE USP_SEL_GET_REQUERIMIENTO(
    PI_ID_REQUERIMIENTO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_REQUERIMIENTO,
            NOMBRE
    FROM  T_GENM_REQUERIMIENTO
    WHERE ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO; 
  END USP_SEL_GET_REQUERIMIENTO;
  
  PROCEDURE USP_SEL_LISTA_BUSQ_REQUERIMIENTO(
    PI_BUSCAR VARCHAR2,
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
                    FROM T_GENM_REQUERIMIENTO R
                    WHERE LOWER(TRANSLATE(R.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                    FLAG_ESTADO = ''1''';
    EXECUTE IMMEDIATE vQUERY_CONT INTO vTOTAL_REG;
    
    vPAGINA_TOTAL := CEIL(TO_NUMBER(vTOTAL_REG) / TO_NUMBER(PI_REGISTROS));
    IF vPAGINA_ACTUAL = 0 THEN
      vPAGINA_ACTUAL := 1;
    END IF;
    IF vPAGINA_ACTUAL > vPAGINA_TOTAL THEN
      vPAGINA_ACTUAL := vPAGINA_TOTAL;
    END IF;

    vPAGINA_INICIAL := vPAGINA_ACTUAL - 1;
    vCOLUMNA := PI_COLUMNA;
    
    vQUERY_SELECT := 'SELECT * FROM 
                        (
                        SELECT  R.ID_REQUERIMIENTO,
                                R.NOMBRE,
                                ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                || vPAGINA_ACTUAL || ' AS PAGINA,'
                                || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                || vTOTAL_REG || ' AS TOTAL_REGISTROS
                        FROM T_GENM_REQUERIMIENTO R
                        WHERE LOWER(TRANSLATE(C.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_REQUERIMIENTO;
  
  -- M PROCESO ---------------
  PROCEDURE USP_UPD_PROCESO(
    PI_ID_PROCESO NUMBER,
    PI_NOMBRE VARCHAR2
  ) AS
  BEGIN
    UPDATE T_MAE_PROCESO 
    SET NOMBRE = PI_NOMBRE 
    WHERE ID_PROCESO = PI_ID_PROCESO;
  END USP_UPD_PROCESO;
  
  PROCEDURE USP_SEL_GET_PROCESO(
    PI_ID_PROCESO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_PROCESO,
            NOMBRE
    FROM  T_MAE_PROCESO
    WHERE ID_PROCESO = PI_ID_PROCESO; 
  END USP_SEL_GET_PROCESO;
  
  PROCEDURE USP_SEL_LISTA_BUSQ_PROCESO(
    PI_BUSCAR VARCHAR2,
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
                    FROM T_MAE_PROCESO P
                    WHERE LOWER(TRANSLATE(P.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                    FLAG_ESTADO = ''1''';
    EXECUTE IMMEDIATE vQUERY_CONT INTO vTOTAL_REG;
    
    vPAGINA_TOTAL := CEIL(TO_NUMBER(vTOTAL_REG) / TO_NUMBER(PI_REGISTROS));
    IF vPAGINA_ACTUAL = 0 THEN
      vPAGINA_ACTUAL := 1;
    END IF;
    IF vPAGINA_ACTUAL > vPAGINA_TOTAL THEN
      vPAGINA_ACTUAL := vPAGINA_TOTAL;
    END IF;

    vPAGINA_INICIAL := vPAGINA_ACTUAL - 1;
    vCOLUMNA := PI_COLUMNA;
    
    vQUERY_SELECT := 'SELECT * FROM 
                        (
                        SELECT  P.ID_PROCESO,
                                P.NOMBRE,
                                ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                || vPAGINA_ACTUAL || ' AS PAGINA,'
                                || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                || vTOTAL_REG || ' AS TOTAL_REGISTROS
                        FROM T_MAE_PROCESO P
                        WHERE LOWER(TRANSLATE(P.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_PROCESO;
  
  -- M ETAPA ---------------
  PROCEDURE USP_UPD_ETAPA(
    PI_ID_ETAPA NUMBER,
    PI_NOMBRE VARCHAR2
  ) AS
  BEGIN
    UPDATE T_MAE_ETAPA 
    SET NOMBRE = PI_NOMBRE 
    WHERE ID_ETAPA = PI_ID_ETAPA;
  END USP_UPD_ETAPA;
  
  PROCEDURE USP_SEL_GET_ETAPA(
    PI_ID_ETAPA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  E.ID_ETAPA,
            E.NOMBRE,
            P.NOMBRE
    FROM  T_MAE_ETAPA E
    LEFT JOIN T_MAE_PROCESO P ON E.ID_PROCESO = P.ID_PROCESO
    WHERE E.ID_ETAPA = PI_ID_ETAPA; 
  END USP_SEL_GET_ETAPA;
  
  PROCEDURE USP_SEL_LISTA_BUSQ_ETAPA(
    PI_BUSCAR VARCHAR2,
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
                    FROM T_MAE_ETAPA E
                    LEFT JOIN T_MAE_PROCESO P ON E.ID_PROCESO = P.ID_PROCESO
                    WHERE 
                    (LOWER(TRANSLATE(E.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                    LOWER(TRANSLATE(P.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'') AND
                    E.FLAG_ESTADO = ''1''';
    EXECUTE IMMEDIATE vQUERY_CONT INTO vTOTAL_REG;
    
    vPAGINA_TOTAL := CEIL(TO_NUMBER(vTOTAL_REG) / TO_NUMBER(PI_REGISTROS));
    IF vPAGINA_ACTUAL = 0 THEN
      vPAGINA_ACTUAL := 1;
    END IF;
    IF vPAGINA_ACTUAL > vPAGINA_TOTAL THEN
      vPAGINA_ACTUAL := vPAGINA_TOTAL;
    END IF;

    vPAGINA_INICIAL := vPAGINA_ACTUAL - 1;
    
    IF PI_COLUMNA = 'ID_ETAPA' THEN
      vCOLUMNA := 'E.ID_ETAPA';
    ELSIF PI_COLUMNA = 'ETAPA' THEN
      vCOLUMNA := 'E.NOMBRE';
    ELSIF PI_COLUMNA = 'PROCESO' THEN
      vCOLUMNA := 'P.NOMBRE';
    ELSE
      vCOLUMNA := PI_COLUMNA;
    END IF;
    
    vQUERY_SELECT := 'SELECT * FROM 
                        (
                        SELECT  E.ID_PROCESO,
                                E.NOMBRE ETAPA,
                                P.NOMBRE PROCESO,
                                ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                || vPAGINA_ACTUAL || ' AS PAGINA,'
                                || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                || vTOTAL_REG || ' AS TOTAL_REGISTROS
                        FROM T_MAE_ETAPA E
                        LEFT JOIN T_MAE_PROCESO P ON E.ID_PROCESO = P.ID_PROCESO
                        WHERE
                        (LOWER(TRANSLATE(E.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                        LOWER(TRANSLATE(P.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'') AND
                        E.FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_ETAPA;

END PKG_SRES_MANTENIMIENTO;