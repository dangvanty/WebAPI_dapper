USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Edit_Product]    Script Date: 10/6/2023 6:46:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 7/6/2023
-- Description:	update Products 
-- =============================================
ALTER PROCEDURE [dbo].[Edit_Product]
@id int ,
@name nvarchar(255),
@seoDescription nvarchar(255),
@description nvarchar(255),
@content ntext,
@seoAlias varchar(255),
@seoTitle nvarchar(255),
@seoKeyword nvarchar(255),
@price float,
@sku varchar(50),
@isActice bit,
@language varchar(50),
@imageUrl nvarchar(255)

AS
BEGIN

	SET NOCOUNT ON;
	set xact_abort on 
	BEGIN TRAN
		BEGIN TRY 
			UPDATE Products SET
				Price = @price,
				Sku = @sku,
				IsActive = @isActice,
				ImageUrl = @imageUrl
			WHERE Id = @id

			UPDATE ProductTranslations SET
				Name = @name,
				Description = @description,
				Content = @content,
				SeoDescription = @seoDescription,
				SeoAlias = @seoAlias,
				SeoKeyword = @seoKeyword,
				SeoTitle = @seoTitle
			WHERE ProductId = @id AND LanguageId = @language

			COMMIT
		END TRY
		BEGIN CATCH 
			ROLLBACK
			DECLARE @ErrorMessage VARCHAR(MAX)
			SELECT @ErrorMessage = 'Lỗi' + ERROR_MESSAGE()
			RAISERROR(@ErrorMessage,16,1)
		END CATCH
	
		
END
