using static System.IO.File;

namespace Utviklertest.Modules;

public static class FileSaver
{
    public static Task SaveFile(string path, Func<DateTimeOffset,string> getData, CancellationToken ct = default) => 
        WriteAllTextAsync(path, getData(DateTimeOffset.UtcNow), ct);
}