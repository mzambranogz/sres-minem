--------------------------------------------------------
-- Archivo creado  - mi�rcoles-junio-17-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Package PKG_SRES_MANTENIMIENTO
--------------------------------------------------------

CREATE OR REPLACE NONEDITIONABLE PACKAGE "PKG_SRES_MANTENIMIENTO" AS

  PROCEDURE USP_SEL_LISTA_BUSQ_CRITERIO(
    PI_BUSCAR VARCHAR2,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );
  
  PROCEDURE USP_SEL_LISTA_BUSQ_USUARIO(
    PI_BUSCAR VARCHAR2,
    PI_REGISTROS NUMBER,
    PI_PAGINA NUMBER,
    PI_COLUMNA VARCHAR2,
    PI_ORDEN VARCHAR2,
    PO_REF OUT SYS_REFCURSOR
  );

END PKG_SRES_MANTENIMIENTO;
