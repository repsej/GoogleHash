using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace HashCode20Solution
{
    class Program
    {
        public static int Capacity;
        public static int NOfPizzas;
        public static int[] V;

        //// IO
        #region IO
        static void ReadProblem(string filename)
        {
            string text = File.ReadAllText(filename);
            var lines = text.Split('\n');

            var l0 = lines[0].Split(' ');

            Capacity = Int32.Parse(l0[0]);
            NOfPizzas = Int32.Parse(l0[1]);


            V = new int[NOfPizzas];

            var l1 = lines[1].Split(' ');
            int i = 0;
            foreach (var p in l1)
            {
                V[i++] = Int32.Parse(p);
//                Console.Write(v[i - 1] + ", ");
            }

            Console.WriteLine("-----------------");
            Console.WriteLine("Problem read: " + filename);
            Console.WriteLine("capacity = " + Capacity);
            Console.WriteLine("nOfPizzas = " + NOfPizzas);

            CacheInit();
        }

        static void WriteSolution(string filename, List<int> pizzas)
        {
            using (StreamWriter file = new StreamWriter(filename))
            {
                file.WriteLine(pizzas.Count);
                string p = string.Join(" ", pizzas);
                file.WriteLine(p);
            }

            Console.WriteLine($"Wrote solution to: {filename}");
        }
        #endregion

        //////// CACHE
        #region CACHE
        static int _cacheHits;

        static Dictionary< (int,int), (int, List<int>) > _cache;
        
        static void CacheInit()
        {
            _cacheHits = 0;
            _cache = new Dictionary<(int, int), (int, List<int>)>();
        }

        static void CacheStatistics()
        {
            Console.WriteLine("Cache size: " + _cache.Count);
            Console.WriteLine("Cache hits: " + _cacheHits);
            Console.WriteLine($"Cache size/prob size:  {100.0 * _cache.Count / ((float) NOfPizzas * ((float)Capacity + 1)):G2}%");
        }

        static bool CacheHasKey(int n, int c)
        {
            bool cacheHit = _cache.ContainsKey((n, c));
            if (cacheHit) _cacheHits++;
            return cacheHit;
        }

        static (int, List<int>) CacheGet(int n, int c)
        {
            return _cache[(n, c)];
        }

        static void CacheSet(int n, int c, (int, List<int>) t)
        {
            _cache.Add((n, c), (t.Item1, new List<int>(t.Item2)));
        }
        #endregion

        //// KNAPSACK
        #region KNAPSACK
        static (int, List<int>) KnapSack( int n, int c )
        { 
            if ( n == -1 || c == 0 ) return (0, new List<int>());

            if (CacheHasKey(n, c)) return CacheGet(n, c);

            int val;
            List<int> l;

            if (V[n] > c)
            {
                (val, l) = KnapSack(n - 1, c);
            }
            else
            {
                int t0, t1;
                List<int> l0, l1;

                (t0, l0) = KnapSack(n - 1, c);

                (t1, l1) = KnapSack(n - 1, c - V[n]);
                t1 += V[n];

                if (t0 > t1)
                {
                    val = t0;
                    l = l0;
                }
                else
                {
                    l1.Add(n);
                    val = t1;
                    l = l1;
                }
            }

            CacheSet(n,c,(val,l));

            return (val, l);
        }

        static int _greedyLimit = 200000;

        static (int, List<int>) KnapSack_Greedy(int n, int c)
        {
            if (n == -1 || c == 0) return (0, new List<int>());

            if (c < _greedyLimit) return KnapSack(n,c);

            int val;
            List<int> l;

            if (V[n] > c)
            {
                (val, l) = KnapSack_Greedy(n - 1, c);
            }
            else
            {
                (val, l) = KnapSack_Greedy(n - 1, c - V[n]);
                val += V[n];
                l.Add(n);
            }

            return (val, l);
        }
        #endregion
        

        static void Main()
        {
            var t = new Thread(()=> Startup(), 10000000);
            t.Start();
            t.Join();
        }

        static void Startup()
        {
            string basePath = @"input/";
            string outputPath = @"../../../output/";
            (string, int)[] problems =
            {
                ("a_example", 10000),
                ("b_small", 10000),
                ("c_medium", 10000),
                ("d_quite_big", 200000),
                ("e_also_big", 10000)
            };

            foreach ((string problem, int problemGreedyLimit) in problems)
            {
                ReadProblem(basePath+problem + ".in");

                _greedyLimit = problemGreedyLimit;
                Console.WriteLine("GreedyLimit = " + _greedyLimit);

                int score;
                List<int> pizzas;

                (score, pizzas) = KnapSack_Greedy(NOfPizzas - 1, Capacity);
                Console.WriteLine($"score = {score}  ({100.0 * score / Capacity:G3}%)");

                CacheStatistics();
                WriteSolution(outputPath+problem + ".out", pizzas);
            }

            //Console.WriteLine("\n    [Any key to close]");
            //Console.ReadKey(true);
        }
    }
}
