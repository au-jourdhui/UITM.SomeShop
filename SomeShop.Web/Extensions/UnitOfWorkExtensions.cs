using System;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SomeShop.DAL;

namespace SomeShop.Web.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddSingleton<Func<UnitOfWork>>(_ => () => new UnitOfWork(new SqlConnection(GlobalVariables.ConnectionString)))
                .AddScoped(_ => new UnitOfWork(new SqlConnection(GlobalVariables.ConnectionString)));
        }
    }
}