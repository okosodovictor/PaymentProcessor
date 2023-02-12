using System;
using System.Security.Cryptography;
using System.Text;
using Payment.Application.Interfaces;

namespace Payment.Application.Managers
{
	public class AESEncryption : IEncryption
    {
        private Options _options;

        public AESEncryption(Options options)
		{
            _options = options;
        }

        public string Decrypt(string cardNumber)
        {
            using (AesManaged tdes = new AesManaged())
            {
                tdes.Key = StringToByteArray(_options.Key);
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform decrypt = tdes.CreateDecryptor())
                {
                    byte[] cipher = StringToByteArray(cardNumber);
                    byte[] plain = decrypt.TransformFinalBlock(cipher, 0, cipher.Length);
                    string decryptedText = Encoding.UTF8.GetString(plain, 0, plain.Length);

                    return decryptedText;
                }
            }
        }

        public string Encrypt(string CardNumber)
        {
            using (AesManaged tdes = new AesManaged())
            {
                tdes.Key = StringToByteArray(_options.Key);
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform crypt = tdes.CreateEncryptor())
                {
                    byte[] plain = Encoding.UTF8.GetBytes(CardNumber);
                    byte[] cipher = crypt.TransformFinalBlock(plain, 0, plain.Length);

                    var encryptedText = ByteArrayToString(cipher);

                    return encryptedText;
                }
            }
        }

        public string Mask(string cardNumber)
        {
            string FirstSixDigit = cardNumber.Substring(0, 6);
            string lastFourDigit = cardNumber.Substring(cardNumber.Length - 4, 4);
            int otherslen = cardNumber.Length - FirstSixDigit.Length - lastFourDigit.Length;

            StringBuilder builder = new StringBuilder();
            builder.Append(FirstSixDigit.Trim());
            for (int i = 0; i < otherslen; i++)
            {
                builder.Append("*");
            }

            builder.Append(lastFourDigit.Trim());
            return builder.ToString();
        }

        private static string ByteArrayToString(byte[] text)
        {
            StringBuilder hex = new StringBuilder(text.Length * 2);
            foreach (byte b in text)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
        }

        public class Options
        {
            public string Key { get; set; }
        }
    }
}

