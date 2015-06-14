using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWIFTReader
{
    public class MT103
    {
        public string RefNo_20 { get; set; }
        public string BankOpCode_23B { get; set; }
        public string InstructionCode_23E { get; set; }
        public string DateCurrencyAmount_32A { get; set; }
        public string CurrencyAmount_33B { get; set; }
        public string Customer_50K { get; set; }
        public string Institution_52A { get; set; }
        public string SenderBank_53A { get; set; }
        public string ReceiverBank_54A { get; set; }
        public string AccountWithInstitution_57A { get; set; }
        public string Beneficiary_59 { get; set; }
        public string RemittanceInfo_70 { get; set; }
        public string DetailsOfCharges_71A { get; set; }
        public string SenderToReceiverInformation_72 { get; set; }
    }
}
