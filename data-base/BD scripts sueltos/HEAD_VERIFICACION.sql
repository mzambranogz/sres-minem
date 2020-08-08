--------------------------------------------------------
-- Archivo creado  - martes-julio-21-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Package PKG_SRE_VERIFICACION
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE "SRES"."PKG_SRE_VERIFICACION" AS
  PROCEDURE USP_INS_REGISTRA_CONVOCATORIATRAZABILIDAD(
    PI_ID_CONVOCATORIA NUMBER,
    PI_DESCRIPCION VARCHAR2,
    PI_UPD_USUARIO NUMBER,
    PO_ROWAFFECTED OUT NUMBER
  );
END PKG_SRE_VERIFICACION;

/
