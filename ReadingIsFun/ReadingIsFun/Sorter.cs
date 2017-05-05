using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ReadingIsFun
{
    class Sorter
    {
        private List<string> paths = new List<string>();
        public Sorter(List<string> books)
        {
            this.paths = books;
        }
        public SortedDictionary<string, List<Tuple<string, string>>> Sort()
        {
            SortedDictionary<string,List<Tuple<string, string>>> result = new SortedDictionary<string, List<Tuple<string, string>>>();
            foreach(var path in paths)
            {
                string name = path.Split('\\').Last().Split('.').First();
                name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
                Tuple<string, string> t = new Tuple<string, string>(name,path);
                if (Char.IsLetter(name[0]))
                {
                    string a = "" + name[0];
                    if (!result.ContainsKey(a))
                    {
                        result.Add(a, new List<Tuple<string, string>>());
                    }
                    result[a].Add(t);
                }
                else
                {
                    string a = "Misc.";
                    if (!result.ContainsKey(a))
                    {
                        result.Add(a, new List<Tuple<string, string>>());
                    }
                    result[a].Add(t);
                }
            }
            foreach(var list in result)
            {
                for(int i=0; i<list.Value.Count;i++)
                {
                    int min = i;
                    for(int j = i+1; j < list.Value.Count; j++)
                    {
                        if (list.Value.ElementAt(min).Item1.CompareTo(list.Value.ElementAt(j).Item1) >0)
                        {
                            Tuple<string, string> tmp = list.Value.ElementAt(min);
                            list.Value[min] = list.Value.ElementAt(j);
                            list.Value[j] = tmp;
                        }
                    }
                }
            }
            return result;
        }
    }
}
