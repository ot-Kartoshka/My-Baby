using System;
using System.Linq;

namespace Program
{
    public class GenPass
    {
        private static readonly Random random = new Random();

        public static void Main()
        {
            try
            {
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int length) || length < 1)
                {
                    Console.Error.WriteLine("Error: Invalid password length.");
                    Environment.Exit(1);
                }

                string password = GeneratePassword(length);
                Console.WriteLine(password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        public static string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}