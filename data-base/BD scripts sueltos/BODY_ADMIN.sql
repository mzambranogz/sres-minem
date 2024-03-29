--------------------------------------------------------
-- Archivo creado  - martes-julio-21-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Package Body PKG_SRES_ADMIN
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE BODY "SRES"."PKG_SRES_ADMIN" AS

  PROCEDURE USP_SEL_USUARIO(
        PO  OUT SYS_REFCURSOR
    ) AS
  BEGIN
    OPEN PO FOR
    SELECT * FROM T_GENM_USUARIO;
  END USP_SEL_USUARIO;
  
  PROCEDURE USP_SEL_USUARIO_CORREO(
        pCORREO  IN VARCHAR2,
        pCURSOR  OUT SYS_REFCURSOR
    ) AS
    BEGIN
      OPEN pCURSOR FOR
      SELECT
      ID_USUARIO, NOMBRES, APELLIDOS, CORREO, CONTRASENA,
      TELEFONO, ANEXO, CELULAR, ID_INSTITUCION, ID_ROL,
      FLAG_ESTADO, REG_USUARIO, REG_FECHA, UPD_USUARIO, UPD_FECHA
      FROM T_GENM_USUARIO
      WHERE CORREO = pCORREO;
    END USP_SEL_USUARIO_CORREO;

END PKG_SRES_ADMIN;

/
