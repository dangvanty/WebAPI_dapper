USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Find_Product_By_Id]    Script Date: 10/6/2023 6:43:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 7/6/2023
-- Description:	Select one Products by id
-- =============================================
ALTER PROCEDURE [dbo].[Find_Product_By_Id]
@id int,
@language varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT p.Id, 
		p.Sku,
		p.Price,
		p.DiscountPrice,
		p.IsActive,
		p.RateCount,
		p.RateTotal, 
		p.ViewCount,
		pt.Name,
		pt.SeoDescription,
		pt.Content,
		pt.Description,
		pt.SeoAlias,
		pt.SeoKeyword,
		pt.SeoTitle,
		pt.LanguageId
    FROM Products p INNER JOIN ProductTranslations pt
	ON p.Id = pt.ProductId
	WHERE Id = @id
END
