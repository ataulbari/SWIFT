using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWIFTReader
{
    public class Parser:DML
    {
        #region Declaration

        DataTable Data;
        private int SWIFTMessageID=0;
        #endregion

        public SWIFTMessage ParseAll(string filePath)
        {
            //StreamReader streamReader = new StreamReader(filePath);
            //string file = streamReader.ReadToEnd();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                fs.Position = 1;

                byte[] newTextBytes = Encoding.ASCII.GetBytes(":xsi");
                fs.Write(newTextBytes, 0, newTextBytes.Length);
            }

            SWIFTMessage objMessage = new SWIFTMessage();
            //String[] data = file.Split(new string[] { "{" }, StringSplitOptions.RemoveEmptyEntries);
            //string[] abcd = file.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //streamReader.Close();

            //foreach (String line in data)
            //{
            //    string transactionLine = line;

            //    if (transactionLine != null)
            //    {
            //        try
            //        {
            //            if (transactionLine.StartsWith("4:"))
            //            {
            //                var tokenized = line.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //                #region Switch

            //                for (int iPointer = 1; iPointer < tokenized.Length - 1; iPointer++)
            //                {
            //                    //string tag = tokenized[iPointer].Substring(transactionLine.IndexOf(':'), transactionLine.IndexOf(':', 1) + 1);
            //                    string tag = tokenized[iPointer].Substring(tokenized[iPointer].IndexOf(':'), tokenized[iPointer].IndexOf(':', 1) + 1);
            //                    string transactionData = tokenized[iPointer].Substring(tag.Length);

            //                    switch (tag)
            //                    {
            //                        case ":20:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);

            //                            objMessage.M20_Document_Credit_Number = transactionData;

            //                            break;
            //                        case ":23:":
            //                            objMessage.M23_Reference_to_PreAdvice = transactionData;
            //                            break;
            //                        case ":23B:":
            //                            objMessage.M23B_BankOpCode = transactionData;
            //                            break;
            //                        case ":23E:":
            //                            objMessage.M23E_InstructionCode = transactionData;
            //                            break;
            //                        case ":27:":
            //                            objMessage.M27_Sequence_of_Total = transactionData;
            //                            break;
            //                        case ":31C:":
            //                            objMessage.M31C_Date_of_Issue = transactionData;
            //                            break;
            //                        case ":31D:":
            //                            objMessage.M31D_Date_and_Place_of_Expiry = transactionData;
            //                            break;
            //                        case ":32B:":
            //                            objMessage.M32B_Currency_Code_and_Amount = transactionData;
            //                            break;
            //                        case ":39A:":
            //                            objMessage.M39A_Percentage_Credit_Amount_Tolerance = transactionData;
            //                            break;
            //                        case ":39B:":
            //                            objMessage.M39B_Maximum_Credit_Amount = transactionData;
            //                            break;
            //                        case ":39C:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M39C_Additional_Amounts_Covered = transactionData;
            //                            break;
            //                        case ":40A:":
            //                        case ":40B:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M40A_Form_of_Documentary_Credit = transactionData;
            //                            break;
            //                        case ":40E:":
            //                            objMessage.M40E_Applicable_Rules = transactionData;
            //                            break;
            //                        case ":41A:":
            //                            objMessage.M41A_Available_With = transactionData;
            //                            break;
            //                        case ":42A:":
            //                            objMessage.M42A_Drawee = transactionData;
            //                            break;
            //                        case ":42C:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M42C_Drafts_at = transactionData;
            //                            break;
            //                        case ":42M:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M42M_Mixed_Payment_Details = transactionData;
            //                            break;
            //                        case ":42P:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M42P_Deferred_Payment_Details = transactionData;
            //                            break;
            //                        case ":43P:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M43P_Partial_Shipments = transactionData;
            //                            break;
            //                        case ":43T:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M43T_Transhipment = transactionData;
            //                            break;
            //                        case ":44A:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M44A_Place_of_Taking_in_Charge_or_Dispatch_from_or_Place_of_Receipt = transactionData;
            //                            break;
            //                        case ":44B:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M44B_Place_of_Final_Destination_or_For_Transportation_to_and_Place_of_Delivery = transactionData;
            //                            break;
            //                        case ":44C:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M44C_Latest_Date_of_Shipment = transactionData;
            //                            break;
            //                        case ":44D:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M44D_Shipment_Period = transactionData;
            //                            break;
            //                        case ":44E:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M44E_Port_of_Loading_or_Airport_of_Departure = transactionData;
            //                            break;
            //                        case ":44F:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M44F_Port_of_Discharge_or_Airport_of_Destination = transactionData;
            //                            break;
            //                        case ":45A:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M45A_Description_of_Goods_and_or_Services = transactionData;
            //                            break;
            //                        case ":46A:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M46A_Documents_Required = transactionData;
            //                            break;
            //                        case ":47A:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M47A_Additional_Conditions = transactionData;
            //                            break;
            //                        case ":48:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M48_Period_for_Presentation = transactionData;
            //                            break;
            //                        case ":49:":
            //                            objMessage.M49_Confirmation_Instructions = transactionData;
            //                            break;
            //                        case ":50:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);

            //                            objMessage.M50_Applicant = transactionData;
            //                            break;

            //                        case ":51A:":
            //                            objMessage.M51A_Applicant_Bank = transactionData;
            //                            break;
            //                        case ":53A:":
            //                            objMessage.M53A_Reimbursing_Bank = transactionData;
            //                            break;
            //                        case ":57A:":
            //                            objMessage.M57A_Advise_Through_Bank = transactionData;
            //                            break;
            //                        case ":71B:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M71B_Charges = transactionData;
            //                            break;

                                    
            //                        case ":72:":
            //                            objMessage.M72_Sender_to_Receiver_Information = transactionData;
            //                            break;

            //                        //======Property for MT100 series========
            //                        case ":30:":
            //                            objMessage.M30_Requested_Execution_Date = transactionData;
            //                            break;
            //                        case ":32A:":
            //                            objMessage.M32A_Date_Currency_Amount = transactionData;
            //                            break;
            //                        case ":50A:":
            //                        case ":50F:":
            //                        case ":50G:":
            //                        case ":50H:":
            //                        case ":50K:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M50A_Ordering_Customer = transactionData;
            //                            break;                                    
            //                        case ":52A:":
            //                        case ":52C:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M52_Account_Servicing_Institution = transactionData;
            //                            break;
            //                        case ":59:":
            //                        case ":59A:":
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);
            //                            objMessage.M59_Beneficiary = transactionData;
            //                            break;

            //                        case ":70:":
            //                            objMessage.RemittanceInfo_70 = transactionData;
            //                            break;
            //                        case ":71A:":
            //                            objMessage.DetailsOfCharges_71A = transactionData;
            //                            break;
                                    
            //                            //========Ignore Unimplemented tags=======
            //                        default:
            //                            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            //                                AddNextLine(ref transactionData, tokenized, ref iPointer);                                        
            //                            break;
            //                    }
            //                }
            //                #endregion
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
            //        }
            //    }
            //}
            return objMessage;
        }

        private void AddNextLine(ref string objMessage, string[] tokenized, ref int iPointer)
        {
            objMessage += "\n" + tokenized[iPointer + 1];
            iPointer = iPointer + 1;

            if (!tokenized[iPointer + 1].Substring(0,3).Contains(':'))
            {
                AddNextLine(ref objMessage, tokenized, ref iPointer);
            }
        }

        public CommonFields ObjectToCommonFields(object NewValue)
        {
            CommonFields objClass = new CommonFields();
            string strNew = string.Empty,strCustomer;

            PropertyInfo[] aPropertyInfo = null;
            if ((NewValue != null))
            {
                aPropertyInfo = NewValue.GetType().GetProperties();
                string[] temp;
                foreach (PropertyInfo aProperty in aPropertyInfo)
                {
                    try
                    {
                        if (aProperty.Name == "M20_Document_Credit_Number")
                            objClass.RefNo = aProperty.GetValue(NewValue, null).ToString();
                        else if (aProperty.Name == "M50_Applicant")
                        {
                            try
                            {
                                strCustomer = aProperty.GetValue(NewValue, null).ToString();
                                if (strCustomer.Length > 0)
                                {
                                    temp = strCustomer.Split('\n');
                                    objClass.CustomerName = temp[0].Trim();
                                    objClass.CustomerAddress = strCustomer.Substring(objClass.CustomerName.Length);
                                    objClass.CustomerCountry= FindCountry(strCustomer.Substring(objClass.CustomerName.Length));
                                }
                            }
                            catch (Exception)
                            {
                                objClass.CustomerName = "";
                                objClass.CustomerAddress = "";
                                objClass.CustomerCountry = "";
                            }
                        }
                        else if (aProperty.Name == "M51A_Applicant_Bank")
                        {
                            strCustomer = aProperty.GetValue(NewValue, null).ToString();
                            if (strCustomer.Length > 0)
                            {
                                DynamicProperty obj = new DynamicProperty();
                                obj.Name = "Sending Institution";
                                obj.Value = strCustomer;
                                objClass.Properties.Add(obj);
                            }
                        }
                        else if (aProperty.Name == "M52_Account_Servicing_Institution")
                        {
                            strCustomer = aProperty.GetValue(NewValue, null).ToString();
                            if (strCustomer.Length > 0)
                            {
                                DynamicProperty obj = new DynamicProperty();
                                obj.Name = "Account Servicing Institution";
                                obj.Value = strCustomer;
                                objClass.Properties.Add(obj);
                            }
                        }
                        else if (aProperty.Name == "M50A_Ordering_Customer")
                        {
                            strCustomer = aProperty.GetValue(NewValue, null).ToString();
                            if (strCustomer.Length > 0)
                            {
                                temp = strCustomer.Split('\n');
                                if(IsDigitsOnly(temp[0].Trim()))
                                    objClass.CustomerName = temp[1].Trim();
                                else
                                    objClass.CustomerName = temp[0].Trim();
                                objClass.CustomerAddress = strCustomer.Substring(objClass.CustomerName.Length);
                                objClass.CustomerCountry = FindCountry(strCustomer.Substring(objClass.CustomerName.Length));
                            }
                        }
                        else if (aProperty.Name == "M59_Beneficiary")
                        {
                            strCustomer = aProperty.GetValue(NewValue, null).ToString();
                            if (strCustomer.Length > 0)
                            {
                                temp = strCustomer.Split('\n');
                                if (IsDigitsOnly(temp[0].Trim()))
                                    objClass.BeneficiaryName = temp[1].Trim();
                                else
                                    objClass.BeneficiaryName = temp[0].Trim();
                                objClass.BeneficiaryAddress = strCustomer.Substring(objClass.BeneficiaryName.Length);
                                objClass.BeneficiaryCountry= FindCountry(strCustomer.Substring(objClass.BeneficiaryName.Length));
                            }                            
                        }
                        else if (aProperty.Name == "M32A_Date_Currency_Amount")
                        {
                            objClass.Amount = aProperty.GetValue(NewValue, null).ToString().Substring(9).Replace(",","");
                            objClass.Currency = aProperty.GetValue(NewValue, null).ToString().Substring(6, 3);
                        }
                        else if (aProperty.Name == "M32B_Currency_Code_and_Amount")
                        {
                            objClass.Amount = aProperty.GetValue(NewValue, null).ToString().Substring(3).Replace(",", "");
                            objClass.Currency = aProperty.GetValue(NewValue, null).ToString().Substring(0, 3);                            
                        }

                        //=======Write full message in a single string======
                        if (!string.IsNullOrEmpty(aProperty.GetValue(NewValue, null).ToString()))
                            strNew = strNew + aProperty.Name + ":: " + aProperty.GetValue(NewValue, null).ToString() + ";; ";                        
                        
                        objClass.FullMessgae = strNew;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return objClass;
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if ((c != '/' && c != '-') && (c < '0' || c > '9'))
                    return false;
            }

            return true;
        }
        
        private string FindCountry(string search)
        {
            string strQuery = "SELECT CountryName FROM Country WHERE ActiveStatus=1", countryName = "";
            
            DataTable dt = ExecuteCommand(strQuery);
            countryName = "";
            search = search.ToUpper().Trim();
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    if (search.Contains(dr["CountryName"].ToString()))
                    {
                        countryName = dr["CountryName"].ToString();
                        break;
                    }
                }
                catch (Exception)
                {
                }
            }

            return countryName;
        }

        public void ReadFiles()
        {
            int MinScore = 0, result = 0;
            string mCriteria = "Jaro Winkler", newFilepath=string.Empty;
            
            string BranchID = System.Configuration.ConfigurationManager.AppSettings["BranchID"].ToString();
            string strQuery = "SELECT [ID],DisplayField,ValueField FROM GlobalConfig WHERE FunctionName='SWIFT'";
            string SAIn = string.Empty, SAOut= string.Empty, AFTIn = string.Empty, AFTOut = string.Empty, BlockIn = string.Empty, BlockOut = string.Empty;

            DataTable dt = ExecuteCommand(strQuery);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["DisplayField"].ToString() == "SAIn")
                    SAIn = dr["ValueField"].ToString();
                else if (dr["DisplayField"].ToString() == "SAOut")
                    SAOut = dr["ValueField"].ToString();
                else if (dr["DisplayField"].ToString() == "BlockOut")
                    BlockOut = dr["ValueField"].ToString();
                else if (dr["DisplayField"].ToString() == "BlockIn")
                    BlockIn= dr["ValueField"].ToString();
                else if (dr["DisplayField"].ToString() == "AFTOut")
                    AFTOut= dr["ValueField"].ToString();
                else if (dr["DisplayField"].ToString() == "AFTIn")
                    AFTIn= dr["ValueField"].ToString();
                else if (dr["DisplayField"].ToString() == "MinScore")
                    int.TryParse(dr["ValueField"].ToString(), out MinScore);
            }
            //=======Scan IN path=======
            #region INPUT File
            DirectoryInfo d = new DirectoryInfo(AFTIn);
            foreach (FileInfo oldFile in d.GetFiles())
            {
                try
                {
                    newFilepath = SAIn + "\\" + oldFile.Name;// Destination Path
                    
                    CommonFields objCommonFields = new CommonFields();
                    objCommonFields = ObjectToCommonFields((object)ParseAll(oldFile.FullName));
                    result = SearchDataFromLists(objCommonFields, newFilepath, MinScore, mCriteria, BranchID,"In");
                    
                    if (result >0)//Match Found
                    {
                        File.Move(oldFile.FullName, BlockIn + "\\" + oldFile.Name);
                        SqlParameter[] sParametre = new SqlParameter[2];
                        sParametre[0] = new SqlParameter("@FilePath", SqlDbType.VarChar);
                        sParametre[0].Value = BlockIn + "\\" + oldFile.Name;

                        sParametre[1] = new SqlParameter("@ID", SqlDbType.Int);
                        sParametre[1].Value = SWIFTMessageID;

                        strQuery = " UPDATE AMLSWIFTMessage SET FilePath =@FilePath  WHERE ID=@ID";
                        InsertData(strQuery, sParametre);
                    }
                    else if (result == 0)//No Match Found
                    {
                        File.Move(oldFile.FullName, SAIn + "\\" + oldFile.Name); 
                    }
                    else if (result == -1)//Error
                        File.Move(oldFile.FullName, SAIn + "\\" + oldFile.Name);
                    
                }
                catch (IOException ex)
                {
                    File.Move(oldFile.FullName, SAIn + "\\" + oldFile.Name); 
                    File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
                }
            }
            #endregion

            //=======Scan Out path=======
            #region OUTPUT File
            DirectoryInfo dir = new DirectoryInfo(SAOut);
            foreach (FileInfo oldFile in dir.GetFiles())
            {
                try
                {
                    newFilepath = AFTOut + "\\" + oldFile.Name;

                    CommonFields objCommonFields = new CommonFields();
                    objCommonFields = ObjectToCommonFields((object)ParseAll(oldFile.FullName));
                    result = SearchDataFromLists(objCommonFields, newFilepath, MinScore, mCriteria, BranchID,"Out");

                    if (result > 0)//Match Found
                    {
                        File.Move(oldFile.FullName, BlockOut + "\\" + oldFile.Name);
                        SqlParameter[] sParametre = new SqlParameter[2];
                        sParametre[0] = new SqlParameter("@FilePath", SqlDbType.VarChar);
                        sParametre[0].Value = BlockOut + "\\" + oldFile.Name;

                        sParametre[1] = new SqlParameter("@ID", SqlDbType.Int);
                        sParametre[1].Value = SWIFTMessageID;

                        strQuery = " UPDATE AMLSWIFTMessage SET FilePath =@FilePath  WHERE ID=@ID";
                        InsertData(strQuery,sParametre);
                    }
                    else if (result == 0)//No Match Found
                    {
                        File.Move(oldFile.FullName, AFTOut + "\\" + oldFile.Name);                        
                    }
                    else if (result == -1)//Error
                    {
                        File.Move(oldFile.FullName, AFTOut + "\\" + oldFile.Name);
                    }
                    
                }
                catch (IOException ex)
                {
                    File.Move(oldFile.FullName, AFTOut + "\\" + oldFile.Name);
                    File.AppendAllText(Application.StartupPath.ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
                }
            }
            #endregion
        }

        public int SearchDataFromLists(CommonFields objCommonFields, string FilePath, int selectedscore, string mCriteria, string BranchID, string MessageType)
        {
            #region Declaration
            int result = -1, i=0;
            string combinedQuery = "";
            SWIFTMessageID = -1;
            SqlParameter[] sParametre = new SqlParameter[12];
            sParametre[i] = new SqlParameter("@Sender", SqlDbType.VarChar);
            sParametre[i++].Value = objCommonFields.CustomerName;

            sParametre[i] = new SqlParameter("@SenderAddress", SqlDbType.VarChar);
            sParametre[i++].Value = objCommonFields.CustomerAddress;

            sParametre[i] = new SqlParameter("@Beneficiary", SqlDbType.VarChar);
            sParametre[i++].Value = objCommonFields.BeneficiaryName;

            sParametre[i] = new SqlParameter("@BAddress", SqlDbType.VarChar);
            sParametre[i++].Value = objCommonFields.BeneficiaryAddress;

            sParametre[i] = new SqlParameter("@Amount", SqlDbType.Decimal);
            if (objCommonFields.Amount == "" || objCommonFields.Amount == null)
                sParametre[i++].Value = 0;
            else
                sParametre[i++].Value = decimal.Parse(objCommonFields.Amount);

            sParametre[i] = new SqlParameter("@Currency", SqlDbType.VarChar);
            sParametre[i++].Value = objCommonFields.Currency;
            sParametre[i] = new SqlParameter("@FilePath", SqlDbType.VarChar);
            sParametre[i++].Value = FilePath;
            sParametre[i] = new SqlParameter("@RefNo", SqlDbType.VarChar);
            sParametre[i++].Value = objCommonFields.RefNo;
            sParametre[i] = new SqlParameter("@EntryDate", SqlDbType.DateTime);
            sParametre[i++].Value = System.DateTime.Now;
            sParametre[i] = new SqlParameter("@Status", SqlDbType.VarChar);
            sParametre[i++].Value = "New";
            sParametre[i] = new SqlParameter("@MessageType", SqlDbType.VarChar);
            sParametre[i++].Value = MessageType;
            sParametre[i] = new SqlParameter("@BranchID", SqlDbType.VarChar);
            sParametre[i++].Value = BranchID;
            
            
            #endregion
            
            Data = ExecuteCommand("select * from ScreeningListConfig where IsSelected=1");

            BeginTran();
            string strQuery = "INSERT INTO AMLSWIFTMessage(Sender,SenderAddress,Beneficiary,BeneficiaryAddress,Amount,Currency,FilePath,RefNo,EntryDate,Status,MessageType,BranchID)values (@Sender,@SenderAddress,@Beneficiary,@BAddress,@Amount,@Currency,@FilePath,@RefNo,@EntryDate,@Status,@MessageType,@BranchID); SELECT SCOPE_IDENTITY();";

            SWIFTMessageID = ExecuteScalar(strQuery, sParametre);

            if (SWIFTMessageID > 0)
            {
                try
                {                    
                    //====Search by Customer====
                    combinedQuery = SearchFromList(objCommonFields.CustomerName, objCommonFields.CustomerCountry, selectedscore, "", mCriteria, "Legal Entities");

                    combinedQuery = combinedQuery + " UNION ALL " + SearchFromList(objCommonFields.CustomerName, objCommonFields.CustomerCountry, selectedscore, "", mCriteria, "Individuals");
                                                                                                                //MatchFieldsValue],[Address],[Algorithom]
                    strQuery = "INSERT INTO AMLSwiftMsgDetails(SWIFTMessageID,SanctionListName,MatchID,MatchPercent,MatchFields,MatchFieldsValue,Address,Algorithom) " +
                            "SELECT " + SWIFTMessageID + " as SWIFTMessageID, ListType,id,score, 'Customer Name' as MatchFields,'" + objCommonFields.CustomerName + "','" + objCommonFields.CustomerAddress + "','Jaro Winkler' AS Algorithom From (" + combinedQuery + ") as TempResult ORDER BY score DESC";
                    
                    result = InsertData(strQuery, null);
                    
                    //====Search by Beneficiary====
                    combinedQuery = SearchFromList(objCommonFields.BeneficiaryName, objCommonFields.BeneficiaryCountry, selectedscore, "", mCriteria, "Legal Entities");

                    combinedQuery = combinedQuery + " UNION ALL " + SearchFromList(objCommonFields.BeneficiaryName, objCommonFields.BeneficiaryCountry, selectedscore, "", mCriteria, "Individuals");

                    strQuery = "INSERT INTO AMLSwiftMsgDetails(SWIFTMessageID,SanctionListName,MatchID,MatchPercent,MatchFields,MatchFieldsValue,Address,Algorithom) " +
                                "SELECT " + SWIFTMessageID + " as SWIFTMessageID, ListType,id,score, 'Beneficiary' as MatchFields,'" + objCommonFields.BeneficiaryName + "','" + objCommonFields.BeneficiaryAddress + "','Jaro Winkler' AS Algorithom From (" + combinedQuery + ") as TempResult ORDER BY score DESC";

                    result = result + InsertData(strQuery, null);

                    //============Intermediate Bank if available================

                    foreach (DynamicProperty dp in objCommonFields.Properties)
                    {
                        combinedQuery = SearchFromList(dp.Value, "", selectedscore, "", mCriteria, "Legal Entities");

                        combinedQuery = combinedQuery + " UNION ALL " + SearchFromList(dp.Value, "", selectedscore, "", mCriteria, "Individuals");

                        strQuery = "INSERT INTO AMLSwiftMsgDetails(SWIFTMessageID,SanctionListName,MatchID,MatchPercent,MatchFields,MatchFieldsValue,Address,Algorithom) " +
                                "SELECT " + SWIFTMessageID + " as SWIFTMessageID, ListType,id,score, '" + dp.Name + "','" + dp.Value + "',' ' AS  Address,'Jaro Winkler' AS Algorithom From (" + combinedQuery + ") as TempResult ORDER BY score DESC";

                        result = result + InsertData(strQuery, null);                       
                    }

                    //=================END===============
                }
                catch (Exception ex)
                {
                    File.AppendAllText(Application.StartupPath .ToString() + "\\ErrorLog.txt", DateTime.Now.ToString("dd/MMM/yyyy HH:mm") + "::" + ex.TargetSite.ReflectedType.Name + "::" + ex.TargetSite.Name + "::" + ex.Message + Environment.NewLine);
        
                    RollBackTran();
                    result = -1;
                }
            }
            if (result != -1)
                CommitTran();
            else
                RollBackTran();
            return result;
        }
        
        private string SearchFromList(string customer, string country, int selectedscore, string city, string mCriteria, string customer_type)
        {
            List<SearchList> TotalListList = new List<SearchList>();

            var Query = "";
            var combinedQuery = ""; ;

            var scorefacor = 0.0;
            scorefacor = (Double)selectedscore / 100;

            string where1 = string.Empty, where2 = string.Empty, where3, where4 = string.Empty, score1 = string.Empty, score2 = string.Empty, score4 = string.Empty;
            string select1 = string.Empty, select4 = string.Empty;
            //=================================================
            #region Query

            score1 = "CAST(1 as float)";
            score2 = "CAST(1 as float)";
            score4 = "CAST(1 as float)";

            if (customer_type == "Legal Entities")
            {
                #region LegalEntities

                select1 = "SELECT id,ISNULL(ENTITY_ALIAS_NAME,'') as CustomerName, ISNULL(ENTITY_ADDRESS,'') as Address ,ISNULL(DESIGNATION + ', ' + COMMENTS1 + ', ' + INDIVIDUAL_DOCUMENT,'') as Information,";
                select4 = select1;

                #region Customer
                if (customer != "")
                {
                    if (mCriteria == "Jaro Winkler" || mCriteria == "Phonetic" || mCriteria == "Both")
                    {
                        where1 = " (ROUND(dbo.JaroSplit(UPPER(ENTITY_ALIAS_NAME),UPPER('" + customer + "')),2) >=" + scorefacor + " )";
                        where4 = where1;

                        score1 = " dbo.JaroSplit(UPPER(ENTITY_ALIAS_NAME),UPPER('" + customer + "')) ";
                        score4 = score1;

                        where2 = " sdnType !='Individual' AND (ROUND(dbo.JaroSplit(UPPER(firstName+' '+lastName),UPPER('" + customer + "')),2) >=" + scorefacor + ")";
                        score2 = " dbo.JaroSplit(UPPER(firstName+' '+lastName),UPPER('" + customer + "')) ";
                    }
                    else
                    {
                        //Exact Match Search
                        where1 = " ( ENTITY_ALIAS_NAME LIKE '%" + customer + "%' )";
                        where4 = where1;
                        where2 = " sdnType !='Individual' AND ( firstName+' '+ lastName  LIKE '%" + customer + "%')";
                    }
                }
                #endregion

                #region Country

                if (country != "")
                {
                    if (where1 == "")
                    {
                        where1 = " ( ENTITY_ADDRESS like '%" + country + "%' )";
                        where2 = " sdnType !='Individual' AND ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' )";
                        where4 = where1;
                    }
                    else
                    {
                        where1 += " AND ( ENTITY_ADDRESS like '%" + country + "%' )";
                        where2 += " AND ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' )";
                        where4 = where1;
                    }
                }
                #endregion

                #region City
                if (city != "")
                {
                    if (where1 == "")
                    {
                        where1 = " ( ENTITY_ADDRESS like '%" + city + "%')";
                        where2 = " sdnType !='Individual' AND ( addressList_address_city like '%" + city + "%' OR  addressList_address_state like '%" + city + "%' )";
                        where4 = where1;
                    }
                    else
                    {
                        where1 += " AND ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' OR  INDIVIDUAL_ADDRESS like '%" + city + "%' )";
                        where2 += " AND ( addressList_address_city like '%" + city + "%' OR  addressList_address_state like '%" + city + "%' )";
                        where4 = where1;
                    }
                }
                #endregion

                #endregion
            }
            else
            {
                #region Individuals
                select1 = "SELECT id,ISNULL(FIRST_NAME,'')+' '+ISNULL(SECOND_NAME,'') as CustomerName,ISNULL(INDIVIDUAL_PLACE_OF_BIRTH,'') as Address ,ISNULL(DESIGNATION + ', ' + COMMENTS1 + ', ' + INDIVIDUAL_DOCUMENT,'') as Information,";
                select4 = select1;

                #region Customer
                if (customer != "")
                {
                    if (mCriteria == "Jaro Winkler" || mCriteria == "Phonetic" || mCriteria == "Both")
                    {
                        where1 = " (ROUND(dbo.JaroSplit(UPPER(FIRST_NAME+' '+SECOND_NAME),UPPER('" + customer + "')),2) >=" + scorefacor + " ) ";

                        where2 = " sdnType ='Individual' AND (ROUND(dbo.JaroSplit(UPPER(firstName+ ' ' +lastName),UPPER('" + customer + "')),2) >=" + scorefacor + " ) ";

                        where4 = where1;

                        score1 = " dbo.JaroSplit(UPPER(FIRST_NAME+' '+SECOND_NAME),UPPER('" + customer + "'))";
                        score2 = " dbo.JaroSplit(UPPER(firstName+' '+lastName),UPPER('" + customer + "'))";
                        score4 = score1;
                    }
                    else
                    {
                        where1 = " ( FIRST_NAME LIKE '%" + customer + "%' OR SECOND_NAME LIKE '%" + customer + "%' )";
                        where2 = " sdnType ='Individual' AND ( firstName+' '+ lastName  LIKE '%" + customer + "%')";
                        where4 = where1;
                    }
                }
                #endregion

                #region Country

                if (country != "")
                {
                    if (where1 == "")
                    {
                        where1 = " ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + country + "%' OR  INDIVIDUAL_ADDRESS like '%" + country + "%' )";
                        where2 = " sdnType ='Individual' AND ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' )";
                        where4 = " ( INDIVIDUAL_ADDRESS like '%" + country + "%' OR  INDIVIDUAL_PLACE_OF_BIRTH like '%" + country + "%' )";
                    }
                    else
                    {
                        where1 += " AND ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + country + "%' OR  INDIVIDUAL_ADDRESS like '%" + country + "%' )";
                        where2 += " AND ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' )";
                        where4 += " AND ( INDIVIDUAL_ADDRESS like '%" + country + "%' OR  INDIVIDUAL_PLACE_OF_BIRTH like '%" + country + "%' )";
                    }
                }
                #endregion

                #region City

                if (city != "")
                {
                    if (where1 == "")
                    {
                        where1 = " ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' OR  INDIVIDUAL_ADDRESS like '%" + city + "%' )";
                        where2 = " sdnType ='Individual' AND ( addressList_address_city like '%" + city + "%' OR  addressList_address_state like '%" + city + "%' )";
                        where4 = " ( INDIVIDUAL_ADDRESS like '%" + city + "%' OR  INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' )";
                    }
                    else
                    {
                        where1 += " AND ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' OR  INDIVIDUAL_ADDRESS like '%" + city + "%' )";
                        where2 += " AND ( addressList_address_city like '%" + city + "%' OR  addressList_address_state like '%" + city + "%' )";
                        where4 += " AND ( INDIVIDUAL_ADDRESS like '%" + city + "%' OR  INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' )";
                    }
                }
                #endregion

                #endregion
            }

            #endregion

            #region Union
            //Query = " select * from ScreeningListConfig where IsSelected=1";
            //DataTable Data = ExecuteCommand(Query);
            foreach (DataRow dr in Data.Rows)
            {
                var ListTypeid = dr["id"].ToString();

                if (ListTypeid == "1")
                {
                    Query = select1 + " 'UNSCR' as ListType, " + ListTypeid + " as ListTypeid, ROUND(" + score1 + " ,2)*100 AS score FROM UNSCR WHERE " + where1 + " ";
                }

                else if (ListTypeid == "2")
                {
                    Query = "SELECT id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,ISNULL(addressList_address_state,'') +' '+ ISNULL(addressList_address_city,'') +' '+ ISNULL(addressList_address_country,'')  as Address,ISNULL(title + ', '+ idList,' ') as Information,'OFACSDN' as ListType," + ListTypeid + " as ListTypeid, ROUND(" + score2 + " ,2)*100 AS score FROM OFACSDN WHERE " + where2 + " ";
                    //========Search in A.K.A list=========
                    if (customer != "")
                    {
                        string scroreOFAC = " ROUND(dbo.JaroSplit(UPPER(firstName+' '+lastName),UPPER('" + customer + "')),2)*100 ";
                        string whereOFAC = " (ROUND(dbo.JaroSplit(UPPER(firstName+' '+lastName),UPPER('" + customer + "')),2) >=" + scorefacor + ") ";

                        if (country.Trim() == "")
                        {
                            #region Without Country
                            if (mCriteria == "Jaro Winkler" || mCriteria == "Phonetic" || mCriteria == "Both")
                            {
                                if (customer_type == "Legal Entities")
                                    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  sdnType ='Entity' AND  ( " + whereOFAC + " ) ";
                                else if (customer_type == "Individuals")
                                    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  sdnType ='Individual' AND ( " + whereOFAC + " ) ";
                                else
                                    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  ( " + whereOFAC + " ) ";
                            }
                            else
                            {
                                scroreOFAC = "CAST(100 as float)";
                                whereOFAC = " ( firstName+' '+ lastName  LIKE '%" + customer + "%')";
                                if (customer_type == "Legal Entities")
                                    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  sdnType ='Entity' AND  ( " + whereOFAC + " ) ";
                                else if (customer_type == "Individuals")
                                    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  sdnType ='Individual' AND ( " + whereOFAC + " ) ";
                                else
                                    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  ( " + whereOFAC + " ) ";
                            }
                            #endregion
                        }
                        else
                        {
                            #region With Country
                            scroreOFAC = " ROUND(dbo.JaroSplit(UPPER(D.firstName+' '+D.lastName),UPPER('" + customer + "')),2)*100 ";
                            whereOFAC = "  ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' ) AND (ROUND(dbo.JaroSplit(UPPER(D.firstName+' '+D.lastName),UPPER('" + customer + "')),2) >=" + scorefacor + ") ";

                            if (mCriteria == "Jaro Winkler" || mCriteria == "Phonetic" || mCriteria == "Both")
                            {
                                if (customer_type == "Legal Entities")
                                    Query = Query + " UNION ALL SELECT D.ID, ISNULL(D.firstName,'') +' '+ISNULL(D.lastName,' ') as CustomerName,  ISNULL(addressList_address_state,'') +' '+ ISNULL(addressList_address_city,'') +' '+ ISNULL(addressList_address_country,'')  as Address,ISNULL(title,'') as Information, " +
                                    " 'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDN M INNER JOIN OFACSDNDetails D ON M.id = D.OFACSDNID where D.sdnType= 'Entity' AND ( " + whereOFAC + " ) ";
                                else if (customer_type == "Individuals")
                                    Query = Query + " UNION ALL SELECT D.ID, ISNULL(D.firstName,'') +' '+ISNULL(D.lastName,' ') as CustomerName,  ISNULL(addressList_address_state,'') +' '+ ISNULL(addressList_address_city,'') +' '+ ISNULL(addressList_address_country,'')  as Address,ISNULL(title,'') as Information, " +
                                    " 'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDN M INNER JOIN OFACSDNDetails D ON M.id = D.OFACSDNID where D.sdnType= 'Individual' AND ( " + whereOFAC + " ) ";
                                else
                                    Query = Query + " UNION ALL SELECT D.ID, ISNULL(D.firstName,'') +' '+ISNULL(D.lastName,' ') as CustomerName,  ISNULL(addressList_address_state,'') +' '+ ISNULL(addressList_address_city,'') +' '+ ISNULL(addressList_address_country,'')  as Address,ISNULL(title,'') as Information, " +
                                    " 'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDN M INNER JOIN OFACSDNDetails D ON M.id = D.OFACSDNID where ( " + whereOFAC + " ) ";
                                //Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  ( " + whereOFAC + " ) ";
                            }
                            else
                            {
                                scroreOFAC = "CAST(100 as float)";
                                whereOFAC = " ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' ) AND ( D.firstName+' '+ D.lastName  LIKE '%" + customer + "%')";
                                if (customer_type == "Legal Entities")
                                    Query = Query + " UNION ALL SELECT D.ID, ISNULL(D.firstName,'') +' '+ISNULL(D.lastName,' ') as CustomerName,  ISNULL(addressList_address_state,'') +' '+ ISNULL(addressList_address_city,'') +' '+ ISNULL(addressList_address_country,'')  as Address,ISNULL(title,'') as Information, " +
                                    " 'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDN M INNER JOIN OFACSDNDetails D ON M.id = D.OFACSDNID where D.sdnType= 'Entity' AND ( " + whereOFAC + " ) ";
                                else if (customer_type == "Individuals")
                                    Query = Query + " UNION ALL SELECT D.ID, ISNULL(D.firstName,'') +' '+ISNULL(D.lastName,' ') as CustomerName,  ISNULL(addressList_address_state,'') +' '+ ISNULL(addressList_address_city,'') +' '+ ISNULL(addressList_address_country,'')  as Address,ISNULL(title,'') as Information, " +
                                    " 'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDN M INNER JOIN OFACSDNDetails D ON M.id = D.OFACSDNID where D.sdnType= 'Individual' AND ( " + whereOFAC + " ) ";
                                else
                                    Query = Query + " UNION ALL SELECT D.ID, ISNULL(D.firstName,'') +' '+ISNULL(D.lastName,' ') as CustomerName,  ISNULL(addressList_address_state,'') +' '+ ISNULL(addressList_address_city,'') +' '+ ISNULL(addressList_address_country,'')  as Address,ISNULL(title,'') as Information, " +
                                    " 'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDN M INNER JOIN OFACSDNDetails D ON M.id = D.OFACSDNID where ( " + whereOFAC + " ) ";


                                //if (customer_type == "Legal Entities")
                                //    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  sdnType ='Entity' AND  ( " + whereOFAC + " ) ";
                                //else if (customer_type == "Individuals")
                                //    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  sdnType ='Individual' AND ( " + whereOFAC + " ) ";
                                //else
                                //    Query = Query + "  UNION ALL SELECT ID as id,ISNULL(firstName,'')+' '+ISNULL(lastName,'') as CustomerName,''  as Address, '' as Information,'OFACSDNDetails' as ListType,7 as ListTypeid, " + scroreOFAC + " AS score FROM OFACSDNDetails WHERE  ( " + whereOFAC + " ) ";
                            }
                            #endregion
                        }
                    }
                }
                else if (ListTypeid == "3")
                {
                    Query = "";
                }
                else if (ListTypeid == "4")
                {
                    Query = select4 + " 'BLACKLIST' as ListType," + ListTypeid + " as ListTypeid, ROUND(" + score4 + " ,2)*100 AS score FROM BLACKLIST WHERE " + where4 + " ";

                }

                else if (ListTypeid == "5")
                {
                    // UK List
                    Query = "";
                }
                else if (ListTypeid == "6")
                {
                    // EU List
                    Query = "";
                }
                if (Query != "")
                {
                    if (combinedQuery != "")
                    {
                        combinedQuery = combinedQuery + " UNION ALL " + Query;
                    }
                    else
                        combinedQuery = Query;
                }
            }
            #endregion

            return combinedQuery;
        }

        //private string SearchFromList(string customer, string country, int selectedscore, string city, string mCriteria, string customer_type)
        //{
        //    //List<SearchList> TotalListList = new List<SearchList>();

        //    var Query = "";
        //    var combinedQuery = ""; ;

        //    var scorefacor = 0.0;
        //    scorefacor = (Double)selectedscore / 100;

        //    string where1 = string.Empty, where2 = string.Empty, where3, where4 = string.Empty, score1 = string.Empty, score2 = string.Empty, score4 = string.Empty;
        //    string select1 = string.Empty, select4 = string.Empty;

        //    //=================================================
        //    #region Jaro Winkler

        //    score1 = "CAST(100 as float)";
        //    score2 = "CAST(100 as float)";
        //    score4 = "CAST(100 as float)";

        //    if (customer_type == "Legal Entities")
        //    {
        //        #region LegalEntities

        //        select1 = "SELECT id, ";
        //        select4 = select1;

        //        #region Customer
        //        if (customer != "")
        //        {
        //            if (mCriteria == "Jaro Winkler" || mCriteria == "Phonetic" || mCriteria == "Both")
        //            {
        //                where1 = " (dbo.JaroSplit(UPPER(ENTITY_ALIAS_NAME),UPPER('" + customer + "')) >=" + scorefacor + " )";
        //                where4 = where1;

        //                score1 = " ROUND(dbo.JaroSplit(UPPER(ENTITY_ALIAS_NAME),UPPER('" + customer + "'))*100,2) ";
        //                score4 = score1;

        //                where2 = " sdnType !='Individual' AND (dbo.JaroSplit(UPPER(firstName+' '+lastName),UPPER('" + customer + "')) >=" + scorefacor + ")";
        //                score2 = " ROUND(dbo.JaroSplit(UPPER(firstName+' '+lastName),UPPER('" + customer + "'))*100,2) ";
        //            }
        //            else
        //            {
        //                //Exact Match Search
        //                where1 = " ( ENTITY_ALIAS_NAME LIKE '%" + customer + "%' )";
        //                where4 = where1;
        //                where2 = " sdnType !='Individual' AND ( firstName+' '+ lastName  LIKE '%" + customer + "%')";
        //            }
        //        }
        //        #endregion

        //        #region Country

        //        if (country != "")
        //        {
        //            if (where1 == "")
        //            {
        //                where1 = " ( ENTITY_ADDRESS like '%" + country + "%' )";
        //                where2 = " sdnType !='Individual' AND ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' )";
        //                where4 = where1;
        //            }
        //            else
        //            {
        //                where1 += " AND ( ENTITY_ADDRESS like '%" + country + "%' )";
        //                where2 += " AND ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' )";
        //                where4 = where1;
        //            }
        //        }
        //        #endregion

        //        #region City
        //        if (city != "")
        //        {
        //            if (where1 == "")
        //            {
        //                where1 = " ( ENTITY_ADDRESS like '%" + city + "%')";
        //                where2 = " sdnType !='Individual' AND ( addressList_address_city like '%" + city + "%' OR  addressList_address_state like '%" + city + "%' )";
        //                where4 = where1;
        //            }
        //            else
        //            {
        //                where1 += " AND ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' OR  INDIVIDUAL_ADDRESS like '%" + city + "%' )";
        //                where2 += " AND ( addressList_address_city like '%" + city + "%' OR  addressList_address_state like '%" + city + "%' )";
        //                where4 = where1;
        //            }
        //        }
        //        #endregion

        //        #endregion
        //    }
        //    else
        //    {
        //        #region Individuals
        //        select1 = "SELECT id,";
        //        select4 = select1;

        //        #region Customer
        //        if (customer != "")
        //        {
        //            if (mCriteria == "Jaro Winkler" || mCriteria == "Phonetic" || mCriteria == "Both")
        //            {
        //                where1 = " (dbo.JaroSplit(UPPER(FIRST_NAME+' '+SECOND_NAME),UPPER('" + customer + "')) >=" + scorefacor + " )";
        //                where2 = " sdnType ='Individual' AND (dbo.JaroSplit(UPPER(firstName+ ' ' +lastName),UPPER('" + customer + "')) >=" + scorefacor + " )";
        //                where4 = where1;

        //                score1 = " ROUND(dbo.JaroSplit(UPPER(FIRST_NAME+' '+SECOND_NAME),UPPER('" + customer + "'))*100,2)";
        //                score2 = " ROUND(dbo.JaroSplit(UPPER(firstName+' '+lastName),UPPER('" + customer + "'))*100,2) ";
        //                score4 = score1;
        //            }
        //            else
        //            {
        //                where1 = " ( FIRST_NAME LIKE '%" + customer + "%' OR SECOND_NAME LIKE '%" + customer + "%' )";
        //                where2 = " sdnType ='Individual' AND ( firstName+' '+ lastName  LIKE '%" + customer + "%')";
        //                where4 = where1;
        //            }
        //        }
        //        #endregion

        //        #region Country

        //        if (country != "")
        //        {
        //            if (where1 == "")
        //            {
        //                where1 = " ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + country + "%' OR  INDIVIDUAL_ADDRESS like '%" + country + "%' )";
        //                where2 = " sdnType ='Individual' AND ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' )";
        //                where4 = " ( INDIVIDUAL_ADDRESS like '%" + country + "%' OR  INDIVIDUAL_PLACE_OF_BIRTH like '%" + country + "%' )";
        //            }
        //            else
        //            {
        //                where1 += " AND ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + country + "%' OR  INDIVIDUAL_ADDRESS like '%" + country + "%' )";
        //                where2 += " AND ( addressList_address_country like '%" + country + "%' OR  placeOfBirth like '%" + country + "%' )";
        //                where4 += " AND ( INDIVIDUAL_ADDRESS like '%" + country + "%' OR  INDIVIDUAL_PLACE_OF_BIRTH like '%" + country + "%' )";
        //            }
        //        }
        //        #endregion

        //        #region City

        //        if (city != "")
        //        {
        //            if (where1 == "")
        //            {
        //                where1 = " ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' OR  INDIVIDUAL_ADDRESS like '%" + city + "%' )";
        //                where2 = " sdnType ='Individual' AND ( addressList_address_city like '%" + city + "%' OR  addressList_address_state like '%" + city + "%' )";
        //                where4 = " ( INDIVIDUAL_ADDRESS like '%" + city + "%' OR  INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' )";
        //            }
        //            else
        //            {
        //                where1 += " AND ( INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' OR  INDIVIDUAL_ADDRESS like '%" + city + "%' )";
        //                where2 += " AND ( addressList_address_city like '%" + city + "%' OR  addressList_address_state like '%" + city + "%' )";
        //                where4 += " AND ( INDIVIDUAL_ADDRESS like '%" + city + "%' OR  INDIVIDUAL_PLACE_OF_BIRTH like '%" + city + "%' )";
        //            }
        //        }
        //        #endregion

        //        #endregion
        //    }

        //    #endregion

        //    //==================Old============================
        //    #region Union
            
        //    Query = " select * from ScreeningListConfig where IsSelected=1";
        //    var Data = ExecuteCommand(Query);
        //    foreach (DataRow dr in Data.Rows)
        //    {
        //        var ListTypeid = dr["id"].ToString();

        //        if (ListTypeid == "1")
        //        {
        //            Query = select1 + " 'UNSCR' as ListType, ROUND(" + score1 + " ,2) AS score FROM UNSCR WHERE " + where1 + " ";
        //        }

        //        else if (ListTypeid == "2")
        //        {
        //            Query = "SELECT id,'OFACSDN' as ListType, ROUND(" + score2 + " ,2) AS score FROM OFACSDN WHERE " + where2 + " ";                    
        //        }
        //        else if (ListTypeid == "3")
        //        {
        //            Query = "";
        //        }
        //        else if (ListTypeid == "4")
        //        {
        //            Query = select4 + " 'BLACKLIST' as ListType, ROUND(" + score4 + " ,2) AS score FROM BLACKLIST WHERE " + where4 + " ";

        //        }

        //        else if (ListTypeid == "5")
        //        {
        //            // UK List
        //            Query = "";
        //        }
        //        else if (ListTypeid == "6")
        //        {
        //            // EU List
        //            Query = "";
        //        }
        //        if (Query != "")
        //        {
        //            if (combinedQuery != "")
        //            {
        //                combinedQuery = combinedQuery + " UNION ALL " + Query;
        //            }
        //            else
        //                combinedQuery = Query;
        //        }
        //    }
        //    #endregion

        //    return combinedQuery;
        //}

    }
}
