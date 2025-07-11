
// (c) 2025 Kazuki Kohzuki

using DeepLEverywhere.Controls;
using System.Resources;

[assembly: NeutralResourcesLanguage("ja-jp")]

namespace DeepLEverywhere;

internal static class Program
{
    internal static readonly MainWindow _mainWindow = new();
    private static readonly DeepLNotifyIcon _notifyIcon = new();

    internal static Config Config { get; private set; } = null!;  // Config will be initialized in Main()

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        _mainWindow.Show();

        var exeocationPath = Path.GetDirectoryName(Application.ExecutablePath)!;
        var configFilePath = Path.Combine(exeocationPath, "config.json");
        if (File.Exists(configFilePath))
        {
            var json = File.ReadAllText(configFilePath);
            Config = Config.LoadJson(json) ?? new Config();
        }
        else
        {
            Config = new Config();
        }

        KeyHook.Monitor(Keys.F9);
        KeyHook.Hook();
        KeyHook.KeyDownCallback += ShowTranslatedText;

        Application.Run();
    } // private static void Main ()

    private static async void ShowTranslatedText(object? sender, KeyEventArgs e)
    {
        var translated = await DeepLTranslator.TranslateFocusedControlText(DeepLTranslator.TargetLanguage);
        if (string.IsNullOrWhiteSpace(translated)) return;
        ResultForm.Show(translated);
    } // private static void ShowTranslatedText ()

    internal static void ExitApplication()
    {
        KeyHook.Unhook();
        _notifyIcon.Hide();
        _mainWindow.Close();
        Application.Exit();
    } // internal static void ExitApplication ()
} // internal static class Program
