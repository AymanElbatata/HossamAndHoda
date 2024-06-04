using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HossamAndHoda2021.Controllers
{
    public class MySPECIALGUID
    {
        public static string GetUniqueKey(int length)
        {
            string guidResult = string.Empty;

            while (guidResult.Length < length)
            {
                // Get the GUID.
                guidResult += Guid.NewGuid().ToString().GetHashCode().ToString("x");
            }

            // Make sure length is valid.
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            // Return the first length bytes.
            return guidResult.Substring(0, length);
        }


        public static string GenerateGUID(int first, int second)
        {
            string get1, get2, allready;
            get1 = GetUniqueKey(first);
            get2 = GetUniqueKey(second);
            allready = get1 + "_" + get2;
            return allready;
        }


        public static string GenerateMyCode(string first, string usercreation, string last)
        {
            string get1, get2, allready;
            get1 = GetUniqueKey(5);
            get2 = "" + DateTime.Now.Millisecond + DateTime.Now.Hour;
            allready = first + get1 + usercreation + get2 + last;
            return allready;
        }

        public static string GenerateMyNumberCode()
        {
            string allready;
            allready = "" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
            return allready;
        }


        public static string GUIDOnlyNumber(int count)
        {
            string accesscode = "";

            var rand = new Random();

            // Generate and display 5 random byte (integer) values.
            var bytes = new byte[4];
            rand.NextBytes(bytes);
            foreach (byte byteValue in bytes)
            {
                accesscode += byteValue;
            }
            accesscode = accesscode.Substring(1, count);
            return accesscode;
        }

        public static string GUIDNumber(int length)
        {
            string guidResult = GenerateGUID(151, 101);
            string Result = string.Empty;

            for (int i = 0; i < guidResult.Length; i++)
            {
                switch (guidResult[i])
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        Result += guidResult[i];
                        break;
                    default:
                        break;
                }
            }
            // Return the first length bytes.
            return Result.Substring(0, length);
        }

    }
}