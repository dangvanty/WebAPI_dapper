
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<tydang>
-- Create date: <27/06/2023>
-- Description:	<delete attribute by id>
-- =============================================
CREATE PROCEDURE Delete_Attribute_ById
@id int
AS
BEGIN
	SET NOCOUNT ON;
	set xact_abort on;
	begin tran
	begin try
	  delete from AttributeOptionValues where OptionId
	  in (select Id from AttributeOptions where AttributeId = @id)

	  delete from AttributeOptions where AttributeId = @id

	  delete from AttributeValueDateTimes where AttributeId = @id
	  delete from AttributeValueDecimals where AttributeId = @id

	  delete from AttributeValueInts where AttributeId = @id
	  delete from AttributeValueTexts where AttributeId = @id
	  delete from AttributeValueVarchars where AttributeId = @id

	  delete from Attributes where Id = @id
	commit
	end try
	begin catch
		rollback
		  DECLARE @ErrorMessage VARCHAR(2000)
		  SELECT @ErrorMessage = 'Lỗi: ' + ERROR_MESSAGE()
		  RAISERROR(@ErrorMessage, 16, 1)
	end catch
END
