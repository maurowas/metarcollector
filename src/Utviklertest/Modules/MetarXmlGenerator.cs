using System.Xml.Linq;
using static System.Globalization.CultureInfo;

namespace Utviklertest.Modules;

public static class MetarXmlGenerator
{
    public static XDocument GenerateXml(DateTimeOffset datetime, string icao, string metarText, bool cloudless) =>
        new(new XElement("METAR",
            new XElement("Airport", 
                new XAttribute("logtime", datetime.ToString("O", InvariantCulture)),
                new XAttribute("icao", icao),
                new XAttribute("metarText", metarText),
                new XAttribute("skyfritt", cloudless ? "true" : "false"))));
}
