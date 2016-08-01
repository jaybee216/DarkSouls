CREATE PROCEDURE [DS3].[CalculateWeaponAR]
	@WeaponId int,
	@InfusionId int = 1,
	@ReinforcementLevel int = 10, 
	@STR int = 0,
	@DEX int = 0,
	@INT int = 0,
	@FTH int = 0,
	@LCK int = 0
AS
BEGIN
SET NOCOUNT ON;

Declare @PhysicalSat int;
Declare @MagicSat int;
Declare @FireSat int;
Declare @LightningSat int;
Declare @DarkSat int;
Declare @Physical float;
Declare @Magic float;
Declare @Fire float;
Declare @Lightning float;
Declare @Dark float;
Declare @StrScaling float;
Declare @DexScaling float;
Declare @IntScaling float;
Declare @FthScaling float;
Declare @LckScaling float;

Select
@PhysicalSat = PhysicalSat,
@MagicSat = MagicSat,
@FireSat = FireSat,
@LightningSat = LightningSat,
@DarkSat = DarkSat,
@Physical = Physical,
@Magic = Magic,
@Fire = Fire,
@Lightning = Lightning,
@Dark = Dark,
@StrScaling = Str,
@DexScaling = Dex,
@IntScaling = Int,
@FthScaling = Faith,
@LckScaling = Luck
From DS3.WeaponValues Where WeaponId = @WeaponId and InfusionId = @InfusionId

Declare @StrCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @PhysicalSat and [Level] = @STR);
Declare @DexCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @PhysicalSat and [Level] = @DEX);
Declare @LuckCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @PhysicalSat and [Level] = @LCK);

Declare @MagicCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @MagicSat and [Level] = @INT);

Declare @FireIntCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @FireSat and [Level] = @INT);
Declare @FireFthCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @FireSat and [Level] = @FTH);

Declare @LightningCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @LightningSat and [Level] = @FTH);

Declare @DarkIntCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @DarkSat and [Level] = @INT);
Declare @DarkFthCoef float = (Select Value From DS3.SaturationValues Where SaturationId = @DarkSat and [Level] = @FTH);

Declare @TotalPhys float =  
CASE 
	WHEN 
		@WeaponId IN (105, 24, 119, 66, 67) OR @InfusionId = 2 --Blessed Weapons, Anri’s Straight Sword, Saint Bident, Lothric’s Holy Sword, Wolnir’s Holy Blade, and Morne’s Great Hammer also receive physical damage from Faith scaling
	THEN
		@Physical + @Physical * (@StrScaling * @StrCoef + @DexScaling * @DexCoef + @LckScaling * @LuckCoef + @FthScaling * @LightningCoef)
	WHEN
		@WeaponId IN (150) --Golden Ritual Spear
	THEN
		@Physical + @Physical * (@StrScaling * @StrCoef + @DexScaling * @DexCoef + @LckScaling * @LuckCoef + @FthScaling * @LightningCoef)
	ELSE 
		@Physical + @Physical * (@StrScaling * @StrCoef + @DexScaling * @DexCoef + @LckScaling * @LuckCoef)
END

Declare @TotalMagic float = 
CASE 
	WHEN 
		@WeaponId IN (150) --Golden Ritual Spear
	THEN
		@Magic + @Magic * (@FthScaling * @LightningCoef)
	ELSE
		@Magic + @Magic * (@IntScaling * @MagicCoef)
END

Declare @TotalFire float = @Fire + @Fire * (@IntScaling * @FireIntCoef + @FthScaling * @FireFthCoef)

Declare @TotalLightning float = @Lightning + @Lightning * (@FthScaling * @LightningCoef)

Declare @TotalDark float = @Dark + @Dark * (@IntScaling * @DarkIntCoef + @FthScaling * @DarkFthCoef)

Declare @UpgradePathId int = (Select [UpgradePathId] From [DS3].[Weapon] Where [WeaponId] = @WeaponId);

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
@TotalPhys as PhysAR, 
@TotalMagic as MagicAR, 
@TotalFire as FireAR, 
@TotalLightning as LightningAR, 
@TotalDark as DarkAR,
--@StrCoef as StrCoef, @DexCoef as DexCoef, @LuckCoef as LckCoef, 
--@MagicCoef as MagicCoef, @FireIntCoef as FireIntCoef, @FireFthCoef as FireFthCoef,
--@LightningCoef as LightningCoef, @DarkIntCoef as DarkIntCoef, @DarkFthCoef as DarkFthCoef,
@TotalPhys + @TotalMagic + @TotalFire + @TotalLightning + @TotalDark as TotalAR

END