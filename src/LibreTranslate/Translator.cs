using Fluent.LibreTranslate;

namespace LibreTranslate;

internal class Translator
{
    private readonly LanguageCode? _sourceLanguage;
    
    public Translator(Uri server, string apiKey, LanguageCode sourceLanguage) : this(server, apiKey)
    {
        _sourceLanguage = sourceLanguage;
    }

    private Translator(Uri server, string apiKey)
    {
        GlobalLibreTranslateSettings.Server = new LibreTranslateServer(server.ToString());
        GlobalLibreTranslateSettings.ApiKey = string.IsNullOrEmpty(apiKey) ? null : apiKey;
    }

    public bool UseRateLimitControl
    {
        get => GlobalLibreTranslateSettings.UseRateLimitControl;
        set => GlobalLibreTranslateSettings.UseRateLimitControl = value;
    }
    
    public TimeSpan RateLimitTimeSpan
    {
        get => GlobalLibreTranslateSettings.RateLimitTimeSpan;
        set => GlobalLibreTranslateSettings.RateLimitTimeSpan = value;
    }
    
    public async Task<string> Translate(string str, LanguageCode fromLang, LanguageCode toLang)
    {
        return await str.TranslateAsync(fromLang, toLang);
    }
    
    public async Task<string> Translate(string str, LanguageCode toLang)
    {
        return await(_sourceLanguage is null ? str.TranslateAsync(toLang) : str.TranslateAsync(_sourceLanguage, toLang));
    }

    public static bool TryGetLanguageCode(string str, out LanguageCode? languageCode)
    {
        try
        {
            languageCode = LanguageCode.FromString(str[..2]);
        }
        catch (Exception _)
        {
            languageCode = null;
            return false;
        }
        return true;
    }
}