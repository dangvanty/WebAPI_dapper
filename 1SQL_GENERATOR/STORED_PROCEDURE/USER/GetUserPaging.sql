USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Get_Role_All_Paging]    Script Date: 14/6/2023 10:32:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<tydang,tydang>
-- Create date: <14/06/2023>
-- Description:	<Get all user paging>
-- =============================================
Create PROCEDURE [dbo].[Get_User_All_Paging] 
@keyword varchar(50),
@pageIndex int,
@pageSize int,
@totalRow int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- không trả về số dòng affect
	SET NOCOUNT ON;

  SELECT @totalRow = count(*)
  FROM AspNetUsers
  Where FullName like @keyword + '%' or @keyword is null

  SELECT  u.Id, u.UserName, u.Email,u.PhoneNumber, u.FullName, u.Address FROM AspNetUsers u
  Where u.FullName like @keyword+'%' or @keyword is null
  ORDER BY u.FullName 
  offset (@pageIndex -1) * @pageSize rows
  fetch next @pageSize row only
END
