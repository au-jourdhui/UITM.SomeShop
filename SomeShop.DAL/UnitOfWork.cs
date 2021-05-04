using SomeShop.DAL.Dao;
using System;
using System.Data;

namespace SomeShop.DAL
{
    public class UnitOfWork : IDisposable
    {
        private IDbConnection _connection;

        public StoredProcedures StoredProcedures { get; set; }
        public BrandDao Brands { get; set; }
        public CartDao Carts { get; set; }
        public CategoryDao Categories { get; set; }
        public CategoryProductDao CategoryProducts { get; set; }
        public ContactInfoDao ContactInfo { get; set; }
        public CharacteristicDao Characteristics { get; set; }
        public CharacteristicProductDao CharacteristicProducts { get; set; }
        public FavouriteProductDao FavouriteProducts { get; set; }
        public ItemDao Items { get; set; }
        public OptionalParameterDao OptionalParameters { get; set; }
        public OptionalParameterProductDao OptionalParameterProducts { get; set; }
        public OrderDao Orders { get; set; }
        public OrderDeliveryTypeDao OrderDeliveryTypes { get; set; }
        public OrderPaymentTypeDao OrderPaymentTypes { get; set; }
        public OrderProductDao OrderProducts { get; set; }
        public OrderStatusDao OrderStatuses { get; set; }
        public ProductDao Products { get; set; }
        public ProductImageDao ProductImages { get; set; }
        public RoleDao Roles { get; set; }
        public SupportMessageDao SupportMessages { get; set; }
        public UserDao Users { get; set; }

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;

            StoredProcedures = new StoredProcedures(connection);
            Brands = new BrandDao(connection);
            Carts = new CartDao(connection);
            Categories = new CategoryDao(connection);
            CategoryProducts = new CategoryProductDao(connection);
            ContactInfo = new ContactInfoDao(connection);
            Characteristics = new CharacteristicDao(connection);
            CharacteristicProducts = new CharacteristicProductDao(connection);
            FavouriteProducts = new FavouriteProductDao(connection);
            Items = new ItemDao(connection);
            OptionalParameters = new OptionalParameterDao(connection);
            OptionalParameterProducts = new OptionalParameterProductDao(connection);
            Orders = new OrderDao(connection);
            OrderDeliveryTypes = new OrderDeliveryTypeDao(connection);
            OrderPaymentTypes = new OrderPaymentTypeDao(connection);
            OrderProducts = new OrderProductDao(connection);
            OrderStatuses = new OrderStatusDao(connection);
            Products = new ProductDao(connection);
            ProductImages = new ProductImageDao(connection);
            Roles = new RoleDao(connection);
            SupportMessages = new SupportMessageDao(connection);
            Users = new UserDao(connection);
        }

        public void Dispose() => _connection.Dispose();
    }
}
