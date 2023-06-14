USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[GET_ALL_ROLE]    Script Date: 14/6/2023 8:37:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<tydang,tydang>
-- Create date: <14/06/2023>
-- Description:	<Get all role>
-- =============================================
ALTER PROCEDURE [dbo].[GET_ALL_ROLE] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- không trả về số dòng affect
	SET NOCOUNT ON;

  SELECT Id,Name,NormalizedName FROM AspNetRoles
END
