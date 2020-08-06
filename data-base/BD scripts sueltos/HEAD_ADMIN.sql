--------------------------------------------------------
-- Archivo creado  - martes-julio-21-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Package PKG_SRES_ADMIN
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE "SRES"."PKG_SRES_ADMIN" AS

  PROCEDURE USP_SEL_USUARIO(
        PO  OUT SYS_REFCURSOR
    );
    
  PROCEDURE USP_SEL_USUARIO_CORREO(
      pCORREO  IN VARCHAR2,
      pCursor  OUT SYS_REFCURSOR
    );
    
END PKG_SRES_ADMIN;

/
