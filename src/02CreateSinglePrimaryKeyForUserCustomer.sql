alter table dbo.UserCustomer drop constraint PK_UserCustomer
alter table dbo.UserCustomer add [Id] bigint identity(1,1) primary key