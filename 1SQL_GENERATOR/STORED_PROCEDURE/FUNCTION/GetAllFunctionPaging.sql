USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Get_Product_All_Paging]    Script Date: 14/6/2023 5:26:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tydang
-- Create date: 14/6/2023
-- Description:	Get_Fuction_All_Paging
-- =============================================
Create PROCEDURE [dbo].[Get_Function_All_Paging]
@keyword nvarchar(50),
@pageIndex int ,
@pageSize int ,
@totalRow int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select @totalRow = count(*) from Functions r
	where (@keyword is null or r.Name like @keyword +'%')

	SELECT [Id]
      ,[Name]
      ,[Url]
      ,[ParentId]
      ,[SortOrder]
      ,[CssClass]
      ,[IsActive]
  FROM [dbo].[Functions] f
	where (@keyword is null or f.Name like @keyword +'%')
	order by f.Name
	offset (@pageIndex - 1) * @pageSize rows
	fetch next @pageSize row only
END
