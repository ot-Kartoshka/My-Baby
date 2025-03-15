using NUnit.Framework;
using Program;
using System;
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
            string password = GenPass.GeneratePassword(length);
            Assert.That(length, Is.EqualTo(password.Length), "Пароль має бути заданої довжини.");
        }

        [Test]
        public void GeneratePassword_ContainsValidCharacters()
        {
            string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            string password = GenPass.GeneratePassword(20);

            foreach (char c in password)
            {
                Assert.That(validChars.Contains(c), Is.True, $"Неприпустимий символ у паролі: {c}");
            }
        }

        [Test]
        public void GeneratePassword_IsRandom()
        {
            string pass1 = GenPass.GeneratePassword(10);
            string pass2 = GenPass.GeneratePassword(10);
            Assert.That(pass1, Is.Not.EqualTo(pass2), "Паролі не повинні повторюватися.");
        }
    }
}
