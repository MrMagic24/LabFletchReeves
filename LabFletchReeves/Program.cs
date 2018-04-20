﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabFletchReeves
{
    class Program
    {
        static void Main(string[] args)
        {
            double x1, x2, e1, e2;
            int m;

            x1 = -3;
            x2 = 10;
            e1 = 0.1;
            e2 = 0.15;
            m = 10;

            var method = new MainMethod();

            Console.WriteLine("Метод Флетчера-Ривса");

            var result = method.FindMinValue(x1, x2, e1, e2, m);

            Console.WriteLine("Минимальное значение функции равно {0}\nТочка [{1}, {2}]\n" +
                              "Количество итераций: {3}\n",
                result.Func, result.Arg.X, result.Arg.Y, result.Iterations);

            Console.ReadKey();
        }
    }
}
