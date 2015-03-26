SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 --- HierarchyAdministrator----
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HierarchyAdministrator]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[HierarchyAdministrator](
	  [Id] [bigint] identity(1,1) NOT NULL,
	  [HierarchyId] bigint NOT NULL,
	  [Name] [nvarchar] (100) NOT NULL,
	  [Title] [nvarchar] (1000) NOT NULL,
	  [Telephone] [nvarchar] (1000) NOT NULL,
	  [Email] [nvarchar] (1000) NOT NULL,
	 CONSTRAINT [PK_HierarchyAdministrator] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 )
END