USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Create_Function]    Script Date: 15/6/2023 5:20:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 15/6/2023
-- Description:	Create permission 
-- =============================================
--create TYPE [dbo].[Permission] AS TABLE(
--    [RoleId] uniqueidentifier,
--    [FunctionId] [varchar](50) NOT NULL,
--    [ActionId] [varchar](50) NOT NULL
--)
--GO
CREATe PROCEDURE [dbo].[Create_Permission]
	@roleId uniqueidentifier,
	@permissions dbo.Permission readonly
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try
	   delete from Permissions where RoleId = @roleId

	   insert into Permissions (RoleId,FunctionId,ActionId)
	   select RoleId,FunctionId,ActionId from @permissions

	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Lỗi: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch



  
END