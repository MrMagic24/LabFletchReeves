using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace LabFletchReeves
{
    public struct Response
    {
        public PointF Arg { get; set; }
        public double Func { get; set; }
        public int Iterations { get; set; }
    }

    class MainMethod
    {
        public double Function(PointF x)
        {
            return (Math.Pow(x.X - 2, 2) + Math.Pow(x.Y - 3, 2));
            //return (double)(2 * Math.Pow(x.X, 2) + x.X * x.Y + Math.Pow(x.Y, 2));
            //return (double)(Math.Pow(x.X - 2, 2) + Math.Pow(3 - x.Y, 2));
            //return (double)(Math.Pow(x.X, 2) + (5 * x.X * x.Y) + Math.Pow(x.Y, 2));
        }

        private double FunctionGrad1(PointF x)
        {
            // (x^2 - 4x + 4) + (y^2 - 6y + 9)
            //return 4 * x.X + x.Y;
            return 2 * x.X - 4;
            //return 2 * x.X + 5 * x.Y;
        }

        private double FunctionGrad2(PointF x)
        {
            //return x.X + 2 * x.Y;
            return 2 * x.Y - 6;
            //return 6 - 2 * x.Y;
            //return 5 * x.X + 2 * x.Y;
        }

        private double Grad(int i, PointF x)
        {
            switch (i)
            {
                case 1:
                    return FunctionGrad1(x);
                case 2:
                    return FunctionGrad2(x);
            }

            throw new ArgumentException("Ошибка!");
        }

        private double StepResolver(double x1, double x2, double c1, double c2)
        {
            return (4 * x1 * c1 + x1 * c2 + x2 * c1 + 2 * x2 * c2) /
                   (4 * c1 * c1 + 2 * c1 * c2 + 2 * c2 * c2);
        }

        private PointF _x, _xPrev;
        private double _e1, _e2, _t, _b;
        private int k, _m;
        private double[] fg1, fg0, d;

        public Response FindMinValue(double x1, double x2, double e1, double e2, int mPredel)
        {
            d = new double[2];
            _x = new PointF(Convert.ToSingle(x1), Convert.ToSingle(x2));
            _xPrev = _x;
            k = 0;
            _m = mPredel;
            _e1 = e1;
            _e2 = e2; 

            THREE:
            fg0 = new[]
            {
                Grad(1, _xPrev),
                Grad(2, _xPrev)
            };

            fg1 = new[]
            {
                Grad(1, _x),
                Grad(2, _x)
            };

            FOUR:
            double fgLength = (double)Math.Sqrt(fg1[0] * fg1[0] + fg1[1] * fg1[1]);

            if (fgLength < _e1)
            {
                goto END;
            }

            FIVE:
            if (k >= _m)
            {
                goto END;
            }

            if (k != 0)
            {
                goto SEVEN;
            }

            SIX:
            d = new[]
            {
                -Grad(1, _x),
                -Grad(2, _x)
            };

            SEVEN:
            double fg0Length = fg0[0] * fg0[0] + fg0[1] * fg0[1];
            double fg1Length = fg1[0] * fg1[0] + fg1[1] * fg1[1];

            _b = fg1Length / fg0Length;

            if (k == 0)
            {
                goto NINE;
            }           

            EIGHT:
            d = new[]
            {
                -Grad(1, _x) + _b * d[0],
                -Grad(2, _x) + _b * d[1]
            };

            NINE:
            _t = StepResolver(_x.X, _x.Y, -d[0], -d[1]);

            TEN:
            _xPrev = _x;

            PointF temp10 = new PointF
            {
                X = Convert.ToSingle(_x.X + _t * d[0]),
                Y = Convert.ToSingle(_x.Y + _t * d[1])
            };

            _x = temp10;

            ELEVEN:
            PointF temp11 = new PointF
            {
                X = _x.X - _xPrev.X,
                Y = _x.Y - _xPrev.Y
            };

            double tempL = (double)Math.Sqrt(temp11.X * temp11.X + temp11.Y * temp11.Y);
            double diffF = Function(_x) - Function(_xPrev);
            double F = Function(_x);
            double F1 = Function(_xPrev);

            if (tempL < _e2 && diffF < _e2)
            {
                goto END;
            }

            k++;
            goto THREE;

            END:
            return new Response
            {
                Arg = _x,
                Func = Function(_x),
                Iterations = k
            };
        }
    }
}
