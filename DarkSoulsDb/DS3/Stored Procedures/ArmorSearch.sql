-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [DS3].[ArmorSearch]
	@SearchValue varchar(50) = null,
	@ArmorId int = null,
	@ArmorTypeId int = null,
	@MaxWeight float = null,
	@MinPoise float = null
AS
BEGIN
SET NOCOUNT ON;

Select 
a.ArmorId,
at.ArmorTypeId,
a.Name, 
at.Name as ArmorType,
a.Physical, 
a.Slash,
a.Strike,
a.Thrust,
a.Magic, 
a.Fire, 
a.Lightning, 
a.Dark, 
--a.Bleed,
a.Poison,
a.Toxic,
a.Curse,
a.Frost,
a.Weight,
a.Poise,
CASE WHEN a.Weight = 0 THEN 0 ELSE a.Poise / a.Weight END As PoiseToWeight,
CASE WHEN a.Weight = 0 THEN 0 ELSE a.Physical / a.Weight END As DefenseToWeight,
a.Durability
From DS3.Armor a
Inner Join DS3.ArmorType at on at.ArmorTypeId = a.ArmorTypeId
Where (@ArmorId is null or @ArmorId = a.ArmorId)
and (@SearchValue is null or CHARINDEX(@SearchValue,a.Name) > 0)
and (@ArmorTypeId = 0 or at.ArmorTypeId = @ArmorTypeId)
and (@MaxWeight is null or a.Weight <= @MaxWeight)
and (@MinPoise is null or a.Poise >= @MinPoise)
END
