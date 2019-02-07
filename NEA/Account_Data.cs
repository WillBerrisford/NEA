using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    class Account_Data
    {
        public string AccountName { get; set; }
        public string AccountID { get; set; }
        public bool SignedIn { get; set; }
        public string Password { get; set; }

        public Account_Data()
        { }

        public Account_Data(string name, string id, bool signed_in, string password)
        {
            AccountName = name;
            AccountID = id;
            SignedIn = signed_in;
            Password = password;
        }

        public string Get_Name()
        {
            return AccountName;
        }

        public string Get_Password()
        {
            return Password;
        }
    }
}
