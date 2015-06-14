using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWIFTReader
{    
    public class SWIFTMessage
    {
        //====================For MT700================= 
        public string M20_Document_Credit_Number { get; set; }
        public string M23_Reference_to_PreAdvice { get; set; }
        public string M23B_BankOpCode { get; set; }
        public string M27_Sequence_of_Total { get; set; }
        public string M23E_InstructionCode { get; set; }
        public string M31C_Date_of_Issue { get; set; }
        public string M31D_Date_and_Place_of_Expiry { get; set; }
        public string M32B_Currency_Code_and_Amount { get; set; }
        public string M39A_Percentage_Credit_Amount_Tolerance { get; set; }
        public string M39B_Maximum_Credit_Amount { get; set; }
        public string M39C_Additional_Amounts_Covered { get; set; }
        public string M40A_Form_of_Documentary_Credit { get; set; }
        public string M40E_Applicable_Rules { get; set; }
        public string M41A_Available_With { get; set; }
        public string M42A_Drawee { get; set; }
        public string M42C_Drafts_at { get; set; }
        public string M42M_Mixed_Payment_Details { get; set; }
        public string M42P_Deferred_Payment_Details { get; set; }
        public string M43P_Partial_Shipments { get; set; }
        public string M43T_Transhipment { get; set; }
        public string M44A_Place_of_Taking_in_Charge_or_Dispatch_from_or_Place_of_Receipt { get; set; }
        public string M44B_Place_of_Final_Destination_or_For_Transportation_to_and_Place_of_Delivery { get; set; }
        public string M44C_Latest_Date_of_Shipment { get; set; }
        public string M44D_Shipment_Period { get; set; }
        public string M44E_Port_of_Loading_or_Airport_of_Departure { get; set; }
        public string M44F_Port_of_Discharge_or_Airport_of_Destination { get; set; }
        public string M45A_Description_of_Goods_and_or_Services { get; set; }
        public string M46A_Documents_Required { get; set; }
        public string M47A_Additional_Conditions { get; set; }
        public string M48_Period_for_Presentation { get; set; }
        public string M49_Confirmation_Instructions { get; set; }
        public string M50_Applicant { get; set; }
        public string M51A_Applicant_Bank { get; set; }
        public string M53A_Reimbursing_Bank { get; set; }
        public string M57A_Advise_Through_Bank { get; set; }
        public string M59_Beneficiary { get; set; }
        public string M71B_Charges { get; set; }
        public string M72_Sender_to_Receiver_Information { get; set; }
        public string M78_Instructions_to_the_Paying_or_Accepting_or_Negotiating_Bank { get; set; }

        //====================End of MT700=================
        public string M30_Requested_Execution_Date { get; set; }
        public string M32A_Date_Currency_Amount { get; set; }
        public string M50A_Ordering_Customer { get; set; }
        public string M52_Account_Servicing_Institution{ get; set; }
        public string RemittanceInfo_70 { get; set; }
        public string DetailsOfCharges_71A { get; set; }

    }
}
