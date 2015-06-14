using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWIFTReader
{
    public class CommonFields
    {
        public CommonFields()
        {
            Properties = new List<DynamicProperty>();
        }

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCountry { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryAddress { get; set; }
        public string BeneficiaryCountry { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string RefNo { get; set; }
        public string FullMessgae { get; set; }
        public List<DynamicProperty> Properties { get; set; }
    }

    public struct DynamicProperty
    {
        public string Name { get; set; }
        public string Value{ get; set; }       
    }
}
