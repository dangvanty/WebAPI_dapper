USE [RestApiAdapper]
GO
/****** Object:  StoredProcedure [dbo].[Get_Attribute_ById]    Script Date: 28/6/2023 12:13:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<tydang>
-- Create date: <27/06/2023>
-- Description:	<Get_Attribute_ById>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Attribute_All]
	@language varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   select
		a.Id,
		a.[Name],
		convert(nvarchar(200),case 
			when a.BackendType= 'int' then convert(nvarchar(200),ai.Value)
			when a.BackendType= 'text' then convert(nvarchar(200),ai.Value) 
			when a.BackendType= 'datetime' then convert(nvarchar(200),adt.Value) 
			when a.BackendType= 'decimal' then convert(nvarchar(200),ad.Value) 
			when a.BackendType= 'varchar' then convert(varchar(200),avc.Value) 
		 end) as [Value]
		 from Attributes a
		 left join AttributeValueInts ai on a.Id = ai.AttributeId and ai.LanguageId=@language 
		 left join AttributeValueTexts avt on a.Id = avt.AttributeId and avt.LanguageId=@language 
		 left join AttributeValueDateTimes adt on a.Id = adt.AttributeId and adt.LanguageId=@language 
		 left join AttributeValueDecimals ad on a.Id = ad.AttributeId and ad.LanguageId=@language 
		 left join AttributeValueVarchars avc on a.Id = avc.AttributeId and avc.LanguageId=@language 

END