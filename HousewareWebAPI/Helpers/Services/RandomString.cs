using System;

namespace HousewareWebAPI.Helpers.Services
{
    public class RandomString
    {
        public static string GetRandomString(int length)
        {            
            try
            {
                char[] allowedChar = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'v', 'w', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                char[] arrayChar = new char[length];
                Random random = new ();
                for (int i = 0; i < length; i++)
                {
                    arrayChar[i] = allowedChar[random.Next(0, allowedChar.Length)];
                }
                return new string(arrayChar);
            }
            catch (Exception e)
            {
                throw new Exception("Can't generate random string. " + e.Message);
            }
        }
    }
}
