using static System.String;

namespace MetarCollector;


public record Keyword
{
    string Value { get; }

    Keyword(string value) => Value = value;

    public override string ToString() => Value;

    public static Keyword Empty => new(string.Empty);

    public static Keyword From(string value, bool throws = true) =>
        value switch
        {
            _ when IsNullOrWhiteSpace(value) || value.Contains(" ") => throws ? throw new Exception("Keyword can't be null or contain whitespace") : Empty,
            _ when value.HasSpecialChars() => throws ? throw new Exception("Keyword can't contain special charactes") : Empty,
            _ => new(value)
        };

    public static implicit operator Keyword(string value) => From(value);
    public static implicit operator string(Keyword keyword) => keyword.Value;
}

public record Icao
{
    private string Value { get; }

    Icao(string value) => Value = value;

    public static Icao Empty => new(String.Empty);

    public override string ToString() => Value;

    public static Icao From(Keyword value, bool throws = true) =>
        value.ToString().Replace(" ","").ToUpper() switch
        {
            { Length: 4 } val => new(val),
            _ => throws ? throw new Exception("Icao is not in correct format") : Empty
        };

    public static implicit operator Icao(string value) => From(value);

    public static implicit operator string(Icao icao) => icao.Value;
}
