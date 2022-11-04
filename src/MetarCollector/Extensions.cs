using MetarParserCore;
using static MetarParserCore.Enums.CloudType;

namespace MetarCollector;

public static class Metar
{
    public static bool IsCloudless(string metarText) =>
        new MetarParser().Parse(metarText).CloudLayers.FirstOrDefault()?.CloudType switch
        {
            SkyClear => true,
            NoCloudDetected => true,
            _ => false
        };
    
    public static bool HasSpecialChars(this string self) => 
        self.Any(ch => !char.IsLetterOrDigit(ch));
}