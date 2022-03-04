using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNA.DataGateHelper;

namespace TestPagedDataCS
{
    class Repository
    {
        List<Customer> customers = new List<Customer>();

        public bool GetPage(int pageNumber, int pageSize)
        {
            ASNA.VisualRPG.Runtime.Database DGDB;
            DataGateDB DGDBManager = new DataGateDB("*Public/Leyland");
            DGDB = DGDBManager.GetConnectionForAVR();

            PagedData pd = new PagedData(DGDB: DGDB,
                                         LibraryName: "qtemp",
                                         ProgramLibrary: "rpzimmie",
                                         RPGProgramToCall: "sqlimmed",
                                         PageSize: pageSize,
                                         CustomClassType: typeof(Customer));

            customers.Clear();

            pd.AfterRowRead += OnAfterRowRead;

            pd.AddSQLSelect("select cmcustno, cmname");
            pd.AddSQLFrom("from examples/cmastnewL2");
            pd.AddSQLOrderBy("order by cmname, cmcustno");

            pd.WriteThenReadTempFile(pageNumber);

            string sql = pd.SQL; 

            foreach (Customer customer in customers)
            {
                Console.WriteLine("{0:00000} - {1}", customer.CMCustNo, customer.CMName);
            }
            
            DGDBManager.Disconnect();

            return pd.MoreRecords;
        }

        private void OnAfterRowRead(object sender, AfterRowReadArgs e)
        {
            Customer cust = (Customer)e.CustomClassInstance;

            customers.Add(cust);
        }
    }
}


// Database Name.: *Public/Leyland
// Library.......: examples
// File..........: CMastNewL2
// Format........: RCMMastL2
// Key field(s)..: cmname, cmcustno
// Type..........: simple logical
// Base file.....: Examples/CMastNew
// Description...: CustomerByName
// Record length.: 151

// Field name   Data type                    Description
// ------------------------------------------------------------------
// CMName       Type(*Char) Len(40)          Customer Name
// CMCustNo     Type(*Packed) Len(9,0)       Customer Number

// CMAddr1      Type(*Char) Len(35)           Address Line 1
// CMCity       Type(*Char) Len(30)           City
// CMState      Type(*Char) Len(2)            State
// CMCntry      Type(*Char) Len(2)            Country Code
// CMPostCode   Type(*Char) Len(10)           Postal Code(zip)
// CMActive     Type(*Char) Len(1)            Active = 1, else 0
// CMFax        Type(*Packed) Len(10,0)       Fax Number
// CMPhone      Type(*Char) Len(20)           Main Phone
//------------------------------------------------------------------