using MetarParserCore;
using MetarParserCore.Objects;
using static MetarParserCore.Enums.CloudType;

namespace MetarCollector;

public static class Extensions
{
    public static bool IsCloudless(this Metar metar) =>
        metar.CloudLayers?
            .Select(x => x.CloudType)
            .All(x => x switch
            {
                SkyClear => true,
                NoCloudDetected => true,
                _ => false
            }) ?? false;

    public static Metar ToMetar(this string metar) =>
        new MetarParser().Parse(metar);
    
    public static bool HasSpecialChars(this string self) => 
        self.Any(ch => !char.IsLetterOrDigit(ch));
}