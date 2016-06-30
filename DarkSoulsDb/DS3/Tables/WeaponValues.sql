CREATE TABLE [DS3].[WeaponValues](
	[WeaponValuesId] [int] IDENTITY(1,1) NOT NULL,
	[WeaponId] [int] NULL,
	[InfusionId] [int] NULL,
	[Bleed] [float] NULL,
	[Poison] [float] NULL,
	[Frost] [float] NULL,
	[Physical] [int] NULL,
	[Magic] [int] NULL,
	[Fire] [int] NULL,
	[Lightning] [int] NULL,
	[Dark] [int] NULL,
	[Str] [float] NULL,
	[Dex] [float] NULL,
	[Int] [float] NULL,
	[Fth] [float] NULL,
	[Lck] [float] NULL,
 CONSTRAINT [PK_WeaponValues] PRIMARY KEY CLUSTERED 
(
	[WeaponValuesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

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