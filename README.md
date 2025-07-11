
# DeepL Everywhere

Translate selected text in any application using DeepL.

## Features

- **Global Hotkey**: Press a hotkey (F9) to translate selected text in any application.
- **Caching**: Caches translations to avoid unnecessary API calls.

## Configuration

Configuration is done through a `config.json` file in the same directory as the executable.
The file shoud contain the following JSON structure:
```json
{
	"api-secret": "YOUR_DEEPL_API_KEY",
	"cache-size": 64
}
```
Do NEVER push the `config.json` file containing your API key to a public repository.

### DeepL API Key (Required)

You can use either a free or a pro DeepL API key.
See [DeepL's official documentation](https://developers.deepl.com/docs) for more information on how to obtain an API key.

### Cache Size (Optional)

Defines how many translations to cache. The default is 64.
Less than or equal to 0 means system maximum cache size (2,147,483,591), which may cause performance issues.

## Supported Languages

DeepL supports the following languages:
- AR - Arabic
- BG - Bulgarian
- CS - Czech
- DA - Danish
- DE - German
- EL - Greek
- EN-GB - English (British)
- EN-US - English (American)
- ES - Spanish
- ES-419 - Spanish (Latin American)
- ET - Estonian
- FI - Finnish
- FR - French
- HE - Hebrew
- HU - Hungarian
- ID - Indonesian
- IT - Italian
- JA - Japanese
- KO - Korean
- LT - Lithuanian
- LV - Latvian
- NB - Norwegian Bokmål
- NL - Dutch
- PL - Polish
- PT-BR - Portuguese (Brazilian)
- PT-PT - Portuguese (all Portuguese variants excluding Brazilian Portuguese)
- RO - Romanian
- RU - Russian
- SK - Slovak
- SL - Slovenian
- SV - Swedish
- TH - Thai
- TR - Turkish
- UK - Ukrainian
- VI - Vietnamese
- ZH-HANS - Chinese (simplified)
- ZH-HANT - Chinese (traditional)

The default target language is determined based on the system language as long as it is supported by DeepL,
or falls back to Japanese if the system language is not supported.
