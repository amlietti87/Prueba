/*
Script created by SQL Examiner 7.1.7.249 at 15/1/2020 11:34:04.
Run this script on [172.16.17.60].ROSBUS_DEV to make it the same as [172.16.17.60].ROSBUS_INSP
*/

GO
SET NOCOUNT ON
SET NOEXEC OFF
SET ARITHABORT ON
SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRAN
GO
--step 1: alter column [SucursalId] of table [dbo].[Insp_TareasRealizadas]--------------------------
ALTER TABLE [dbo].[Insp_TareasRealizadas] ALTER COLUMN [SucursalId] [int] NULL
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 1 is completed with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 1 is completed with errors' SET NOEXEC ON END
GO
--step 2: alter column [Valor] of table [dbo].[Insp_TareasRealizadasDetalle]------------------------
ALTER TABLE [dbo].[Insp_TareasRealizadasDetalle] ALTER COLUMN [Valor] [nchar](500) NULL
GO
IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN PRINT 'step 2 is completed with errors' ROLLBACK TRAN END
GO
IF @@TRANCOUNT = 0 BEGIN PRINT 'step 2 is completed with errors' SET NOEXEC ON END
GO
----------------------------------------------------------------------
IF @@TRANCOUNT > 0 BEGIN COMMIT TRAN PRINT 'Synchronization is successfully completed.' END
GO
SET NOEXEC OFF
GO
