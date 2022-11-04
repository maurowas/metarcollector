using System.Xml.Linq;
using FluentAssertions;
using Xunit;
using static MetarCollector.Modules.MetarXmlGenerator;

namespace MetarCollector.Tests;


public class MetarXmlGeneratorTests
{
    private const string Expected1 = @"<METAR>
    <Airport logtime='{0}' icao='{1}' metarText='{2}' skyfritt='{3}' />
</METAR>";
    
    [Theory]
    [InlineData("2022-10-30T10:30:22Z","ENGM", "ENGM 030020Z 20005KT 9999 BKN009 05/05 Q1007=", false, Expected1)]
    [InlineData("1900-01-01T00:00:00Z","ENGM", "ENGM 030020Z 20005KT 9999 BKN009 05/05 Q1007=", false, Expected1)]
    public void MetarXmlGenerator(string date, string icao, string metarText, bool cloudless, string expected) =>
        GenerateXml(DateTimeOffset.Parse(date), icao, metarText, cloudless).ToString()
            .Should().Be(XDocument.Parse(string.Format(expected,
                DateTimeOffset.Parse(date).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                icao, metarText, cloudless ? "true" : "false")).ToString());
}