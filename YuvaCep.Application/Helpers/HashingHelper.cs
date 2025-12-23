using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Helpers
{
    public static class HashingHelper
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordHash = Convert.ToBase64String(hash);
            }
        }
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var  hmac = new HMACSHA512(storedSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                if (computeHash.Length != storedHash.Length) return false;
                
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}
