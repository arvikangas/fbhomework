using FuseboxHomework.Core.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FuseboxHomework.Core.Services;
using RestEase.HttpClientFactory;
using FuseboxHomework.Core.EF.Repositories;
using Microsoft.Extensions.Configuration;

namespace FuseboxHomework.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            string connectionString;
            using (var provider = services.BuildServiceProvider())
            {
                var c = provider.GetService<IConfiguration>();
                connectionString = c.GetConnectionString("sqlite");

                if(string.IsNullOrWhiteSpace(connectionString))
                {
                    var folder = Environment.SpecialFolder.LocalApplicationData;
                    var path = Environment.GetFolderPath(folder);
                    connectionString = $"Data Source={System.IO.Path.Join(path, "fb_homework_sqlite.db")}";
                }
            }

            services.AddDbContext<FbDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddMediatR(c => c.RegisterServicesFromAssemblies(typeof(Extensions).Assembly));

            services.AddRestEaseClient<IEleringApi>("https://dashboard.elering.ee");

            services.AddTransient<IPricesRepository, PricesRepository>();

            return services;


        }
    }
}
