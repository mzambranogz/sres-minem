create or replace PACKAGE BODY PKG_SRES_MANTENIMIENTO AS

  --M CRITERIO-------------------------------
  PROCEDURE USP_PRC_MAN_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
      (SELECT C.ID_CRITERIO FROM T_GENM_CRITERIO C WHERE C.ID_CRITERIO = PI_ID_CRITERIO)
    INTO vID
    FROM DUAL;
    
    IF vID IS NULL THEN
      vID := SQ_GENM_CRITERIO.NEXTVAL();
      INSERT INTO T_GENM_CRITERIO
      (ID_CRITERIO, NOMBRE, REG_USUARIO, REG_FECHA)
      VALUES
      (vID, PI_NOMBRE, PI_USUARIO_GUARDAR, SYSDATE);
    ELSE
      UPDATE T_GENM_CRITERIO C SET
      C.NOMBRE = PI_NOMBRE,
      C.UPD_USUARIO = PI_USUARIO_GUARDAR,
      C.UPD_FECHA = SYSDATE
      WHERE C.ID_CRITERIO = PI_ID_CRITERIO;
    END IF;
  END USP_PRC_MAN_CRITERIO;
  
  PROCEDURE USP_DEL_CRITERIO(
    PI_ID_CRITERIO NUMBER,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
  BEGIN
    UPDATE T_GENM_CRITERIO 
    SET FLAG_ESTADO = '0',
        UPD_USUARIO = PI_USUARIO_GUARDAR,
        UPD_FECHA = SYSDATE
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
                    WHERE LOWER(TRANSLATE(C.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND
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
                        WHERE LOWER(TRANSLATE(C.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_CRITERIO;
  
  PROCEDURE USP_SEL_ALL_CRITERIO(
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_CRITERIO,
            NOMBRE
    FROM  T_GENM_CRITERIO
    WHERE FLAG_ESTADO = '1'; 
  END USP_SEL_ALL_CRITERIO;
  
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
                    LOWER(TRANSLATE(U.NOMBRES,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                    LOWER(TRANSLATE(U.APELLIDOS,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                    LOWER(TRANSLATE(U.CORREO,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                    LOWER(TRANSLATE(U.CELULAR,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%''
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
                        LOWER(TRANSLATE(U.NOMBRES,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                        LOWER(TRANSLATE(U.APELLIDOS,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                        LOWER(TRANSLATE(U.CORREO,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                        LOWER(TRANSLATE(U.CELULAR,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%''
                        ) AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));

    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_USUARIO;

  PROCEDURE USP_UPD_CAMBIA_ESTADO_USUARIO(
    PI_ID_USUARIO IN NUMBER,
    PI_FLAG_ESTADO IN VARCHAR2,
    PI_UPD_USUARIO IN NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
  BEGIN
    UPDATE T_GENM_USUARIO U SET
    U.FLAG_ESTADO = PI_FLAG_ESTADO,
    U.UPD_USUARIO = PI_UPD_USUARIO,
    U.UPD_FECHA = SYSDATE
    WHERE U.ID_USUARIO = PI_ID_USUARIO;
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_UPD_CAMBIA_ESTADO_USUARIO;

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
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
    (SELECT U.ID_USUARIO FROM T_GENM_USUARIO U WHERE U.ID_USUARIO = PI_ID_USUARIO)
    INTO vID
    FROM DUAL;
    
    IF vID IS NULL THEN
      PI_ID_USUARIO := SQ_GENM_USUARIO.NEXTVAL();
      INSERT INTO T_GENM_USUARIO
      (ID_USUARIO, NOMBRES, APELLIDOS, CORREO, CONTRASENA, TELEFONO, ANEXO, CELULAR, ID_INSTITUCION, ID_ROL, REG_USUARIO, REG_FECHA)
      VALUES
      (PI_ID_USUARIO, PI_NOMBRES, PI_APELLIDOS, PI_CORREO, PI_CONTRASENA, PI_TELEFONO, PI_ANEXO, PI_CELULAR, PI_ID_INSTITUCION, PI_ID_ROL, PI_UPD_USUARIO, SYSDATE);
    ELSE
      UPDATE T_GENM_USUARIO U SET
      U.NOMBRES = PI_NOMBRES,
      U.APELLIDOS = PI_APELLIDOS,
      U.CORREO = PI_CORREO,
      U.TELEFONO = PI_TELEFONO,
      U.ANEXO = PI_ANEXO,
      U.CELULAR = PI_CELULAR,
      U.ID_INSTITUCION = PI_ID_INSTITUCION,
      U.ID_ROL = PI_ID_ROL
      WHERE U.ID_USUARIO = PI_ID_USUARIO;
    END IF;
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_MAN_GUARDA_USUARIO;
  
  PROCEDURE USP_SEL_OBTIENE_USUARIO(
    PI_ID_USUARIO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    U.ID_USUARIO,
    U.NOMBRES,
    U.APELLIDOS,
    U.CORREO,
    U.TELEFONO,
    U.ANEXO,
    U.CELULAR,
    U.ID_INSTITUCION,
    U.ID_ROL
    FROM T_GENM_USUARIO U
    WHERE U.ID_USUARIO = PI_ID_USUARIO;
  END USP_SEL_OBTIENE_USUARIO;
  
  PROCEDURE USP_SEL_OBTIENE_USUARIO_INSTITUCION_CORREO(
    PI_ID_INSTITUCION NUMBER,
    PI_CORREO VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    U.ID_USUARIO,
    U.NOMBRES,
    U.APELLIDOS,
    U.CORREO,
    U.TELEFONO,
    U.ANEXO,
    U.CELULAR,
    U.ID_INSTITUCION,
    U.ID_ROL
    FROM T_GENM_USUARIO U
    WHERE U.ID_INSTITUCION = PI_ID_INSTITUCION
    AND U.CORREO = PI_CORREO;
  END USP_SEL_OBTIENE_USUARIO_INSTITUCION_CORREO;

  -- M ROL --------------------------------------
  PROCEDURE USP_SEL_LISTA_ROL_ESTADO(
    PI_FLAG_ESTADO VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT R.ID_ROL, R.NOMBRE, R.FLAG_ESTADO
    FROM T_MAE_ROL R
    WHERE R.FLAG_ESTADO = PI_FLAG_ESTADO;
  END USP_SEL_LISTA_ROL_ESTADO;
  
  PROCEDURE USP_SEL_ALL_EVALUADOR(
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_USUARIO,
            (NOMBRES || ' ' || APELLIDOS) NOMBRE_COMPLETO
    FROM  T_GENM_USUARIO
    WHERE ID_ROL = 2 AND FLAG_ESTADO = '1'; 
  END USP_SEL_ALL_EVALUADOR;
  
  -- M SECTOR --------------
  PROCEDURE USP_SEL_LISTA_SECTOR_ESTADO(
    PI_FLAG_ESTADO VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT S.ID_SECTOR, S.NOMBRE, S.FLAG_ESTADO
    FROM T_MAE_SECTOR S
    WHERE S.FLAG_ESTADO = PI_FLAG_ESTADO;
  END USP_SEL_LISTA_SECTOR_ESTADO;

  -- M REQUERIMIENTO ---------------
  PROCEDURE USP_PRC_MAN_REQUERIMIENTO(
    PI_ID_REQUERIMIENTO NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
      (SELECT R.ID_REQUERIMIENTO FROM T_GENM_REQUERIMIENTO R WHERE R.ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO)
    INTO vID
    FROM DUAL;
    
    IF vID IS NULL THEN
      vID := SQ_GENM_REQUERIMIENTO.NEXTVAL();
      INSERT INTO T_GENM_REQUERIMIENTO
      (ID_REQUERIMIENTO, NOMBRE, REG_USUARIO, REG_FECHA)
      VALUES
      (vID, PI_NOMBRE, PI_USUARIO_GUARDAR, SYSDATE);
    ELSE
      UPDATE T_GENM_REQUERIMIENTO R SET
      R.NOMBRE = PI_NOMBRE,
      R.UPD_USUARIO = PI_USUARIO_GUARDAR,
      R.UPD_FECHA = SYSDATE
      WHERE R.ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO;
    END IF;
  END USP_PRC_MAN_REQUERIMIENTO;
  
  PROCEDURE USP_DEL_REQUERIMIENTO(
    PI_ID_REQUERIMIENTO NUMBER,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
  BEGIN
    UPDATE T_GENM_REQUERIMIENTO
    SET FLAG_ESTADO = '0', 
        UPD_USUARIO = PI_USUARIO_GUARDAR,
        UPD_FECHA = SYSDATE
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
                    WHERE LOWER(TRANSLATE(R.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND
                    R.FLAG_ESTADO = ''1''';
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
                        WHERE LOWER(TRANSLATE(R.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_REQUERIMIENTO;
  
  PROCEDURE USP_SEL_ALL_REQUERIMIENTO(
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_REQUERIMIENTO,
            NOMBRE
    FROM  T_GENM_REQUERIMIENTO
    WHERE FLAG_ESTADO = '1'; 
  END USP_SEL_ALL_REQUERIMIENTO;
  
  -- M PROCESO ---------------
  PROCEDURE USP_UPD_PROCESO(
    PI_ID_PROCESO NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
  BEGIN
    UPDATE T_MAE_PROCESO 
    SET NOMBRE = PI_NOMBRE,
        UPD_USUARIO = PI_USUARIO_GUARDAR,
        UPD_FECHA = SYSDATE
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
                    WHERE LOWER(TRANSLATE(P.NOMBRE,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                    P.FLAG_ESTADO = ''1''';
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
                        WHERE LOWER(TRANSLATE(P.NOMBRE,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                        P.FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_PROCESO;
  
  -- M ETAPA ---------------
  PROCEDURE USP_UPD_ETAPA(
    PI_ID_ETAPA NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
  BEGIN
    UPDATE T_MAE_ETAPA 
    SET NOMBRE = PI_NOMBRE,
        UPD_USUARIO = PI_USUARIO_GUARDAR,
        UPD_FECHA = SYSDATE
    WHERE ID_ETAPA = PI_ID_ETAPA;
  END USP_UPD_ETAPA;
  
  PROCEDURE USP_SEL_GET_ETAPA(
    PI_ID_ETAPA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  E.ID_ETAPA,
            E.NOMBRE ETAPA,
            P.NOMBRE PROCESO
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
                    (LOWER(TRANSLATE(E.NOMBRE,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                    LOWER(TRANSLATE(P.NOMBRE,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'') AND
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
                        SELECT  E.ID_ETAPA,
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
                        (LOWER(TRANSLATE(E.NOMBRE,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' OR
                        LOWER(TRANSLATE(P.NOMBRE,''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''�?É�?ÓÚáéíóú'',''AEIOUaeiou'')) ||''%'') AND
                        E.FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_ETAPA;
  
  PROCEDURE USP_SEL_ALL_ETAPA(
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  E.ID_ETAPA,
            E.NOMBRE ETAPA,
            P.NOMBRE PROCESO
    FROM  T_MAE_ETAPA E 
    INNER JOIN T_MAE_PROCESO P ON E.ID_PROCESO = P.ID_PROCESO
    WHERE E.FLAG_ESTADO = '1'; 
  END USP_SEL_ALL_ETAPA;

  -- M INSTITUCION ---------------
  PROCEDURE USP_SEL_OBTIENE_INSTITUCION_RUC(
    PI_RUC VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    I.ID_INSTITUCION,
    I.RUC,
    I.RAZON_SOCIAL,
    I.DOMICILIO_LEGAL,
    I.ID_SECTOR,
    I.FLAG_ESTADO
    FROM T_GENM_INSTITUCION I
    WHERE I.RUC = PI_RUC;
  END USP_SEL_OBTIENE_INSTITUCION_RUC;

  PROCEDURE USP_SEL_OBTIENE_INSTITUCION(
    PI_ID_INSTITUCION NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT
    I.ID_INSTITUCION,
    I.RUC,
    I.RAZON_SOCIAL,
    I.DOMICILIO_LEGAL,
    I.ID_SECTOR,
    I.FLAG_ESTADO
    FROM T_GENM_INSTITUCION I
    WHERE I.ID_INSTITUCION = PI_ID_INSTITUCION;
  END USP_SEL_OBTIENE_INSTITUCION;
  
  -- M ANNO -------------
  PROCEDURE USP_PRC_MAN_ANNO(
    PI_ID_ANNO NUMBER,
    PI_NOMBRE VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
      (SELECT A.ID_ANNO FROM T_MAE_ANNO A WHERE A.ID_ANNO = PI_ID_ANNO)
    INTO vID
    FROM DUAL;
    
    IF vID IS NULL THEN
      vID := SQ_GENM_ANNO.NEXTVAL();
      INSERT INTO T_MAE_ANNO
      (ID_ANNO, NOMBRE, REG_USUARIO, REG_FECHA)
      VALUES
      (vID, PI_NOMBRE, PI_USUARIO_GUARDAR, SYSDATE);
    ELSE
      UPDATE T_MAE_ANNO A SET
      A.NOMBRE = PI_NOMBRE,
      A.UPD_USUARIO = PI_USUARIO_GUARDAR,
      A.UPD_FECHA = SYSDATE
      WHERE A.ID_ANNO = PI_ID_ANNO;
    END IF;
  END USP_PRC_MAN_ANNO;
  
  PROCEDURE USP_DEL_ANNO(
    PI_ID_ANNO NUMBER,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
  BEGIN
    UPDATE T_MAE_ANNO 
    SET FLAG_ESTADO = '0',
        UPD_USUARIO = PI_USUARIO_GUARDAR,
        UPD_FECHA = SYSDATE
    WHERE ID_ANNO = PI_ID_ANNO;
  END USP_DEL_ANNO;

  PROCEDURE USP_SEL_GET_ANNO(
    PI_ID_ANNO NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_ANNO,
            NOMBRE
    FROM  T_MAE_ANNO
    WHERE ID_ANNO = PI_ID_ANNO; 
  END USP_SEL_GET_ANNO;

  PROCEDURE USP_SEL_LISTA_BUSQ_ANNO(
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
                    FROM T_MAE_ANNO A
                    WHERE LOWER(TRANSLATE(A.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND
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
                        SELECT  A.ID_ANNO,
                                A.NOMBRE,
                                ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                || vPAGINA_ACTUAL || ' AS PAGINA,'
                                || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                || vTOTAL_REG || ' AS TOTAL_REGISTROS
                        FROM T_MAE_ANNO A
                        WHERE LOWER(TRANSLATE(A.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_ANNO;
  
  PROCEDURE USP_SEL_ALL_ANNO(
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_ANNO,
            NOMBRE
    FROM  T_MAE_ANNO 
    WHERE FLAG_ESTADO = '1'; 
  END USP_SEL_ALL_ANNO;

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
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
    (SELECT C.ID_CONVOCATORIA FROM T_GENM_CONVOCATORIA C WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA)
    INTO vID
    FROM DUAL;
    
    IF vID IS NULL THEN
      PI_ID_GET := SQ_GENM_CONVOCATORIA.NEXTVAL();
      INSERT INTO T_GENM_CONVOCATORIA
      (ID_CONVOCATORIA, NOMBRE, FECHA_INICIO, FECHA_FIN, LIMITE_POSTULANTE, REG_USUARIO, REG_FECHA)
      VALUES
      (PI_ID_GET, PI_NOMBRE, PI_FECHA_INICIO, PI_FECHA_FIN, PI_LIMITE_POSTULANTE, PI_USUARIO_GUARDAR, SYSDATE);
    ELSE
      UPDATE T_GENM_CONVOCATORIA C SET
      C.NOMBRE = PI_NOMBRE,
      C.FECHA_INICIO = PI_FECHA_INICIO,
      C.FECHA_FIN = PI_FECHA_FIN,
      C.LIMITE_POSTULANTE = PI_LIMITE_POSTULANTE,
      C.UPD_USUARIO = PI_USUARIO_GUARDAR,
      C.UPD_FECHA = SYSDATE
      WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA;
      PI_ID_GET := PI_ID_CONVOCATORIA;
    END IF;
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_PRC_MAN_CONVOCATORIA;
  
  PROCEDURE USP_SEL_LISTA_BUSQ_CONVOCAT(
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
                    FROM T_GENM_CONVOCATORIA C
                    WHERE LOWER(TRANSLATE(C.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND
                    C.FLAG_ESTADO = ''1''';
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
                        SELECT  C.ID_CONVOCATORIA,
                                C.NOMBRE,
                                TO_CHAR(C.FECHA_INICIO, ''dd/MM/yyyy'') TXT_FECHA_INICIO,
                                TO_CHAR(C.FECHA_FIN, ''dd/MM/yyyy'') TXT_FECHA_FIN,
                                C.LIMITE_POSTULANTE,
                                ROW_NUMBER() OVER (ORDER BY ' || vCOLUMNA || ' ' || PI_ORDEN ||') AS ROWNUMBER,'
                                || vPAGINA_TOTAL || ' AS TOTAL_PAGINAS,'
                                || vPAGINA_ACTUAL || ' AS PAGINA,'
                                || PI_REGISTROS || ' AS CANTIDAD_REGISTROS,'
                                || vTOTAL_REG || ' AS TOTAL_REGISTROS
                        FROM T_GENM_CONVOCATORIA C
                        WHERE LOWER(TRANSLATE(C.NOMBRE,''����������'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''����������'',''AEIOUaeiou'')) ||''%'' AND
                        C.FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_CONVOCAT;
  
  PROCEDURE USP_SEL_GET_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_CONVOCATORIA,
            NOMBRE,
            TO_CHAR(FECHA_INICIO, 'yyyy-MM-dd') TXT_FECHA_INICIO,
            TO_CHAR(FECHA_FIN, 'yyyy-MM-dd') TXT_FECHA_FIN,
            LIMITE_POSTULANTE
    FROM  T_GENM_CONVOCATORIA
    WHERE ID_CONVOCATORIA = PI_ID_CONVOCATORIA; 
  END USP_SEL_GET_CONVOCATORIA;
  
  PROCEDURE USP_DEL_CONVOCATORIA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_USUARIO_GUARDAR NUMBER
  ) AS
  BEGIN
    UPDATE T_GENM_CONVOCATORIA 
    SET FLAG_ESTADO = '0',
        UPD_USUARIO = PI_USUARIO_GUARDAR,
        UPD_FECHA = SYSDATE
    WHERE ID_CONVOCATORIA = PI_ID_CONVOCATORIA;
  END USP_DEL_CONVOCATORIA;
  
  PROCEDURE USP_PRC_CONVOCATORIA_REQ(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_REQUERIMIENTO NUMBER,
    PI_FLAG_ESTADO VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
    (SELECT COUNT(*) FROM T_GEND_CONVOCATORIA_REQUERIM C WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND C.ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO)
    INTO vID
    FROM DUAL;
    
    IF vID = 0 THEN
      INSERT INTO T_GEND_CONVOCATORIA_REQUERIM
      (ID_CONVOCATORIA, ID_REQUERIMIENTO, FLAG_ESTADO, REG_USUARIO, REG_FECHA)
      VALUES
      (PI_ID_CONVOCATORIA, PI_ID_REQUERIMIENTO, PI_FLAG_ESTADO, PI_USUARIO_GUARDAR, SYSDATE);
    ELSE
      UPDATE T_GEND_CONVOCATORIA_REQUERIM C SET
      C.FLAG_ESTADO = PI_FLAG_ESTADO,
      C.UPD_USUARIO = PI_USUARIO_GUARDAR,
      C.UPD_FECHA = SYSDATE
      WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND C.ID_REQUERIMIENTO = PI_ID_REQUERIMIENTO;
    END IF;
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_PRC_CONVOCATORIA_REQ;
  
  PROCEDURE USP_PRC_CONVOCATORIA_CRI(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_CRITERIO NUMBER,
    PI_FLAG_ESTADO VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
    (SELECT COUNT(*) FROM T_GEND_CONVOCATORIA_CRITERIO C WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND C.ID_CRITERIO = PI_ID_CRITERIO)
    INTO vID
    FROM DUAL;
    
    IF vID = 0 THEN
      INSERT INTO T_GEND_CONVOCATORIA_CRITERIO
      (ID_CONVOCATORIA, ID_CRITERIO, FLAG_ESTADO, REG_USUARIO, REG_FECHA)
      VALUES
      (PI_ID_CONVOCATORIA, PI_ID_CRITERIO, PI_FLAG_ESTADO, PI_USUARIO_GUARDAR, SYSDATE);
    ELSE
      UPDATE T_GEND_CONVOCATORIA_CRITERIO C SET
      C.FLAG_ESTADO = PI_FLAG_ESTADO,
      C.UPD_USUARIO = PI_USUARIO_GUARDAR,
      C.UPD_FECHA = SYSDATE
      WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND C.ID_CRITERIO = PI_ID_CRITERIO;
    END IF;
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_PRC_CONVOCATORIA_CRI;
  
  PROCEDURE USP_PRC_CONVOCATORIA_EVA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_USUARIO NUMBER,
    PI_FLAG_ESTADO VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
    (SELECT COUNT(*) FROM T_GEND_CONVOCATORIA_EVALUADOR C WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND C.ID_USUARIO = PI_ID_USUARIO)
    INTO vID
    FROM DUAL;
    
    IF vID = 0 THEN
      INSERT INTO T_GEND_CONVOCATORIA_EVALUADOR
      (ID_CONVOCATORIA, ID_USUARIO, FLAG_ESTADO, REG_USUARIO, REG_FECHA)
      VALUES
      (PI_ID_CONVOCATORIA, PI_ID_USUARIO, PI_FLAG_ESTADO, PI_USUARIO_GUARDAR, SYSDATE);
    ELSE
      UPDATE T_GEND_CONVOCATORIA_EVALUADOR C SET
      C.FLAG_ESTADO = PI_FLAG_ESTADO,
      C.UPD_USUARIO = PI_USUARIO_GUARDAR,
      C.UPD_FECHA = SYSDATE
      WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND C.ID_USUARIO = PI_ID_USUARIO;
    END IF;
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_PRC_CONVOCATORIA_EVA;
  
  PROCEDURE USP_PRC_CONVOCATORIA_ETA(
    PI_ID_CONVOCATORIA NUMBER,
    PI_ID_ETAPA NUMBER,
    PI_DIAS VARCHAR2,
    PI_USUARIO_GUARDAR NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
    (SELECT COUNT(*) FROM T_GEND_CONVOCATORIA_ETAPA C WHERE C.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND C.ID_ETAPA = PI_ID_ETAPA)
    INTO vID
    FROM DUAL;
    
    IF vID = 0 THEN
      INSERT INTO T_GEND_CONVOCATORIA_ETAPA
      (ID_CONVOCATORIA, ID_ETAPA, DIAS, FLAG_ESTADO, REG_USUARIO, REG_FECHA)
      VALUES
      (PI_ID_CONVOCATORIA, PI_ID_ETAPA, PI_DIAS, '1', PI_USUARIO_GUARDAR, SYSDATE);
    ELSE
      UPDATE T_GEND_CONVOCATORIA_ETAPA E SET
      E.DIAS = PI_DIAS,
      E.UPD_USUARIO = PI_USUARIO_GUARDAR,
      E.UPD_FECHA = SYSDATE
      WHERE E.ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND E.ID_ETAPA = PI_ID_ETAPA;
    END IF;
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_PRC_CONVOCATORIA_ETA;
  
  PROCEDURE USP_SEL_LISTA_CONVOCAT_REQ(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_REQUERIMIENTO,
            FLAG_ESTADO
    FROM  T_GEND_CONVOCATORIA_REQUERIM
    WHERE ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND FLAG_ESTADO = '1'; 
  END USP_SEL_LISTA_CONVOCAT_REQ;
  
  PROCEDURE USP_SEL_LISTA_CONVOCAT_CRI(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_CRITERIO,
            FLAG_ESTADO
    FROM  T_GEND_CONVOCATORIA_CRITERIO
    WHERE ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND FLAG_ESTADO = '1'; 
  END USP_SEL_LISTA_CONVOCAT_CRI;
  
  PROCEDURE USP_SEL_LISTA_CONVOCAT_EVA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_USUARIO,
            FLAG_ESTADO
    FROM  T_GEND_CONVOCATORIA_EVALUADOR
    WHERE ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND FLAG_ESTADO = '1'; 
  END USP_SEL_LISTA_CONVOCAT_EVA;
  
  PROCEDURE USP_SEL_LISTA_CONVOCAT_ETA(
    PI_ID_CONVOCATORIA NUMBER,
    PO_REF OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN PO_REF FOR
    SELECT  ID_ETAPA,
            DIAS,
            FLAG_ESTADO
    FROM  T_GEND_CONVOCATORIA_ETAPA
    WHERE ID_CONVOCATORIA = PI_ID_CONVOCATORIA AND FLAG_ESTADO = '1'; 
  END USP_SEL_LISTA_CONVOCAT_ETA;

  -- INSTITUCION|
  PROCEDURE USP_MAN_GUARDA_INSTITUCION(
    PI_ID_INSTITUCION IN OUT NUMBER,
    PI_RUC VARCHAR2,
    PI_RAZON_SOCIAL VARCHAR2,
    PI_DOMICILIO_LEGAL VARCHAR2,
    PI_ID_SECTOR NUMBER,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    SELECT
    (SELECT I.ID_INSTITUCION FROM T_GENM_INSTITUCION I WHERE I.ID_INSTITUCION = PI_ID_INSTITUCION)
    INTO vID
    FROM DUAL;
    
    IF vID IS NULL THEN
      PI_ID_INSTITUCION := SQ_GENM_INSTITUCION.NEXTVAL();
      INSERT INTO T_GENM_INSTITUCION
      (ID_INSTITUCION, RUC, RAZON_SOCIAL, DOMICILIO_LEGAL, ID_SECTOR, REG_USUARIO)
      VALUES
      (PI_ID_INSTITUCION, PI_RUC, PI_RAZON_SOCIAL, PI_DOMICILIO_LEGAL, PI_ID_SECTOR, PI_UPD_USUARIO);
    ELSE
      UPDATE T_GENM_INSTITUCION I SET
        I.RAZON_SOCIAL = PI_RAZON_SOCIAL,
        I.DOMICILIO_LEGAL = PI_DOMICILIO_LEGAL,
        I.ID_SECTOR = PI_ID_SECTOR,
        I.UPD_USUARIO = PI_UPD_USUARIO,
        I.UPD_FECHA = SYSDATE
      WHERE I.ID_INSTITUCION = vID;
    END IF;
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_MAN_GUARDA_INSTITUCION;
END PKG_SRES_MANTENIMIENTO;