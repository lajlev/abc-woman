namespace FoosBall.Main
{
    using System;
    using System.Text;
    
    // Calculates a MD5 hash from the given string and uses the given
    // encoding.
    public class Md5
    {
        public static string CalculateMd5(string input, Encoding useEncoding)
        {
            var cryptoService = new System.Security.Cryptography.MD5CryptoServiceProvider();
 
            var inputBytes = useEncoding.GetBytes(input);
            inputBytes = cryptoService.ComputeHash(inputBytes);
            return BitConverter.ToString(inputBytes).Replace("-", string.Empty);
        }

        // Calculates a MD5 hash from the given string. 
        // (By using the default encoding)
        public static string CalculateMd5(string input)
        {
            // That's just a shortcut to the base method
            return CalculateMd5(input, Encoding.Default);
        }

        // Calculates a MD5 hash from the given string. 
        // (By using the default encoding)
        public static string GetGravatarEmailHash(string email)
        {
            var gravatarUrl = "http://www.gravatar.com/avatar/" + CalculateMd5(email.ToLower().Trim(), Encoding.Default);
            return gravatarUrl.ToLower();
        }
    }
}