﻿CREATE TABLE [DS3].[UpgradePath](
	[UpgradePathId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[MaxUpgradeLevel] [int] NOT NULL,
 CONSTRAINT [PK_UpgradePath] PRIMARY KEY CLUSTERED 
(
	[UpgradePathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO