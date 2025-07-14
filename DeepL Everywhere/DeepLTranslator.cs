
// (c) 2025 Kazuki Kohzuki

using DeepL;
using System.Globalization;

namespace DeepLEverywhere;

/// <summary>
/// Provides methods for translating text using the DeepL API.
/// </summary>
internal static partial class DeepLTranslator
{
    private static readonly string[] supportedLanguages = [
        "AR", "BG", "CS", "DA", "DE", "EL", "EN-GB", "EN-US", "ES", "ES-419",
        "ET", "FI", "FR", "HE", "HU", "ID", "IT", "JA", "KO", "LT",
        "LV", "NB", "NL", "PL", "PT-BR", "PT-PT", "RO", "RU", "SK", "SL",
        "SV", "TH", "TR", "UK", "VI", "ZH-HANS", "ZH-HANT",
    ];

    private static Translator? translator;
    private static IDictQueue<string, string>? translationCache;

    private static Translator Translator => translator ??= new(Program.Config.ApiSecret!);

    private static IDictQueue<string, string> TranslationCache => translationCache ??= CreateCacheInstance();

    /// <summary>
    /// Gets the collection of supported languages for translation.
    /// </summary>
    /// <value>
    /// A read-only collection of language codes supported by the DeepL API.
    /// </value>
    internal static IReadOnlyCollection<string> SupportedLanguages => supportedLanguages;

    private static string targetLanguage;

    static DeepLTranslator()
    {
        var userLanguage = CultureInfo.CurrentCulture;

        var full = userLanguage.Name.ToUpperInvariant();
        if (supportedLanguages.Contains(full))
        {
            targetLanguage = full;  // Set to user's language if supported
        }
        else
        {
            var shortLang = userLanguage.TwoLetterISOLanguageName.ToUpperInvariant();
            if (supportedLanguages.Contains(shortLang))
                targetLanguage = shortLang;  // Set to two-letter code if supported
            else
                targetLanguage = "JA";  // Default to Japanese if user's language is not supported
        }
    } // cctor ()

    /// <summary>
    /// Gets or sets the target language for translation.
    /// </summary>
    /// <remarks>
    /// Changing the target language will clear the translation cache
    /// because translations should differ based on the language.
    /// </remarks>
    internal static string TargetLanguage
    {
        get => targetLanguage;
        set
        {
            if (value == targetLanguage) return;  // No change
            targetLanguage = value;
            TranslationCache.Clear();  // Clear cache when target language changes
        }
    }

    /// <summary>
    /// Asynchronously translates the given text to the specified target language.
    /// </summary>
    /// <param name="originalText">The text to translate.</param>
    /// <param name="targetLanguage">The language code to translate the text into.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the translated text.</returns>
    internal static async Task<string> Translate(string originalText, string targetLanguage)
    {
        if (string.IsNullOrWhiteSpace(originalText)) return string.Empty;  // No text to translate

        originalText = originalText.Trim();
        // Check if the translation is already cached
        if (TranslationCache.TryGetValue(originalText, out var cachedTranslation))
            return cachedTranslation;  // Return cached translation to avoid unnecessary API calls

        try
        {
            var result = await Translator.TranslateTextAsync(originalText, null, targetLanguage);
            var translatedText = result.Text.Trim();
            TranslationCache.Add(originalText, translatedText);  // Cache the translation result
            return translatedText;
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log them)
            Console.WriteLine($"Translation error: {ex.Message}");
            return $"Failed to translate: {originalText}";
        }
    } // internal static async Task<string> Translate (string, LanguageCode)

    /// <summary>
    /// Asynchronously translates the text from the currently focused control to the specified target language.
    /// </summary>
    /// <param name="targetLanguage">The language code to translate the text into.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the translated text.</returns>
    internal static async Task<string> TranslateFocusedControlText(string targetLanguage)
    {
        var text = GetTextFromFocusedControl();
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;  // No text to translate
        return await Translate(text, targetLanguage);
    } // internal static async Task<string> TranslateFocusedControlText (string)

    private static string GetTextFromFocusedControl()
    {
        try
        {
            SendKeys.SendWait("^c");
            Thread.Sleep(100);  // Wait for clipboard to update
            if (Clipboard.ContainsText()) return Clipboard.GetText();
            return string.Empty; // No originalText in clipboard
        }
        catch
        {
            return string.Empty;
        }
    } // private static string GetTextFromFocusedControl ()

    private static IDictQueue<string, string> CreateCacheInstance()
    {
        var cacheSize = Program.Config.CacheSize ?? 64;
        if (cacheSize <= 0) return new EmptyDictQueue<string, string>();
        return new DictQueue<string, string>(cacheSize);
    } // private static IDictQueue<string, string> CreateCacheInstance ()

    /// <summary>
    /// Clears the translation cache.
    /// </summary>
    internal static void ClearCache()
    {
        TranslationCache.Clear();
    } // internal static void ClearCache ()
} // internal static class Translator
