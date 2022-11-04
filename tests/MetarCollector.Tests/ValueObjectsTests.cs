using Xunit;
using static Xunit.Assert;

namespace MetarCollector.Tests;

public class ValueObjects
{
    [Theory]
    [InlineData("BLA", "BLA")]
    [InlineData("keyword", "keyword")]
    public void Should_Create_Keyword(string keyword, string expected) =>
        Equal(expected, Keyword.From(keyword));
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("contains empty spaces")]
    public void Should_Not_Create_Keyword(string keyword) =>
        Throws<Exception>(() => Keyword.From(keyword, true));
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("contains empty spaces")]
    public void Should_Create_Empty_Keyword(string keyword) =>
        Equal(Keyword.Empty, Keyword.From(keyword, false));

    [Theory]
    [InlineData("ENGM", "ENGM")]
    [InlineData("anna", "ANNA")]
    public void Should_Create_Icao(string icao, string expected) =>
        Equal(expected, Icao.From(icao));
    
    [Theory]
    [InlineData("ENGMA")]
    [InlineData("AN")]
    public void Should_Not_Create_Icao(string icao) =>
        Throws<Exception>(() => Icao.From(icao));

}