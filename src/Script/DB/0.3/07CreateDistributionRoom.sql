/****** Object:  Table [dbo].[Park]    Script Date: 03/31/2015 10:33:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DistributionRoom]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[DistributionRoom](
		[HierarchyId] [bigint] NOT NULL,
		[Location] nvarchar(100) NULL, 
		[Level] int NULL,
		[TransformerVoltage] bigint NULL
	 CONSTRAINT [PK_DistributionRoom] PRIMARY KEY CLUSTERED 
	(
		[HierarchyId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


