--------------------------------------------------------
-- Archivo creado  - miÈrcoles-junio-17-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Package Body PKG_SRES_MANTENIMIENTO
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE BODY "SRES"."PKG_SRES_MANTENIMIENTO" AS

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
                    WHERE LOWER(TRANSLATE(C.NOMBRE,''¡…Õ”⁄·ÈÌÛ˙'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''¡…Õ”⁄·ÈÌÛ˙'',''AEIOUaeiou'')) ||''%'' AND
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
                        WHERE LOWER(TRANSLATE(C.NOMBRE,''¡…Õ”⁄·ÈÌÛ˙'',''AEIOUaeiou'')) like ''%''|| LOWER(TRANSLATE('''|| PI_BUSCAR ||''',''¡…Õ”⁄·ÈÌÛ˙'',''AEIOUaeiou'')) ||''%'' AND
                        FLAG_ESTADO = ''1''
                        )
                    WHERE  ROWNUMBER BETWEEN ' || TO_CHAR(PI_REGISTROS * vPAGINA_INICIAL + 1) || ' AND ' || TO_CHAR(PI_REGISTROS * (vPAGINA_INICIAL + 1));
    
    OPEN PO_REF FOR vQUERY_SELECT;
  END USP_SEL_LISTA_BUSQ_CRITERIO;

END PKG_SRES_MANTENIMIENTO;

/
