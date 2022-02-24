using System.Collections.Generic;


namespace HashCode22Solution
{
    partial class HashCode
    {
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
