using System.Transactions;
using Fluent.LibreTranslate;
using Karambolo.PO;
using LibreTranslate;

var libreTranslateServer = new Uri("http://localhost:5000");
var libreTranslateApiKey = string.Empty;
var templateFile = "/home/nvanlaerebeke/projects/nomadesk/nomadesk-ctrller/src/lib/locale/default.pot";
var targetFile = "/home/nvanlaerebeke/projects/nomadesk/nomadesk-ctrller/src/lib/locale/nl_BE/LC_MESSAGES/default.po";

var parser = new POParser(new POParserSettings());

var templateResult = parser.Parse(File.OpenRead(templateFile));
var targetResult = parser.Parse(File.OpenRead(targetFile));

var newCatalog = await targetResult.Catalog.updateWithNewEntriesAsync(templateResult.Catalog, new Translator(libreTranslateServer, libreTranslateApiKey, LanguageCode.English));
newCatalog.Write(targetFile);

foreach (var entry in newCatalog.Values)
{
    Console.WriteLine($"{entry.Key}");
    foreach (var translation in entry)
    {
        Console.WriteLine($"   - {translation}");
    }
}

Console.WriteLine("Done");