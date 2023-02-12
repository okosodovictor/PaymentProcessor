
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Payment.Application.Interfaces;
using Payment.Application.Interfaces.Bank;
using Payment.Application.Managers;
using Payment.Application.Managers.Services;
using Payment.Application.Repositories;
using Payment.Banks.BankAPIClients;
using Payment.Persistence.EFConfiguration;
using Payment.Persistence.Repository;

namespace Payment.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //build config
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = builder.Configuration["ConnectionStrings:PaymentDatabase"];
        }

        builder.Services.AddDbContext<PaymentDbContext>(options =>
            options.UseNpgsql(connectionString));


        builder.Services.AddPersistence(config);
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();

        if (builder.Configuration.GetValue<bool>("app:bank:mock"))
        {
            builder.Services.AddScoped<IBankClient, MockBankSimulator>();
        }
        else
        {
            builder.Services.AddScoped<IBankClient, UbsBankClient>();
            builder.Services.AddScoped<IBankClient, DeutscheBankClient>();
        }

        builder.Services.AddScoped<IEncryption, AESEncryption>();
        builder.Services.AddScoped(services => new AESEncryption.Options
        {
            Key = builder.Configuration.GetValue<string>("app:encryption:key")
        });

        builder.Services.AddScoped<IMerchantRepository, MerchantRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

