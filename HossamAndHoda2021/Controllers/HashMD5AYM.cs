using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HossamAndHoda2021.Controllers
{
    public class HashMD5AYM
    {
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }


        public static string GenerateNewPasswordMD5(string password)
        {
            password = HashMD5AYM.CreateMD5("KarovTest2020" + password + "WEBDEV2020");
            return password;
        }

    }
}