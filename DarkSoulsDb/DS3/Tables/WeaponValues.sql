CREATE TABLE [DS3].[WeaponValues](
	[WeaponId] [int] NOT NULL,
	[InfusionId] [int] NOT NULL,
	[Str] [float] NOT NULL,
	[Dex] [float] NOT NULL,
	[Int] [float] NOT NULL,
	[Faith] [float] NOT NULL,
	[Luck] [float] NOT NULL,
	[Physical] [float] NOT NULL,
	[Magic] [float] NOT NULL,
	[Fire] [float] NOT NULL,
	[Lightning] [float] NOT NULL,
	[Dark] [float] NOT NULL,
	[PhysicalSat] [float] NOT NULL,
	[MagicSat] [float] NOT NULL,
	[FireSat] [float] NOT NULL,
	[LightningSat] [float] NOT NULL,
	[DarkSat] [float] NOT NULL
)
GO

ALTER TABLE [DS3].[WeaponValues]   ADD  CONSTRAINT [FK_WeaponValues_Infusion] FOREIGN KEY([InfusionId])
REFERENCES [DS3].[Infusion] ([InfusionId])
GO

ALTER TABLE [DS3].[WeaponValues] CHECK CONSTRAINT [FK_WeaponValues_Infusion]
GO

ALTER TABLE [DS3].[WeaponValues]   ADD  CONSTRAINT [FK_WeaponValues_Weapon] FOREIGN KEY([WeaponId])
REFERENCES [DS3].[Weapon] ([WeaponId])
GO

ALTER TABLE [DS3].[WeaponValues] CHECK CONSTRAINT [FK_WeaponValues_Weapon]
GO