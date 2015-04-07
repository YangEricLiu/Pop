/****** Object:  Table [dbo].[Park]    Script Date: 03/31/2015 10:33:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleLineDiagram]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SingleLineDiagram](
	    [Id] [bigint] identity(1,1) NOT NULL,
		[HierarchyId] [bigint] NOT NULL,
		[Key] nvarchar(100) NOT NULL, 
		[Order] int NOT NULL,
		[CreateTime] datetime NOT NULL,
		[CreateUser] nvarchar(100) NOT NULL,
		[UpdateTime] datetime NOT NULL,
		[UpdateUser] nvarchar(100) NOT NULL
	 CONSTRAINT [PK_SingleLineDiagram] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


