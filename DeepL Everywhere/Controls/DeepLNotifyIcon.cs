
// (c) 2025 Kazuki Kohzuki

using System.Globalization;

namespace DeepLEverywhere.Controls;

internal sealed class DeepLNotifyIcon
{
    private readonly NotifyIcon _notifyIcon;

    internal DeepLNotifyIcon()
    {
        this._notifyIcon = new()
        {
            Text = $"DeepL Everywhere - {new CultureInfo(DeepLTranslator.TargetLanguage).NativeName}",
            Icon = Properties.Resources.Icon,
            ContextMenuStrip = new ContextMenuStrip(),
            Visible = true,
        };

        #region target languages

        var targetLanguagesMenuItem = new ToolStripMenuItem("Target Language");
        this._notifyIcon.ContextMenuStrip.Items.Add(targetLanguagesMenuItem);

        void AddLanguageMenuItem(string languageCode)
        {
            var languageName = new CultureInfo(languageCode).NativeName;
            var menuItem = new ToolStripMenuItem(languageName)
            {
                Checked = languageCode == DeepLTranslator.TargetLanguage,
                Tag = languageCode,
            };
            menuItem.Click += (sender, e) =>
            {
                DeepLTranslator.TargetLanguage = languageCode;
                this._notifyIcon.Text = $"DeepL Everywhere - {languageName}";
            };
            targetLanguagesMenuItem.DropDownItems.Add(menuItem);
        } // void AddLanguageMenuItem (string, string, [bool])

        foreach (var code in DeepLTranslator.SupportedLanguages)
            AddLanguageMenuItem(code);

        targetLanguagesMenuItem.DropDownOpening += (sender, e) =>
        {
            var targetLanguage = DeepLTranslator.TargetLanguage;
            foreach (var item in targetLanguagesMenuItem.DropDownItems)
            {
                if (item is not ToolStripMenuItem menuItem) continue;
                if (menuItem.Tag is not string languageCode) continue;
                menuItem.Checked = (languageCode == targetLanguage);
            }
        };

        #endregion target languages

        var clearCacheMenuItem = new ToolStripMenuItem("Clear Cache");
        clearCacheMenuItem.Click += (sender, e) => DeepLTranslator.ClearCache();
        this._notifyIcon.ContextMenuStrip.Items.Add(clearCacheMenuItem);

        this._notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

        var exitMenuItem = new ToolStripMenuItem("Exit");
        exitMenuItem.Click += (sender, e) => Program.ExitApplication();
        this._notifyIcon.ContextMenuStrip.Items.Add(exitMenuItem);
    } // ctor ()

    internal void Hide()
    {
        this._notifyIcon.Visible = false;
    } // internal void Hide ()
} // internal sealed class DeepLNotifyIcon
