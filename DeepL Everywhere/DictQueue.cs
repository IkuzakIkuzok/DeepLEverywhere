
// (c) 2025 Kazuki Kohzuki

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DeepLEverywhere;

/// <summary>
/// Represents a dictionary with a queue-like behavior, where the oldest items are removed when the capacity is exceeded.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
internal class DictQueue<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> dictionary;
    private Queue<TKey> keys;
    private readonly int capacity;

    /// <summary>
    /// Initializes a new instance of the <see cref="DictQueue{TKey, TValue}"/> class with a specified capacity.
    /// </summary>
    /// <param name="capacity">The maximum number of items the queue can hold.</param>
    internal DictQueue(int capacity)
    {
        this.capacity = capacity;
        this.dictionary = new(capacity);
        this.keys = new(capacity);
    } // ctor (int capacity)

    /// <inheritdoc/>
    public TValue this[TKey key]
    {
        get => this.dictionary[key];
        set => this.dictionary[key] = value;
    }

    /// <inheritdoc/>
    public ICollection<TKey> Keys => this.dictionary.Keys;

    /// <inheritdoc/>
    public ICollection<TValue> Values => this.dictionary.Values;

    /// <inheritdoc/>
    public int Count => this.dictionary.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).IsReadOnly;

    /// <inheritdoc/>
    public void Add(TKey key, TValue value)
    {
        if (this.dictionary.Count >= this.capacity)
        {
            var oldestKey = this.keys.Dequeue();
            this.dictionary.Remove(oldestKey);
        }
        this.dictionary.Add(key, value);
        this.keys.Enqueue(key);
    } // public void Add (TKey key, TValue value)

    /// <inheritdoc/>
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        if (this.dictionary.Count >= this.capacity)
        {
            var oldestKey = this.keys.Dequeue();
            this.dictionary.Remove(oldestKey);
        }
        this.dictionary.Add(item.Key, item.Value);
        this.keys.Enqueue(item.Key);
    } // public void Add (KeyValuePair<TKey, TValue> item)

    /// <inheritdoc/>
    public void Clear()
    {
        this.dictionary.Clear();
        this.keys.Clear();
    } // public void Clear ()

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<TKey, TValue> item)
        => ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).Contains(item);

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
        => this.dictionary.ContainsKey(key);

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        => ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => this.dictionary.GetEnumerator();

    /// <inheritdoc/>
    public bool Remove(TKey key)
    {
        if (this.dictionary.Remove(key))
        {
            this.keys = new Queue<TKey>(this.keys.Where(k => !EqualityComparer<TKey>.Default.Equals(k, key)));
            return true;
        }
        return false;
    } // public bool Remove (TKey key)

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).Remove(item))
        {
            this.keys = new Queue<TKey>(this.keys.Where(k => !EqualityComparer<TKey>.Default.Equals(k, item.Key)));
            return true;
        }
        return false;
    } // public bool Remove (KeyValuePair<TKey, TValue> item)

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => this.dictionary.TryGetValue(key, out value);

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => this.dictionary.GetEnumerator();
} // internal class DictQueue<TKey, TValue> : IDictionary<TKey, TValue>
