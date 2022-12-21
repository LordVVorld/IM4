using System;
using System.Collections.Generic;
using System.Linq;

namespace Q1
{
    public static class Extentions
    {
        public static double[] Repeat(this Func<double> operation, int count)
        {
            double[] result = new double[count];
            for (int index = 0; index < count; ++index)
            {
                result[index] = operation();
            }
            return result;
        }
    }
}
