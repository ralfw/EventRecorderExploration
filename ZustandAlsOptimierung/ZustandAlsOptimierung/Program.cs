using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZustandAlsOptimierung
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Aggregator();
            a.Sum(1);
            a.Sum(2);
            Console.WriteLine(a.Sum(3));

            var sum = 0;
            sum = a.Sum(1, sum);
            sum = a.Sum(2, sum);
            Console.WriteLine(a.Sum(3, sum));

            var r = a.Sum(1, new int[] {});
            r = a.Sum(2, r.Item2);
            r = a.Sum(3, r.Item2);
            Console.WriteLine(r.Item1);

            a.SumWithHistory(1);
            a.SumWithHistory(2);
            Console.WriteLine(a.SumWithHistory(3));

            Console.WriteLine(a.AvgWithHistory(4));
        }
    }


    class Aggregator
    {
        private int _sum;

        public int Sum(int i)
        {
            _sum += i;
            return _sum;
        }


        public int Sum(int i, int z)
        {
            return z + i;
        }


        public Tuple<int, IEnumerable<int>> Sum(int i, IEnumerable<int> history)
        {
            var sum = history.Sum() + i;
            var extended_history = history.Concat(new[] {i});
            return new Tuple<int, IEnumerable<int>>(sum, extended_history);
        }


        private List<int> _history = new List<int>();

        public int SumWithHistory(int i)
        {
            _history.Add(i);
            return _history.Sum();
        }

        public double AvgWithHistory(int i)
        {
            _history.Add(i);
            return _history.Average();
        }
    }
}
