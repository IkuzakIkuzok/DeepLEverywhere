
// (c) 2025 Kazuki Kohzuki

using System.Collections;

namespace DeepLEverywhere;

internal class EmptyDictQueue<TKey, TValue> : IDictQueue<TKey, TValue> where TKey : notnull
{
    internal EmptyDictQueue() { }

    /// <inheritdoc/>
    public TValue this[TKey key]
    {
        get => throw new KeyNotFoundException();
        set => throw new NotSupportedException("Cannot add items to an empty queue.");
    }

    /// <inheritdoc/>
    public ICollection<TKey> Keys => [];

    /// <inheritdoc/>
    public ICollection<TValue> Values => [];

    /// <inheritdoc/>
    public int Count => 0;

    /// <inheritdoc/>
    public bool IsReadOnly => true;

    /// <inheritdoc/>
    public void Add(TKey key, TValue value)
    {
        // Do nothing, as this is an empty queue.
    } // public void Add (TKey, TValue)

    /// <inheritdoc/>
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        // Do nothing, as this is an empty queue.
    } // public void Add (KeyValuePair<TKey, TValue>)

    /// <inheritdoc/>
    public void Clear()
    {
        // Do nothing, as this is an empty queue.
    } // public void Clear ()

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<TKey, TValue> item)
        => false;

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
        => false;

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        => throw new NotSupportedException("Cannot copy items from an empty queue.");

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();

    /// <inheritdoc/>
    public bool Remove(TKey key)
        => false;

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<TKey, TValue> item)
        => false;

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, out TValue value)
    {
        value = default!;
        return false;
    } // public bool TryGetValue (TKey, out TValue)

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
} // internal class EmptyDictQueue<TKey, TValue> : IDictQueue<TKey, TValue> where TKey : notnull
