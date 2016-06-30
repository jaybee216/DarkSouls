-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [DS3].[CalculateWeaponAR]
	@WeaponId int,
	@InfusionId int = 1,
	@ReinforcementLevel int = 10, 
	@STR int = 0,
	@DEX int = 0,
	@INT int = 0,
	@FTH int = 0
AS
BEGIN
SET NOCOUNT ON;

Declare @BasePhys int = (Select [Physical] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);
Declare @BaseFire int = (Select [Fire] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);
Declare @BaseMagic int = (Select [Magic] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);
Declare @BaseLightning int = (Select [Lightning] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);
Declare @BaseDark int = (Select [Dark] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);

Declare @WeaponStrScaling float = (Select [Str] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);
Declare @WeaponDexScaling float = (Select [Dex] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);
Declare @WeaponIntScaling float = (Select [Int] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);
Declare @WeaponFthScaling float = (Select [Fth] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);
Declare @WeaponLckScaling float = (Select [Lck] From [DS3].[WeaponValues] Where [WeaponId] = @WeaponId and InfusionId = @InfusionId);

Declare @StrScaling float = (Select ISNULL([Scalar], 0) From [DS3].[StatScaling] Where [Value] = @STR)
Declare @DexScaling float = (Select ISNULL([Scalar], 0) From [DS3].[StatScaling] Where [Value] = @DEX)
Declare @IntScaling float = (Select ISNULL([Scalar], 0) From [DS3].[StatScaling] Where [Value] = @INT)
Declare @FthScaling float = (Select ISNULL([Scalar], 0) From [DS3].[StatScaling] Where [Value] = @FTH)

Declare @UpgradePathId int = (Select [UpgradePathId] From [DS3].[Weapon] Where [WeaponId] = @WeaponId);

Declare @BaseModifier float = 
CASE WHEN @UpgradePathId = 1 THEN
	(Select [BaseModifier] From [DS3].[Reinforcement] Where [ReinforcementLevel] = @ReinforcementLevel)
WHEN @UpgradePathId = 2 THEN
	(Select [TwinklingBaseModifier] From [DS3].[Reinforcement] Where [ReinforcementLevel] = @ReinforcementLevel)
WHEN @UpgradePathId = 3 THEN
	(Select [ScaleBaseModifier] From [DS3].[Reinforcement] Where [ReinforcementLevel] = @ReinforcementLevel)
WHEN @UpgradePathId is null THEN
	1
END

Declare @ScalingModifier float = (Select [ScalingModifier] From [DS3].[Reinforcement] Where [ReinforcementLevel] = @ReinforcementLevel)

Declare @StrBonus float = (@BasePhys * @BaseModifier * @WeaponStrScaling * @ScalingModifier * @StrScaling) / 100;
Declare @DexBonus float = (@BasePhys * @BaseModifier * @WeaponDexScaling * @ScalingModifier * @DexScaling) / 100;

Declare @FireBonus float = (@BaseFire * @BaseModifier * @WeaponIntScaling * @WeaponFthScaling * @ScalingModifier * @IntScaling) / 100;
Declare @MagicBonus float = (@BaseMagic * @BaseModifier * @WeaponIntScaling * @ScalingModifier * @IntScaling) / 100;

Declare @PhysAR float = (@BasePhys * @BaseModifier) + @StrBonus + @DexBonus;
Declare @FireAR float = (@BaseFire * @BaseModifier);-- + @FireIntBonus + @FireFthBonus;
Declare @MagicAR float = (@BaseMagic * @BaseModifier);-- + @MagicIntBonus + @MagicFthBonus;
Declare @LightningAR float = (@BaseLightning * @BaseModifier);-- + @IntBonus + @FthBonus;
Declare @DarkAR float = (@BaseDark * @BaseModifier);-- + @IntBonus + @FthBonus;

Declare @TotalAR float = @PhysAR + @FireAR + @MagicAR + @LightningAR + @DarkAR;

Declare @WeaponName varchar(50) = CASE WHEN @InfusionId = 1 THEN 
									'' 
								  ELSE
									 (Select Name from DS3.Infusion Where @InfusionId = InfusionId) + ' '
								  END
								   +
								  (Select Name from DS3.Weapon Where @WeaponId = WeaponId) 
								  + 
								  CASE WHEN @UpgradePathId = 1 THEN
									' +' + CONVERT(VARCHAR, @ReinforcementLevel)
								  WHEN @UpgradePathId = 2 OR @UpgradePathId = 3 THEN
									' +' + CONVERT(VARCHAR, (@ReinforcementLevel / 2))
								  WHEN @UpgradePathId is null THEN
									''
								  END

Select 
@WeaponId as WeaponId,
@ReinforcementLevel as ReinforcementLevel,
@WeaponName as WeaponName,
@TotalAR as TotalAR, 
@StrScaling as StrScaling, 
@DexScaling as DexScaling, 
@BaseModifier as BaseModifier, 
@ScalingModifier as ScalingModifier,
@WeaponStrScaling * @ScalingModifier as WpnStrScaling,
@WeaponDexScaling * @ScalingModifier as WpnDexScaling,
@WeaponIntScaling * @ScalingModifier as WpnIntScaling,
@WeaponFthScaling * @ScalingModifier as WpnFthScaling,
--@StrBonus as StrBonus, 
--@DexBonus as DexBonus, 
--@MagicBonus as MagicBonus, 
--@FireBonus as FireBonus, 
@BasePhys * @BaseModifier as BasePhys,
@BaseMagic * @BaseModifier as BaseMagic,
@BaseFire * @BaseModifier as BaseFire,
@BaseLightning * @BaseModifier as BaseLightning,
@BaseDark * @BaseModifier as BaseDark,
@PhysAR as PhysAR, 
@FireAR as FireAR, 
@MagicAR as MagicAR, 
@LightningAR as LightningAR, 
@DarkAR as DarkAR;

END