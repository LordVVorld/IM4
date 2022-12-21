using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Q1
{
    internal static class Program
    {
        private static int selectionSet = 100;
        private static double MC = 40;
        private static double SC = 10;
        private static readonly Dictionary<int, double> PearsonTable = new Dictionary<int, double>{
            { 1, 3.8 }, { 2, 6.0 }, { 3, 7.8 }, { 4, 9.5 },
            { 5, 11.1 }, { 6, 12.6}, {7, 14.1}, {8 , 15.5}
        };

        static void Main(string[] args)
        {
            //Генерация случайной величины и разбиение её значений по интервалам Стрёрджерса
            RandomVariable variable = new RandomVariable(selectionSet, MC, SC);     
            SturgessIntervals intervals = new SturgessIntervals(variable);

            //Нахождение выборочного среднего 
            double xMiddle = (double)intervals
                .Select(interval => interval.Middle * interval.Count).Sum()
                / intervals.Select(interval => interval.Count).Sum();

            //Выделение списка частот и нахождение списка теоретических частот
            List<double> frequencies = intervals.Select(interval => (double)interval.Count).ToList();
            List<double> theorFrequencies = GetTheorFrequancies(intervals, xMiddle, variable.Sigma);

            //Блок выводов сигмы и мат.ожидания
            Console.WriteLine($"Теор. знач. мат.ожидания: {MC}");
            Console.WriteLine($"Теор. знач. сигмы: {SC}");
            Console.WriteLine($"Оценка мат.ожидания: {variable.MathExpectation}");
            Console.WriteLine($"Оценка сигмы: {variable.Sigma}");
            Console.WriteLine($"Ошибка мат.ожидания: {Math.Round(MC - variable.MathExpectation, 2)}");
            Console.WriteLine($"Ошибка дисперсии: {Math.Round(SC - variable.Sigma, 2)}\n");

            //Вывод интервалов и частот
            Console.WriteLine($"С шагом {intervals.Step} образовано {intervals.Count} интервалов:");
            intervals.ForEach(interval =>
            {
                Console.WriteLine($"{interval.Index + 1}) [{interval.LowerBorder}-{interval.UpperBorder}], " +
                    $"n = {interval.Count}, n' = {theorFrequencies[interval.Index]}");
            });
            Console.WriteLine();

            //Объединение малочисленных частот (в т.ч. теоретических)
            while(frequencies.Any(value => value < 5))
            {
                for (int index = 0; index < frequencies.Count / 2; index++)
                {
                    if (frequencies[index] < 5)
                    {
                        frequencies[index + 1] += frequencies[index];
                        frequencies.Remove(frequencies[index]);
                        theorFrequencies[index + 1] += theorFrequencies[index];
                        theorFrequencies.Remove(theorFrequencies[index]);
                        break;
                    }
                }
                for (int index = frequencies.Count - 1; index > 0; --index)
                {
                    if (frequencies[index] < 5)
                    {
                        frequencies[index - 1] += frequencies[index];
                        frequencies.Remove(frequencies[index]);
                        theorFrequencies[index - 1] += theorFrequencies[index];
                        theorFrequencies.Remove(theorFrequencies[index]);
                        break;
                    }
                }
            }

            //Вывод частот объединённых интервалов
            Console.WriteLine($"В результате объединения малочисленных частот образовано {frequencies.Count} интервалов:");
            for (int index = 0; index < frequencies.Count; index++)
            {
                Console.WriteLine($"{index + 1}) n = {frequencies[index]}, n' = {theorFrequencies[index]}");
            }
            Console.WriteLine();

            //Нахождение наблюдаемого и критического значения критерия Пирсона, выделение числа степеней свободы
            double Hi2Observed = 0;
            for (int index = 0; index < frequencies.Count; index++)
            {
                Hi2Observed += Math.Pow(frequencies[index] - theorFrequencies[index], 2) / theorFrequencies[index];
            }
            Hi2Observed = Math.Round(Hi2Observed, 3);
            int degreesOfFreedom = frequencies.Count - 3;
            double Hi2Crit = PearsonTable[frequencies.Count - 2];

            //Вывод итогов
            Console.WriteLine($"При числе степеней свободы {degreesOfFreedom} и уровне значимости 0,05:\n" +
                $"X2набл = {Hi2Observed}, X2крит = {Hi2Crit}\n" +
                $"Следовательно гипотеза {((Hi2Observed < Hi2Crit) ? "принимается" : "отвергается")}");
        }

        private static List<double> GetTheorFrequancies(SturgessIntervals intervals, double xMiddle, double sigma)
        {
            List<double> result = new List<double>();
            for (int index = 0; index < intervals.Count; index++)
            {
                result.Add(new double());
                if (intervals[index].Count != 0)
                {
                    result[index] = CalculateFrequancy(
                            intervals[index].Middle,
                            intervals.Step,
                            sigma,
                            xMiddle
                        );
                }
            }
            return result;
        }

        private static double CalculateFrequancy(double xi, double step, double sigma, double xMiddle)
        {
            double Ui = (xi - xMiddle) / sigma;
            double fi = Math.Exp(-1 * Math.Pow(Ui, 2) / 2) / (Math.Sqrt(2 * Math.PI));
            return fi * selectionSet * step / sigma;
        }
    }
}