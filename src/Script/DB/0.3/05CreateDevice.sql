/****** Object:  Table [dbo].[Device]    Script Date: 03/31/2015 10:33:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Device]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Device](
		[HierarchyId] [bigint] NOT NULL,
		[GatewayId] [bigint] NULL,
		[Description] nvarchar(250) NULL,
		[Factory] nvarchar(100) NULL,
	 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
	(
		[HierarchyId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


