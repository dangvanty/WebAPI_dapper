USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Get_Product_All]    Script Date: 10/6/2023 6:43:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 7/6/2023
-- Description:	Select all Products
-- =============================================
-- Exec Get_Product_All 'en-US'
ALTER PROCEDURE [dbo].[Get_Product_All]
@language varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		p.Id, 
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

    FROM Products p inner join ProductTranslations pt 
	ON p.Id = pt.ProductId
	Where pt.LanguageId = @language
END
