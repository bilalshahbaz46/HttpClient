using System;
using System.Collections.Generic;
using System.Text;

namespace TestingSomething
{
    public class Item1
    {
        public Item1(Dictionary<string,string> dic, string ticker)
        {
            ticket = ticker;
            fields = dic;
        }
        public string ticket { get; set; }
        public Dictionary<string, string> fields { get; set; }
    }
}
