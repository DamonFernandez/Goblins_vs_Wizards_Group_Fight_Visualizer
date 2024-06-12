namespace COIS2020.StarterCode.Assignments;

using System;
using System.Collections.Generic;

/// <summary>   
/// Defines behaviour for an ordered, sortable collection of elements without a fixed size limit.
/// </summary>
///
/// <typeparam name="T">The type of elements in the list.</typeparam>
public interface ISriList<T> : IEnumerable<T> where T : IComparable<T>, IEquatable<T>
{
    /// <summary>
    /// Gets the total number of elements in this list.
    /// </summary>
    int Count { get; }


    /// <summary>
    /// Retrieves the item currently stored at a given index.
    /// </summary>
    ///
    /// <param name="index">The zero-based index of the item to get.</param>
    /// 
    /// <returns>The element at the specified index.</returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para>If <paramref name="index"/> is less than zero.</para>
    /// -or-
    /// <para>If <paramref name="index"/> is greater than or equal to <see cref="Count" />.</para>
    /// </exception>
    T Get(int index);


    /// <summary>
    /// Searches the list for a given item.
    /// </summary>
    ///
    /// <param name="item">The item to search for.</param>
    ///
    /// <returns>
    /// The index of the first occurrence of <paramref name="item"/> in the list, or -1 if not found.
    /// </returns>
    int FindIndex(T item);


    /// <summary>
    /// Inserts an item at a specific location within the list.
    /// </summary>
    ///
    /// <param name="index">The zero-based index at which to insert the item.</param>
    /// <param name="item">The item to be inserted.</param>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para>If <paramref name="index"/> is less than zero.</para>
    /// -or-
    /// <para>If <paramref name="index"/> is greater than <see cref="Count" />.</para>
    /// </exception>
    ///
    /// <remarks>
    /// Even though it is technically out of range, it is valid to call this method with an <paramref name="index"/>
    /// equal to <see cref="Count"/>; doing so is equivalent to <see cref="AddBack">adding to the end of the list</see>.
    /// </remarks>
    void InsertAt(int index, T item);


    /// <summary>
    /// Adds an item to the end of the list.
    /// </summary>
    ///
    /// <param name="item">The item to be added.</param>
    void AddBack(T item);


    /// <summary>
    /// Adds an item to the beginning of the list.
    /// </summary>
    ///
    /// <param name="item">The item to be added.</param>
    void AddFront(T item);


    /// <summary>
    /// Removes the item at the specified index from the list and returns it.
    /// </summary>
    ///
    /// <param name="index">The zero-based index of the item to remove from the list.</param>
    ///
    /// <returns>
    /// The item which was previously at the specified index.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para>If <paramref name="index"/> is less than zero.</para>
    /// -or-
    /// <para>If <paramref name="index"/> is greater than or equal to <see cref="Count" />.</para>
    /// </exception>
    T RemoveAt(int index);


    /// <summary>
    /// Removes the first item from the list and returns it.
    /// </summary>
    ///
    /// <returns>
    /// The first item in the list.
    /// </returns>
    ///
    /// <exception cref="InvalidOperationException">
    /// If <see cref="Count" /> is zero.
    /// </exception>
    T RemoveFirst();


    /// <summary>
    /// Removes the last item from the list and returns it.
    /// </summary>
    ///
    /// <returns>
    /// The last item in the list.
    /// </returns>
    ///
    /// <exception cref="InvalidOperationException">
    /// If <see cref="Count" /> is zero.
    /// </exception>
    T RemoveLast();


    /// <summary>
    /// Removes all items from the list.
    /// </summary>
    ///
    /// <remarks>
    /// The list's <see cref="Count"/> will be zero after this operation completes, but some internal properties may be
    /// maintained (e.g. an already-allocated capacity needn't be shrunk).
    /// </remarks>
    void Clear();


    /// <summary>
    /// Switches the positions of two items in the list.
    /// </summary>
    ///
    /// <param name="index1">The index of the first item.</param>
    /// <param name="index2">The index of the second item.</param>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para>If either <paramref name="index1"/> or <paramref name="index2"/> is less than zero.</para>
    /// -or-
    /// <para>If either <paramref name="index1"/> or <paramref name="index2"/> is greater than or equal to
    /// <see cref="Count" />.</para>
    /// </exception>
    void Swap(int index1, int index2);


    /// <summary>
    /// Shifts all items in the list towards the beginning, wrapping around; the first item becomes the last.
    /// </summary>
    ///
    /// <remarks>
    /// Does nothing if <see cref="Count"/> is less than or equal to one.
    /// </remarks>
    void RotateLeft();


    /// <summary>
    /// Shifts all items in the list towards the end, wrapping around; the last item becomes the first.
    /// </summary>
    ///
    /// <remarks>
    /// Does nothing if <see cref="Count"/> is less than or equal to one.
    /// </remarks>
    void RotateRight();


    /// <summary>
    /// Sorts the items in this list according to their implementation of <see cref="IComparable{T}"/>.
    /// </summary>
    void Sort();
}
