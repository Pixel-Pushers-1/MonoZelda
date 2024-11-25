using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoZelda.Link.Equippables;

public class InsertionOrderedList<T> : IEnumerable<T>
{
    private readonly List<T?> items; // Maintains items with placeholders for removed ones
    private readonly Dictionary<T, int> indexMap; // Tracks the original insertion index
    private int count;

    public InsertionOrderedList()
    {
        items = new List<T?>();
        indexMap = new Dictionary<T, int>();
    }

    public int Count
    {
        get => items.Count;
    }

    public T? this[int index]
    {
        get
        {
            if (index < 0 || index >= items.Count)
                throw new IndexOutOfRangeException("Index is out of bounds.");

            return items[index];
        }
        set
        {
            if (index < 0 || index >= items.Count)
                throw new IndexOutOfRangeException("Index is out of bounds.");

            items[index] = value;
        }
    }

    public void Add(T item)
    {
        if (indexMap.ContainsKey(item))
        {
            // If the item exists, place it back in its original position
            int originalIndex = indexMap[item];
            if (items[originalIndex] == null) // Only reinsert if it was removed
                items[originalIndex] = item;
        }
        else
        {
            // Assign a new index and append it to the list
            int newIndex = items.Count;
            indexMap[item] = newIndex;
            items.Add(item);
        }
    }

    public void Remove(T item)
    {
        if (indexMap.TryGetValue(item, out int index))
        {
            items[index] = default; // Placeholder for removed item
        }
    }

    public bool Contains(T item)
    {
        return items.Contains(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in items)
        {
            if (item != null)
                yield return item!;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
