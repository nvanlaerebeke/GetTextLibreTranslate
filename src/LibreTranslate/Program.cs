namespace LibreTranslate;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await new Launcher().Start(args);
    }
}