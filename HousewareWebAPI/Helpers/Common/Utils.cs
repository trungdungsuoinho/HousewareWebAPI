﻿using PhoneNumbers;
using System.Security.Cryptography;
using System.Text;

namespace HousewareWebAPI.Helpers.Common
{
    public class Utils
    {
        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }

        public static string ParseInternationalPhoneNumber(string phoneToParce, string region = "VN")
        {
            PhoneNumber phoneNumber = PhoneNumberUtil.GetInstance().Parse(phoneToParce, "VN");
            return PhoneNumberUtil.GetInstance().Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
        }
    }
}
