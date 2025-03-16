using NUnit.Framework;
using System;
using System.Diagnostics;
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
        public void Program_ReadsLengthFromStdin()
        {
            var process = StartProcessWithInput("8");

            string output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            Assert.That(output.Length, Is.EqualTo(8), $"Очікувалось 8 символів, але отримано: {output.Length}");
        }

        [Test]
        public void Program_ReturnsExitCode0_OnSuccess()
        {
            var process = StartProcessWithInput("10");
            process.WaitForExit();

            Assert.That(process.ExitCode, Is.EqualTo(0), $"Очікуваний код виходу: 0, отримано: {process.ExitCode}");
        }

        [Test]
        public void Program_ReturnsNonZeroExitCode_OnInvalidInput()
        {
            var process = StartProcessWithInput("invalid");
            process.WaitForExit();

            Assert.That(process.ExitCode, Is.Not.EqualTo(0), "Очікувався ненульовий код виходу при помилці.");
        }

        [Test]
        public void Program_WritesErrorToStderr_OnInvalidInput()
        {
            var process = StartProcessWithInput("0");

            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Assert.That(error, Does.Contain("Error: Invalid password length"), $"Отримано stderr: {error}");
        }

        private Process StartProcessWithInput(string input)
        {
            string exePath = Path.GetFullPath(Path.Combine(
    TestContext.CurrentContext.TestDirectory,
    @"..\..\..\program\bin\Debug\program.exe"
));

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = "",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            using (var writer = process.StandardInput)
            {
                writer.WriteLine(input);
            }

            return process;
        }
    }
}
