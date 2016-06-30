CREATE TABLE [DS3].[Weapon](
	[WeaponId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[WeaponTypeId] [int] NOT NULL,
	[Weight] [decimal](8, 2) NOT NULL,
	[IsBuffable] [bit] NOT NULL,
	[StrReq] [int] NOT NULL,
	[DexReq] [int] NOT NULL,
	[IntReq] [int] NOT NULL,
	[FthReq] [int] NOT NULL,
	[Critical] [int] NULL,
	[DamageTypeId] [int] NULL,
	[SkillId] [int] NULL,
	[UpgradePathId] [int] NULL,
	[CanInfuse] [bit] NOT NULL CONSTRAINT [DF_Weapon_CanInfuse]  DEFAULT ((0)),
 CONSTRAINT [PK_Weapon] PRIMARY KEY CLUSTERED 
(
	[WeaponId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [DS3].[Weapon]   ADD  CONSTRAINT [FK_Weapon_UpgradePath] FOREIGN KEY([UpgradePathId])
REFERENCES [DS3].[UpgradePath] ([UpgradePathId])
GO

ALTER TABLE [DS3].[Weapon] CHECK CONSTRAINT [FK_Weapon_UpgradePath]
GO

ALTER TABLE [DS3].[Weapon]  ADD  CONSTRAINT [FK_Weapon_WeaponType] FOREIGN KEY([WeaponTypeId])
REFERENCES [DS3].[WeaponType] ([WeaponTypeId])
GO

ALTER TABLE [DS3].[Weapon] CHECK CONSTRAINT [FK_Weapon_WeaponType]
GO