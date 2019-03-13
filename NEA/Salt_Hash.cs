using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    //This class stores the both the salt used in the password proccess as well as the hashed password itself
    public class Salt_Hash
    {
        string Salt { get; set; }
        string Hash { get; set; }

        public Salt_Hash()
        { }

        //sets the salt and has using given values
        public Salt_Hash(string salt, string hash)
        {
            Salt = salt;
            Hash = hash;
        }

        public string Get_Salt()
        {
            return Salt;
        }

        public string Get_Hash()
        {
            return Hash;
        }
    }
}
