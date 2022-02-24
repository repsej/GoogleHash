using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace HashCode22Solution
{
    class HashCode
    {
        public static void Main()
        {
            string inputPath = "../../../Input/";
            string outputPath = "../../../Output/";
            int N = 1; // Solve only the first N problems
            var files = Directory.GetFiles(inputPath).Take(N);

            var workers = new List<Thread>();

            foreach (var path in files)
            {
                var problem = new Problem(path);
                var t = new Thread(() => problem.Calculate(), 100000000);

                workers.Add(t);
                t.Start();
                t.Join();
            }

            foreach (var thread in workers)
            {
                thread.Join();
            }
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

        public class Problem
        {
            public (int L, int G, int S, int B, int F, int N) P;
            public (string N, int B)[] SD;
            public (string N, int M, int D, int U, string[] S)[] Features;
            public Problem(string path)
            {
                // L G S B F N - problem
                // N B - Service description -
                // N M D U newline S[] -

                var pr = new ProblemReader(path);
                pr.Read(out P);
                pr.Read(out SD, P.S);

                Features = new (string N, int M, int D, int U, string[] S)[P.F];
                for (int i = 0; i < P.F; i++)
                {
                    ref var f = ref Features[i];
                    pr.Read(out f.N);
                    pr.Read(out f.M);
                    pr.Read(out f.D);
                    pr.Read(out f.U);
                    pr.Read(out f.S, f.M);
                }
            }

            internal void Calculate()
            {
                // Solve problem
            }
        }
        public static IEnumerable<string> ReadLinesFile(string filename)
        {
            string line;
            // Read the file and display it line by line.
            StreamReader file = new StreamReader(filename);
            while ((line = file.ReadLine()) != null)
            {
                foreach (var word in line.Split(' '))
                {
                    yield return word;
                }
            }
            file.Close();
        }

        public static IEnumerable<object[]> Probread(string fileName)
        {
            StringMap stringMap = new StringMap();

            //return Read(fileName).ToArray();
            //IEnumerable<int[]> Read(string fileName)

            {
                foreach (var line in ReadLinesFile(fileName))
                {
                    var tokens = line.Split(" ");
                    var lineresult = new object[tokens.Length];
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        var token = tokens[i];
                        if (int.TryParse(token, out int number))
                        {
                            lineresult[i] = number;
                        }
                        else
                        {
                            lineresult[i] = token;
                        }
                    }
                    yield return lineresult;
                }
            }
        }
        public static IEnumerable<T> Repeat<T>(int C, Func<T> A)
        {
            for (int i = 0; i < C; i++)
            {
                yield return A();
            }
        }
        public class ProblemReader
        {
            StringMap SM = new StringMap();
            string[] _tokens;
            int _pos = 0;

            public (T0, T1) RV<T0, T1>()
            {
                (T0, T1) v;
                Read(out v);
                return v;
            }

            public void Read<T0>(out T0 v)
            {
                if (typeof(T0) == typeof(int))
                {
                    v = (T0)Convert.ChangeType(I(), typeof(T0));
                }
                else
                {
                    v = (T0)Convert.ChangeType(O(), typeof(T0));
                }
            }

            public void Read<T0, T1>(out (T0 v0, T1 v1) tup)
            {
                Read(out tup.v0);
                Read(out tup.v1);
            }

            public void Read<T0, T1, T2>(out (T0 v0, T1 v1, T2 v2) tup)
            {
                Read(out tup.v0);
                Read(out tup.v1);
                Read(out tup.v2);
            }

            public void Read<T0, T1, T2, T3>(out (T0 v0, T1 v1, T2 v2, T3 v3) tup)
            {
                Read(out tup.v0);
                Read(out tup.v1);
                Read(out tup.v2);
                Read(out tup.v3);
            }

            public void Read<T0, T1, T2, T3, T4>(out (T0 v0, T1 v1, T2 v2, T3 v3, T4 v4) tup)
            {
                Read(out tup.v0);
                Read(out tup.v1);
                Read(out tup.v2);
                Read(out tup.v3);
                Read(out tup.v4);
            }

            public void Read<T0, T1, T2, T3, T4, T5>(out (T0 v0, T1 v1, T2 v2, T3 v3, T4 v4, T5 v5) tup)
            {
                Read(out tup.v0);
                Read(out tup.v1);
                Read(out tup.v2);
                Read(out tup.v3);
                Read(out tup.v4);
                Read(out tup.v5);
            }

            internal void Read<T0>(out T0[] tarr, int s)
            {
                tarr = new T0[s];
                for (int i = 0; i < s; i++)
                    Read(out tarr[i]);
            }

            internal void Read<T0, T1>(out (T0, T1)[] tarr, int s)
            {
                tarr = new (T0, T1)[s];
                for (int i = 0; i < s; i++)
                    Read(out tarr[i]);
            }

            internal void Read<T0, T1, T2>(out (T0, T1, T2)[] tarr, int s)
            {
                tarr = new (T0, T1, T2)[s];
                for (int i = 0; i < s; i++)
                    Read(out tarr[i]);
            }
            internal void Read<T0, T1, T2, T3>(out (T0, T1, T2, T3)[] tarr, int s)
            {
                tarr = new (T0, T1, T2, T3)[s];
                for (int i = 0; i < s; i++)
                    Read(out tarr[i]);
            }

            public object O()
            {
                var token = S();
                if (int.TryParse(token, out int number))
                    return number;
                return token;
            }
            public string S()
            {
                return _tokens[_pos++];
            }

            public int I()
            {
                var token = S();
                if (int.TryParse(token, out int number))
                {
                    return number;
                }
                else
                {
                    return SM.Add(token);
                }
            }

            public ProblemReader(string path)
            {
                _tokens = ReadTokens(path).ToArray();
            }
            public static IEnumerable<string> ReadTokens(string filename)
            {
                string line;
                StreamReader file = new StreamReader(filename);
                while ((line = file.ReadLine()) != null)
                {
                    foreach (var word in line.Split(' '))
                    {
                        yield return word;
                    }
                }
                file.Close();
            }
        }

        public class StringMap
        {
            private List<string> _strings = new List<string>();
            private Dictionary<string, int> _index = new Dictionary<string, int>();
            private int _size = 0;
            public int Add(string key)
            {
                if (_index.TryGetValue(key, out int val))
                    return val;

                _strings.Add(key);
                _index.Add(key, _size);
                _size++;
                return _size - 1;
            }

            public int Lookup(string key)
            {
                return _index[key];
            }

            public string Lookup(int index)
            {
                return _strings[index];
            }
        }
    }
}
