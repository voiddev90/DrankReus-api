using System;
using System.Security.Cryptography;
using System.Text;

namespace DrankReus_api.Helpers
{

    public static class UserHelper
    {
        public static string HashPassword(string password)
        {
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }

        public static bool PasswordMatch(string userPassword, string password)
        {
            if(userPassword == HashPassword(password)) return true;
            return false;
        }
    }
    
}