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
ALTER PROCEDURE [dbo].[Get_Permission_ByRoleId]
	@roleId uniqueidentifier null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select 
		p.FunctionId,
		p.ActionId,
		p.RoleId
	from Permissions p inner join Actions a
		on p.ActionId = a.Id 
	where p.RoleId = @roleId
END