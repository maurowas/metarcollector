using Xunit;
using static Xunit.Assert;

namespace MetarCollector.Tests;

public class MetarTests
{
    [Theory]
    [InlineData("METAR EHLE 280925Z AUTO 21009G19KT 060V130 5000 -RA FEW007 BKN014CB BKN017 02/M01 Q1001 BECMG 6000")]
    [InlineData("ENGM 030020Z 20005KT 9999 BKN009 05/05 Q1007=")]
    [InlineData("ENGM 030020Z 20005KT 9999 OVC009 BKN009 SKC 05/05 Q1007=")]
    [InlineData("ENGM 030020Z 20005KT 9999 05/05 Q1007=")]
    public void Should_Be_Cloudy(string metar) =>
        False(metar.ToMetar().IsCloudless());
    
    [Theory]
    [InlineData("ENGM 030020Z 20005KT 9999 NCD 05/05 Q1007=")]
    [InlineData("ENGM 030020Z 20005KT 9999 SKC 05/05 Q1007=")]
    [InlineData("ENGM 030020Z 20005KT 9999 NCD SKC 05/05 Q1007=")]
    public void Should_Be_Cloudless(string metar) =>
        True(metar.ToMetar().IsCloudless());
}