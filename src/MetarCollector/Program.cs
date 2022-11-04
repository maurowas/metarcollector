using MetarCollector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using static System.Console;
using static MetarCollector.Modules.MetarRetrieverService;

CancelKeyPress += delegate
{
    WriteLine("SIGINT captured. Shuting down...");
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    BuildHost(args)
        .Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
    
static IHost BuildHost(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((ctx, services) =>
        {
            services.AddMetarRetriever(ctx.Configuration)
                .AddHostedService<OrchestratorService>();
        })
        .UseSerilog()
        .Build();