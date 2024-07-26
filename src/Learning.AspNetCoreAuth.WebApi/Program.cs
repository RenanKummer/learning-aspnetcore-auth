using Learning.AspNetCoreAuth.Core.Extensions;
using Learning.AspNetCoreAuth.Infrastructure.Extensions;
using Learning.AspNetCoreAuth.WebApi.Extensions;
using Learning.AspNetCoreAuth.WebApi.Formatters;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithApplicationName()
    .Enrich.WithExceptionDetails()
    .WriteTo.Async(sink => sink.Console(new CompactJsonFormatter(new SerilogNullJsonValueFormatter())))
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting application...");
    var builder = WebApplication.CreateBuilder(args);

    builder
        .ConfigureOptions()
        .ConfigureLoggingProvider();

    builder.Services
        .AddCoreServices()
        .AddInfrastructureServices()
        .AddWebApiServices();
    
    var app = builder.Build();

    app.UseWebApiMiddlewares();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
