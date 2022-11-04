using MetarCollector.Infrastructure;
using MetarCollector.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using static MetarCollector.Metar;
using static MetarCollector.Modules.FileSaver;
using static MetarCollector.Modules.MetarXmlGenerator;

namespace MetarCollector;

public class OrchestratorService : BackgroundService
{
    public const string XmlOutputPath = nameof(XmlOutputPath);
    public const string DefaultIcao = nameof(DefaultIcao);
    
    private readonly GetLatestMetar _getLatestMetar;
    private readonly IConfiguration _configuration;
    private readonly IHostApplicationLifetime _lifetime;

    public OrchestratorService(GetLatestMetar getLatestMetar, IConfiguration configuration, IHostApplicationLifetime lifetime)
    {
        _getLatestMetar = getLatestMetar;
        _configuration = configuration;
        _lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var xmlOutPath = _configuration.ThrowWhenNotSet(XmlOutputPath);
        var icao = _configuration.ThrowWhenNotSet(DefaultIcao);

        var metarText = await _getLatestMetar(icao, ct: ct);
        
        await SaveFile(xmlOutPath,dt =>
            GenerateXml(dt, icao, metarText, IsCloudless(metarText)).ToString(), ct);

        _lifetime.StopApplication();
    }
}