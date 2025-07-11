
# DeepL Everywhere

Translate selected text in any application using DeepL.

## Features

- **Global Hotkey**: Press a hotkey (F9) to translate selected text in any application.
- **Caching**: Caches translations to avoid unnecessary API calls.

## Configuration

### DeepL API Key

You can use either a free or a pro DeepL API key.
See [DeepL's official documentation](https://developers.deepl.com/docs) for more information on how to obtain an API key.
To set up your DeepL API key, create a file named `config.json` in the same directory as the executable file.
The file should contain the following JSON structure:
```json
{
	"api-secret": "YOUR_DEEPL_API_KEY"
}
```
