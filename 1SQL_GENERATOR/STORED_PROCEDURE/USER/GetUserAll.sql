USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[GET_ALL_ROLE]    Script Date: 14/6/2023 10:30:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<tydang,tydang>
-- Create date: <14/06/2023>
-- Description:	<GET_USER_ALL>
-- =============================================
CREATE PROCEDURE [dbo].[GET_USER_ALL] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- không trả về số dòng affect
	SET NOCOUNT ON;

  SELECT u.Id, u.UserName, u.Email,u.PhoneNumber, u.FullName, u.Address FROM AspNetUsers u
END
