using System;
using System.Linq;

namespace Program
{
    public class GenPass
    {
        private static readonly Random random = new Random();

        public static int Main(string[] args)
        {
            try
            {
                int length;
                if (args.Length == 0)
                {
                    Console.Write("");
                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out length) || length < 1)
                    {
                        Console.Error.WriteLine("Error: Invalid password length.");
                        return 1;
                    }
                }
                else
                {
                    if (!int.TryParse(args[0], out length) || length < 1)
                    {
                        Console.Error.WriteLine("Error: Invalid password length.");
                        return 1;
                    }
                }

                string password = GeneratePassword(length);
                Console.WriteLine(password);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return 1;
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
