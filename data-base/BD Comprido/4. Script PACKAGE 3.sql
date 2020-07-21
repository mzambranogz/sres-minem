prompt
prompt Creating package PKG_SRE_VERIFICACION
prompt =====================================
prompt
CREATE OR REPLACE NONEDITIONABLE PACKAGE SRES."PKG_SRE_VERIFICACION" AS
  PROCEDURE USP_INS_REGISTRA_CONVOCATORIATRAZABILIDAD(
    PI_ID_CONVOCATORIA NUMBER,
    PI_DESCRIPCION VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
END PKG_SRE_VERIFICACION;
/


prompt
prompt Creating package body PKG_SRE_VERIFICACION
prompt ==========================================
prompt
CREATE OR REPLACE NONEDITIONABLE PACKAGE BODY SRES."PKG_SRE_VERIFICACION" AS
  PROCEDURE USP_INS_REGISTRA_CONVOCATORIATRAZABILIDAD(
    PI_ID_CONVOCATORIA NUMBER,
    PI_DESCRIPCION VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  ) AS
    vID NUMBER;
  BEGIN
    vID := SQ_GEND_CONVOCATORIA_TRAZA.NEXTVAL();
    
    INSERT INTO T_GEND_CONVOCATORIA_TRAZA
    (ID_CONVOCATORIA, ID_TRAZABILIDAD, DESCRIPCION, REG_USUARIO)
    VALUES
    (PI_ID_CONVOCATORIA, vID, PI_DESCRIPCION, PI_UPD_USUARIO);
    
    PO_ROWAFFECTED := SQL%ROWCOUNT;
  END USP_INS_REGISTRA_CONVOCATORIATRAZABILIDAD;
END PKG_SRE_VERIFICACION;
/


prompt Done
spool off
set define on