CREATE TABLE [DS3].[StartingClass](
	[StartingClassId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Level] [int] NOT NULL,
	[Vigor] [int] NOT NULL,
	[Attunement] [int] NOT NULL,
	[Endurance] [int] NOT NULL,
	[Vitality] [int] NOT NULL,
	[Strength] [int] NOT NULL,
	[Dexterity] [int] NOT NULL,
	[Intelligence] [int] NOT NULL,
	[Faith] [int] NOT NULL,
	[Luck] [int] NOT NULL,
 CONSTRAINT [PK_StartingClass] PRIMARY KEY CLUSTERED 
(
	[StartingClassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO