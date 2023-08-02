using Karambolo.PO;

namespace LibreTranslate;

internal static class POParserExtensionMethods
{
    
    public static POParseResult ParseFile(this POParser parser, string path)
    {
        using var fileStream = File.OpenRead(path);
        return parser.Parse(fileStream);
    }
}