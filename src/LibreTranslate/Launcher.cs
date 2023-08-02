using CommandLine;
using Fluent.LibreTranslate;
using Karambolo.PO;

namespace LibreTranslate;

internal class Launcher
{
    public async Task Start(string[] args)
    {
        var o = Parser.Default.ParseArguments<Options>(args);
        if (o is null || o.Errors.Any())
        {
            Environment.Exit(1);
        }
        var options = o.Value;
        var parser = new POParser(new POParserSettings());
        var templateResult = parser.ParseFile(options.ReferenceFile);
        var targetResult = parser.ParseFile(options.Target);
        await targetResult.Catalog.updateWithNewEntriesAsync(
            templateResult.Catalog, 
            new Translator(
                new Uri(options.Server), 
                options.ApiKey, 
                LanguageCode.English
            )
        );
        targetResult.Catalog.Write(options.Target);
    }
}