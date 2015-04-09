/****** Object:  Table [dbo].[Park]    Script Date: 03/31/2015 10:33:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Park]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Park](
		[HierarchyId] [bigint] NOT NULL,
		[FloorSpace] decimal(19,2) NULL,
		[BuildingArea] decimal(19,2) NULL,
	 CONSTRAINT [PK_Park] PRIMARY KEY CLUSTERED 
	(
		[HierarchyId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


