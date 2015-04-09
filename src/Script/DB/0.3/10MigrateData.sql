insert into [dbo].[Park] ([HierarchyId]) select id from dbo.hierarchy where [type] = 1
insert into [dbo].[Building] ([HierarchyId]) select id from dbo.hierarchy where [type] =2
insert into [dbo].[DistributionRoom] ([HierarchyId]) select id from dbo.hierarchy where [type] = 3
insert into [dbo].[DistributionCabinet] ([HierarchyId]) select id from dbo.hierarchy where [type] = 4
insert into [dbo].[Device] ([HierarchyId]) select id from dbo.hierarchy where [type] = 5


