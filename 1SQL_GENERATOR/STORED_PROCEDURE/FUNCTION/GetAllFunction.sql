USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[GET_ALL_ROLE]    Script Date: 14/6/2023 5:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<tydang,tydang>
-- Create date: <14/06/2023>
-- Description:	<GET_ALL_FUNCTION>
-- =============================================
create PROCEDURE [dbo].[GET_ALL_FUNCTION] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- không trả về số dòng affect
	SET NOCOUNT ON;

 SELECT [Id]
      ,[Name]
      ,[Url]
      ,[ParentId]
      ,[SortOrder]
      ,[CssClass]
      ,[IsActive]
  FROM [dbo].[Functions]
END
