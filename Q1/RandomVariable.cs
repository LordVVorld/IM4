using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Q1
{
    public class RandomVariable : List<double>
    {
        public double MathExpectation { get; private set; }
        public double Dispersion { get; private set; }
        public double Sigma { get; private set; }

        public RandomVariable(int valuesCount, double Mx = 0, double sigma = 1)
        {
            for (int index = 0; index < valuesCount; ++index)
            {
                Thread.Sleep(1);
                Add(GenerateRandomValue(Mx, sigma));
            }

            Sort();

            MathExpectation = Math.Round(this.Sum() / valuesCount, 2);

            Dispersion = Math.Round(this
                .Select(value => Math.Pow(value - MathExpectation, 2))
                .Sum() / (valuesCount - 1), 2);

            Sigma = Math.Sqrt(Dispersion);
        }

        private double GenerateRandomValue(double Mx, double sigma)
        {
            double result = new Func<double>(new Random().NextDouble).Repeat(12).Sum() - 6;
            return Math.Round(Mx + result * sigma, 2);
        }
    }
}
