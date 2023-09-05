--https://rosariobus.visualstudio.com/PLANIFICACION/_workitems/edit/5087
--Firma digital - eliminar el estado borrador y la acción aprobar
--actualizo estado aprobado a pendiente de firma
UPDATE FD_Estados SET Descripcion='Pendiente firma', importarDocumentoOK=1 where Id=2
--se borran las acciones permitidas asociadas a la accion aprobar y estado borrador
DELETE FROM FD_Acciones where AccionPermitidaID=1
--se borra la accion Aprobar
DELETE FROM FD_AccionesPermitidas where Id=1
--si existen documentos con estado Borrador los tengo que actualizar a pendiente de firma --en PRD no existen registros
UPDATE FD_DocumentosProcesados SET EstadoId=2 where EstadoId=1
--los documentos historicos con el estado borrador se tienen que borrar --en PRD no existen registros
DELETE FROM FD_DocumentosProcesadosHistorico where EstadoId=1
--se borra el estado Borrador
DELETE FROM FD_Estados where Id=1