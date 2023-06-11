USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Create_Product]    Script Date: 11/6/2023 9:44:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 7/6/2023
-- Description:	Create Products 
-- =============================================
ALTER PROCEDURE [dbo].[Create_Product]
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
@imageUrl nvarchar(255),
@imageList nvarchar(Max),
@language varchar(50),
@categoryIds varchar(50),
@id int output
AS
BEGIN
	
	SET NOCOUNT ON;
	set xact_abort on 
	BEGIN TRAN
		BEGIN TRY 
			INSERT INTO Products (Price,Sku,IsActive,ImageUrl, ImageList)
			VALUES (@price,@sku,@isActice,@imageUrl, @imageList)
			SET @id = SCOPE_IDENTITY();

			INSERT INTO ProductTranslations(ProductId,LanguageId,Name,Content,Description,SeoDescription,SeoAlias,SeoKeyword,SeoTitle)
			VALUES (@id,@language,@name,@content,@description,@seoDescription,@seoAlias,@seoKeyword,@seoTitle)

			DELETE FROM ProductInCategories WHERE ProductId = @id

			INSERT INTO ProductInCategories
			SELECT @id as ProductId, CAST(String as INT) as CategoryId from ufn_CSVToTable(@categoryIds,',')
			COMMIT
		END TRY
		BEGIN CATCH 
			ROLLBACK
			DECLARE @ErrorMessage VARCHAR(MAX)
			SELECT @ErrorMessage = 'Lỗi' + ERROR_MESSAGE()
			RAISERROR(@ErrorMessage,16,1)
		END CATCH
	

END
