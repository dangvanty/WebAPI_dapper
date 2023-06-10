USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Delete_Product_By_Id]    Script Date: 10/6/2023 6:42:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 7/6/2023
-- Description:	Delete_Product_By_Id
-- required soft: sql server > 2015 
-- =============================================
ALTER PROCEDURE [dbo].[Delete_Product_By_Id]
@id int 
AS
BEGIN
	
	SET NOCOUNT ON;
	set xact_abort on;
	BEGIN TRAN
		BEGIN TRY
			DELETE FROM Products WHERE Id = @id
			DELETE FROM ProductTranslations WHERE ProductId = @id
			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK
			DECLARE @ErrorMessage VARCHAR(MAX)
			SELECT @ErrorMessage = 'Lỗi' + ERROR_MESSAGE()
			RAISERROR(@ErrorMessage,16,1)

		END CATCH 
END
