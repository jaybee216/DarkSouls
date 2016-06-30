CREATE TABLE [DS3].[Armor](
	[ArmorId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ArmorTypeId] [int] NOT NULL,
	[Weight] [decimal](8, 2) NULL,
	[Durability] [decimal](8, 2) NULL,
	[Poise] [decimal](8, 2) NULL,
	[Poison] [float] NULL,
	[Toxic] [float] NULL,
	[Blood] [float] NULL,
	[Curse] [float] NULL,
	[Frost] [float] NULL,
	[Physical] [float] NULL,
	[Slash] [float] NULL,
	[Strike] [float] NULL,
	[Thrust] [float] NULL,
	[Magic] [float] NULL,
	[Fire] [float] NULL,
	[Lightning] [float] NULL,
	[Dark] [float] NULL,
 CONSTRAINT [PK_Armor] PRIMARY KEY CLUSTERED 
(
	[ArmorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [DS3].[Armor]  WITH CHECK ADD  CONSTRAINT [FK_Armor_ArmorType] FOREIGN KEY([ArmorTypeId])
REFERENCES [DS3].[ArmorType] ([ArmorTypeId])
GO

ALTER TABLE [DS3].[Armor] CHECK CONSTRAINT [FK_Armor_ArmorType]
GO