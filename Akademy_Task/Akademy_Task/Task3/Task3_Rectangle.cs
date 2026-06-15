using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akademy_Task.Task3
{
    internal class Task3_Rectangle
    {
        private double _x;
        private double _y;
        private double _width;
        private double _height;

        /// <summary>
        /// Конструктор класса Rectangle.
        /// </summary>
        /// <param name="x">Координата X левого верхнего угла.</param>
        /// <param name="y">Координата Y левого верхнего угла.</param>
        /// <param name="width">Ширина прямоугольника (положительное число).</param>
        /// <param name="height">Высота прямоугольника (положительное число).</param>
        public Task3_Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Координата X левого верхнего угла.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Координата Y левого верхнего угла.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Ширина прямоугольника. Не может быть отрицательной или нулевой.
        /// </summary>
        public double Width
        {
            get => _width;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Ширина должна быть положительным числом.", nameof(value));
                }
                _width = value;
            }
        }

        /// <summary>
        /// Высота прямоугольника. Не может быть отрицательной или нулевой.
        /// </summary>
        public double Height
        {
            get => _height;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Высота должна быть положительным числом.", nameof(value));
                }
                _height = value;
            }
        }

        /// <summary>
        /// Периметр прямоугольника (только для чтения).
        /// </summary>
        public double Perimeter
        {
            get => 2 * (_width + _height);
        }

        /// <summary>
        /// Площадь прямоугольника (только для чтения).
        /// </summary>
        public double Area
        {
            get => _width * _height;
        }

        /// <summary>
        /// Возвращает строковое представление прямоугольника.
        /// </summary>
        public override string ToString()
        {
            return $"Прямоугольник: левый верхний угол ({_x}, {_y}), ширина = {_width:F2}, высота = {_height:F2}, площадь = {Area:F2}, периметр = {Perimeter:F2}";
        }

        /// <summary>
        /// Выводит подробную информацию о прямоугольнике.
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine(ToString());
        }
    }
}
