using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    class Salt_Hash
    {
        string Salt { get; set; }
        string Hash { get; set; }

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
