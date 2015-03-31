/****** Object:  Table [dbo].[Gateway]    Script Date: 03/31/2015 10:33:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Gateway]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Gateway](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](512) NOT NULL,
		[Mac] [nvarchar](64) NOT NULL,
		[UniqueId] [nvarchar](64) NOT NULL,
		[RegisterTime] [datetime] NOT NULL,
		[HierarchyId] [bigint] NULL,
		[CustomerId] [bigint] NOT NULL,
	 CONSTRAINT [PK_Gateway] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


