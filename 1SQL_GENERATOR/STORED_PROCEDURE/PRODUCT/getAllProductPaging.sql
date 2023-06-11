USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Get_Product_All_Paging]    Script Date: 11/6/2023 8:54:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tydang
-- Create date: 8/6/2023
-- Description:	Get_Product_All_Paging
-- =============================================
ALTER PROCEDURE [dbo].[Get_Product_All_Paging]
@keyword nvarchar(50),
@categoryId int,
@pageIndex int ,
@pageSize int ,
@language varchar(50),
@totalRow int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @totalRow = COUNT(*) 
	FROM Products p INNER JOIN ProductTranslations pt
	ON p.Id = pt.ProductId
	LEFT JOIN ProductInCategories pic ON p.id = pic.ProductId
	LEFT JOIN Categories c ON  c.Id =pic.CategoryId
	WHERE (Sku like @keyword + '%' or @keyword is null) and p.IsActive = 1
	and pt.LanguageId = @language
	and pic.CategoryId = @categoryId

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
		pt.LanguageId,
		c.Name as CategoryName
	FROM Products p INNER JOIN ProductTranslations pt 
	ON p.Id = pt.ProductId
	LEFT JOIN ProductInCategories pic ON p.id = pic.ProductId
	LEFT JOIN Categories c ON  c.Id =pic.CategoryId
	WHERE (@keyword is null or Sku like @keyword + '%') and p.IsActive =1
	and pt.LanguageId = @language
	and pic.CategoryId = @categoryId
	ORDER BY p.CreatedAt desc
	offset (@pageIndex -1) * @pageSize rows
	fetch next @pageSize row only
END
