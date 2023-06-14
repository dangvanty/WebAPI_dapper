USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Create_Product]    Script Date: 14/6/2023 5:29:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 7/6/2023
-- Description:	Create Products 
-- =============================================
CREATE PROCEDURE [dbo].[Create_Function]
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
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Functions]
           ([Id]
           ,[Name]
           ,[Url]
           ,[ParentId]
           ,[SortOrder]
           ,[CssClass]
           ,[IsActive])
     VALUES
           (@id
           ,@name
           ,@url
           ,@parentId
           ,@sortOrder
           ,@cssClass
           ,@isActive)
END
