using FluentAssertions;
using MetarCollector.Modules;
using Xunit;

namespace MetarCollector.Tests;

public class ExtractMetarTests
{
    private const string ValidXml = @"<metno:aviationProducts
   xmlns:metno='http://api.met.no'
    xmlns:gml='http://www.opengis.net/gml/3.2'
    xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
    xsi:schemaLocation='http://api.met.no aviation_products.xsd'>
    <metno:meteorologicalAerodromeReport>
    <metno:icaoAirportIdentifier>ENGM</metno:icaoAirportIdentifier>
    <metno:validTime>
    <gml:TimeInstant gml:id='it-1' >
    <gml:timePosition>2022-11-03T00:20:00</gml:timePosition>
    </gml:TimeInstant>
    </metno:validTime>
    <metno:metarType></metno:metarType>
    <metno:metarText>
    ENGM 030020Z 20005KT 9999 BKN009 05/05 Q1007=
    </metno:metarText>
    </metno:meteorologicalAerodromeReport>
    </metno:aviationProducts>"; 
    
    private const string ValidXmlWithAdditionalLinebreaks = @"<metno:aviationProducts
   xmlns:metno='http://api.met.no'
    xmlns:gml='http://www.opengis.net/gml/3.2'
    xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
    xsi:schemaLocation='http://api.met.no aviation_products.xsd'>
    <metno:meteorologicalAerodromeReport>
    <metno:icaoAirportIdentifier>ENGM</metno:icaoAirportIdentifier>
    <metno:validTime>
    <gml:TimeInstant gml:id='it-1' >
    <gml:timePosition>2022-11-03T00:20:00</gml:timePosition>
    </gml:TimeInstant>
    </metno:validTime>
    <metno:metarType></metno:metarType>
    <metno:metarText>
    ENGM 030020Z 20005KT 9999 BKN009 05/05 Q1007=




    </metno:metarText>
    </metno:meteorologicalAerodromeReport>
    </metno:aviationProducts>"; 
    
    private const string ValidMultipleXml = @"<metno:aviationProducts
   xmlns:metno='http://api.met.no'
    xmlns:gml='http://www.opengis.net/gml/3.2'
    xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
    xsi:schemaLocation='http://api.met.no aviation_products.xsd'>
    <metno:meteorologicalAerodromeReport>
    <metno:icaoAirportIdentifier>ENGM</metno:icaoAirportIdentifier>
    <metno:validTime>
    <gml:TimeInstant gml:id='it-1' >
    <gml:timePosition>2022-11-03T00:20:00</gml:timePosition>
    </gml:TimeInstant>
    </metno:validTime>
    <metno:metarType></metno:metarType>
    <metno:metarText>
    ENGM 030020Z 20005KT 9999 BKN009 05/05 Q1007=
    </metno:metarText>
    </metno:meteorologicalAerodromeReport>
   <metno:meteorologicalAerodromeReport>
        <metno:icaoAirportIdentifier>ENGM</metno:icaoAirportIdentifier>
        <metno:validTime>
            <gml:TimeInstant gml:id='it-49' >
    <gml:timePosition>2022-11-03T23:50:00</gml:timePosition>
    </gml:TimeInstant>
    </metno:validTime>
    <metno:metarType></metno:metarType>
    <metno:metarText>
    ENGM 032350Z 16012KT 9999 OVC008 09/08 Q1004=
    </metno:metarText>
    </metno:meteorologicalAerodromeReport>
    </metno:aviationProducts>";

    private const string EmptyXml = @"<metno:aviationProducts
   xmlns:metno='http://api.met.no'
    xmlns:gml='http://www.opengis.net/gml/3.2'
    xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
    xsi:schemaLocation='http://api.met.no aviation_products.xsd'>
    </metno:aviationProducts>";
    
    [Theory]
    [InlineData(ValidXml, true, "ENGM 030020Z 20005KT 9999 BKN009 05/05 Q1007=", "Should get metar text")]
    [InlineData(ValidXmlWithAdditionalLinebreaks, true, "ENGM 030020Z 20005KT 9999 BKN009 05/05 Q1007=", "Should get a washed metar text")]
    [InlineData(ValidMultipleXml, true, "ENGM 032350Z 16012KT 9999 OVC008 09/08 Q1004=", "Should get latest metar text")]
    [InlineData("", false, "", "Shouldn't be able to extract")]
    [InlineData(null, false, "", "Shouldn't be able to extract")]
    [InlineData(EmptyXml, false, "", "Shouldn't be able to extract")]
    public void ExtractMetar(string xml, bool isValid, string metar, string explaination)
    {
        xml.TryExtractMetar(out var result)
            .Should().Be(isValid, explaination);

        result.Should().Be(metar, explaination);
    }
}