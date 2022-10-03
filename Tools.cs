using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btcChart
{
    public class Datum
    {
        public string sequence { get; set; }
        public string price { get; set; }
        public string size { get; set; }
        public string side { get; set; }
        public object time { get; set; }
    }

    public class Root
    {
        public string code { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Data
    {
        public long time { get; set; }
        public string symbol { get; set; }
        public string buy { get; set; }
        public string sell { get; set; }
        public string changeRate { get; set; }
        public string changePrice { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string vol { get; set; }
        public string volValue { get; set; }
        public string last { get; set; }
        public string averagePrice { get; set; }
        public string takerFeeRate { get; set; }
        public string makerFeeRate { get; set; }
        public string takerCoefficient { get; set; }
        public string makerCoefficient { get; set; }
    }

    public class Hour24
    {
        public string code { get; set; }
        public Data data { get; set; }
    }
}
