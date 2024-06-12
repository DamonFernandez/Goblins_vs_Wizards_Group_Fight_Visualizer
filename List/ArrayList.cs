namespace COIS2020.DamonFernandez0813575.Assignments;

using System;
using System.Collections;
using System.Collections.Generic;
using COIS2020.StarterCode.Assignments;

public class ArrayList<T> : ISriList<T> where T : IComparable<T>, IEquatable<T>
{
    /// <summary>
    /// How many items an ArrayList should have space for when it is first constructed.
    /// </summary>
    private const int DEFAULT_CAPACITY = 4;

    /// <summary>
    /// Internal buffer of items.
    /// </summary>
    protected T[] buffer;
    protected T[] tempBuffer;

    // The number of items currently in the list, AKA the number of `buffer` slots that currently have an item in them.
    public int Count { get; protected set; }

    /// <summary>
    /// The total number of items this list's internal buffer has space for. When <c>Capacity</c> equals
    /// <see cref="Count"/>, an insertion will trigger a resize/reallocation.
    /// </summary>
    public int Capacity { get => buffer.Length; }


    /// <summary>
    /// Creates a new ArrayList.
    /// </summary>
    public ArrayList()
    {
        buffer = new T[DEFAULT_CAPACITY];
        Count = 0;
    }

    /// <summary>
    /// Creates a new ArrayList with a specified pre-allocated capacity.
    /// </summary>
    ///
    /// <param name="capacity">How many items to reserve space for.</param>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="capacity"/> is less than or equal to zero.
    /// </exception>
    public ArrayList(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        buffer = new T[capacity];
        Count = 0;
    }


    protected void grow()
    {
        T[] tempBuffer = new T[Count * 2];


        for (int i = 0; i < Count; i++)
        {
            tempBuffer[i] = buffer[i];
        }
        buffer = tempBuffer;
    }


    public T Get(int index)
    {
        if (index < 0 || index > Count)
        {
            throw new ArgumentOutOfRangeException();
        }

        return buffer[index];
    }



    public int FindIndex(T item)
    {

        for (int i = 0; i < Count; i++)
        {
            if (buffer[i].Equals(item))
            {
                return i;
            }
        }

        return -1;

    }

    public void InsertAt(int index, T item)
    {

        if (Count >= buffer.Length)
        {
            grow();
        }
        if (index < 0 || index > Count)
        {
            throw new ArgumentOutOfRangeException();
        }


        for (int i = Count; i > index; i--)
        {
            buffer[i] = buffer[i - 1];
        }




        buffer[index] = item;
        Count++;


    }

    public void AddBack(T item)
    {
        InsertAt(Count, item);
    }

    public void AddFront(T item)
    {
        InsertAt(0, item);
    }

    public T RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
        {
            throw new ArgumentOutOfRangeException();
        }

        T[] tempBuffer = new T[Count - 1];
        T removedItem = buffer[index];
        int bufferIteratorVar = 0;

        for (int i = 0; i < Count; i++)
        {
            if (i != index)

            {
                tempBuffer[bufferIteratorVar] = buffer[i];
                bufferIteratorVar++;
            }
        }

        buffer = tempBuffer;
        Count--;


        return removedItem;

    }

    public T RemoveFirst()
    {
        return RemoveAt(0);
    }

    public T RemoveLast()
    {
        return RemoveAt(Count - 1);
    }

    public void Clear()
    {
        buffer = new T[DEFAULT_CAPACITY];
        Count = 0;

    }

    public void Swap(int index1, int index2)
    {
        T tempVar = buffer[index1];
        buffer[index1] = buffer[index2];
        buffer[index2] = tempVar;
    }

    public void RotateLeft()
    {

        if (Count > 1)
        {


            T firstElement = buffer[0];


            for (int i = 0; i < Count - 1; i++)
            {
                buffer[i] = buffer[i + 1];
            }


            buffer[Count - 1] = firstElement;

        }

    }

    public void RotateRight()
    {
        if (Count > 1)
        {


            T lastElement = buffer[Count - 1];


            for (int i = Count - 1; i > 0; i--)
            {
                buffer[i] = buffer[i - 1];
            }


            buffer[0] = lastElement;

        }
    }

    public void Sort()
    {
        int n = Count;
        bool swapped = false;

        for (int i = 0; i < n - 1; i++)
        {
            for (int y = 0; y < n - 1 - i; y++)
            {
                if (buffer[y].CompareTo(buffer[y + 1]) < 0)
                {
                    Swap(y, y + 1);
                    swapped = true;
                }
            }
            if (swapped == false)
            {
                break;
            }
        }

    }


    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return buffer[i];

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
