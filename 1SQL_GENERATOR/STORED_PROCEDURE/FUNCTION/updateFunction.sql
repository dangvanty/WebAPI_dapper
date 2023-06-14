USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Edit_Product]    Script Date: 14/6/2023 5:32:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 14/6/2023
-- Description:	update Function
-- =============================================
CREATE PROCEDURE [dbo].[Update_Function]
@id varchar(50),
@name nvarchar(50),
@url nvarchar(50),
@parentId varchar(50),
@sortOrder int,
@cssClass nvarchar(50),
@isActive bit
AS
BEGIN

	SET NOCOUNT ON;
	UPDATE [dbo].[Functions]
   SET [Name] = @name
      ,[Url] = @url
      ,[ParentId] = @parentId
      ,[SortOrder] = @sortOrder
      ,[CssClass] = @cssClass
      ,[IsActive] = @isActive
 WHERE [Id] = @id
	
		
END
