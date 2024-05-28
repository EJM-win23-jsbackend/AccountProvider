using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddPooledDbContextFactory<DataContext>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("Account_Identity_Database"))
           .UseLazyLoadingProxies());
        services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<UserManager<ApplicationUser>>();
    })
    .Build();

host.Run();
