USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Delete_Product_By_Id]    Script Date: 14/6/2023 5:35:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dangvanty
-- Create date: 14/6/2023
-- Description:	Delete_Fucntion_By_Id
-- required soft: sql server > 2015 
-- =============================================
CREATE PROCEDURE [dbo].[Delete_Function_ById]
@id varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;
	delete from Functions where Id = @id
END
