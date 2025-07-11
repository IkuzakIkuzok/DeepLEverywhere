
// (c) 2025 Kazuki Kohzuki

using System.ComponentModel;

namespace DeepLEverywhere.Controls;

[DesignerCategory("code")]
internal class MainWindow : Form
{
    internal MainWindow()
    {
        this.Text = "DeepL Everywhere";
        this.Size = new();
        this.ShowInTaskbar = false;
    } // ctor ()

    override protected void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Hide();
    } // override protected void OnShown (EventArgs e)
} // internal class MainWindow : Form
