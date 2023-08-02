using System.Linq.Expressions;
using Fluent.LibreTranslate;
using Karambolo.PO;

namespace LibreTranslate;

internal static class PoFileUpdater
{
    public static void Write(this POCatalog catalog, string targetFile)
    {
        using var writeStream = File.OpenWrite(targetFile);
        var generator = new POGenerator(new POGeneratorSettings());
        generator.Generate(writeStream, catalog);
    }
    
    public static async Task updateWithNewEntriesAsync(this POCatalog existingCatalog, POCatalog updateCatalog, Translator translator)
    {
        var newKeys = updateCatalog.Keys.Except(existingCatalog.Keys).ToList();
        var removedKeys = existingCatalog.Keys.Except(updateCatalog.Keys).ToList();

        existingCatalog.RemoveKeys(removedKeys);
        existingCatalog.AddKeys(newKeys);
        await existingCatalog.TranslateEmpty(translator);
    }

    private static void AddKeys(this POCatalog catalog, IEnumerable<POKey> keys)
    {
        foreach (var key in keys)
        {
            catalog.Add(new POSingularEntry(key) { Translation = string.Empty });
        }
    }
    
    private static void RemoveKeys(this POCatalog catalog, IEnumerable<POKey> keys)
    {
        foreach (var poKey in keys)
        {
            if (!catalog.Keys.Contains(poKey))
            {
                continue;
            }
            catalog.Remove(catalog[poKey]);
        }
    }

    private static async Task TranslateEmpty(this POCatalog catalog, Translator translator)
    {
        if(!Translator.TryGetLanguageCode(catalog.Language, out var lang))
        {
            return;
        } 
        for (var i = 0; i < catalog.Count; i++)
        {
            if (!catalog[i].Count.Equals(1) || !string.IsNullOrEmpty(catalog[i][0]))
            {
                continue;
            }
            catalog[i] = await catalog[i].TranslateAsync(translator, lang!);
        }
    }
    
    private static async Task<IPOEntry> TranslateAsync(this IPOEntry entry, Translator translator, LanguageCode targetLanguage)
    {
        return new POSingularEntry(entry.Key)
        {
            Translation = await translator.Translate(entry.Key.ToString(), targetLanguage)
        };
    }
}