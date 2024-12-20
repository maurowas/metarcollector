﻿using System.Xml.Linq;
using static System.Globalization.CultureInfo;

namespace MetarCollector.Modules;

public static class MetarXmlGenerator
{
    public static XDocument GenerateXml(DateTimeOffset datetime, Icao icao, string metarText, bool cloudless) =>
        new(new XElement("METAR",
            new XElement("Airport", 
                new XAttribute("logtime", datetime.ToString("yyyy-MM-ddTHH:mm:ssZ")),
                new XAttribute("icao", icao),
                new XAttribute("metarText", metarText),
                new XAttribute("skyfritt", cloudless ? "true" : "false"))));
}
