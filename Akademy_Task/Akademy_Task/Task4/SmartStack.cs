using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akademy_Task.Task4
{
    /// <summary>
    /// Реализация стека с дополнительными возможностями.
    /// </summary>
    /// <typeparam name="T">Тип хранимых элементов.</typeparam>
    internal class SmartStack<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;

        /// <summary>
        /// Конструктор без параметров. Создаёт массив ёмкостью 4 элемента.
        /// </summary>
        public SmartStack()
        {
            _items = new T[4];
            _count = 0;
        }

        /// <summary>
        /// Конструктор с указанием начальной ёмкости.
        /// </summary>
        /// <param name="capacity">Начальная ёмкость стека.</param>
        public SmartStack(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Ёмкость должна быть положительным числом.", nameof(capacity));
            }
            _items = new T[capacity];
            _count = 0;
        }

        /// <summary>
        /// Конструктор, принимающий коллекцию элементов.
        /// </summary>
        /// <param name="collection">Коллекция элементов.</param>
        public SmartStack(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "Коллекция не может быть null.");
            }

            var tempList = new List<T>(collection);
            _items = new T[tempList.Count == 0 ? 4 : tempList.Count];
            _count = 0;

            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                Push(tempList[i]);
            }
        }

        /// <summary>
        /// Количество элементов в стеке.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Ёмкость внутреннего массива.
        /// </summary>
        public int Capacity => _items.Length;

        /// <summary>
        /// Добавляет элемент на вершину стека.
        /// </summary>
        /// <param name="item">Добавляемый элемент.</param>
        public void Push(T item)
        {
            if (_count == _items.Length)
            {
                Resize(_items.Length * 2);
            }
            _items[_count++] = item;
        }

        /// <summary>
        /// Добавляет коллекцию элементов на вершину стека.
        /// Последний элемент коллекции станет вершиной стека.
        /// </summary>
        /// <param name="collection">Коллекция элементов.</param>
        public void PushRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "Коллекция не может быть null.");
            }

            var tempList = new List<T>(collection);
            int neededSpace = tempList.Count;

            if (_count + neededSpace > _items.Length)
            {
                int newCapacity = _items.Length;
                while (newCapacity < _count + neededSpace)
                {
                    newCapacity *= 2;
                }
                Resize(newCapacity);
            }

            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                _items[_count++] = tempList[i];
            }
        }

        /// <summary>
        /// Удаляет и возвращает элемент с вершины стека.
        /// </summary>
        /// <returns>Элемент с вершины стека.</returns>
        public T Pop()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек пуст. Невозможно выполнить операцию Pop.");
            }
            T item = _items[--_count];
            _items[_count] = default(T);
            return item;
        }

        /// <summary>
        /// Возвращает элемент с вершины стека без удаления.
        /// </summary>
        /// <returns>Элемент с вершины стека.</returns>
        public T Peek()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек пуст. Невозможно выполнить операцию Peek.");
            }
            return _items[_count - 1];
        }

        /// <summary>
        /// Проверяет наличие элемента в стеке.
        /// </summary>
        /// <param name="item">Искомый элемент.</param>
        /// <returns>true, если элемент найден; иначе false.</returns>
        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (Equals(_items[i], item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Индексатор для доступа к элементу по глубине (0 — вершина).
        /// </summary>
        /// <param name="index">Глубина элемента (0 — вершина).</param>
        /// <returns>Элемент на указанной глубине.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Индекс вышел за границы стека.");
                }
                return _items[_count - 1 - index];
            }
        }

        /// <summary>
        /// Возвращает перечислитель, обходящий стек от вершины к основанию.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Изменяет ёмкость внутреннего массива.
        /// </summary>
        private void Resize(int newCapacity)
        {
            T[] newArray = new T[newCapacity];
            Array.Copy(_items, 0, newArray, 0, _count);
            _items = newArray;
        }
    }
}
