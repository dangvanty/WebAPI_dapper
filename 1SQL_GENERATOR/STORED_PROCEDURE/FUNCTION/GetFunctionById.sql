USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Find_Product_By_Id]    Script Date: 14/6/2023 5:18:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 14/6/2023
-- Description:	Select one function by id
-- =============================================
Create PROCEDURE [dbo].[Find_Function_By_Id]
@id varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT [Id]
      ,[Name]
      ,[Url]
      ,[ParentId]
      ,[SortOrder]
      ,[CssClass]
      ,[IsActive]
  FROM [dbo].[Functions]
  Where Id = @id
END
