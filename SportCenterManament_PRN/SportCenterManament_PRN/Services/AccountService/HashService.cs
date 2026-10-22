using System;
using System.Security.Cryptography;
using System.Text;

namespace SportCenterManament_PRN.Services.AccountService
{
    public class HashService
    {
        private static readonly string PRIVATEKEY = "skaijdaosjdpcjdp1o2903u1290j90dns9axdj9asdjisaxghiu1hcbkdlabsodf";

        public static string GenerateSHA512(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            string saltedInput = input + PRIVATEKEY;
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(saltedInput);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static string HashPasswordBCrypt(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword, 11);
        }

        public static bool VerifyPassword(string rawPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(rawPassword) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            try
            {
                if (storedHash.StartsWith("$2a$") || storedHash.StartsWith("$2b$") || storedHash.StartsWith("$2y$"))
                {
                    if (BCrypt.Net.BCrypt.Verify(rawPassword, storedHash))
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }

            string sha512Hash = GenerateSHA512(rawPassword);
            if (string.Equals(sha512Hash, storedHash, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(rawPassword, storedHash, StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }
    }
}
