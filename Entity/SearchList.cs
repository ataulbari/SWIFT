using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWIFTReader
{
    public partial class SearchList 
    {

        #region Properties
        
        public int id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Information { get; set; }
        public string ListType { get; set; }
        public int ListTypeid { get; set; }
        public double score { get; set; }

        #endregion Properties
    }
}
