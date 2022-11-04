using MetarParserCore;
using static MetarParserCore.Enums.CloudType;

namespace Utviklertest;

public static class Metar
{
    public static bool IsCloudless(string metarText) =>
        new MetarParser().Parse(metarText).CloudLayers.FirstOrDefault()?.CloudType switch
        {
            SkyClear => true,
            NoCloudDetected => true,
            _ => false
        };
}