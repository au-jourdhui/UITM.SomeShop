use SomeShopDatabase;
go

insert Users (Email, Password, FirstName, RoleId)
values ('admin@mail.com', dbo.MD5HashPassword('QwErty123'), N'Administrator', dbo.GetUserRoleId(N'Administrator'))

