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
            var files = Directory.GetFiles(inputPath);

            var workers = new List<Thread>();

            foreach (var path in files)
            {
                Console.WriteLine("Reading:" + path);
                var problem = new Problem(path, path.Replace("Input", "Output"));
                var t = new Thread(() => problem.Calculate(), 100000000);
                                
                workers.Add(t);
                t.Start();
//                t.Join();
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
        /*
an integer C (1 ≤ C≤ 105) – the number of contributors,
an integer P (1 ≤ P ≤ 105) – the number of projects.
This is followed by C sections describing individual contributors. Each contributor is described by the following lines:

the first line contains:
the contributor's name (ASCII string of at most 20 characters, all of which are lowercase or uppercase English alphabet letters a-z and A-Z, or numbers 0-9),
an integer N (1≤ N ≤ 100) - the number of skills of the contributor.
the next N lines describe individual skills of the contributor. Each such line contains:
the name of the skill (ASCII string of at most 20 characters, all of which are lowercase or uppercase English alphabet letters a-z and A-Z, numbers 0-9, dashes '-' or pluses '+'),
an integer Li (1≤ Li ≤ 10) - skill level.

This is followed by P sections describing individual projects. Each project is described by the following lines:

the first line contains:
the name of the project (ASCII string of at most 20 characters, all of which are lowercase or uppercase English alphabet letters a-z and A-Z or numbers 0-9),
an integer Di (1 ≤Di ≤ 105) – the number of days it takes to complete the project,
an integer Si (1 ≤ Si ≤ 105) – the score awarded for project’s completion,
an integer Bi (1 ≤ Bi ≤ 105) – the “best before” day for the project,
an integer Ri (1 ≤ Ri ≤ 100) – the number of roles in the project.
the next Ri lines describe the skills in the project:
a string Xk – the name of the skill (ASCII string of at most 20 characters, all of which are lowercase or uppercase English alphabet letters a-z and A-Z, numbers 0-9, dashes '-' or pluses '+'),
an integer Lk (1≤Lk≤100) – the required skill level.
         */


        public class Problem
        {
            string outpath;
            int C; int P;
            (string CName, int N, Dictionary<string, int> Skills, int Available)[] Contributors;
            (string PName, int Days, int Score, int BestBefore, int R, (string SkillName, int Level)[] Skills)[] Projects;
            // (Name N (SkillName L)[N])[C] -- contributors
            // (Name D S B R (X L)[R])[P]

            public Problem(string path, string outpath)
            {
                this.outpath = outpath;
                // L G S B F N - problem
                // N B - Service description -
                // N M D U newline S[] -

                var pr = new ProblemReader(path);
                pr.Read(out C);
                pr.Read(out P);

                Contributors = new (string CName, int N, Dictionary<string, int> Skills, int)[C];
                for (int i = 0; i < C; i++)
                {
                    ref var c = ref Contributors[i];
                    pr.Read(out c.CName); pr.Read(out c.N);

                    c.Skills = new Dictionary<string, int>();
                    for (int j = 0; j < c.N; j++)
                        c.Skills.Add(pr.S(), pr.I());
                }

                Projects = new (string PName, int Days, int Score, int BestBefore, int R, (string SkillName, int Level)[])[P];

                var set = new HashSet<string>();
                for (int i = 0; i < P; i++)
                {
                    ref var p = ref Projects[i];
                    pr.Read(out p.PName);
                    pr.Read(out p.Days);
                    pr.Read(out p.Score);
                    pr.Read(out p.BestBefore);
                    pr.Read(out p.R);
                    pr.Read(out p.Skills, p.R);
                    foreach (var s in p.Skills)
                        set.Add(s.SkillName);
                }
                Console.WriteLine($"skills:{set.Count} P:{P} C:{C}");

            }

            int GetSkill(int contributor, string key)
            {
                if (Contributors[contributor].Skills.TryGetValue(key, out int skill))
                    return skill;
                return 0;
            }


            internal void Calculate()
            {
                var scheduledProjects = new SortedDictionary<int, int> ();
                int day = 0;
                int score = 0;
                var availableProjects = new HashSet<int>();
                for (int i = 0; i < P; i++)
                    availableProjects.Add(i);
                var solution = new List<(string, string[] )>();


                while(availableProjects.Count > 0)
                {
                    var dailyProjects = new List<int>(availableProjects);
                    dailyProjects.OrderBy(v => Projects[v].Days);
                    while (dailyProjects.Count > 0)
                    {
                        var pi = dailyProjects.First();
                        dailyProjects.Remove(pi);

                        var p = Projects[pi];
                        var pscore = p.Score - Math.Max(0, (day + p.Days) - p.BestBefore);
                        if (pscore <= 0)
                        {
                            availableProjects.Remove(pi);
                            continue;
                        }

                        var attemptContributors = new List<int>();
                        var foundP = true;
                        foreach (var sk in p.Skills)
                        {
                            var foundC = false;
                            for (int c = 0; c < C; c++)
                            {
                                ref var contributor = ref Contributors[c];

                                if (contributor.Available > day)
                                    continue;

                                if (attemptContributors.Contains(c))
                                    continue;

                                var level = GetSkill(c, sk.SkillName);
                                {
                                    if (level == sk.Level - 1)
                                    {
                                        if(attemptContributors.Any(ct => GetSkill(ct, sk.SkillName) >= sk.Level))
                                        {
                                            attemptContributors.Add(c);
                                            foundC = true;
                                            break;

                                        }
                                    }
                                    
                                    if (level >= sk.Level)
                                    {
                                        attemptContributors.Add(c);
                                        foundC = true;
                                        break;
                                    }

                                }
                            }
                            if (!foundC)
                            {
                                foundP = false;
                                break;
                            }


                        }

                        if (foundP)
                        {
                            scheduledProjects.TryAdd(day + p.Days, pi);
                            score += pscore;

                            solution.Add((p.PName, attemptContributors.Select(s => Contributors[s].CName).ToArray()));
                            availableProjects.Remove(pi);

                            for (int subi = 0; subi < attemptContributors.Count; subi++)
                            {
                                var sc = attemptContributors[subi];
                                Contributors[sc].Available = day + p.Days;
                                if(!Contributors[sc].Skills.ContainsKey(p.Skills[subi].SkillName))
                                {
                                    Contributors[sc].Skills.Add(p.Skills[subi].SkillName, 1); 
                                }
                                if (p.Skills[subi].Level >= Contributors[sc].Skills[p.Skills[subi].SkillName])
                                {
                                    Contributors[sc].Skills[p.Skills[subi].SkillName]++;
                                }
                            }


                        }
                    }

                    if (scheduledProjects.Count > 0)
                    {
                        var nextDay = scheduledProjects.First().Key;
                        scheduledProjects.Remove(nextDay);

                        day = nextDay;
                    }
                    else
                    {
                        day++;
                        break;
                    }

//                    Console.WriteLine($" {day} {availableProjects.Count} {score} {outpath}");

                }


                using (StreamWriter file = new StreamWriter(outpath))
                {

                    file.WriteLine(solution.Count);
                    foreach(var s in solution)
                    {
                        file.WriteLine(s.Item1);
                        file.WriteLine(string.Join(' ', s.Item2));
                    }
                }

                Console.WriteLine($"Wrote solution to: {outpath} : {score}");


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
