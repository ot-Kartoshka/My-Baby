using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace PasswordGeneratorTests
{
    [TestFixture]
    public class GenPassTests
    {
        [Test]
        public void GeneratePassword_CorrectLength()
        {
            int length = 10;
            string password = Program.GenPass.GeneratePassword(length);
            Assert.That(password.Length, Is.EqualTo(length), "Пароль має бути заданої довжини.");
        }

        [Test]
        public void GeneratePassword_ContainsValidCharacters()
        {
            string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            string password = Program.GenPass.GeneratePassword(20);

            foreach (char c in password)
            {
                Assert.That(validChars.Contains(c), $"Неприпустимий символ у паролі: {c}");
            }
        }

        [Test]
        public void GeneratePassword_IsRandom()
        {
            string pass1 = Program.GenPass.GeneratePassword(10);
            string pass2 = Program.GenPass.GeneratePassword(10);
            Assert.That(pass1, Is.Not.EqualTo(pass2), "Паролі не повинні повторюватися.");
        }

        [Test]
        public void Program_ReturnsExitCode0_OnSuccess()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                int exitCode = Program.GenPass.Main(new string[] { "10" });
                Assert.That(exitCode, Is.EqualTo(0), $"Очікуваний код виходу: 0, отримано: {exitCode}");
            }
        }

        [Test]
        public void Program_ReturnsNonZeroExitCode_OnInvalidInput()
        {
            int exitCode = Program.GenPass.Main(new string[] { "asd" });
            Assert.That(exitCode, Is.Not.EqualTo(0), "Очікувався ненульовий код виходу при помилці.");
        }

        [Test]
        public void Program_WritesErrorToStderr_OnInvalidInput()
        {
            using (var sw = new StringWriter())
            {
                Console.SetError(sw);
                Program.GenPass.Main(new string[] { "0" });
                string error = sw.ToString();
                Assert.That(error, Does.Contain("Error: Invalid password length"), $"Отримано stderr: {error}");
            }
        }

        [Test]
        public void Program_ReadsLengthFromStdin()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("8"))
                {
                    Console.SetIn(sr);
                    Program.GenPass.Main(new string[] { });
                    string output = sw.ToString().Trim();
                    Assert.That(output.Length, Is.EqualTo(8), $"Очікувалось 8 символів, але отримано: {output.Length}");
                }
            }
        }
    }
}