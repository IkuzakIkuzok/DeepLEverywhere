
// (c) 2025 Kazuki Kohzuki

namespace DeepLEverywhere;

internal interface IDictQueue<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull;
