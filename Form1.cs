using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWIFTReader
{
    public partial class Form1 : Form
    {
        #region Declaratrion

        Parser objParser = new Parser();
        CommonFields objCommonFields;
        
        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFile.Text = open.FileName;
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            var fileName = txtFile.Text;
            if (System.IO.File.Exists(fileName))
            {

                //MT103 objMessage = new MT103();
                ////MT103Parser objMT103Parser = new MT103Parser();
                //StreamReader streamReader = new StreamReader(fileName);
                //string text = streamReader.ReadToEnd();
                //objMessage = objMT103Parser.Parse(text);

                //textBox1.Text = objMessage.RefNo_20;
                //textBox2.Text = objMessage.BankOpCode_23B;
                //textBox3.Text = objMessage.DateCurrencyAmount_32A;
                //textBox4.Text = objMessage.CurrencyAmount_33B;
                //textBox5.Text = objMessage.Customer_50K;
                //textBox6.Text = objMessage.Institution_52A;
                //textBox7.Text = objMessage.SenderBank_53A;
                //textBox8.Text = objMessage.ReceiverBank_54A ;
                //textBox9.Text = objMessage.AccountWithInstitution_57A ;
                //textBox10.Text = objMessage.Beneficiary_59;
                //textBox11.Text = objMessage.RemittanceInfo_70;
                

            
                //streamReader.Close();
                
                //var header = new Raptorious.SharpMt940Lib.Mt940Format.Separator("STARTUMSE");
                //int LineCount = data.Length;
                //Data = String.Join(Environment.NewLine, data);

                //var trailer = new Raptorious.SharpMt940Lib.Mt940Format.Separator("-");
                //var genericFomat = new Raptorious.SharpMt940Lib.Mt940Format.GenericFormat(header, trailer);

                //var parsed = SharpMt940Lib.Mt940Parser.Parse(genericFomat, fileName);
                //gridResult.DataSource = parsed;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            objParser.ReadFiles();
            this.Close();
        }
    }
}
