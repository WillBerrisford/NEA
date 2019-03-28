using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    class Account_Data //this accounts backs up account information so it can be used to ensure that the user stays logged in
    {
        public string AccountName { get; set; } //stores the username
        public string AccountID { get; set; } //stores the unique user ID
        public bool SignedIn { get; set; } //whether the user is signed in or not
        public string Password { get; set; } //stores the password of the user

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
