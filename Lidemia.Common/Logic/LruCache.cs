// Created on 26/11/2026 15:57 by Laserson

using System.Diagnostics.CodeAnalysis;

namespace Lidemia.Common.Logic;

public sealed class LruCache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, LinkedListNode<Entry>> _map;
    private readonly LinkedList<Entry> _recency = new(); // head = most recently used

    private readonly struct Entry
    {
        public readonly TKey Key;
        public readonly TValue Value;

        public Entry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public LruCache(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity,
                "Capacity must be greater than zero.");

        _capacity = capacity;
        _map = new Dictionary<TKey, LinkedListNode<Entry>>(capacity);
    }

    public int Count => _map.Count;
    public int Capacity => _capacity;

    /// <summary>
    /// Looks up a key and marks it as most recently used.
    /// </summary>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (_map.TryGetValue(key, out var node))
        {
            Touch(node);
            value = node.Value.Value;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Gets or sets an entry. The getter throws if the key is absent.
    /// </summary>
    public TValue this[TKey key]
    {
        get => TryGetValue(key, out var v)
            ? v
            : throw new KeyNotFoundException($"Key '{key}' was not found in the cache.");
        set => Set(key, value);
    }

    /// <summary>
    /// Inserts or updates a key, marking it as most recently used and
    /// evicting the least recently used entry if the cache is full.
    /// </summary>
    public void Set(TKey key, TValue value)
    {
        if (_map.TryGetValue(key, out var existing))
        {
            existing.Value = new Entry(key, value);
            Touch(existing);
            return;
        }

        if (_map.Count == _capacity)
        {
            // Evict least recently used (tail).
            var oldest = _recency.Last!;
            _recency.RemoveLast();
            _map.Remove(oldest.Value.Key);
        }

        var node = _recency.AddFirst(new Entry(key, value));
        _map.Add(key, node);
    }

    /// <summary>
    /// Returns the cached value, or creates, stores, and returns a new one.
    /// </summary>
    public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
    {
        if (TryGetValue(key, out var cached))
            return cached;

        var created = factory(key);
        Set(key, created);
        return created;
    }

    public bool Remove(TKey key)
    {
        if (!_map.Remove(key, out var node))
            return false;

        _recency.Remove(node);
        return true;
    }

    public void Clear()
    {
        _map.Clear();
        _recency.Clear();
    }

    // Move an existing node to the head (most recently used position).
    private void Touch(LinkedListNode<Entry> node)
    {
        if (node.List is not null && node.Previous is null)
            return; // already at the head — avoid pointless churn

        _recency.Remove(node);
        _recency.AddFirst(node);
    }
}