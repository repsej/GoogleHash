using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace HashCode22Solution
{
    partial class HashCode
    {
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
    }
}
