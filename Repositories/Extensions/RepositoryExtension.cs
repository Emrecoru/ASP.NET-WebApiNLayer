using App.Repositories.Categories;
using App.Repositories.Interceptors;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                var connectionString = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();
                opt.UseSqlServer(connectionString!.Local, options =>
                {
                    options.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.GetName().Name);
                });

                opt.AddInterceptors(new AuditDbContextInterceptor());
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
