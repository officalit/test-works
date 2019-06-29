using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fuse8_TestWork_Ignatuk.Models
{
    public class Coin
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public string logo { get; set; }
        public string price { get; set; }
        public string percent_change_1h { get; set; }
        public string percent_change_24h { get; set; }
        public string market_cap { get; set; }
        public string last_updated { get; set; }
    }
}
