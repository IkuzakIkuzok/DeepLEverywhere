
// (c) 2025 Kazuki Kohzuki

using System.ComponentModel;

namespace DeepLEverywhere.Controls;

[DesignerCategory("code")]
internal sealed class ResultForm : Form
{
    private static readonly Font resultFont = new("Noto Sans JP", 12, FontStyle.Regular);

    private readonly TextBox _resultTextBox;

    private ResultForm()
    {
        this.Text = "DeepL Translation Result";
        this.Size = new(600, 400);
        this.Icon = Properties.Resources.Icon;
        this.KeyPreview = true;

        this._resultTextBox = new TextBox
        {
            Multiline = true,
            Dock = DockStyle.Fill,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BackColor = SystemColors.Window,
            ForeColor = SystemColors.ControlText,
            Font = resultFont,
            Parent = this,
        };
    } // ctor ()

    private void SetText(string text)
    {
        this._resultTextBox.Text = text;
        var size = TextRenderer.MeasureText(text, resultFont);

        if (size.Width < 500)
        {
            this.Width = size.Width + 100; // Add some padding
            this.Height = size.Height + 100; // Add some padding
            return;
        }
    } // private void SetText (string text)

    override protected void OnShown(EventArgs e)
    {
        base.OnShown(e);
        SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);
        this._resultTextBox.SelectionLength = 0;
        Activate();
    } // override protected void OnShown (EventArgs e)

    override protected void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.KeyCode == Keys.Escape) Close();
    } // override protected void OnKeyDown (KeyEventArgs)

    internal static void Show(string text)
    {
        var form = new ResultForm();
        form.SetText(text);
        form.Show();
    } // internal static void Show (string)
} // internal sealed class ResultForm : Form
