using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Payment.Persistence.EFConfiguration
{
	public static class DependencyInjectionExtensions
	{
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseNpgsql(configuration["ConnectionStrings:PaymentDatabase"]));

            return services;
        }
    }
}

