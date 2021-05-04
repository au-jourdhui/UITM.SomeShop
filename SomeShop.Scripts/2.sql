use SomeShopDatabase;
go

if object_id(N'dbo.GetItemsInfo') is null
  exec('create procedure dbo.GetItemsInfo as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetItemsInfo null, null, null, 5
-- ============================================================================

alter procedure dbo.GetItemsInfo
(
  @count int = 8,
  @newItems bit = 0,
  @saleItems bit = 0,
  @userId int = null
)
as  
begin  
  if @count is not null
    select top (@count) 
      p.Id as ProductId
    , i.Id as ItemId
    , p.BrandId
    , p.Name
    , p.Description
    , i.Price
    , i.OldPrice
    , pimg.Image
    , case 
        when isnull(fp.IsDeleted, 1) = 1 then 0
        else 1
      end as Favourite
      from dbo.Products p
      left join dbo.FavouriteProducts fp on fp.ProductId = p.Id and fp.UserId = @userId
      join (select i1.* 
              from dbo.Items i1
              join (select min(Id) as Id
                         , min(Price) as Price 
                         from dbo.Items
                         where IsDeleted = 0
                         group by ProductId) i2 
              on i1.Id = i2.Id) i 
            on i.ProductId = p.Id
      outer apply
      (
        select top 1 * 
          from dbo.ProductImages pim where pim.ProductId = p.Id
                                       and pim.IsDeleted = 0
      ) pimg
      where p.IsDeleted = 0 
        and i.IsDeleted = 0
        and (isnull(@saleItems, 0) = 0
          or (@saleItems = 1 
          and i.OldPrice is not null))
      order by
        case when exists (select 1 from Brands b where b.Id = p.BrandId and b.Name = 'TianDe') then 1 end desc
      , case when isnull(@newItems, 0) = 1 then p.DateCreated end desc
      , newid()
  else
    select
      p.Id as ProductId
    , i.Id as ItemId
    , p.Name
    , p.Description
    , i.Price
    , i.OldPrice
    , pimg.Image
    , case 
        when isnull(fp.IsDeleted, 1) = 1 then 0
        else 1
      end as Favourite
      from dbo.Products p
      left join dbo.FavouriteProducts fp on fp.ProductId = p.Id and fp.UserId = @userId
      join (select i1.* 
              from dbo.Items i1
              join (select min(Id) as Id
                         , min(Price) as Price 
                         from dbo.Items 
                         where IsDeleted = 0
                         group by ProductId) i2 
              on i1.Id = i2.Id) i 
            on i.ProductId = p.Id
      outer apply
      (
        select top 1 * 
          from dbo.ProductImages pim where pim.ProductId = p.Id
                                       and pim.IsDeleted = 0
      ) pimg
      where p.IsDeleted = 0 
        and i.IsDeleted = 0
        and (isnull(@saleItems, 0) = 0
          or (@saleItems = 1 
          and i.OldPrice is not null))
      order by
        case when exists (select 1 from Brands b where b.Id = p.BrandId and b.Name = 'TianDe') then 1 end desc
      , case when isnull(@newItems, 0) = 1 then p.DateCreated end desc
end;
go


if object_id(N'dbo.CategoriesForProduct') is null
  exec('create procedure dbo.CategoriesForProduct as set nocount on;');
go
 
 -- ============================================================================
 -- Example    : exec dbo.CategoriesForProduct 8
 -- ============================================================================
 
alter procedure dbo.CategoriesForProduct
(  
    @productId as int = null
)  
as  
begin

;with CategoryParents as 
(
  select *
    from Categories
    where Id in (
                  select c.Id
                    from Categories c
                    join CategoryProducts cp on cp.CategoryId = c.Id
                    join Products p on p.Id = cp.ProductId
                    where p.Id = @ProductId 
                      and c.IsDeleted = 0
                      and cp.IsDeleted = 0
                      and p.IsDeleted = 0
                )
  union all
  select c.*
    from Categories c
    inner join CategoryParents cp on cp.ParentId = c.Id
)
select distinct *
  from CategoryParents
  order by Id
 
end;
go

if object_id(N'dbo.GetProductImages') is null
  exec('create procedure dbo.GetProductImages as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetProductImages 3
-- ============================================================================

alter procedure dbo.GetProductImages
(  
    @productId as int = null
)  
as  
begin  
  
  select pim.* 
    from Products p
    join ProductImages pim on pim.ProductId = p.Id
    where p.Id = @productId
      and p.IsDeleted = 0
      and pim.IsDeleted = 0

end;
go

if object_id(N'dbo.GetItemWithParameters') is null
  exec('create procedure dbo.GetItemWithParameters as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetItemWithParameters 7
-- ============================================================================

alter procedure dbo.GetItemWithParameters 
(  
    @productId as int
)  
as  
begin  
  
  select i.Id as ItemId
       , i.Price
       , i.OldPrice
       , params.Parameters
    from Products p
    join Items i on i.ProductId = p.Id
  cross apply
  (
    select top 1 string_agg(concat(op.Name, ' - ', opp.Value), ', ') as Parameters
      from OptionalParameterProducts opp
      join OptionalParameters op on op.Id = opp.OptionalParameterId
      where opp.ItemId = i.Id
        and opp.IsDeleted = 0
        and op.IsDeleted = 0
  ) params
    where p.Id = @productId
      and p.IsDeleted = 0
      and i.IsDeleted = 0
    order by i.Price

end;
go

if (object_ID('dbo.MD5HashPassword') is not null)
   drop function dbo.MD5HashPassword
go

-- ============================================================================
-- Example    : select dbo.MD5HashPassword('qwe')
-- ============================================================================

create function dbo.MD5HashPassword(@password nvarchar(100))
returns char(32)
as 
begin 
  return convert(varchar(32), hashbytes('MD5', @password), 2) 
end
go

if (object_ID('dbo.GetUserRoleId') is not null)
   drop function dbo.GetUserRoleId
go

-- ============================================================================
-- Example    : select dbo.GetUserRoleId(N'User')
-- ============================================================================

create function dbo.GetUserRoleId(@role nvarchar(100) null)
returns int
as 
begin 
  return (select top 1 r.Id from dbo.Roles r where r.Name = @role)
end
go



if object_id(N'dbo.GetCategoryHierarchy') is null
  exec('create procedure dbo.GetCategoryHierarchy as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetCategoryHierarchy 2
-- ============================================================================

alter procedure dbo.GetCategoryHierarchy 
(  
    @toLevelId as int = null
)  
as  
begin
 
  ;with CategoryHierarchy as 
  (
    select *
      , 0 as Level
      , cast(row_number() over(partition by ParentId order by Name) as varchar(max)) as Path 
      , case
          when exists (select * from Categories where ParentId = par.Id)
          then 1
          else 0
        end as IsParent
      from Categories par
      where ParentId is null
    union all
    select c.*
      , Level + 1
      , Path + cast(row_number() over(partition by c.ParentId order by c.Name) as varchar(max))
      , case
          when exists (select * from Categories where ParentId = c.Id)
          then 1
          else 0
        end as IsParent
      from Categories c
      inner join CategoryHierarchy ch on ch.Id = c.ParentId
  )
  select ch.Id
       , ch.Name
       , ch.ParentId
       , ch.DateCreated
       , ch.DateModified
       , ch.IsDeleted
       , ch.Level
       , ch.IsParent
    from CategoryHierarchy ch
    where (@toLevelId is null or ch.Level <= @toLevelId)
    order by Path

end;
go

if object_id(N'dbo.GetFavouriteProductInfo') is null
  exec('create procedure dbo.GetFavouriteProductInfo as set nocount on;');
go
 
 -- ============================================================================
 -- Example    : exec dbo.GetFavouriteProductInfo 5
 -- ============================================================================
 
alter procedure dbo.GetFavouriteProductInfo
(  
  @userId int = null
)  
as  
begin

  select
        p.Id as ProductId
      , i.Id as ItemId
      , p.Name
      , p.Description
      , i.Price
      , i.OldPrice
      , pimg.Image
      , 1 as Favourite
        from dbo.Products p
        join dbo.FavouriteProducts fp on fp.ProductId = p.Id and fp.UserId = @userId
        join (select i1.* 
                from dbo.Items i1
                join (select min(Id) as Id
                           , min(Price) as Price 
                           from dbo.Items 
                           group by ProductId) i2 
                on i1.Id = i2.Id) i 
              on i.ProductId = p.Id
        outer apply
        (
          select top 1 * 
            from dbo.ProductImages pim where pim.ProductId = p.Id
                                         and pim.IsDeleted = 0
        ) pimg
        where p.IsDeleted = 0 
          and i.IsDeleted = 0
          and fp.IsDeleted = 0
 
end;
go

if (object_ID('dbo.GetCategoryForProducts') is not null)
   drop function dbo.GetCategoryForProducts
go

-- ============================================================================
-- Example    : select * from dbo.GetCategoryForProducts(1)
-- ============================================================================
create function dbo.GetCategoryForProducts(@productId as int) 
    returns @categories table
    (
        Id int not null
      , Name nvarchar(100)
      , ParentId int
      , DateCreated datetime
      , DateModified datetime
      , IsDeleted bit
    )
as
begin
  ;with CategoryParents as 
  (
    select *
      from Categories
      where Id in (
                    select c.Id
                      from Categories c
                      join CategoryProducts cp on cp.CategoryId = c.Id
                      join Products p on p.Id = cp.ProductId
                      where p.Id = @ProductId 
                        and c.IsDeleted = 0
                        and cp.IsDeleted = 0
                        and p.IsDeleted = 0
                  )
    union all
    select c.*
      from Categories c
      inner join CategoryParents cp on cp.ParentId = c.Id
  )
  insert into @categories
    select distinct *
      from CategoryParents
      order by Id
      option (maxrecursion 0)

  return
end
go

if object_id(N'dbo.SearchItemInfo') is null
  exec('create procedure dbo.SearchItemInfo as set nocount on;');
go
 
 -- ============================================================================
 -- Example    : exec dbo.SearchItemInfo 1, 4, 2, N'Balm', 140, 200; exec dbo.SearchItemInfo null, null, null, N'Milk'
 -- ============================================================================
 
alter procedure dbo.SearchItemInfo
(  
  @userId int = null,
  @categoryId int = null,
  @brandId int = null,
  @search nvarchar(100) = null,
  @minPrice float = null,
  @maxPrice float = null
)  
as  
begin

  select
      p.Id as ProductId
    , i.Id as ItemId
    , p.Name
    , p.Description
    , i.Price
    , i.OldPrice
    , pimg.Image
    , case 
        when isnull(fp.IsDeleted, 1) = 1 then 0
        else 1
      end as Favourite
      from dbo.Products p
      left join dbo.FavouriteProducts fp on fp.ProductId = p.Id and fp.UserId = @userId
      join (select i1.* 
              from dbo.Items i1
              join (select min(Id) as Id
                         , min(Price) as Price 
                         from dbo.Items
                         where IsDeleted = 0
                         group by ProductId) i2 
              on i1.Id = i2.Id) i 
            on i.ProductId = p.Id
      outer apply
      (
        select top 1 * 
          from dbo.ProductImages pim where pim.ProductId = p.Id
                                       and pim.IsDeleted = 0
      ) pimg
      outer apply
      (
        select cpp.* 
          from dbo.GetCategoryForProducts(p.Id) as cpp
          where cpp.Id = @categoryId
      ) cfp
      where p.IsDeleted = 0 
        and i.IsDeleted = 0
        and (@categoryId is null or cfp.Id is not null) -- Filter by category 
        and (@brandId is null or p.BrandId = @brandId)  -- Filter by brand
        and (@search is null 
             or exists(
                        select *
                          from string_split(@search, ' ') es
                          left join Products ep on ep.Id = p.Id
                          left join Brands eb on eb.Id = ep.BrandId
                          outer apply
                          (
                            select cpp.* 
                              from dbo.GetCategoryForProducts(p.Id) as cpp
                          ) ecfp
                          where ep.Name like concat('%', es.value, '%')
                             or eb.Name like concat('%', es.value, '%')
                             or ecfp.Name like concat('%', es.value, '%')
                      )
            )                                           -- Filter by search
        and (@minPrice is null or i.Price >= @minPrice)
        and (@maxPrice is null or i.Price <= @maxPrice)
      order by
        case when exists (select 1 from Brands b where b.Id = p.BrandId and b.Name = 'TianDe') then 1 end desc
      , p.DateCreated
 
end;
go

if (object_ID('dbo.GetProductNumericValueByOption') is not null)
   drop function dbo.GetProductNumericValueByOption
go

-- ============================================================================
-- Example    : select dbo.GetProductNumericValueByOption('MinPrice')
-- ============================================================================

create function dbo.GetProductNumericValueByOption(@option char(10))
returns float
as 
begin
  declare @value float = null;
  if (@option = 'MinPrice')
    select @value = min(i.Price) from dbo.Items i
  if (@option = 'MaxPrice')
    select @value = max(i.Price) from dbo.Items i
  return @value;  
end
go

if object_id(N'dbo.GetCartItemsInfo') is null
  exec('create procedure dbo.GetCartItemsInfo as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetCartItemsInfo 1
-- ============================================================================

alter procedure dbo.GetCartItemsInfo 
(  
    @userId as int
)  
as  
begin

  select
      p.Id as ProductId
    , i.Id as ItemId
    , concat(p.Name, iif(datalength(chars.CharLine) > 0, concat(' | (', chars.CharLine, ')'), '')) as Name
    , round(i.Price, 2) as Price
    , c.Quantity
    , round(i.Price * c.Quantity, 2) as Total
    , pimg.Image
      from dbo.Cart c
      join dbo.Items i on i.Id = c.ItemId
      join dbo.Products p on p.Id = i.ProductId
      outer apply
      (
        select top 1 * 
          from dbo.ProductImages pim where pim.ProductId = p.Id
                                       and pim.IsDeleted = 0
      ) pimg
      outer apply 
      (
        select string_agg(concat(op.Name, ': ', opp.Value), ', ') as CharLine
          from OptionalParameterProducts opp
          join OptionalParameters op on op.Id = opp.OptionalParameterId
          where opp.ItemId = i.Id
            and op.IsDeleted = 0
            and opp.IsDeleted = 0
            and i.IsDeleted = 0
      ) chars
      where c.UserId = @userId
        and c.IsDeleted = 0
        and p.IsDeleted = 0 
        and i.IsDeleted = 0

end;
go

if object_id(N'dbo.MergeItemToCart') is null
  exec('create procedure dbo.MergeItemToCart as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.MergeItemToCart 1, 7, 2
-- ============================================================================

alter procedure dbo.MergeItemToCart 
(  
    @userId as int
  , @itemId as int
  , @quantity as int = null
  , @isDeleted as bit = 0
)  
as  
begin

  merge dbo.Cart as trg
    using 
    (
      select @itemId
           , @userId
           , @quantity
           , @isDeleted
    ) as src (ItemId, UserId, Quantity, IsDeleted)
    on (trg.ItemId = src.ItemId and trg.UserId = src.UserId)
    when matched then
      update set trg.Quantity = iif(trg.IsDeleted = 0, iif(src.Quantity is null, trg.Quantity + 1, src.Quantity), iif(src.Quantity is null, 1, src.Quantity)) 
               , trg.IsDeleted = src.IsDeleted
               , trg.DateModified = getdate()
    when not matched then
      insert (ItemId, UserId, Quantity)
      values (src.ItemId, src.UserId, iif(src.Quantity is not null and src.Quantity > 0, src.Quantity, 1))
    ;

    select top 1
      p.Id as ProductId
    , i.Id as ItemId
    , concat(p.Name, iif(datalength(chars.CharLine) > 0, concat(' | (', chars.CharLine, ')'), '')) as Name
    , round(i.Price, 2) as Price
    , c.Quantity
    , round(i.Price * c.Quantity, 2) as Total
    , pimg.Image
    , summary.Sum
      from dbo.Cart c
      join dbo.Items i on i.Id = c.ItemId
      join dbo.Products p on p.Id = i.ProductId
      outer apply
      (
        select top 1 * 
          from dbo.ProductImages pim where pim.ProductId = p.Id
                                       and pim.IsDeleted = 0
      ) pimg
      outer apply
      (
        select sum(c.Quantity * i.Price) as Sum
          from dbo.Cart c
          join dbo.Items i on i.Id = c.ItemId
          where c.UserId = 1
            and c.IsDeleted = 0
            and i.IsDeleted = 0
      ) summary
      outer apply 
      (
        select string_agg(concat(ch.Name, ': ', chp.Value), ', ') as CharLine
          from CharacteristicProducts chp
          join Characteristics ch on ch.Id = chp.CharacteristicId
          where chp.ProductId = p.Id
            and ch.IsDeleted = 0
            and chp.IsDeleted = 0
      ) chars
      where c.UserId = @userId
        and c.ItemId = @itemId
        and c.IsDeleted = 0
        and p.IsDeleted = 0 
        and i.IsDeleted = 0

end;
go

if object_id(N'dbo.CreateOrder') is null
  exec('create procedure dbo.CreateOrder as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.CreateOrder 1
-- ============================================================================

alter procedure dbo.CreateOrder 
(  
    @userId as int
  , @deliveryTypeId as int
  , @paymentTypeId as int
  , @email nvarchar(100)
  , @firstName nvarchar(200)
  , @lastName nvarchar(200) 
  , @phone char(13)
  , @address nvarchar(200)
  , @city nvarchar(100)
  , @region nvarchar(150)
)  
as  
begin

  begin transaction;
  begin try
    
    insert Orders
    (
        UserId
      , OrderStatus
      , OrderDeliveryType
      , OrderPaymentType
      , Email
      , FirstName
      , LastName
      , Phone
      , Address
      , City
      , Region
    )
    values
    (
        @userId
      , (select top 1 os.Id from dbo.OrderStatuses os where Status = N'In progress')
      , @deliveryTypeId
      , @paymentTypeId
      , @email
      , @firstName
      , @lastName
      , @phone
      , @address
      , @city
      , @region
    )

    declare @orderId int = SCOPE_IDENTITY();

    merge dbo.OrderProducts as trg
      using 
      (
        select @orderId
             , c.ItemId
             , c.Quantity
             , i.Price
          from dbo.Cart c
          join dbo.Items i on i.Id = c.ItemId
          where c.UserId = @userId
      ) as src (OrderId, ItemId, Quantity, Price)
      on (trg.ItemId = src.ItemId and trg.OrderId = src.OrderId)
      when not matched then
        insert (OrderId, ItemId, Quantity, Price)
        values (src.OrderId, src.ItemId, src.Quantity, src.Price)
       ;
    
    delete dbo.Cart
    where UserId = @userId

    commit transaction;
  end try
  begin catch
    rollback transaction;
    throw;
  end catch

end;
go

if object_id(N'dbo.GetOrderDetails') is null
  exec('create procedure dbo.GetOrderDetails as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetOrderDetails 1
-- ============================================================================

alter procedure dbo.GetOrderDetails 
(  
    @userId as int
)  
as  
begin

  select o.Id as OrderId
       , concat(u.FirstName, ' ', u.LastName) as Name
       , o.DateCreated as Date
       , os.Status as Status
       , odt.Type as DeliveryType
       , opt.Type as PaymentType
       , (select top 1 sum(Quantity * Price) from OrderProducts op where OrderId = o.Id and op.IsDeleted = 0) as Total
    from dbo.Orders o
    join dbo.Users u on u.Id = o.UserId
    join dbo.OrderDeliveryTypes odt on odt.Id = o.OrderDeliveryType
    join dbo.OrderPaymentTypes opt on opt.Id = o.OrderPaymentType
    join dbo.OrderStatuses os on os.Id = o.OrderStatus
    where u.Id = @userId

end;
go

if object_id(N'dbo.GetOrderProductsInfo') is null
  exec('create procedure dbo.GetOrderProductsInfo as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetOrderProductsInfo 1, 1
-- ============================================================================

alter procedure dbo.GetOrderProductsInfo 
(  
    @userId as int
  , @orderId as int
)  
as  
begin

  select i.Id as ItemId
       , p.Id as ProductId
       , pimg.Image as Image
       , concat(p.Name, iif(datalength(chars.CharLine) > 0, concat(' | (', chars.CharLine, ')'), '')) as Name
       , op.Price as Price
       , op.Quantity as Quantity
       , op.Price * op.Quantity as Total
    from dbo.Orders o
    join dbo.Users u on u.Id = o.UserId
    join dbo.OrderProducts op on op.OrderId = o.Id
    join dbo.Items i on i.Id = op.ItemId
    join dbo.Products p on p.Id = i.ProductId
    outer apply 
    (
      select top 1 *
        from dbo.ProductImages pi
        where pi.ProductId = p.Id
          and pi.IsDeleted = 0
    ) pimg
    outer apply 
    (
      select string_agg(concat(ch.Name, ': ', chp.Value), ', ') as CharLine
        from CharacteristicProducts chp
        join Characteristics ch on ch.Id = chp.CharacteristicId
        where chp.ProductId = p.Id
          and ch.IsDeleted = 0
          and chp.IsDeleted = 0
    ) chars
    where o.Id = @orderId
      and o.UserId = @userId
      and o.IsDeleted = 0
      and u.IsDeleted = 0
      and op.IsDeleted = 0
      and i.IsDeleted = 0
      and p.IsDeleted = 0

end;
go

if object_id(N'dbo.GetProductsInfo_Admin') is null
  exec('create procedure dbo.GetProductsInfo_Admin as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetProductsInfo_Admin N'Cream'
-- ============================================================================

alter procedure dbo.GetProductsInfo_Admin 
(  
    @search as nvarchar(100) = null
)  
as  
begin

  declare @trimLength int = 30;
  
  select p.Id as ProductId
       , p.Name as ProductName
       , b.Name as BrandName
       , iif(
                len(cat.CategoriesString) > @trimLength - 3
              , concat(rtrim(substring(cat.CategoriesString, 0, @trimLength)), '...')
              , cat.CategoriesString
            ) as Categories
       , p.IsDeleted as IsDeleted
    from Products p
    join Brands b on b.Id = p.BrandId
    outer apply 
    (
      select string_agg(c.Name, ', ') as CategoriesString
        from Categories c
        join CategoryProducts cp on cp.CategoryId = c.Id
        where cp.ProductId = p.Id
          and cp.IsDeleted = 0
          and c.IsDeleted = 0
    ) cat
    where 1=1
          and (@search is null 
               or exists(
                          select *
                            from string_split(@search, ' ') s
                            outer apply
                            (
                              select cpp.* 
                                from dbo.GetCategoryForProducts(p.Id) as cpp
                            ) cfp
                            where p.Id = try_cast(s.value as int)
                                  or (
                                        s.value is not null
                                    and (
                                             p.Name like concat('%', s.value, '%')
                                          or b.Name like concat('%', s.value, '%')
                                          or cfp.Name like concat('%', s.value, '%')
                                        )
                                     )
                        )
              )
    order by p.Id desc
      
end;
go

if object_id(N'dbo.GetOptionalParametersForProduct_Admin') is null
  exec('create procedure dbo.GetOptionalParametersForProduct_Admin as set nocount on;');
go
  
-- ============================================================================  
-- Example    : exec dbo.GetOptionalParametersForProduct_Admin 3  
-- ============================================================================  
  
alter procedure dbo.GetOptionalParametersForProduct_Admin   
(    
    @productId as int = null  
)    
as    
begin  
    
  select distinct op.*   
    from Items i  
    join OptionalParameterProducts opp on i.Id = opp.ItemId  
    join OptionalParameters op on opp.OptionalParameterId = op.Id  
    where 1=iif(@productId is null, 0, 1)  
      and i.ProductId = @productId
      and op.IsDeleted = 0
      and opp.IsDeleted = 0
        
end;
go

if object_id(N'dbo.GetOptionalParametersForItem_Admin') is null
  exec('create procedure dbo.GetOptionalParametersForItem_Admin as set nocount on;');
go
  
-- ============================================================================  
-- Example    : exec dbo.GetOptionalParametersForItem_Admin 2  
-- ============================================================================  
  
alter procedure dbo.GetOptionalParametersForItem_Admin
(    
    @itemId as int = null  
)    
as    
begin  
    
  select @itemId as ItemId
       , op.Id as OptionalParameterId
       , opp.Id as OptionalParameterProductId
       , op.Name as OptionalParameterName
       , opp.Value as OptionalParameterValue
       , op.IsDeleted as IsDeletedOptionalParameter
       , opp.IsDeleted as IsDeletedOptionalParameterProduct
    from Items i  
    join OptionalParameterProducts opp on i.Id = opp.ItemId  
    join OptionalParameters op on opp.OptionalParameterId = op.Id  
    where 1=iif(@itemId is null, 0, 1)  
      and i.Id = @itemId
      and i.IsDeleted = 0
      and opp.IsDeleted = 0
      and op.IsDeleted = 0
        
end; 
go


if object_id(N'dbo.GetOrdersInfo_Admin') is null
  exec('create procedure dbo.GetOrdersInfo_Admin as set nocount on;');
go

-- ============================================================================
-- Example    : exec dbo.GetOrdersInfo_Admin N'Alex'
-- ============================================================================

alter procedure dbo.GetOrdersInfo_Admin 
(  
    @search as nvarchar(100) = null
)  
as  
begin

  declare @trimLength int = 30;
  
  select o.Id as OrderId
       , ltrim(rtrim(concat(o.FirstName, ' ', o.LastName))) as Customer
       , (
            select count(op.Quantity) 
              from OrderProducts op 
              where op.IsDeleted = 0 
                and op.OrderId = o.Id
          ) as ItemsCount
       , (
            select round(sum(op.Quantity * op.Price), 2) 
              from OrderProducts op 
              where op.IsDeleted = 0 
                and op.OrderId = o.Id
          ) as TotalSum
       , o.City as OrderCity
       , os.Status as Status
       , odt.Type as OrderDeliveryType
       , opt.Type as OrderPaymentType
       , o.DateCreated as DateCreated
       , o.DateModified as DateModified
       , o.IsDeleted as IsDeleted
    from Orders o
    join OrderStatuses os on o.OrderStatus = os.Id
    join OrderDeliveryTypes odt on o.OrderDeliveryType = odt.Id
    join OrderPaymentTypes opt on o.OrderPaymentType = opt.Id
    where 1=1
          and (@search is null 
               or exists(
                          select *
                            from string_split(@search, ' ') s
                            where o.Id = try_cast(s.value as int)
                                  or (
                                        try_cast(s.value as int) is null
                                    and (
                                             o.FirstName like concat('%', s.value, '%')
                                          or o.LastName like concat('%', s.value, '%')
                                          or o.City like concat('%', s.value, '%')
                                          or o.Region like concat('%', s.value, '%')
                                          or o.Address like concat('%', s.value, '%')
                                          or o.Email like concat('%', s.value, '%')
                                          or o.Phone like concat('%', s.value, '%')
                                          or odt.Type like concat('%', s.value, '%')
                                          or opt.Type like concat('%', s.value, '%')
                                          or os.Status like concat('%', s.value, '%')
                                        )
                                     )
                        )
              )
    order by o.Id desc
      
end;
go

if (object_ID('dbo.GetCartSum') is not null)
   drop function dbo.GetCartSum
go
-- ============================================================================
-- Example    : select dbo.GetCartSum(1)
-- ============================================================================

create function dbo.GetCartSum(@userId int null)
returns float
as 
begin 
  return (
            select top 1 isnull(sum(c.Quantity * i.Price), 0)
              from Cart c
              join Items i on c.ItemId = i.Id
              where c.UserId = @userId 
                and c.IsDeleted = 0
                and i.IsDeleted = 0
         )
end
go

if (object_ID('dbo.GetOrderSum') is not null)
   drop function dbo.GetOrderSum
go
-- ============================================================================
-- Example    : select dbo.GetOrderSum(1)
-- ============================================================================

create function dbo.GetOrderSum(@orderId int null)
returns float
as 
begin 
  return (select top 1 isnull(sum(Quantity * Price), 0) from OrderProducts where OrderId = @orderId and IsDeleted = 0)
end
go