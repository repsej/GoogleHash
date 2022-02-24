using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace HashCode22Solution
{
    partial class HashCode
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
                Console.WriteLine("Reading:" + path);
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
    }
}
