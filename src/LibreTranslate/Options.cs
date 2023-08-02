using CommandLine;

namespace LibreTranslate;

internal class Options
{
    [Option('d', "referencefile", Required = true, HelpText = "Default Reference Language File (default.pot).")]
    public string ReferenceFile { get; set; } = string.Empty;
    
    [Option('t', "target", Required = true, HelpText = "Target pot file, will update existing (empty) translations.")]
    public string Target { get; set; } = string.Empty;

    [Option('s', "server", Required = true, HelpText = "LibreTranslate Server URL.")]
    public string Server { get; set; } = string.Empty;

    [Option('k', "apikey", Required = false, HelpText = "LibreTranslate Api Key")]
    public string ApiKey { get; set; } = string.Empty;
}