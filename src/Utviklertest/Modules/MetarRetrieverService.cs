using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Utviklertest.Infrastructure;
using static System.DateOnly;
using static System.DateTime;
using static System.String;
using static Microsoft.AspNetCore.WebUtilities.QueryHelpers;

namespace Utviklertest.Modules;

public delegate Task<string> GetLatestMetar(string icao, DateOnly date = default, CancellationToken ct = default);

public static class MetarRetrieverService
{
    const string MetarServiceUrl = nameof(MetarServiceUrl);
    
    public static GetLatestMetar Setup(HttpClient httpClient) =>
        async (icao, date, ct) =>
        {
            var resp = await httpClient.GetAsync(GetUrl(), ct);

            resp.EnsureSuccessStatusCode();

            return await LatestMetarFromResponse();

            async Task<string> LatestMetarFromResponse()
            {
                XNamespace ns = "http://api.met.no";
                XNamespace gml = "http://www.opengis.net/gml/3.2";
                return XDocument.Load(await resp.Content.ReadAsStreamAsync(ct))
                    .Descendants(ns + "meteorologicalAerodromeReport")
                    .OrderByDescending(x => (DateTime)x.Descendants(gml + "timePosition").First())
                    .FirstOrDefault()!
                    .Element(ns + "metarText")!
                    .Value
                    .Replace("\n", "").Replace("\t", "");
            }

            string GetUrl() =>
                AddQueryString("metar.xml", new Dictionary<string, string>
                {
                    { "icao", icao},
                    { "date", date == default 
                        ? FromDateTime(UtcNow).ToDateFormat() 
                        : date.ToDateFormat() },
                    { "extended", "false" },
                    { "offset", "+00:00" }
                });
        };

    public static IServiceCollection AddMetarRetriever(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(sp =>
            Setup(new()
            {
                BaseAddress = new Uri(configuration[MetarServiceUrl] ?? throw new Exception("MetarService Url is not set."))
            }));

        return services;
    }

    static string ToDateFormat(this DateOnly dateOnly) =>
        dateOnly.ToString("yyyy-MM-dd");
    
}
