-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [DS3].[WeaponSearch]
	@SearchValue varchar(50) = null,
	@WeaponId int = null,
	@WeaponTypeId int = null,
	@InfusionId int = null,
	@ReinforcementLevel int = 10,
	@MaxWeight float = null,
	@STR int = null,
	@DEX int = null,
	@INT int = null,
	@FTH int = null
AS
BEGIN
SET NOCOUNT ON;

Select 
w.WeaponId,
wt.WeaponTypeId,
i.InfusionId,
w.Name, 
wt.Name as WeaponType,
i.Name as Infusion,
wv.Physical, 
wv.Magic, 
wv.Fire, 
wv.Lightning, 
wv.Dark, 
--w.Bleed,
--w.Poison,
--w.Frost,
w.StrReq, 
w.DexReq, 
w.IntReq, 
w.FthReq, 
wv.Str * 100 as StrScaling, 
wv.Dex * 100 as DexScaling, 
wv.Int * 100 as IntScaling, 
wv.Faith * 100 as FthScaling, 
wv.Luck * 100 as LckScaling, 
w.Weight,
w.Critical,
up.Type as UpgradePath
From DS3.Weapon w
Inner Join DS3.WeaponValues wv on wv.WeaponId = w.WeaponId
Inner Join DS3.Infusion i on wv.InfusionId = i.InfusionId
Inner Join DS3.WeaponType wt on wt.WeaponTypeId = w.WeaponTypeId
Inner Join DS3.UpgradePath up on w.UpgradePathId = up.UpgradePathId
Where (@WeaponId is null or @WeaponId = w.WeaponId)
and (@SearchValue is null or CHARINDEX(@SearchValue,w.Name) > 0)
and (@InfusionId is null or i.InfusionId = @InfusionId)
and (@WeaponTypeId = 0 or wt.WeaponTypeId = @WeaponTypeId)
and (@MaxWeight is null or w.Weight <= @MaxWeight)
and (@STR is null or w.StrReq <= @STR)
and (@DEX is null or w.DexReq <= @DEX)
and (@INT is null or w.IntReq <= @INT)
and (@FTH is null or w.FthReq <= @FTH)
END