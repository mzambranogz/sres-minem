CREATE OR REPLACE NONEDITIONABLE PACKAGE BODY PKG_SRES_CRITERIO AS
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
                    WHERE 
                    LOWER(TRANSLATE(C.NRO_INFORME,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_NRO_INFORME ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                    LOWER(TRANSLATE(C.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_NOMBRE ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND ' ||
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
                          WHERE 
                          LOWER(TRANSLATE(C.NRO_INFORME,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_NRO_INFORME ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND
                          LOWER(TRANSLATE(C.NOMBRE,''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_NOMBRE ||''',''ÁÉÍÓÚáéíóú'',''AEIOUaeiou'')) ||''%'' AND ' ||
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
END PKG_SRES_CRITERIO;
