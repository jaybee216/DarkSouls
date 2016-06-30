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

Declare @BaseModifier float = (Select [BaseModifier] From [DS3].[Reinforcement] Where [ReinforcementLevel] = @ReinforcementLevel)
Declare @ScalingModifier float = (Select [ScalingModifier] From [DS3].[Reinforcement] Where [ReinforcementLevel] = @ReinforcementLevel)

Select 
w.WeaponId,
wt.WeaponTypeId,
i.InfusionId,
w.Name, 
wt.Name as WeaponType,
i.Name as Infusion,
wv.Physical * @BaseModifier as Physical, 
wv.Magic * @BaseModifier as Magic, 
wv.Fire * @BaseModifier as Fire, 
wv.Lightning * @BaseModifier as Lightning, 
wv.Dark * @BaseModifier as Dark, 
wv.Bleed,
wv.Poison,
wv.Frost,
w.StrReq, 
w.DexReq, 
w.IntReq, 
w.FthReq, 
wv.Str * @ScalingModifier as StrScaling, 
wv.Dex * @ScalingModifier as DexScaling, 
wv.Int * @ScalingModifier as IntScaling, 
wv.Fth * @ScalingModifier as FthScaling, 
wv.Lck * @ScalingModifier as LckScaling, 
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
