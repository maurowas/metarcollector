using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.DateOnly;
using static System.DateTime;
using static Microsoft.AspNetCore.WebUtilities.QueryHelpers;

namespace MetarCollector.Modules;

public delegate Task<string> GetLatestMetar(Icao icao, DateOnly date = default, CancellationToken ct = default);

public static class MetarRetrieverService
{
    const string MetarServiceUrl = nameof(MetarServiceUrl);
    
     static GetLatestMetar Setup(HttpClient httpClient) =>
        async (icao, date, ct) =>
        {
            var resp = await httpClient.GetAsync(GetUrl(), ct);

            resp.EnsureSuccessStatusCode();

            return (await resp.Content.ReadAsStringAsync(ct))
                .TryExtractMetar(out var metarText) 
                    ? metarText 
                    : throw new Exception("Couldn't extract metarText");

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

    public static bool TryExtractMetar(this string json, out string metarText)
    {
        try
        {
            XNamespace ns = "http://api.met.no";
            XNamespace gml = "http://www.opengis.net/gml/3.2";
            metarText = XDocument.Parse(json)
                .Descendants(ns + "meteorologicalAerodromeReport")
                .OrderByDescending(x => (DateTime)x.Descendants(gml + "timePosition").First())
                .FirstOrDefault()!
                .Element(ns + "metarText")!
                .Value
                .Replace("\n", "")
                .Replace("\t", "")
                .Trim();

            return true;
        }
        catch (Exception)
        {
            metarText = string.Empty;
            return false;
        }
    }
    
}
