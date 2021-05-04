if not exists (select name from master.dbo.sysdatabases where name = 'SomeShopDatabase')
  create database SomeShopDatabase;
go

use SomeShopDatabase;
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Brands' 
               and t.schema_id = schema_id('dbo'))
  create table Brands
  (
    Id int not null primary key identity,
    Name nvarchar(100) not null unique,
    Country nvarchar(200) null,
    Logo nvarchar(200) null,
    BackgroundImage nvarchar(200) null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Products' 
               and t.schema_id = schema_id('dbo'))
  create table Products
  (
    Id int not null primary key identity,
    Name nvarchar(100) not null,
    Description nvarchar(max) null,
    BrandId int null foreign key references Brands(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    unique (Name, BrandId)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Items' 
               and t.schema_id = schema_id('dbo'))
  create table Items
  (
    Id int not null primary key identity,
    ProductId int null foreign key references Products(Id),
    Price float not null,
    OldPrice float null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='ProductImages' 
               and t.schema_id = schema_id('dbo'))
  create table ProductImages
  (
    Id int not null primary key identity,
    ProductId int not null foreign key references Products(Id),
    Image nvarchar(200) not null, 
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Categories' 
               and t.schema_id = schema_id('dbo'))
  create table Categories
  (
    Id int not null primary key identity,
    Name nvarchar(100) not null,
    ParentId int null foreign key references Categories(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    unique (Name, ParentId)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='CategoryProducts' 
               and t.schema_id = schema_id('dbo'))
  create table CategoryProducts
  (
    Id int not null primary key identity,
    ProductId int not null foreign key references Products(Id),
    CategoryId int not null foreign key references Categories(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)

    unique (ProductId, CategoryId)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Characteristics' 
               and t.schema_id = schema_id('dbo'))
  create table Characteristics
  (
    Id int not null primary key identity,
    Name nvarchar(100) not null unique,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='CharacteristicProducts' 
               and t.schema_id = schema_id('dbo'))
  create table CharacteristicProducts
  (
    Id int not null primary key identity,
    ProductId int not null foreign key references Products(Id),
    CharacteristicId int not null foreign key references Characteristics(Id),
    Value nvarchar(200) not null default (N'—'),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='OptionalParameters' 
               and t.schema_id = schema_id('dbo'))
  create table OptionalParameters
  (
    Id int not null primary key identity,
    Name nvarchar(100) not null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='OptionalParameterProducts' 
               and t.schema_id = schema_id('dbo'))
  create table OptionalParameterProducts
  (
    Id int not null primary key identity,
    ItemId int not null foreign key references Items(Id),
    OptionalParameterId int not null foreign key references OptionalParameters(Id),
    Value nvarchar(200) not null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Roles' 
               and t.schema_id = schema_id('dbo'))
  create table Roles
  (
    Id int not null primary key identity,
    Name nvarchar(50) not null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Users' 
               and t.schema_id = schema_id('dbo'))
  create table Users
  (
    Id int not null primary key identity,
    Email nvarchar(100) not null,
    Password char(32) not null,
    Token uniqueidentifier not null default(newid()),
    FirstName nvarchar(200) not null,
    LastName nvarchar(200) null,
    Phone char(13) null,
    Birthdate date null,
    RoleId int not null foreign key references Roles(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Cart' 
               and t.schema_id = schema_id('dbo'))
  create table Cart
  (
    Id int not null primary key identity,
    UserId int not null foreign key references Users(Id),
    ItemId int not null foreign key references Items(Id),
    Quantity int not null check (Quantity > 0),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),

    unique (UserId, ItemId)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='OrderStatuses' 
               and t.schema_id = schema_id('dbo'))
  create table OrderStatuses
  (
    Id int not null primary key identity,
    Status nvarchar(100) not null unique,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='OrderDeliveryTypes' 
               and t.schema_id = schema_id('dbo'))
  create table OrderDeliveryTypes
  (
    Id int not null primary key identity,
    Type nvarchar(100) not null unique,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='OrderPaymentTypes' 
               and t.schema_id = schema_id('dbo'))
  create table OrderPaymentTypes
  (
    Id int not null primary key identity,
    Type nvarchar(100) not null unique,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='Orders' 
               and t.schema_id = schema_id('dbo'))
  create table Orders
  (
    Id int not null primary key identity,
    UserId int not null foreign key references Users(Id),
    OrderStatus int not null foreign key references OrderStatuses(Id),
    OrderDeliveryType int not null foreign key references OrderDeliveryTypes(Id),
    OrderPaymentType int not null foreign key references OrderPaymentTypes(Id),
    Email nvarchar(100) not null,
    FirstName nvarchar(200) not null,
    LastName nvarchar(200) null,
    Phone char(13) null,
    Address nvarchar(200) not null,
    City nvarchar(100) not null,
    Region nvarchar(150) not null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='OrderProducts' 
               and t.schema_id = schema_id('dbo'))
  create table OrderProducts
  (
    Id int not null primary key identity,
    OrderId int not null foreign key references Orders(Id),
    ItemId int not null foreign key references Items(Id),
    Quantity int not null check (Quantity > 0),
    Price float not null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),

    unique (OrderId, ItemId)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='FavouriteProducts' 
               and t.schema_id = schema_id('dbo'))
  create table FavouriteProducts
  (
    Id int not null primary key identity,
    UserId int not null foreign key references Users(Id),
    ProductId int not null foreign key references Products(Id),
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),

    unique (UserId, ProductId)
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='SupportMessages' 
               and t.schema_id = schema_id('dbo'))
  create table SupportMessages
  (
    Id int not null primary key identity,
    Name nvarchar(200) not null,
    Email nvarchar(100) not null,
    Message nvarchar(1024) not null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),
  );
go

if not exists (select 1 
               from sys.tables t 
               where t.name='ContactInfo' 
               and t.schema_id = schema_id('dbo'))
  create table ContactInfo
  (
    Id int not null primary key identity,
    City nvarchar(50) not null,
    Address nvarchar(100) not null,
    Phones nvarchar(100) null,
    TimeTable nvarchar(1024) null,
    GoogleUrl nvarchar(1024) null,
    DateCreated datetime not null default(getdate()),
    DateModified datetime not null default(getdate()),
    IsDeleted bit not null default(0),
  );
go