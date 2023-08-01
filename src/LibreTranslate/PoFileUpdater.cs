using System.Linq.Expressions;
using Fluent.LibreTranslate;
using Karambolo.PO;

namespace LibreTranslate;

internal static class PoFileUpdater
{
    public static void Write(this POCatalog catalog, string targetFile)
    {
        var generator = new POGenerator(new POGeneratorSettings());
        generator.Generate(File.OpenWrite(targetFile), catalog);
    }
    
    public static async Task<POCatalog> updateWithNewEntriesAsync(this POCatalog existingCatalog, POCatalog updateCatalog, Translator translator)
    {
        var newKeys = updateCatalog.Keys.Except(existingCatalog.Keys).ToList();
        var removedKeys = existingCatalog.Keys.Except(updateCatalog.Keys).ToList();

        existingCatalog.RemoveKeys(removedKeys);
        await existingCatalog.AddAndTranslateEntriesAsync(updateCatalog.Values.Where(entry => newKeys.Contains(entry.Key)).ToList(), translator);
        return existingCatalog;
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

    private static async Task AddAndTranslateEntriesAsync(this POCatalog catalog, List<IPOEntry> entries, Translator translator)
    {
        if(!Translator.TryGetLanguageCode(catalog.Language, out var lang))
        {
            return;
        } 
        
        foreach (var entry in entries)
        {
            if (!entry.Count.Equals(1) || !string.IsNullOrEmpty(entry[0]))
            {
                continue;
            }
            
            catalog.Add(await entry.TranslateAsync(translator, lang));
            catalog.Add(entry);
        }
    }
    
    private static async Task<IPOEntry> TranslateAsync(this IPOEntry entry, Translator translator, LanguageCode targetLanguage)
    {
        return new POSingularEntry(entry.Key)
        {
            Translation = await translator.Translate(entry[0], targetLanguage)
        };
    }
}