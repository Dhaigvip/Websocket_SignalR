using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public class QueryMetaData
    {
        public QueryMetaData()
        {
            RoundingScheme = AmountRounding.None;
            Legends = new List<string>();
            SplitBy = string.Empty;
        }
        public string Name { get; set; }
        public string XAxis { get; set; }
        public string YAxis { get; set; }
        public string SplitBy { get; set; }
        public List<string> Legends { get; set; }
        public string YAxisLabel { get; set; }
        public string XAxisLabel { get; set; }
        public AmountRounding RoundingScheme { get; set; }
    }

}