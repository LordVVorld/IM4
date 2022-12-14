using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q1
{
    internal static class Program
    {
        private static int selectionSet = 100;
        private static Random randomize = new Random();
        private static double MC = 40;
        private static double SC = 10;

        static void Main(string[] args)
        {
            double[] ranVals = new double[selectionSet];
            for (int index = 0; index < selectionSet; index++)
            {
                ranVals[index] = Repeat(randomize.NextDouble, 12).Sum() - 6;
            };

            double mathExp = ranVals.Sum() / selectionSet;
            double disp = ranVals.Mutate(element => Math.Pow(element - mathExp, 2)).Sum() / selectionSet;
            double meanSqrDiv = Math.Sqrt(disp);
            double mathExpDiv = MC - mathExp;
            double meanSqrDivDiv = SC - meanSqrDiv;

            int stepCount = (int)((ranVals.Max() - ranVals.Min()) / (1 + 3.3221 * Math.Log(selectionSet)));
            double step = (ranVals.Max() - ranVals.Min()) / stepCount;
            Array.Sort(ranVals);

            //Array groups = new double[stepCount];
        }

        public static T[] Repeat<T>(Func<T> operation, int count)
        {
            T[] result = new T[count];
            for (int index = 0; index < count; index++)
            {
                result.Append(operation());
            }
            return result;
        }
        public static double[] Mutate(this double[] array, Func<double, double> mutator)
        {
            var result = new double[array.Length];
            for (int index = 0; index < array.Length; index++)
            {
                result[index] = mutator(array[index]);
            }
            return result;
        }
    }
}
