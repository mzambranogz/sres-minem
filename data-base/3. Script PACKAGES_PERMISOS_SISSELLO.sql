--------------------------------------------------------
-- Permisos para paquetes
-- Archivo creado  - viernes-Marzo-20-2020 
-- Se ejecuta desde el System  
--------------------------------------------------------

alter session set "_ORACLE_SCRIPT"=true;
create user REES_CON identified by 123456;

GRANT EXECUTE ON SISSELLO.PKG_SISSELLO_ADMIN TO REES_CON;
GRANT EXECUTE ON SISSELLO.PKG_SISSELLO_CRITERIO TO REES_CON;
GRANT EXECUTE ON SISSELLO.PKG_SISSELLO_MANTENIMIENTO TO REES_CON;
GRANT EXECUTE ON SISSELLO.PKG_SISSELLO_VERIFICACION TO REES_CON;
