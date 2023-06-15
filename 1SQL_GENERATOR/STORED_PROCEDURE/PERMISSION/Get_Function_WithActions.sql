USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[GET_ALL_FUNCTION]    Script Date: 15/6/2023 1:18:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<tydang,tydang>
-- Create date: <15/06/2023>
-- Description:	<GET_ALL_FUNCTION>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Function_ByPermission] 
@userId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- không trả về số dòng affect
	SET NOCOUNT ON;

 select 
		f.Id,
		f.Name,
		f.Url,
		f.ParentId,
		f.IsActive,
		f.SortOrder,
		f.CssClass
	 from Functions f
	join Permissions p on f.Id = p.FunctionId
	join AspNetRoles r on p.RoleId = r.Id
	join Actions a on p.ActionId = a.Id
	join AspNetUserRoles ur on r.Id = ur.RoleId
	where ur.UserId = @userId and a.Id ='VIEW'
END
