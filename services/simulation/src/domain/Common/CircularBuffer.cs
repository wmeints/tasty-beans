using System.Collections;

namespace TastyBeans.Simulation.Domain.Common;

public class CircularBuffer<T>: IEnumerable<T>
{
    private int _size;
    private int _nextPosition;
    private T[] _buffer;
    private int _itemCount;

    public CircularBuffer(int size)
    {
        _buffer = new T[size];
        _nextPosition = 0;
        _itemCount = 0;
        _size = size;
    }

    public void Add(T item)
    {
        _buffer[_nextPosition] = item;
        
        _itemCount = Math.Min(_itemCount + 1, _size);
        _nextPosition = (_nextPosition + 1) % _size;
    }

    public int MaxSize => _size;
    public int Count => _itemCount;

    public T this[int index] => _buffer[index];
    public IEnumerator<T> GetEnumerator()
    {
        return new List<T>(_buffer).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}